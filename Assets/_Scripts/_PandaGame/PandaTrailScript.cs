using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class PandaTrailScript : MonoBehaviour
{
    // Members
    private Vector3 _previous;
    private int _samples = 0;
    private Vector3 _zOffset;

    // References
    public Transform tracking;
    private LineRenderer _lineRenderer;

    void Awake()
    {
        // Set references
        _lineRenderer = GetComponent<LineRenderer>();

        // Initialize first two points 
        _zOffset = new Vector3(0f, 0f, -0.2f);
        _lineRenderer.SetPosition(0, tracking.position - _zOffset);
        _lineRenderer.SetPosition(1, tracking.position - _zOffset);

        // Save previous position
        _previous = tracking.position;
    }

    void Update()
    {
        // If object is not moving enough, don't expand
        if (Vector3.Distance(_previous, tracking.position - _zOffset) < 0.25)
            return;

        if (_samples < 2)
        {
            // First two samples we don't expand the array
            _lineRenderer.SetPosition(_samples, tracking.position - _zOffset);
        }
        else
        {
            // The rest we add one vertex per frame
            _lineRenderer.SetVertexCount(_samples + 1);
            _lineRenderer.SetPosition(_samples, tracking.position - _zOffset);
        }

        // Increment samples
        _samples++;

        // Save previous position
        _previous = tracking.position - _zOffset;
    }
}
