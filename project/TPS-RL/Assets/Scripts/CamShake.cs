using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private float shakeRatio;
    private float shakeTimer;
    private float shakeTotal;
    private Vector3 shakeDir;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        shakeTimer = 0f;
        shakeTotal = 1f;
        shakeDir = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        ShakeFunc();
    }

    private void ShakeFunc()
    {
        if (shakeTimer > 0f && shakeTotal > 0f)
        {
            float rate = shakeTimer / shakeTotal;

            Vector3 shakePos = Vector3.zero;
            shakePos.x = Random.Range(-Mathf.Abs(shakeDir.x) * rate, Mathf.Abs(shakeDir.x) * rate);
            shakePos.y = Random.Range(-Mathf.Abs(shakeDir.y) * rate, Mathf.Abs(shakeDir.y) * rate);
            shakePos.z = Random.Range(-Mathf.Abs(shakeDir.z) * rate, Mathf.Abs(shakeDir.z) * rate);
            gameObject.transform.localPosition += shakePos * shakeRatio;

            shakeTimer -= Time.deltaTime;
        }
    }

    public void StartFunc(float rate, float time)
    {
        this.shakeRatio = rate;

        shakeTimer = time;
        shakeTotal = time;

        startPos = gameObject.transform.localPosition;
    }

    public void StopFunc()
    {
        shakeTimer = 0f;
        shakeTotal = 0f;

        gameObject.transform.localPosition = startPos;
    }
}
