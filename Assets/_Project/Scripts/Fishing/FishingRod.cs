using UnityEngine;
using UnityEngine.InputSystem;

public class FishingRod : MonoBehaviour
{
    [Header("References")]
    public Transform castPoint;
    public GameObject lurePrefab;
    public LineRenderer fishingLine;

    [Header("Casting")]
    public float minCastForce = 4f;
    public float maxCastForce = 11f;
    public float chargeSpeed = 5f;

    [Header("Retrieve")]
    public float retrieveSpeed = 6f;

    private GameObject currentLure;
    private float currentCastForce;
    private bool hasStartedRetrieve = false;

    private void Start()
    {
        // Леска изначально скрыта
        if (fishingLine != null)
        {
            fishingLine.positionCount = 2;
            fishingLine.enabled = false;
        }
    }

    private void Update()
    {
        HandleCasting();
        HandleRetrieve();
        UpdateFishingLine();
    }

    private void HandleCasting()
    {
        if (Mouse.current == null)
            return;

        // ПКМ — начинаем заряжать заброс
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            currentCastForce = minCastForce;
        }

        // Держим ПКМ — увеличиваем силу
        if (Mouse.current.rightButton.isPressed)
        {
            currentCastForce += chargeSpeed * Time.deltaTime;

            currentCastForce = Mathf.Clamp(
                currentCastForce,
                minCastForce,
                maxCastForce
            );
        }

        // Отпускаем ПКМ — забрасываем
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            Cast();
        }
    }

    private void Cast()
    {
        // Если старая приманка ещё существует — удаляем её
        if (currentLure != null)
        {
            Destroy(currentLure);
        }

        // Создаём новую приманку на кончике удилища
        currentLure = Instantiate(
            lurePrefab,
            castPoint.position,
            castPoint.rotation
        );

        Rigidbody rb = currentLure.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Немного направляем заброс вверх,
            // чтобы приманка летела по дуге
            Vector3 direction =
                (castPoint.forward + Vector3.up * 0.55f).normalized;

            float adjustedForce = currentCastForce * 0.6f;

            rb.AddForce(
                direction * adjustedForce,
                ForceMode.Impulse
            );
        }

        hasStartedRetrieve = false;
    }

    private void HandleRetrieve()
    {
        if (currentLure == null || Mouse.current == null)
            return;

        Rigidbody rb = currentLure.GetComponent<Rigidbody>();

        // ЛКМ — подмотка
        if (Mouse.current.leftButton.isPressed)
        {
            hasStartedRetrieve = true;

            Vector3 target = castPoint.position;

            Vector3 direction =
                (target - currentLure.transform.position).normalized;

            if (rb != null)
            {
                rb.linearVelocity = direction * retrieveSpeed;
            }

            // Если приманка почти дошла до удилища — убираем её
            if (
                Vector3.Distance(
                    currentLure.transform.position,
                    target
                ) < 0.4f
            )
            {
                Destroy(currentLure);
                currentLure = null;
                hasStartedRetrieve = false;
            }
        }
        else if (hasStartedRetrieve)
        {
            // Пока временно останавливаем приманку после подмотки.
            // Позже это заменит физика воды.
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void UpdateFishingLine()
    {
        if (fishingLine == null || castPoint == null)
            return;

        // Пока приманки нет — леску не показываем
        if (currentLure == null)
        {
            fishingLine.enabled = false;
            return;
        }

        fishingLine.enabled = true;

        // Начало лески — кончик удилища
        fishingLine.SetPosition(
            0,
            castPoint.position
        );

        // Конец лески — приманка
        fishingLine.SetPosition(
            1,
            currentLure.transform.position
        );
    }
}