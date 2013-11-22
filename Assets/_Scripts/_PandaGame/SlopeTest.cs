using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlopeTest : MonoBehaviour
{
    private int samples = 0;
    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;

    void Awake()
    {
        // Set references
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);

        // Reset whatever points edge collider might have
        edgeCollider.points = new Vector2[2];
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Get mouse position and convert to world pos
            Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            __pos.z = 0f; // Ditch z-axis, because line renderer only accepts Vector3
            samples++; // Count samples for indexing

            Vector2[] __oldPointSet = edgeCollider.points;
            if (samples == 1 || samples == 2)
            {
                // Special case, because edge collider always have at least two points
                __oldPointSet[samples - 1] = new Vector2(__pos.x, __pos.y);
                edgeCollider.points = __oldPointSet;

                // Update line renderer
                lineRenderer.SetVertexCount(samples);
                lineRenderer.SetPosition(samples - 1, __pos);
            }
            else
            {
                // Create new array of size points + 1
                Vector2[] __newPointSet = new Vector2[__oldPointSet.Length + 1];
                for (int i = 0; i < __oldPointSet.Length; i++)
                    __newPointSet[i] = __oldPointSet[i];

                // Assign new point and save new point set to edge collider
                __newPointSet[__oldPointSet.Length] = new Vector2(__pos.x, __pos.y);
                edgeCollider.points = __newPointSet;

                // Update line renderer
                lineRenderer.SetVertexCount(samples);
                lineRenderer.SetPosition(samples - 1, __pos);
            }
        }
    }
}
