using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Body _body;

    public virtual void Awake() {
        _body = GetComponent<Body>();
    }

    public virtual void StartUpdate() {
        _body.UpdateStates();
    }

    public virtual void Reset() {
        _body.Reset();
        gameObject.SetActive(true);
    }
    
    public virtual void ShootingPhase() {
        
    }

    public virtual void EndUpdate() {
        
    }

    public virtual void UpdateVision(Vector2 target) {
           
    }

    public virtual void SetIndicatorState(bool state) {
        
    }
}
