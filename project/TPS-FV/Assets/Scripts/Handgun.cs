using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : MonoBehaviour
{
    public GameObject shootPrefab;
    public GameObject shellPrefab;
    public Transform shootLocator;
    public Transform shellLocator;
    private int count;

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

        for (int i = 0; i < 2; i++)
        {
            GameObject shootEffect = Instantiate(shootPrefab, shootLocator.position, shootLocator.rotation);
            shootEffect.GetComponent<BulletMovement>().setDmg(50);
            shootEffect.GetComponent<BulletMovement>().setSpd(50);
            shootEffect.transform.Translate(shootEffect.transform.forward * i * 5, Space.World);
            Destroy(shootEffect, 2f);

            if (count++ == 0) shootEffect.GetComponent<BulletMovement>().setFlr();
        }
    }
}
