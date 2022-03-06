using UnityEngine;

public class StunGunTrigger: AbilityTrigger {
    public GameObject bulletPrefab;
    protected override void TriggerMe() {
        Instantiate(bulletPrefab);
        
    }
}
