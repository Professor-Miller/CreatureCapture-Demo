using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;

// this is a custom alias for the Touch class from the EnhancedTouch namespace
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MainMenuHandler : MonoBehaviour
{
    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        #if UNITY_EDITOR
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Game");
        }
        #else
        // Touch.activeTouches is a list of all active touches on the screen
        // if there are no active touches, we return early
        if (Touch.activeTouches.Count == 0) return;

        // Touch.activeTouches[0] is the first touch in the list (the one that just began)
        Touch touch = Touch.activeTouches[0];

        // if the touch just began, we load the game scene
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            SceneManager.LoadScene("Game");
        }
        #endif
    }
}
