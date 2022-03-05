using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementRaycaster : MonoBehaviour {
    private const float _acceptanceClickRange = 0.25f;
    
    public Camera camera;

    public Vector2 targetPoint = Vector2.negativeInfinity;
    
    private LineRenderer _lineRenderer;
    private TurnDirector _director;

    private void Awake() {
        _lineRenderer = transform.Find("LineRenderer").GetComponent<LineRenderer>();
        _director = GetComponent<TurnDirector>();
    }

    public void PhaseUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsTargetPointValid()) {
            Debug.Log("Next phase accepted by spacebar");
            _director.NextPhase();
            return;
        }
        
        if (Input.GetMouseButton(0)) {
            var worldMousePos = camera.ScreenToWorldPoint(Input.mousePosition);

            //move on if the player clicks in the same place as an existing, valid location.
            if (IsTargetPointValid() && Vector2.Distance(targetPoint, worldMousePos) < -_acceptanceClickRange) {
                Debug.Log("Next phase accepted");
                _director.NextPhase();
                return;
            }

            //shoot a ray at the mouse
            Vector2 targetDirection = worldMousePos - transform.position;

            int mask = LayerMask.GetMask("MoveTarget", "Enemy");

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 5000f, mask);

            if (hit.collider) {
                _lineRenderer.SetPositions(new Vector3[] {transform.position, hit.point});
                if (hit.collider.transform.GetComponent<Enemy>() != null) {
                    _lineRenderer.endColor = Color.red;
                    targetPoint = Vector2.negativeInfinity;
                }
                else {
                    _lineRenderer.endColor = Color.green;
                    targetPoint = hit.point;
                }
            }
            else {
                targetPoint = Vector2.negativeInfinity;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            //set the end turn button visible based on validity.
            if (IsTargetPointValid()) {
                Debug.Log("valid position selected");
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            ResetTargetPoint();
        }
    

    }
    
    public bool IsTargetPointValid() {
        return targetPoint != Vector2.negativeInfinity;
    }

    public void ResetTargetPoint() {
        Debug.Log("reset line");
        targetPoint = Vector2.negativeInfinity;
        _lineRenderer.SetPositions(new Vector3[]{transform.position,transform.position});
    }

    public Vector2 GetNextPosition() {
        Vector2 nearPoint =  targetPoint - (Vector2) transform.position;
        nearPoint *= 0.99f;
        return IsTargetPointValid() ? (nearPoint + (Vector2) transform.position) : transform.position;
    }
}
