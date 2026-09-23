using System;
using UnityEngine;
using TMPro;

public class CreatureHandler : MonoBehaviour
{
    public CreatureDataSO CreatureData { get; private set; }
    public static event Action OnCreatureHatched;

    [SerializeField] private Canvas displayCanvas;
    [SerializeField] private TextMeshProUGUI displayName;

    private GameObject _currentModel;


    // Initializes the creature handler with the given creature data.
    public void Initialize(CreatureDataSO data)
    {
        SetCreature(data);
        if (Camera.main == null) return;

        displayCanvas.worldCamera = Camera.main;
    }

    public void Hatch(CreatureDataSO creature)
    {
        // Only eggs can hatch.
        if (CreatureData.CreatureID != "Egg") return;

        Debug.Log($"Hatched into {creature.CreatureID}!");

        SetCreature(creature);
        OnCreatureHatched?.Invoke();
    }

    // Sets the creature to the given data, destroying the previous model if necessary.
    private void SetCreature(CreatureDataSO data)
    {
        CreatureData = data;

        // Remove the previous model.
        if (_currentModel != null)
        {
            Destroy(_currentModel);
        }

        // Create the new model as a child of this encounter.
        _currentModel = Instantiate(data.ModelPrefab, transform);

        _currentModel.transform.localPosition = Vector3.zero;
        _currentModel.transform.localRotation = Quaternion.Euler(0, 180, 0);

        _currentModel.name = data.CreatureID;
        displayName.text = data.CreatureID;
    }

    public void Capture()
    {
        gameObject.SetActive(false);
    }


    public void ResetCreature(CreatureDataSO creature)
    {
        gameObject.SetActive(true);

        SetCreature(creature);
    }
}
