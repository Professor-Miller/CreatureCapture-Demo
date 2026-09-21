using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MainMenuHandler : MonoBehaviour
{
    // Enhanced touch support is enabled when the menu is active.
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        // Load the game scene when the user interacts with the menu.
#if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Game");
        }
        // Load the game scene when the user interacts with the menu on mobile.
    #else
        if (Touch.activeTouches.Count == 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            SceneManager.LoadScene("Game");
        }
    #endif
    }
}
