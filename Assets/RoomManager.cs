using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {
    public GameObject player;
    
    private Room[] _rooms;
    private Room _currentRoom;
    private int _currentRoomIndex;

    void Awake() {
        _rooms = GetComponentsInChildren<Room>();
    }

    public void Setup() {
        SwapToRoom(0);
    }

    private void SwapToRoom(int index) {
        ActivateRoomCamera(index);
        _currentRoom = _rooms[index];
        _currentRoomIndex = index;
        _currentRoom.Init(this);
    }

    public void ActivateRoomCamera(int index) {
        foreach (Room room in _rooms) {
            room.SetCameraOff();
        }
        
        _rooms[index].SetCameraOn();
    }

    public void CheckForRoomChange() {
        if (_currentRoom.playerInExitZone) {
            int nextRoom = _currentRoomIndex + 1;
            if (nextRoom >= _rooms.Length) {
                nextRoom = 0;
            }
            SwapToRoom(nextRoom);
        }
    }
}
