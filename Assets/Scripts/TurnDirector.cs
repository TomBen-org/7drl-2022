using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurnDirector : MonoBehaviour
{
    public enum Phase {
        Start,
        MoveSelect,
        ActionSelect,
        MoveBegin,
        Moving,
        Enemies,
        End
    }
    
    public Phase currentPhase;
    public int currentTurn;

    private MovementRaycaster _caster;
    private WallPosition _wallPosition;
    private RoomManager _roomManager;
    private AbilityPlanner _planner;

    private void Awake() {
        _caster = GetComponent<MovementRaycaster>();
        _wallPosition = GetComponent<WallPosition>();
        _roomManager = FindObjectOfType<RoomManager>();
        _planner = FindObjectOfType<AbilityPlanner>();
    }

    private void Start() {
        DOTween.Init();
        _roomManager.Setup();
    }
    
    private void Update() {
        switch (currentPhase) {
            case Phase.Start:
                _caster.DrawMoveArcTriangle();
                NextPhase();
                break;
            case Phase.MoveSelect:
                _caster.PhaseUpdate();
                break;
            case Phase.ActionSelect:
                _planner.PhaseUpdate();
                break;
            case Phase.MoveBegin:
                NextPhase();
                break;
            case Phase.Moving:
                break;
            case Phase.Enemies:
                _wallPosition.SetFacing();
                NextPhase();
                break;
            case Phase.End:
                EndTurn();
                break;
            default:
                break;
        }
        
    }

    public void NextPhase() {
        switch (currentPhase) {
            case Phase.Start:
                currentPhase = Phase.MoveSelect;
                _caster.SetMoveArcState(true);
                _caster.SetMoveLineState(true);
                break;
            case Phase.MoveSelect:
                _caster.SetMoveArcState(false);
                currentPhase = Phase.ActionSelect;
                break;
            case Phase.ActionSelect:
                currentPhase = Phase.MoveBegin;
                break;
            case Phase.MoveBegin:
                currentPhase = Phase.Moving;
                _caster.SetMoveLineState(false);
                _wallPosition.StartTweenMove(_caster.GetNextPosition());
                break;
            case Phase.Moving:
                currentPhase = Phase.Enemies;
                break;
            case Phase.Enemies:
                currentPhase = Phase.End;
                break;
            case Phase.End:
                EndTurn();
                break;
            default:
                break;
        }
    }

    public void CancelPlanningPhase() {
        currentPhase = Phase.MoveSelect;
    }
    
    private void EndTurn() {
        //refresh player abilities
        _roomManager.CheckForRoomChange();
        _caster.ResetTargetPoint();
        currentTurn++;
        currentPhase = Phase.Start;
        
    }
}
