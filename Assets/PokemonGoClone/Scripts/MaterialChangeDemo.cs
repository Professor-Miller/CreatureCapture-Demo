using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MaterialChangeDemo : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private Material[] materials;

    private int _currentMaterialIndex = 0;

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
    #if UNITY_EDITOR
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            CheckTouch(mousePosition);
        }
    #else
        if (Touch.activeTouches.Count == 0) return;

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            CheckTouch(touch.screenPosition);
        }
    #endif
    }

    public void CheckTouch(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Renderer objectRenderer))
            {
                _currentMaterialIndex = (_currentMaterialIndex + 1) % materials.Length;
                objectRenderer.material = materials[_currentMaterialIndex];
            }
        }
    }

}
