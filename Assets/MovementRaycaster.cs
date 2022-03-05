using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRaycaster : MonoBehaviour
{
    public enum TurnMode {
        Start,
        MoveSelect,
        ActionSelect,
        Moving,
        End
    }

    public TurnMode currentMode;
    public Camera camera;
    
    private LineRenderer _lineRenderer;

    private void Awake() {
        _lineRenderer = transform.Find("LineRenderer").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMode == TurnMode.MoveSelect && Input.GetMouseButton(0)) {
            //shoot a ray at the mouse
            Vector2 targetDirection = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            int mask = LayerMask.GetMask("MoveTarget", "Enemy");
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection,5000f, mask);

            if (hit.collider) {
                _lineRenderer.SetPositions(new Vector3[]{transform.position,hit.point});
                if (hit.collider.transform.GetComponent<Enemy>() != null) {
                    _lineRenderer.endColor = Color.red;
                }
                else {
                    _lineRenderer.endColor = Color.green;
                }
            }
            
        }
    }
}
