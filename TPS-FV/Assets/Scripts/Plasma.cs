using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : MonoBehaviour
{
    public GameObject shootPrefab;
    public GameObject shellPrefab;
    public Transform shootLocator;
    public Transform shellLocator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        GameObject shootEffect = Instantiate(shootPrefab, shootLocator.position + new Vector3(0,0.5f,0), shootLocator.rotation);
        shootEffect.GetComponent<BulletMovement>().setDmg(100);
        shootEffect.GetComponent<BulletMovement>().setSpd(20);
        shootEffect.GetComponent<BulletMovement>().setFlr();
        Destroy(shootEffect, 3f);
        GameObject shellEffect = Instantiate(shellPrefab, shellLocator.position, shellLocator.rotation);
        shellEffect.transform.Rotate(-45f, +90f, 0);
        Destroy(shellEffect, 5f);
    }
}
