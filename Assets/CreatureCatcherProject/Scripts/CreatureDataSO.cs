using UnityEngine;

[CreateAssetMenu(menuName = "CreatureDataSO/Creature")]
public class CreatureDataSO : ScriptableObject
{
    public string CreatureID;
    public GameObject ModelPrefab;
}
