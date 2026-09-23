using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arImageManager;

    [SerializeField] private GameObject encounterPrefab;
    [SerializeField] private GameObject environmentPrefab;
    [SerializeField] private CreatureDataSO startingCreature;

    private GameObject _currentEncounter;
    private GameObject _currentEnvironment;

    void OnEnable() => arImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    void OnDisable() => arImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);

    // Called when tracked images are added or removed.
    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            SpawnEncounter(image);
        }
    }

    // Spawns an encounter at the given tracked image's position.
    private void SpawnEncounter(ARTrackedImage trackedImage)
    {
        // Prevent multiple encounters from being spawned.
        if (_currentEncounter != null) return;

        // Instantiate the encounter and environment prefabs.
        _currentEncounter = Instantiate(encounterPrefab, trackedImage.transform);
        _currentEnvironment = Instantiate(environmentPrefab, trackedImage.transform);

        // Position the encounter and environment prefabs at the tracked image's position.
        var creatureOffset = new Vector3(0, 0.05f, 0);
        _currentEncounter.transform.localPosition = Vector3.zero + creatureOffset;
        _currentEncounter.transform.localRotation = Quaternion.identity;

        _currentEnvironment.transform.localPosition = Vector3.zero;
        _currentEnvironment.transform.localRotation = Quaternion.identity;

        // Get the creature handler component and initialize it with the starting creature.
        CreatureHandler creatureHandler = _currentEncounter.GetComponent<CreatureHandler>();
        creatureHandler.Initialize(startingCreature);
    }


    // Resets the current encounter to the starting creature (the egg).
    public void ResetEncounter()
    {
        if (_currentEncounter == null) return;

        CreatureHandler creatureHandler = _currentEncounter.GetComponent<CreatureHandler>();
        creatureHandler.ResetCreature(startingCreature);
    }
}
