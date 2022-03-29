using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    private float fadeTimer;
    private float fadeTotal;
    Light lt;

    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = 0f;
        fadeTotal = 5f;
        lt = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        fadeTimer += Time.deltaTime;

        if (fadeTimer < fadeTotal)
        {
            lt.intensity -= fadeTimer / fadeTotal * 0.05f;
        }
    }
}
