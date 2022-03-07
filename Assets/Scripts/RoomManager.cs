using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public GameObject player;
    
    private Room[] _rooms;
    public Room currentRoom;
    private int _currentRoomIndex;
    private AbilityPlanner _planner;

    void Awake() {
        _rooms = GetComponentsInChildren<Room>();
        _planner = FindObjectOfType<AbilityPlanner>();
    }

    public void Setup() {
        SwapToRoom(0);
    }

    private void SwapToRoom(int index) {
        ActivateRoomCamera(index);
        currentRoom = _rooms[index];
        _currentRoomIndex = index;
        _planner.SetAbilities(currentRoom.abilities);
        currentRoom.Init(this);
        
    }

    public void ActivateRoomCamera(int index) {
        foreach (Room room in _rooms) {
            room.SetCameraOff();
        }
        
        _rooms[index].SetCameraOn();
    }

    public void CheckForRoomChange() {
        if (currentRoom.playerInExitZone) {
            int nextRoom = _currentRoomIndex + 1;
            if (nextRoom >= _rooms.Length) {
                nextRoom = 0;
            }
            SwapToRoom(nextRoom);
        }
    }
    
    public void StartPhaseUpdate() {
        foreach (var enemy in currentRoom.enemies) {
            enemy.StartUpdate();
        }
    }

    public void EndPhaseUpdate() {
        foreach (var enemy in currentRoom.enemies) {
            enemy.EndUpdate();
        }
    }

    public void UpdateEnemyVision() {
        foreach (var enemy in currentRoom.enemies) {
            enemy.UpdateVision();
        }
    }
}