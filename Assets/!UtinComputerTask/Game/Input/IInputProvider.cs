using UnityEngine;

public interface IInputProvider
{
    void Enable();
    void Disable();

    bool IsHolding { get; }
    bool PressedThisFrame { get; }
    bool ReleasedThisFrame { get; }

    void LateUpdate();

    Vector3 GetPointerWorldPosition();
}