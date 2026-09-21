using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnTouchedScreen;

    // Enhanced touch support is enabled when the menu is active.
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        // Set the touch position and invoke the event in the editor
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            OnTouchedScreen?.Invoke(mousePosition);
        }
        // Set the touch position and invoke the event on the mobile device
#else
        if (Touch.activeTouches.Count == 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            OnTouchedScreen?.Invoke(touch.screenPosition);
        }
#endif
    }
}
