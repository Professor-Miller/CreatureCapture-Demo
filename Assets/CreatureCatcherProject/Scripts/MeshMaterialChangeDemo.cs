using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;


using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MeshMaterialChangeDemo : MonoBehaviour
{
    [SerializeField] private Camera arCamera; // The camera to use for raycasting.
    [SerializeField] private Material[] materials; // The array of materials to cycle through.

    private int _currentMaterialIndex = 0; // The index of the current material in the materials array.

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
    #if UNITY_EDITOR
        // In the Unity Editor, check for mouse input.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            CheckTouch(mousePosition);
        }
    #else
        // On the actual device, check for touch input.
        if (Touch.activeTouches.Count == 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            CheckTouch(touch.screenPosition);
        }
    #endif
    }

    // Checks if the touch position is on a mesh and changes its material if so.
    public void CheckTouch(Vector2 screenPosition)
    {
        // Casts a ray from the camera to the touch position and checks if it hits a mesh.
        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        // If the ray hits a mesh, changes its material to the next material in the array.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the hit collider has a Renderer component, changes its material to the next material in the array.
            if (hit.collider.TryGetComponent(out Renderer objectRenderer))
            {
                // Increments the current material index and wraps it around the materials array.
                _currentMaterialIndex = (_currentMaterialIndex + 1) % materials.Length;
                // Sets the material of the hit object to the next material in the array.
                objectRenderer.material = materials[_currentMaterialIndex];
            }
        }
    }
}
