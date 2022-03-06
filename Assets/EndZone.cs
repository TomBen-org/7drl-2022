using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour {
    private Room _room;

    void Awake() {
        Grid grid = FindObjectOfType<Grid>();
        Vector3Int cell = grid.WorldToCell(transform.position);
        Vector2 newPos = grid.GetCellCenterWorld(cell);
        transform.position = newPos;
    }
    
    public void Setup(Room room) {
        _room = room;
    }
    
    public void OnTriggerEnter2D(Collider2D col) {
        if (col == _room.playerCollider) {
            _room.playerInExitZone = true;
        }
    }
}
