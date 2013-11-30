using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterScript : MonoBehaviour {

    private MeshFilter _meshFilter;

	void Awake () {
        _meshFilter = GetComponent<MeshFilter>();

        const int blockCount = 6;

        /*
         * Next block of code generates a square mesh that has triangles 
         * such that top row consists of many smaller triangles compared
         * to base, which only consist of two triangles.
         * 
         * This allows us to modify top vertices in our mesh to create
         * illusion of liquid movement.
         * 
         * Quad indices are ordered like this:
         * A-----B
         * |    /|
         * |   / |
         * |  /  |
         * | /   |
         * C-----D
         * Winding order is clockwise order, so upper quad has triangles
         * <A,B,C> and <B,D,C>.
         */ 

        Vector3[] __vertices = new Vector3[2 + 2 * blockCount];
        Vector2[] __uvs = new Vector2[__vertices.Length];
        for (int i = 0; i < __vertices.Length / 2; i++)
        {
            // Calculate x-coordinate
            float __x = i * (1f / (float)((__vertices.Length - 1) / 2));
            __x -= 0.5f;

            Vector3 __vUpper = new Vector3(__x, +0.50f, 0f);
            Vector3 __vLower = new Vector3(__x, +0.30f, 0f);
            __vertices[i] = __vUpper;
            __vertices[i + __vertices.Length / 2] = __vLower;

            Vector2 __uvUpper = new Vector2(__x, 1f);
            Vector2 __uvLower = new Vector2(__x, 0.8f);
            __uvs[i] = __uvUpper;
            __uvs[i + __vertices.Length / 2] = __uvLower;
        }

        // Copy and add two extra verticess
        Vector3[] __nv = new Vector3[__vertices.Length + 2];
        for (int i = 0; i < __vertices.Length; i++)
            __nv[i] = __vertices[i];
        __nv[__nv.Length - 2] = new Vector3(-0.5f, -0.5f, 0f);
        __nv[__nv.Length - 1] = new Vector3(+0.5f, -0.5f, 0f);

        // Copy and add two extra uv's
        Vector2[] __nuv = new Vector2[__uvs.Length + 2];
        for (int i = 0; i < __uvs.Length; i++)
            __nuv[i] = __uvs[i];
        __nuv[__nuv.Length - 2] = new Vector2(0f, 0f);
        __nuv[__nuv.Length - 1] = new Vector2(1f, 0f);

        // Create vertices
        List<int> __indices = new List<int>();
        for (int i = 0; i < blockCount; i++)
        {
            // Quad point indices
            int a = i + 0;
            int b = i + 1;
            int c = i + __vertices.Length / 2;
            int d = i + 1 + __vertices.Length / 2;

            // Triangle ABC
            __indices.Add(a);
            __indices.Add(b);
            __indices.Add(c);

            // Triangle BDC
            __indices.Add(b);
            __indices.Add(d);
            __indices.Add(c);
        }

        // Add two bottom triangles
        __indices.Add(__vertices.Length / 2); // a
        __indices.Add(__vertices.Length - 1); // b
        __indices.Add(__nv.Length - 2);       // c

        __indices.Add(__vertices.Length - 1); // b
        __indices.Add(__nv.Length - 1);       // d
        __indices.Add(__nv.Length - 2);       // c

        // Composite mesh
        Mesh __mesh = new Mesh();
        __mesh.vertices = __nv;
        __mesh.uv = __nuv;
        __mesh.SetIndices(__indices.ToArray(), MeshTopology.Triangles, 0);
        __mesh.RecalculateNormals();
        _meshFilter.mesh = __mesh;
	}
	
	void Update () {
        Vector3[] __vertices = _meshFilter.mesh.vertices;
        int __topVertexCount = (__vertices.Length - 2) / 2;

        for (int i = 0; i < __topVertexCount; i++)
        {
            Vector3 __v = __vertices[i];
            float __amplitude = 0.5f + 0.5f * Mathf.Cos(Time.time + Mathf.PI * 2f * (float)i / (float)__topVertexCount);
            __v.y = 0.3f + 0.2f * __amplitude;
            __vertices[i] = __v;
        }

        _meshFilter.mesh.vertices = __vertices;
	}
}
