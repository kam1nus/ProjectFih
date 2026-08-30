# ProjectFih Agent Rules

ProjectFih is a stylized fishing sandbox game built with Unity 6 and URP.

## Roles

The human game director decides:
- gameplay feel
- mechanics
- visual direction
- progression
- world design
- what should or should not exist in the game

AI agents implement technical work based on explicit tasks.

Do not invent major gameplay systems unless the task explicitly asks for them.

## Core Development Rules

- Preserve existing working behavior unless the task requires changing it.
- Inspect existing code before editing it.
- Make the smallest reasonable change needed for the task.
- Do not refactor unrelated systems.
- Do not rename or move existing files unless explicitly requested.
- Do not delete working systems without explicit permission.
- Do not add third-party packages unless explicitly requested.
- Keep public Inspector settings understandable.

## Unity

- Unity version: 6000.3.23f1
- Render pipeline: URP
- Input: Unity Input System
- Main prototype scene: FishingPrototype
- Language: C#

## Existing Systems

### Player
PlayerController.cs currently handles:
- WASD movement
- running
- jumping
- mouse camera control

Do not modify PlayerController unless the task explicitly requires it.

### FishingRod
FishingRod.cs currently handles:
- RMB cast charging
- RMB release casting
- LMB retrieving
- lure spawning
- fishing line rendering

Preserve existing casting and retrieving unless the task explicitly changes them.

### Lure
LurePrototype.cs currently uses Rigidbody physics.

## Fishing Design Philosophy

Fishing should feel physical rather than UI-driven.

The player should learn what is happening through:
- rod movement
- line movement and tension
- reel sounds
- drag behavior
- water movement
- visible fish only when naturally visible

Avoid exposing information the player could not realistically perceive.

For example:
- do not immediately display fish species and weight during a deep-water bite
- do not use underwater cameras for ordinary fishing
- fish size should be inferred partly through tackle behavior

## Architecture

Prefer focused reusable components over large manager scripts.

Good examples:
- FishingLineController
- RodBendController
- ReelController
- WaterInteraction
- FishBrain
- FishPerception
- LureSignals

Avoid one giant FishingManager controlling unrelated systems.

Use ScriptableObjects for reusable data when appropriate.

Where practical, systems should not prevent future multiplayer implementation.

Do not implement multiplayer unless explicitly requested.

## Code Quality

- Write clear C#
- Keep classes focused
- Avoid unnecessary complexity
- Add comments where behavior is non-obvious
- Avoid hard-coded scene references where reusable references can be assigned in Inspector
- Do not silently change input bindings
- Do not suppress errors instead of fixing them

## Task Workflow

Before changing files:
1. Inspect the relevant existing files.
2. Explain briefly what files need to change.
3. Implement only the requested task.

After changing files, report:
- files created
- files modified
- what behavior changed
- any Unity Inspector setup required
- any known limitations
- how the human should test it

If the task can be completed without editing a file, do not edit it.

## Safety Rule

Never perform large project-wide refactors without explicit approval.

If a task is ambiguous and could substantially change gameplay or architecture, ask before implementing it.
