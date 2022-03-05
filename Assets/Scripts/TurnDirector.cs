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
        Moving,
        Enemies,
        End
    }
    
    public Phase currentPhase;
    public int currentTurn;

    private MovementRaycaster _caster;
    private WallPosition _wallPosition;

    private void Awake() {
        _caster = GetComponent<MovementRaycaster>();
        _wallPosition = GetComponent<WallPosition>();
    }

    private void Start() {
        DOTween.Init();
    }
    
    private void Update() {
        switch (currentPhase) {
            case Phase.Start:
                _caster.DrawMoveArcTriangle();
                Debug.Log("Beginning MoveSelect Phase");
                NextPhase();
                break;
            case Phase.MoveSelect:
                _caster.PhaseUpdate();
                break;
            case Phase.ActionSelect:
                Debug.Log("Skipping ActionSelect Phase");
                NextPhase();
                break;
            case Phase.Moving:
                //Debug.Log("Moving Player");
                _wallPosition.PhaseUpdate();
                break;
            case Phase.Enemies:
                _wallPosition.SetFacing();
                NextPhase();
                break;
            case Phase.End:
                Debug.Log("Starting a new Turn");
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
                break;
            case Phase.MoveSelect:
                currentPhase = Phase.ActionSelect;
                break;
            case Phase.ActionSelect:
                currentPhase = Phase.Moving;
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
    
    private void EndTurn() {
        //refresh player abilities
        _caster.ResetTargetPoint();
        currentTurn++;
        currentPhase = Phase.Start;
    }
}
