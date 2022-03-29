using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public GameObject flashEffect;
    public GameObject ashesEffect;
    public GameObject lightPrefab;

    public void FlashFunc()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponentInParent<PlayerController>().AcceptHurtFunc(10);
            }
            else if (collider.tag == "Enemy")
            {
                collider.GetComponentInParent<EnemyController>().AcceptHurtFunc(10);
            }
            else if (collider.tag == "Guard")
            {
                collider.GetComponentInParent<GuardController>().AcceptHurtFunc(10);
            }
            else if (collider.tag == "Box")
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 10f);
            }
        }

        float playerDistance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (playerDistance < 10)
        {
            Camera.main.GetComponent<CamShake>().StartFunc(0.5f - playerDistance * 0.05f, 1f);
        }

        GameObject flash = Instantiate(flashEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        flash.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        Destroy(flash, 10f);
        GameObject ashes = Instantiate(ashesEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        ashes.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(ashes, 5f);
        GameObject light = Instantiate(lightPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        light.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(light, 5f);

        light.GetComponent<Light>().intensity += 2;

        Destroy(gameObject);
    }
}
