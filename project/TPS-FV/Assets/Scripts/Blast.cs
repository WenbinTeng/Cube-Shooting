using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    public GameObject blastEffect;
    public GameObject flareEffect;
    public GameObject lightPrefab;

    public void ExplodeFunc()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponentInParent<PlayerController>().AcceptHurtFunc(50);
            }
            else if (collider.tag == "Enemy")
            {
                collider.GetComponentInParent<EnemyController>().AcceptHurtFunc(100);
            }
            else if (collider.tag == "Guard")
            {
                collider.GetComponentInParent<GuardController>().AcceptHurtFunc(100);
            }
            else if (collider.tag == "Box")
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position, 10f);
            }
        }

        float playerDistance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (playerDistance < 50)
        {
            Camera.main.GetComponent<CamShake>().StartFunc(0.5f - playerDistance * 0.01f, 0.5f);
        }

        GameObject blast = Instantiate(blastEffect, transform.position, Quaternion.identity);
        blast.transform.rotation = Quaternion.Euler(-90f, 0, 0);
        Destroy(blast, 10f);
        GameObject flare = Instantiate(flareEffect, transform.position, Quaternion.identity);
        flare.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(flare, 5f);
        GameObject light = Instantiate(lightPrefab, transform.position, Quaternion.identity);
        light.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(light, 5f);

        light.GetComponent<Light>().intensity -= 2;

        Destroy(gameObject);
    }
}
