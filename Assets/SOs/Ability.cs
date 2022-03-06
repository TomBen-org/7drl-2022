using UnityEngine;

[CreateAssetMenu(fileName = "new Ability", menuName = "Gameplay/Ability", order = 1)]
public class Ability : ScriptableObject
{
    public GameObject triggerPrefabName;
    public bool needsTarget = true;
}
