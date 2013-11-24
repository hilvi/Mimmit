using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicLine : MonoBehaviour
{
    #region MEMBERS
    private int _samples = 0;
    private EdgeCollider2D _edgeCollider;
    private LineRenderer _lineRenderer;

    // Keep track of previous position to prevent 
    // drawing continuously on the same spot.
    private const float _minLineDelta = 0.1f;
    private Vector2 _previousPosition; 
    #endregion

    #region UNITY_METHODS
    void Awake()
    {
        // Set references
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetVertexCount(0);

        // Reset whatever points edge collider might have
        _edgeCollider.points = new Vector2[2];
    }
    #endregion

    #region METHODS
    public void Step()
    {

        // Get mouse position and convert to world pos
        Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        __pos.z = 0f; // Ditch z-axis, because line renderer only accepts Vector3
        Vector2 __pos2d = new Vector2(__pos.x, __pos.y);

        // If distance to previous point is too close, don't draw
        float __distance = Vector2.Distance(__pos2d, _previousPosition);
        if (__distance < _minLineDelta)
            return;
        else
            // Count samples for tracking point indices
            _samples++;
        
        Vector2[] __oldPointSet = _edgeCollider.points;
        if (_samples == 1 || _samples == 2)
        {
            // Special case, because edge collider always have at least two points
            __oldPointSet[_samples - 1] = __pos2d;
            _edgeCollider.points = __oldPointSet;

            // Update line renderer
            _lineRenderer.SetVertexCount(_samples);
            _lineRenderer.SetPosition(_samples - 1, __pos);
            _previousPosition = __pos2d;
        }
        else
        {
            // Create new array of size points + 1
            Vector2[] __newPointSet = new Vector2[__oldPointSet.Length + 1];
            for (int i = 0; i < __oldPointSet.Length; i++)
                __newPointSet[i] = __oldPointSet[i];

            // Assign new point and save new point set to edge collider
            __newPointSet[__oldPointSet.Length] = __pos2d;
            _edgeCollider.points = __newPointSet;

            // Update line renderer
            _lineRenderer.SetVertexCount(_samples);
            _lineRenderer.SetPosition(_samples - 1, __pos);
            _previousPosition = __pos2d;
        }
    }

    public void SetStartingPosition(Vector2 position)
    {
        _previousPosition = position;
    }
    #endregion
}
