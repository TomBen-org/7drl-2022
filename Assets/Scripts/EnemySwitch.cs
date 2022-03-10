using System;
using UnityEngine;

public class EnemySwitch : Enemy {
    public GameObject[] triggerTargets;
    
    public override void EndUpdate() {
        if (triggerTargets.Length>0) {
            foreach (var triggerTarget in triggerTargets) {
                triggerTarget.GetComponent<ButtonTarget>().InteractWith(_body.isStunned);    
            }
        }
    }

    public override void Reset() {
        base.Reset();
        if (triggerTargets.Length>0) {
            foreach (var triggerTarget in triggerTargets) {
                triggerTarget.GetComponent<ButtonTarget>().InteractWith(_body.isStunned);    
            }
        }
    }
}
