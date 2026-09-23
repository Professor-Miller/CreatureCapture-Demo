using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnTouchBegan;
    public static event Action<Vector2> OnTouchMoved;
    public static event Action<Vector2> OnTouchEnded;

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    private void Update()
    {
#if UNITY_EDITOR

        CheckMouseInput();
#else

        CheckTouchInput();
#endif
    }

#if UNITY_EDITOR
    private void CheckMouseInput()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Input Began");
            OnTouchBegan?.Invoke(mousePosition);
        }

        if (Mouse.current.leftButton.isPressed)
        {
            OnTouchMoved?.Invoke(mousePosition);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Debug.Log("Input Ended");
            OnTouchEnded?.Invoke(mousePosition);
        }
    }
#else
    private void CheckTouchInput()
    {
        if (Touch.activeTouches.Count == 0)
            return;

        Touch touch = Touch.activeTouches[0];

        switch (touch.phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:
                OnTouchBegan?.Invoke(touch.screenPosition);
                break;
            case UnityEngine.InputSystem.TouchPhase.Moved:
                OnTouchMoved?.Invoke(touch.screenPosition);
                break;
            case UnityEngine.InputSystem.TouchPhase.Ended:
                OnTouchEnded?.Invoke(touch.screenPosition);
                break;
        }
    }
#endif
}
