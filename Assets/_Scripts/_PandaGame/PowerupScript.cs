using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PowerupScript : MonoBehaviour
{

    public delegate void CaptureBall();
    public static event CaptureBall OnBallCapture;

    private bool _used = false;

    // References
    private MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.materials[1].color = Color.clear;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Ball")
        {
            if (!_used)
            {
                OnBallCapture();
                RemoveLeaf();
                DisableGlow();
                DisableLeaves();
                _used = true;
            }
        }
    }

    private void RemoveLeaf()
    {
        _meshRenderer.materials[0].color = Color.clear;
        _meshRenderer.materials[1].color = Color.white;
    }

    private void DisableGlow()
    {
        var __glow = transform.FindChild("Glow");
        __glow.gameObject.SetActive(false);
    }

    private void DisableLeaves()
    {
        var __sys = GetComponentInChildren<ParticleSystem>();
        __sys.enableEmission = false;
    }
}
