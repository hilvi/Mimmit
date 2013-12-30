using UnityEngine;
using System.Collections;

public class AppleSliceAnimation : FoodAnimation
{
    public Transform cutApple01, cutApple02;

    private Vector3 _bowlPos;
    private float _elapsedTime;

    void Awake()
    {
        _bowlPos = new Vector3(2.316593f, -1.258288f, 0f);
    }

    void Update()
    {
        if (!_started)
            return;

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > 0.1f)
        {
            Vector3 __pos = cutApple01.position;
            cutApple01.position = Vector3.Lerp(__pos, _bowlPos, Time.deltaTime * 10f);

            Vector3 __scale = cutApple01.localScale;
            __scale.x = Mathf.Lerp(__scale.x, 0f, Time.deltaTime * 10f);
            __scale.y = Mathf.Lerp(__scale.y, 0f, Time.deltaTime * 10f);
            cutApple01.localScale = __scale;
        }

        if (_elapsedTime > 1f)
        {
            Vector3 __pos = cutApple02.position;
            cutApple02.position = Vector3.Lerp(__pos, _bowlPos, Time.deltaTime * 10f);

            Vector3 __scale = cutApple02.localScale;
            __scale.x = Mathf.Lerp(__scale.x, 0f, Time.deltaTime * 10f);
            __scale.y = Mathf.Lerp(__scale.y, 0f, Time.deltaTime * 10f);
            cutApple02.localScale = __scale;
        }
    }
}
