using UnityEngine;

public class CreatureHandler : MonoBehaviour
{
    public CreatureDataSO CreatureData { get; private set; }

    private GameObject _currentModel;

    // Initializes the creature handler with the given creature data.
    public void Initialize(CreatureDataSO data)
    {
        SetCreature(data);
    }

    public void Hatch(CreatureDataSO creature)
    {
        // Only eggs can hatch.
        if (CreatureData.CreatureID != "Egg") return;

        Debug.Log($"Hatched into {creature.CreatureID}!");

        SetCreature(creature);
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
        _currentModel.transform.localRotation = Quaternion.identity;

        _currentModel.name = data.CreatureID;
    }
}
