using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Body _body;

    private void Awake() {
        _body = GetComponent<Body>();
    }

    public virtual void StartUpdate() {
        
    }

    public virtual void EndUpdate() {
        
    }

    public virtual void UpdateVision() {
        
    }
}
