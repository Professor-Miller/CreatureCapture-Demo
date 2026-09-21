using UnityEngine;

public class TouchRaycastHandler : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private CreatureDataSO[] _creatureHatchingOptions;

    void OnEnable() => TouchInputHandler.OnTouchedScreen += CheckTouch;
    void OnDisable() => TouchInputHandler.OnTouchedScreen -= CheckTouch;

    // Checks if the touch position hits an egg and hatches a random creature if it does.
    private void CheckTouch(Vector2 screenPosition)
    {
        // Creates a ray from the camera's position to the touch position.
        var ray = arCamera.ScreenPointToRay(screenPosition);

        // Checks if the ray hits an object and gets the hit information.
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        // Gets the creature handler component from the hit object.
        var creatureHandler = hit.collider.GetComponent<CreatureHandler>();

        // If there is no creature handler, return early.
        if (creatureHandler == null) return;

        // If the creature is not an egg, return early.
        if (creatureHandler.CreatureData.CreatureID != "Egg") return;

        // Gets a random creature from the available hatching options.
        var creature = GetRandomCreature();

        // Hatches the creature using the creature handler.
        creatureHandler.Hatch(creature);
    }

    // Returns a random creature from the available hatching options.
    private CreatureDataSO GetRandomCreature()
    {
        // Generates a random index to select a creature from the hatching options.
        int randomIndex = Random.Range(0, _creatureHatchingOptions.Length);

        // Returns the creature at the random index.
        return _creatureHatchingOptions[randomIndex];
    }
}
