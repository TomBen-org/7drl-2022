using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDirector : MonoBehaviour
{
    public enum Phase {
        Start,
        MoveSelect,
        ActionSelect,
        Moving,
        End
    }
    
    public Phase currentPhase;
    public int currentTurn;

    private MovementRaycaster _caster;

    private void Awake() {
        _caster = GetComponent<MovementRaycaster>();
    }

    private void Update() {
        switch (currentPhase) {
            case Phase.Start:
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
                Debug.Log("Moving Player");
                transform.position = _caster.GetNextPosition();
                transform.LookAt(_caster.GetNextFacing() + (Vector2) transform.position);
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
                break;
            case Phase.Moving:
                transform.position = _caster.GetNextPosition();
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
