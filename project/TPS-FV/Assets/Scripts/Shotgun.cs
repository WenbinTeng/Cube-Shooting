using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public GameObject shootPrefab;
    public GameObject shellPrefab;
    public Transform flareLocator;
    public Transform shellLocator;
    private int count = 0;

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
        GameObject shellEffect = Instantiate(shellPrefab, shellLocator.position, shellLocator.rotation);
        shellEffect.transform.Rotate(-45f, +90f, 0);
        Destroy(shellEffect, 5f);

        count = 0;

        for (int i = 0; i < 5; i++)
        {
            GameObject shootEffect = Instantiate(shootPrefab, flareLocator.position, flareLocator.rotation);
            shootEffect.GetComponent<BulletMovement>().setDmg(50);
            shootEffect.GetComponent<BulletMovement>().setSpd(50);
            shootEffect.transform.Rotate(0, Random.Range(-10f, +10f), 0);
            Destroy(shootEffect, 2f);

            if (count++ == 0) shootEffect.GetComponent<BulletMovement>().setFlr();
        }
    }
}
