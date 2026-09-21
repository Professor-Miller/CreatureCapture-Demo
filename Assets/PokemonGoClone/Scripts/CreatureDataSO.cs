using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Creature")]
public class CreatureDataSO : ScriptableObject
{
    public string CreatureID;
    public GameObject ModelPrefab;
}
