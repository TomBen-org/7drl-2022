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

    public virtual void EndUpdate() {
        
    }

    public virtual void UpdateVision() {
        
    }
}
