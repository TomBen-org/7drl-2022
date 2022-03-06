using UnityEngine;

public class EnemySwitch : Enemy {
    public GameObject triggerTarget;
    public override void EndUpdate() {
        Debug.Log(_body.isStunned);
        triggerTarget.GetComponent<ButtonTarget>().InteractWith(_body.isStunned);
    }
}
