using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject shootPrefab;
    public GameObject shellPrefab;
    public Transform shootLocator;
    public Transform shellLocator;

    private LineRenderer lineRenderer;

    private float rayColor;
    private float rayAlpha;

    private float enableTimer;
    private float enableIntvl;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        rayColor = 1f;
        rayAlpha = 1f;

        enableTimer = 0.0f;
        enableIntvl = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        AimFunc();

        RayFunc();

    }

    public void Shoot()
    {
        GameObject shootEffect = Instantiate(shootPrefab, shootLocator.position, shootLocator.rotation);
        shootEffect.GetComponent<BulletMovement>().setDmg(50);
        shootEffect.GetComponent<BulletMovement>().setSpd(50);
        RandomRotate(shootEffect);
        Destroy(shootEffect, 2f);

        GameObject shellEffect = Instantiate(shellPrefab, shellLocator.position, shellLocator.rotation);
        shellEffect.transform.Rotate(-45f, 90f, 0);
        Destroy(shellEffect, 2f);
    }

    private void RandomRotate(GameObject Object)
    {
        float rx = Random.Range(-1f, 1f);
        float ry = Random.Range(-1f, 1f);
        float rz = Random.Range(-1f, 1f);
        Object.transform.Rotate(rx, ry, rz);
    }

    private void AimFunc()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Time.timeScale >= 0.01f)
            {
                Vector3 mousePoint = new Vector3(hit.point.x, transform.parent.position.y, hit.point.z);
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.LookRotation(mousePoint - transform.parent.position), 0.2f);
            }
        }
    }

    private void RayFunc()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.forward * 10);
        }

        if (Input.GetButton("Fire1"))
        {
            enableTimer = 0f;

            rayColor = 0.6f;
            rayAlpha = 0.0f;

            lineRenderer.enabled = false;
        }
        else
        {
            enableTimer += Time.deltaTime;

            if (enableTimer > enableIntvl)
            {
                gameObject.GetComponent<Renderer>().material.color = new Color(rayColor, rayColor, rayColor, rayAlpha);

                lineRenderer.enabled = true;

                rayColor = rayColor < 1f ? rayColor + Time.deltaTime / 2 : 1f;
                rayAlpha = rayAlpha < 1f ? rayAlpha + Time.deltaTime / 2 : 1f;
            }
        }
    }
}
