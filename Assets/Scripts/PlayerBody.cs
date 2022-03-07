using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody:Body {
    public override void Kill() {
        if (deathExplosion != null) {
            Transform explosionLocation = transform.Find("ExplosionLocation").transform;
            Instantiate(deathExplosion, explosionLocation.position, Quaternion.identity);
        }
        TurnDirector director = FindObjectOfType<TurnDirector>();
        director.SetPlayerDeadState(true);
        
    }
}
