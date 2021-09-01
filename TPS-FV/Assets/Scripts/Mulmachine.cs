using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mulmachine : MonoBehaviour
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
        GameObject shootEffect = Instantiate(shootPrefab, shootLocator.position, shootLocator.rotation);
        shootEffect.GetComponent<BulletMovement>().setDmg(50);
        shootEffect.GetComponent<BulletMovement>().setSpd(75);
        shootEffect.GetComponent<BulletMovement>().setFlr();
        RandomRotate(shootEffect);
        Destroy(shootEffect, 2f);
        GameObject shellEffect = Instantiate(shellPrefab, shellLocator.position, shellLocator.rotation);
        shellEffect.transform.Rotate(-45f, +90f, 0);
        Destroy(shellEffect, 5f);
    }

    private void RandomRotate(GameObject Object)
    {
        float rx = Random.Range(-1f, 1f);
        float ry = Random.Range(-1f, 1f);
        float rz = Random.Range(-1f, 1f);
        Object.transform.Rotate(rx, ry, rz);
    }
}
