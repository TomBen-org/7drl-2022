using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallPosition : MonoBehaviour {
    public Vector2 currentFacing;
    
    private Dictionary<Vector2,float> facingSettings = new Dictionary<Vector2,float>() {
        {Vector2.left, 0},
        {Vector2.up, -90f},
        {Vector2.down, 90f},
        {Vector2.right, 180f}
    };

    public void Start() {
        Move(transform.position);
    }

    public void Move(Vector2 position) {
        Stick(position);
        SetFacing();
    }
    
    public void Stick(Vector2 position) {
        var vectorsToTry = new Dictionary<Vector2, float> {
            {Vector2.left, 999f},
            {Vector2.up, 999f},
            {Vector2.right, 999f},
            {Vector2.down, 999f}
        };
        Dictionary<Vector2, Vector2> options = new Dictionary<Vector2, Vector2>();

        int mask = LayerMask.GetMask("MoveTarget");

        foreach (Vector2 key in vectorsToTry.Keys.ToArray()) {
            RaycastHit2D hit = Physics2D.Raycast(position, key, 500f, mask);
            if (hit.collider) {
                float dist = Vector2.Distance(hit.point, position);
                vectorsToTry[key] = dist;
                options.Add(key,hit.point);
            }
        }
        
        Vector2[] directionsToSort = vectorsToTry.Keys.ToArray();
        Array.Sort(directionsToSort, (a,b) => {
            float aDist = vectorsToTry[a];
            float bDist = vectorsToTry[b];
            return aDist.CompareTo(bDist);
        });

        Vector2 targetPoint = options[directionsToSort[0]];
        
        Vector2 nearPoint =  targetPoint - (Vector2) position;
        nearPoint *= 0.99f;
        transform.position = nearPoint + (Vector2) position;
    }
    
    public void SetFacing() {
        var vectorsToTry = new Dictionary<Vector2, float> {
            {Vector2.left, 999f},
            {Vector2.up, 999f},
            {Vector2.right, 999f},
            {Vector2.down, 999f}
        };

        int mask = LayerMask.GetMask("MoveTarget");
        Vector2 nextPos = transform.position;
        
        foreach (Vector2 key in vectorsToTry.Keys.ToArray()) {
            RaycastHit2D hit = Physics2D.Raycast(nextPos, key, 0.5f, mask);
            if (hit.collider) {
                float dist = Vector2.Distance(hit.point, nextPos);
                Debug.Log(key.ToString() + dist.ToString());
                vectorsToTry[key] = dist;
            }
        }

        Vector2[] directionsToSort = vectorsToTry.Keys.ToArray();
        Array.Sort(directionsToSort, (a,b) => {
            float aDist = vectorsToTry[a];
            float bDist = vectorsToTry[b];
            return aDist.CompareTo(bDist);
        });

        currentFacing = directionsToSort[0];
        transform.rotation = Quaternion.Euler(new Vector3(0,0,facingSettings[currentFacing]));
    }

}
