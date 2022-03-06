public class StunProjectile: Projectile {
    public int effectTurns = 2;
    protected override void ApplyEffect(Body target) {
        target.Stun(effectTurns);
    }
}
