using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody:Body {
    public override void Kill() {
        if (deathExplosion != null) {
            AudioManager.Instance.PlayAudio(AudioManager.GameSfx.canonHit);
            Transform explosionLocation = transform.Find("ExplosionLocation").transform;
            Instantiate(deathExplosion, explosionLocation.position, Quaternion.identity);
        }
        TurnDirector director = FindObjectOfType<TurnDirector>();
        director.SetPlayerDeadState(true);
        
    }
}
