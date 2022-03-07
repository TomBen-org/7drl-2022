using UnityEngine;

public class KillProjectile: Projectile {
    public GameObject hitExplosion;
    protected override void ApplyEffect(Body target) {
        target.Kill();
        if (hitExplosion) {
            Instantiate(hitExplosion, transform.position, Quaternion.identity);
        }
    }
}
