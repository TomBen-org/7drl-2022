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

    public bool playerFinishedMoving;
    public Phase currentPhase;
    public int currentTurn;

    private MovementRaycaster _caster;
    private WallJumper _wallJumper;
    private RoomManager _roomManager;
    private AbilityPlanner _planner;

    private void Awake() {
        _caster = GetComponent<MovementRaycaster>();
        _wallJumper = GetComponent<WallJumper>();
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
                playerFinishedMoving = false;
                _caster.DrawMoveArcTriangle();
                _roomManager.StartPhaseUpdate();
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
                if (CheckTurnEnd()) {
                    NextPhase();
                }
                break;
            case Phase.Enemies:
                _wallJumper.SetFacing();
                NextPhase();
                break;
            case Phase.End:
                _roomManager.EndPhaseUpdate();
                EndTurn();
                break;
            default:
                break;
        }
        
    }

    private bool CheckTurnEnd() {
        return playerFinishedMoving && FindObjectsOfType<Projectile>().Length == 0;
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
                _wallJumper.StartTweenMove(_caster.GetNextPosition());
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
        _caster.SetMoveArcState(true);
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
