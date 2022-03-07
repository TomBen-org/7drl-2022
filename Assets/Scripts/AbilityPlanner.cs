using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class AbilityPlanner : MonoBehaviour {
    private List<AbilitySetting> _plannedAbilities;

    public float acceptanceClickRange = 0.5f;

    public List<Ability> abilities;
    public List<float> abilityEnergy;
    
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
        abilityEnergy = new List<float>();
    }

    public void BeginPlanningPhase() {
        UIView.Show("Game","ActionBar");
        RechargeAbilities();
        UpdateButtonSelectability();
        currentPhase = InnerPhase.Selection;
    }

    public void CancelPlanningPhase() {
        UIView.Hide("Game","ActionBar");
        DeleteAllPlannedAbilities(true);
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

    private void DeleteAllPlannedAbilities(bool refund) {
        foreach (var setting in _plannedAbilities) {
            if (refund) {
                abilityEnergy[setting.abilityIndex] += 1;    
            }
            Destroy(setting.indicator);
        }
        _plannedAbilities.Clear();
        
    }
    
    private void DeleteLastPlannedAbility() {
        AbilitySetting popped = _plannedAbilities[^1];
        Destroy(popped.indicator);
        abilityEnergy[popped.abilityIndex] += 1;
        _plannedAbilities.Remove(popped);
        UpdateButtonSelectability();
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
        if (!_currentPlacementIndicator) {
            _currentPlacementIndicator = Instantiate(placementIndicatorPrefab);
            //_currentPlacementIndicator.transform.position = _caster.GetClosestPointOnMoveLine();    
        }
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
    public void SelectAbility(string id) {
        if (currentPhase == InnerPhase.Selection) {
            for (int i = 0; i < abilities.Count; i++) {
                if (abilities[i].buttonId == id) {
                    _selectedAbility = i;
                    BeginPlacementRound();
                    return;
                }
            }
            Debug.LogError("Player clicked button for ability " + id + "but it was not available.");
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
        abilityEnergy[_selectedAbility]--;
        _currentPlacementIndicator = null;
        _selectedAbility = -1;
        UpdateButtonSelectability();
        currentPhase = InnerPhase.Selection;
    }

    private void RechargeAbilities() {
        for (int i = 0; i < abilities.Count; i++) {
            Ability ability = abilities[i];
            float energy = abilityEnergy[i] + ability.perTurnRecharge;
            abilityEnergy[i] = Mathf.Min(energy,ability.maxPlacements);
        }
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
        
        DeleteAllPlannedAbilities(false);
    }

    private void UpdateButtonSelectability() {
        for (int i = 0; i < abilities.Count; i++) {
            Ability ability = abilities[i];
            float energy = abilityEnergy[i];
            
            foreach (UIButton button in UIButton.GetButtons("Game",ability.buttonId)) {
                button.interactable = (energy >= 1);
            }
        }
    }

    public void SetAbilities(Ability[] newAbilities) {
        foreach (Ability ability in abilities) {
            foreach (UIButton button in UIButton.GetButtons("Game",ability.buttonId)) {
                button.gameObject.SetActive(false);
            }
        }
        
        abilities = newAbilities.ToList();
        abilityEnergy.Clear();
        
        
        foreach (Ability ability in abilities) {
            abilityEnergy.Add(ability.maxPlacements);
            foreach (UIButton button in UIButton.GetButtons("Game",ability.buttonId)) {
                button.gameObject.SetActive(true);
            }
        }
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