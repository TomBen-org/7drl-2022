using UnityEngine;

public class StunGunTrigger: AbilityTrigger {
    public GameObject bulletPrefab;
    protected override void TriggerMe() {
        GameObject bullet = Instantiate(bulletPrefab);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Setup(_setting);
    }
}
