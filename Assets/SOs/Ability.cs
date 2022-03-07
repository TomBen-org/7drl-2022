using UnityEngine;

[CreateAssetMenu(fileName = "new Ability", menuName = "Gameplay/Ability", order = 1)]
public class Ability : ScriptableObject
{
    public GameObject triggerPrefabName;
    public bool needsTarget = true;
    public string buttonId;
    public int maxPlacements = 1;
    public float perTurnRecharge = 1;
}
