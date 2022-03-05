using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour {
    private Room _room;

    public void Setup(Room room) {
        _room = room;
    }
    
    public void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("entering exit zone collider");
        if (col == _room.playerCollider) {
            _room.playerInExitZone = true;
        }
    }
}
