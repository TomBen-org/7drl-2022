using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile: MonoBehaviour {

    public TurnDirector.Phase updatePhase;
    public string targetLayerName;
    private TurnDirector _director;
    private void Awake() {
        _director = FindObjectOfType<TurnDirector>();
    }

    private void Start() {
        
    }

    private void Update() {
        if (_director.currentPhase == updatePhase) {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == LayerMask.GetMask("MoveTarget")) {
            Destroy(gameObject);
        } else if (col.gameObject.layer == LayerMask.GetMask(targetLayerName)) {
            Body victim = col.transform.GetComponent<Body>();
            if (victim != null) {
                ApplyEffect(victim);
            }
        }
    }

    protected virtual void ApplyEffect(Body target) {
        Debug.Log(target.gameObject.name + " hit with generic projectile");
    }
}
