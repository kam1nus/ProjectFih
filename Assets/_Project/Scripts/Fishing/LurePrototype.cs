using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LurePrototype : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}