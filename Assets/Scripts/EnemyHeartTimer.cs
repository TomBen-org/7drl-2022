
using TMPro;
using UnityEngine;

public class EnemyHeartTimer : Enemy {
    public int fullTurns = 3;
    public GameObject explosionPrefab;
    
    private int _turnsRemaining;

    private TextMeshPro _text;
    
    public override void Awake() {
        base.Awake();
        _text = transform.Find("Text").GetComponent<TextMeshPro>();
        Reset();
    }

    public override void Reset() {
        base.Reset();
        _turnsRemaining = fullTurns;
        _text.text = _turnsRemaining.ToString();   
    }

    public override void EndUpdate() {
        if (_turnsRemaining == 0) {
            if (explosionPrefab != null) {
                Transform explosionLocation = transform.Find("ExplosionLocation").transform;
                Instantiate(explosionPrefab, explosionLocation.position, Quaternion.identity);
            }
            FindObjectOfType<TurnDirector>().SetPlayerDeadState(true);
            _body.Kill();
        }
        else {
            _turnsRemaining--;
            _text.text = _turnsRemaining.ToString();    
        }
    }
}
