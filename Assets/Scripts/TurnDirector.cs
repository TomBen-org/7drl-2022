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
        Enemy,
        EnemyShooting,
        End
    }

    public bool playerFinishedMoving;
    public Phase currentPhase;
    public int currentTurn;
    public bool playerIsDead;
    
    private MovementRaycaster _caster;
    private WallJumper _wallJumper;
    private RoomManager _roomManager;
    private AbilityPlanner _planner;
    private Transform _clickHelper;

    private void Awake() {
        _caster = GetComponent<MovementRaycaster>();
        _wallJumper = GetComponent<WallJumper>();
        _roomManager = FindObjectOfType<RoomManager>();
        _planner = FindObjectOfType<AbilityPlanner>();
        _clickHelper = FindObjectOfType<ClickHelper>().transform;
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
                if (_planner.abilities.Count == 0) {
                    NextPhase();
                    return;
                }
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
            case Phase.Enemy:
                _roomManager.EnemiesGenerateProjectiles();
                NextPhase();
                break;
            case Phase.EnemyShooting:
                if (CheckShootingOver()) {
                    NextPhase();
                }
                break;
            case Phase.End:
                _roomManager.EndPhaseUpdate();
                EndTurn();
                break;
            default:
                break;
        }
        
    }

    private bool CheckShootingOver() {
        return FindObjectsOfType<Projectile>().Length == 0;
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
                _roomManager.SetEnemyIndicatorState(true);
                break;
            case Phase.MoveSelect:
                _caster.SetMoveArcState(false);
                currentPhase = Phase.ActionSelect;
                break;
            case Phase.ActionSelect:
                _clickHelper.position = new Vector2(1000f, 1000f);
                currentPhase = Phase.MoveBegin;
                break;
            case Phase.MoveBegin:
                currentPhase = Phase.Moving;
                _caster.SetMoveLineState(false);
                _wallJumper.StartTweenMove(_caster.GetNextPosition());
                _roomManager.SetEnemyIndicatorState(false);
                break;
            case Phase.Moving:
                if (_roomManager.currentRoom.playerInExitZone) {
                    currentPhase = Phase.End;
                    return;
                }
                currentPhase = Phase.Enemy;
                break;
            case Phase.Enemy:
                currentPhase = Phase.EnemyShooting;
                break;
            case Phase.EnemyShooting:
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
        if (playerIsDead) {
            _roomManager.currentRoom.ResetRoom();
            SetPlayerDeadState(false);
        }
        else {
            _roomManager.CheckForRoomChange();
        }
        _caster.ResetTargetPoint();
        currentTurn++;
        currentPhase = Phase.Start;
    }

    public void SetPlayerDeadState(bool state) {
        playerIsDead = state;
        transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = !state;
        _clickHelper.position = new Vector2(1000f, 1000f);
        if (state) {
            _roomManager.SetEnemyIndicatorState(false);
        }
    }

}
