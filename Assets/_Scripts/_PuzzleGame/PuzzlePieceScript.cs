using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PuzzlePieceScript : MonoBehaviour
{
    Texture _previous;
    Material _mat;
    public float ratio = 256;
    public Texture texture;

    // Use this for initialization
    void Start()
    {
        _mat = new Material(Shader.Find("Transparent/Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {
        if (texture != _previous)
        {
            _mat.mainTexture = texture;
            float __height = texture.height;
            float __width = texture.width;
            float __heightRatio = __height / ratio;
            float __widthRatio = __width / ratio;

            Vector3 __size = new Vector3(1, 1, 1);
            __size.y *= __heightRatio;
            __size.x *= __widthRatio;
            gameObject.transform.localScale = __size;

            _previous = texture;
            renderer.sharedMaterial = _mat;
        }
    }
}
