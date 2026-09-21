using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arImageManager;

    [SerializeField] private GameObject encounterPrefab;
    [SerializeField] private CreatureDataSO startingCreature;

    void OnEnable() => arImageManager.trackablesChanged.AddListener(OnTrackedImageChanged);
    void OnDisable() => arImageManager.trackablesChanged.RemoveListener(OnTrackedImageChanged);


    // Cycle through the added trackables and spawn encounters.
    private void OnTrackedImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            SpawnEncounter(image);
        }
    }

    // Spawn an encounter at the position of the tracked image.
    private void SpawnEncounter(ARTrackedImage trackedImage)
    {
        var encounter = Instantiate(encounterPrefab, trackedImage.transform);

        encounter.transform.localPosition = Vector3.zero;
        encounter.transform.localRotation = Quaternion.identity;

        var creatureHandler = encounter.GetComponent<CreatureHandler>();

        creatureHandler.Initialize(startingCreature);
    }
}
