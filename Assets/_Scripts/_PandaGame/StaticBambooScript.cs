using UnityEngine;
using System.Collections;

public class StaticBambooScript : MonoBehaviour
{
    void Update()
    {
        // Set texture tiling proportional to its scale
        Vector2 __newTiling = new Vector2(transform.localScale.x, 1f);
        renderer.material.mainTextureScale = __newTiling;
    }
}
