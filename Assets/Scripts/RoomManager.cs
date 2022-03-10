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
        AudioManager.Instance.PlayAudio(AudioManager.MusicType.gameplay);
    }

    public void SwapToRoom(int index) {
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

    public bool CheckForRoomChange() {
        if (currentRoom.playerInExitZone) {
            int nextRoom = _currentRoomIndex + 1;
            if (nextRoom >= _rooms.Length) {
                nextRoom = 0;
            }
            SwapToRoom(nextRoom);
            return true;
        }

        return false;
    }
    
    public void StartPhaseUpdate() {
        foreach (var enemy in currentRoom.enemies) {
            enemy.StartUpdate();
        }
    }

    public void EndPhaseUpdate() {
        foreach (var enemy in currentRoom.enemies) {
            if (enemy.gameObject.activeSelf) {
                enemy.EndUpdate();    
            }
        }
    }

    public void UpdateEnemyVision(Vector2 target) {
        foreach (var enemy in currentRoom.enemies) {
            enemy.UpdateVision(target);
        }
    }

    public void EnemiesGenerateProjectiles() {
        foreach (var enemy in currentRoom.enemies) {
            if (enemy.gameObject.activeSelf) {
                enemy.ShootingPhase();
            }
        }
    }

    public void SetEnemyIndicatorState(bool state) {
        foreach (var enemy in currentRoom.enemies) {
            enemy.SetIndicatorState(state);
        }
    }

}
