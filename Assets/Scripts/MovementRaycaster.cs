using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementRaycaster : MonoBehaviour {
    private const float _acceptanceClickRange = 0.25f;
    
    public Camera myCamera;
    public float maxAimAngle = 30f;
    public Vector2 targetPoint = Vector2.negativeInfinity;

    private bool _registered;
    
    private LineRenderer _lineRenderer;
    private TurnDirector _director;
    private WallPosition _wallPosition;
    private MeshFilter _moveArcFilter;

    private void Awake() {
        _lineRenderer = transform.Find("LineRenderer").GetComponent<LineRenderer>();
        _moveArcFilter = FindObjectOfType<MoveArcIndicator>().GetComponent<MeshFilter>();
        _wallPosition = GetComponent<WallPosition>();
        _director = GetComponent<TurnDirector>();
    }

    public void PhaseUpdate()
    {
        var worldMousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetKeyDown(KeyCode.Space) && IsTargetPointValid()) {
            Debug.Log("Next phase accepted by spacebar");
            _director.NextPhase();
            return;
        }

        if (Input.GetMouseButtonDown(0) && Vector2.Distance(targetPoint, worldMousePos) > _acceptanceClickRange) {
            _registered = false;
        }
        
        if (Input.GetMouseButton(0)) {
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
            //move on if the player clicks in the same place as an existing, valid location.
            if (_registered) {
                if (IsTargetPointValid() && Vector2.Distance(targetPoint, worldMousePos) < _acceptanceClickRange) {
                    _director.NextPhase();
                }
            }
            else if (IsTargetPointValid()) {
                _registered = true;
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
    
    public bool IsWithinViewCone(Vector2 point) {
        Vector2 currentRotation = _wallPosition.currentFacing;
        Vector2 directionToPoint = point - (Vector2) transform.position;

        float angle = Mathf.Atan2(directionToPoint.x - currentRotation.x, currentRotation.y - directionToPoint.y);
        // if (angle < 0f) {
        //     angle += (Mathf.PI * 2);
        // }              
        angle *= Mathf.Rad2Deg;
        Debug.Log("Angle:" + angle.ToString());
        return Math.Abs(angle%360f) < maxAimAngle;
    }

    public void DrawMoveArcTriangle() {
        float depth = 50f;
        float currentRotation = _wallPosition.GetRotationAtCurrentAngle();
        float maxRot = currentRotation + maxAimAngle;
        float minRot = currentRotation - maxAimAngle;
        maxRot *= Mathf.Deg2Rad;
        minRot *= Mathf.Deg2Rad;
        Vector2 maxPoint1 = new Vector2(depth * Mathf.Sin(maxRot), depth * Mathf.Cos(maxRot));
        Vector2 maxPoint2 = new Vector2(depth * Mathf.Sin(minRot), depth * Mathf.Cos(minRot));

        Mesh triangle = new Mesh();
        triangle.vertices = new Vector3[] {
            transform.position,
            (Vector3) maxPoint2 + transform.position,
            (Vector3) maxPoint1 + transform.position,
            
        };
        triangle.uv = new Vector2[] {
            Vector3.zero,
            maxPoint2,
            maxPoint1,
        };
        triangle.triangles = new int[] {0, 1, 2};
        _moveArcFilter.mesh = triangle;
    }
}
