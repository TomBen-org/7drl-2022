using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Room : MonoBehaviour {
    private CinemachineVirtualCamera _vcam;
    private EndZone _endZone;
    public Collider2D playerCollider;
    private Transform _spawnPoint;
    private RoomManager _manager;

    public bool playerInExitZone;

    void Awake() {
        _spawnPoint = transform.Find("SpawnPoint");
        _vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        _endZone = GetComponentInChildren<EndZone>();
        
        Grid grid = FindObjectOfType<Grid>();
        Vector3Int cell = grid.WorldToCell(_spawnPoint.transform.position);
        Vector2 newPos = grid.GetCellCenterWorld(cell);
        _spawnPoint.transform.position = newPos;
    }

    public void Init(RoomManager manager) {
        playerInExitZone = false;
        _manager = manager;
        _manager.player.GetComponent<WallPosition>().InstantMove(_spawnPoint.position);
        playerCollider = _manager.player.GetComponent<Collider2D>();
        _endZone.Setup(this);
    }
    
    public void SetCameraOff() {
        _vcam.Priority = 0;
    }

    public void SetCameraOn() {
        _vcam.Priority = 10;
    }

    
}
