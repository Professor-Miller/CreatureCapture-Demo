using UnityEngine;

public class TouchRaycastHandler : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private CreatureDataSO[] creatureHatchingOptions;

    void OnEnable() => TouchInputHandler.OnTouchBegan += CheckTouch;
    void OnDisable() => TouchInputHandler.OnTouchBegan -= CheckTouch;

    // Runs when the user touches the screen.
    private void CheckTouch(Vector2 screenPosition)
    {
        // Casts a ray from the camera to the touch position.
        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        // If the ray doesn't hit anything, return early.
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        // Gets the creature handler from the hit collider.
        CreatureHandler creatureHandler = hit.collider.GetComponent<CreatureHandler>();

        // If the creature handler is null, return early.
        if (creatureHandler == null) return;

        // If the creature is not an egg, return early.
        if (creatureHandler.CreatureData.CreatureID != "Egg") return;

        // Gets a random creature from the hatching options.
        CreatureDataSO creature = GetRandomCreature();

        // Hatches the creature.
        creatureHandler.Hatch(creature);
    }

    private CreatureDataSO GetRandomCreature()
    {
        // Gets a random index from the hatching options.
        int randomIndex = Random.Range(0, creatureHatchingOptions.Length);

        // Returns the creature at the random index.
        return creatureHatchingOptions[randomIndex];
    }
}
