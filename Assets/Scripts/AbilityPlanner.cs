using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class AbilityPlanner : MonoBehaviour {
    private List<AbilitySetting> _plannedAbilities;

    public float acceptanceClickRange = 0.5f;

    public List<Ability> abilities;  
    
    public enum InnerPhase {
        Start,
        Selection,
        Placement,
        Targeting,
    }

    public GameObject placementIndicatorPrefab;
    private GameObject _currentPlacementIndicator;
    private Vector2 _currentTargetPoint;
    private bool _targetRegistered;
    
    public InnerPhase currentPhase;
    private int _selectedAbility;

    private TurnDirector _director;
    private MovementRaycaster _caster;

    void Awake() {
        _director = FindObjectOfType<TurnDirector>();
        _caster = FindObjectOfType<MovementRaycaster>();
        _plannedAbilities = new List<AbilitySetting>();
    }

    public void BeginPlanningPhase() {
        UIView.Show("Game","ActionBar");
        currentPhase = InnerPhase.Selection;
    }

    public void CancelPlanningPhase() {
        UIView.Hide("Game","ActionBar");
        DeleteAllPlannedAbilities();
        _director.CancelPlanningPhase();
        currentPhase = InnerPhase.Start;
    }

    public void CompletePlanningPhase() {
        UIView.Hide("Game","ActionBar");
        currentPhase = InnerPhase.Start;
        _director.NextPhase();
    }
    
    public void PhaseUpdate()
    {
        switch (currentPhase) {
            case InnerPhase.Start:
                BeginPlanningPhase();
                
                break;
            case InnerPhase.Selection:
                if (Input.GetMouseButtonUp(1)) {
                    if (_plannedAbilities.Count > 0) {
                        DeleteLastPlannedAbility();
                    }
                    else {
                        CancelPlanningPhase();    
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    CheckAcceptedPlanning();
                }
                break;
            case InnerPhase.Placement:
                if (Input.GetMouseButtonUp(1)) {
                    BeginSelectionPhase();
                }
                PlaceIndicatorOnLine();
                if (Input.GetMouseButtonDown(0)) {
                    BeginTargetingPhase();
                }
                break;
            case InnerPhase.Targeting:
                if (abilities[_selectedAbility].needsTarget == false) {
                    AcceptAbilityPlacement();
                    return;
                }
                TargetingPhaseUpdate();
                break;
        }
    }

    private void DeleteAllPlannedAbilities() {
        foreach (var setting in _plannedAbilities) {
            Destroy(setting.indicator);
        }
        _plannedAbilities.Clear();
    }
    
    private void DeleteLastPlannedAbility() {
        AbilitySetting popped = _plannedAbilities[^1];
        Destroy(popped.indicator);
        _plannedAbilities.Remove(popped);
    }

    private void BeginTargetingPhase() {
        currentPhase = InnerPhase.Targeting;
    }
    
    private void BeginSelectionPhase() {
        currentPhase = InnerPhase.Selection;
        _selectedAbility = -1;
        DestroyIndicator();
    }

    private void BeginPlacementRound() {
        currentPhase = InnerPhase.Placement;
        _currentPlacementIndicator = Instantiate(placementIndicatorPrefab);
        _currentPlacementIndicator.transform.position = _caster.GetClosestPointOnMoveLine();
    }

    private void PlaceIndicatorOnLine() {
        if (_currentPlacementIndicator != null) {
            _currentPlacementIndicator.transform.position = _caster.GetClosestPointOnMoveLine();
        }
    }

    private void DestroyIndicator() {
        if (_currentPlacementIndicator != null) {
            Destroy(_currentPlacementIndicator);
            _currentPlacementIndicator = null;
        }
    }

    //Selected via unity Event in the "Game/ActionBar" UIView
    public void SelectAbility(int abilityIndex) {
        if (currentPhase == InnerPhase.Selection) {
            _selectedAbility = abilityIndex;
            BeginPlacementRound();
        }
    }

    public void TargetingPhaseUpdate()
    {
        Vector2 worldMousePos = _caster.myCamera.ScreenToWorldPoint(Input.mousePosition);
        LineRenderer _lineRenderer = _currentPlacementIndicator.GetComponentInChildren<LineRenderer>();
        Vector2 abilityPos = _currentPlacementIndicator.transform.position;
        
        if (Input.GetMouseButtonDown(0) && _currentTargetPoint != Vector2.negativeInfinity) {
            //move on if the player clicks in the same place as an existing, valid location.
            if (_targetRegistered) {
                AcceptAbilityPlacement();
            }
        }
        
        //shoot a ray at the mouse
        Vector2 targetDirection = worldMousePos - abilityPos;

        int mask = LayerMask.GetMask("MoveTarget");

        RaycastHit2D hit = Physics2D.Raycast(abilityPos, targetDirection, 5000f, mask);

        if (hit.collider) {
            _lineRenderer.SetPositions(new Vector3[] {abilityPos, hit.point});
            _currentTargetPoint = hit.point;
            _targetRegistered = true;
        }
        else {
            _currentTargetPoint = Vector2.negativeInfinity;
            _targetRegistered = false;
        }
        

        if (Input.GetMouseButtonUp(1)) {
            ResetTargetPoint();
            BeginPlacementRound();
        }
    }
    
    private void ResetTargetPoint() {
        _currentTargetPoint = Vector2.negativeInfinity;
        _targetRegistered = false;
        _currentPlacementIndicator.GetComponentInChildren<LineRenderer>().SetPositions(new Vector3[]{transform.position,transform.position});
    }

    private void AcceptAbilityPlacement() {
        AbilitySetting newSetting = new AbilitySetting(
            _caster.GetPercentageOfLine(transform.position, _caster.targetPoint, _currentPlacementIndicator.transform.position),
            _selectedAbility,
                _currentTargetPoint,
            _currentPlacementIndicator
            );
        _plannedAbilities.Add(newSetting);
        _currentPlacementIndicator = null;
        _selectedAbility = -1;
        currentPhase = InnerPhase.Selection;
    }

    private void CheckAcceptedPlanning() {
        Vector2 worldMousePos = _caster.myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(_caster.targetPoint, worldMousePos) < acceptanceClickRange) {
            BuildAbilityObjects();
            CompletePlanningPhase();
        }
    }

    private void BuildAbilityObjects() {
        foreach (AbilitySetting setting in _plannedAbilities) {
            //build a object for each
            GameObject trigger = Instantiate(abilities[setting.abilityIndex].triggerPrefabName);
            Vector2 pos = setting.indicator.transform.position;
            trigger.transform.position = pos;
            AbilityTrigger at = trigger.GetComponent<AbilityTrigger>();
            at.Setup(setting);
        }
        
        DeleteAllPlannedAbilities();
    }
}


public class AbilitySetting {
    public float position;
    public int abilityIndex;
    public Vector2 target;
    public GameObject indicator;

    public AbilitySetting(float position, int abilityIndex, Vector2 target, GameObject indicator) {
        this.position = position;
        this.abilityIndex = abilityIndex;
        this.target = target;
        this.indicator = indicator;
    }
}