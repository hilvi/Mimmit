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

        // Line has been modified, re-compute tiling
        RecalculateTiling();
    }

    public void Strip()
    {

        // If samples equal to 2, simply delete the line
        if (_samples <= 2)
        {
            Destroy(this.gameObject);
            return;
        }

        // Get mouse position and convert to world pos
        Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        __pos.z = 0f; // Ditch z-axis, because line renderer only accepts Vector3
        Vector2 __pos2d = new Vector2(__pos.x, __pos.y);

        // Is end-point closer to mouse pos than start-point
        Vector2[] __oldPointSet = _edgeCollider.points;
        Vector2 __start = __oldPointSet[0];
        Vector2 __end = __oldPointSet[__oldPointSet.Length - 1];

        float __distToStart = Vector2.Distance(__start, __pos2d);
        float __distToEnd = Vector2.Distance(__end, __pos2d);

        // Too far, ignore
        const float distanceToDelete = 0.5f;
        if (__distToStart > distanceToDelete && __distToEnd > distanceToDelete)
            return;

        // Determine if user wants to delete start point or end point
        if (__distToStart < __distToEnd)
        {
            // Copy old point set to new point set, removing first element
            Vector2[] __newPointSet = new Vector2[__oldPointSet.Length - 1];
            for (int i = 1; i < __oldPointSet.Length; i++)
                __newPointSet[i - 1] = __oldPointSet[i];

            // Save the results
            _edgeCollider.points = __newPointSet;

            /* 
             * For some reason LineRenderere does not have facilities for clearing
             * all the points. This is why we use SetVertexCount to zero and then
             * re-make the lineRenderer
             */ 
            _lineRenderer.SetVertexCount(0);
            _lineRenderer.SetVertexCount(_samples - 1);

            // Copy points from new point set to to line renderer
            for (int i = 0; i < __newPointSet.Length; i++)
            {
                Vector3 __v = new Vector3(__newPointSet[i].x, __newPointSet[i].y, 0f);
                _lineRenderer.SetPosition(i, __v);
            }

            // Reduce number of samples
            _samples--;
        }
        else
        {
            // Copy old point set to new point set, removing last element
            Vector2[] __newPointSet = new Vector2[__oldPointSet.Length - 1];
            for (int i = 0; i < __oldPointSet.Length - 1; i++)
                __newPointSet[i] = __oldPointSet[i];

            // Save the results
            _edgeCollider.points = __newPointSet;

            /* 
             * Compared to case where start point had to be removed, removing last
             * point is trivial by just changing size of line renderer.
             */ 
            _lineRenderer.SetVertexCount(_samples - 1);

            // Reduce number of samples
            _samples--;
        }

        // Line has been modified, re-compute tiling
        RecalculateTiling();
    }

    public void SetStartingPosition(Vector2 position)
    {
        _previousPosition = position;
    }

    public Vector2 GetStartPoint()
    {
        if (_samples < 2)
            return Vector2.zero;

        return _edgeCollider.points[0];
    }

    public Vector2 GetEndPoint()
    {
        if (_samples < 2)
            return Vector2.zero;

        return _edgeCollider.points[_edgeCollider.points.Length - 1];
    }

    private void RecalculateTiling()
    {
        Vector2[] __pointSet = _edgeCollider.points;
        float __length = __pointSet.Length;
        float __totalLength = 0f;

        // Step along the line, compute distance between two consecutive points
        for (int i = 0, j = 1; j < __length - 1; i++, j++)
            __totalLength += Vector2.Distance(__pointSet[i], __pointSet[j]);

        // Calculate new tiling value
        Vector2 __newTiling = new Vector2(Mathf.Ceil(__totalLength), 1f);
        _lineRenderer.material.mainTextureScale = __newTiling;
    }
    #endregion
}
