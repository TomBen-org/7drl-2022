using UnityEngine;

public class EnemySwitch : Enemy {
    public GameObject triggerTarget;
    public override void EndUpdate() {
        triggerTarget.GetComponent<ButtonTarget>().InteractWith(!_body.isStunned);
    }
}
