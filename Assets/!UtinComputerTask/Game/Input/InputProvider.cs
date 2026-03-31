using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider : IInputProvider
{
    private bool _isHolding;
    private bool _pressedThisFrame;
    private bool _releasedThisFrame;

    private Camera _camera;
    private InputSystem_Actions _input;

    public bool IsHolding => _isHolding;
    public bool PressedThisFrame => _pressedThisFrame;
    public bool ReleasedThisFrame => _releasedThisFrame;

    public InputProvider(Camera camera)
    {
        _camera = camera;
        _input = new InputSystem_Actions();
    }

    public void Enable()
    {
        _input.Enable();

        _input.Player.Press.started += OnPressStarted;
        _input.Player.Press.canceled += OnPressCanceled;
    }

    public void Disable()
    {
        _input?.Disable();

        _input.Player.Press.started -= OnPressStarted;
        _input.Player.Press.canceled -= OnPressCanceled;
    }

    public void LateUpdate()
    {
        _pressedThisFrame = false;
        _releasedThisFrame = false;
    }

    public Vector3 GetPointerWorldPosition()
    {
        Vector2 screenPos = _input.Player.PointerPosition.ReadValue<Vector2>();

        Vector3 world = _camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
        world.z = 0f;

        return world;
    }

    private void OnPressStarted(InputAction.CallbackContext ctx)
    {
        _isHolding = true;
        _pressedThisFrame = true;
    }

    private void OnPressCanceled(InputAction.CallbackContext ctx)
    {
        _isHolding = false;
        _releasedThisFrame = true;
    }
}