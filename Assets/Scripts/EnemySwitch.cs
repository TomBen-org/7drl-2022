using System;
using UnityEngine;

public class EnemySwitch : Enemy {
    public GameObject triggerTarget;
    
    public override void EndUpdate() {
        if (triggerTarget) {
            triggerTarget.GetComponent<ButtonTarget>().InteractWith(_body.isStunned);
        }
    }
}
