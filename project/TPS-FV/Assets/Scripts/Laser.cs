using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject LaserPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        GameObject prefab = Instantiate(LaserPrefab, transform.position, transform.rotation);
        prefab.GetComponent<BulletMovement>().setDmg(50);
        prefab.GetComponent<BulletMovement>().setSpd(20);

        float rx = Random.Range(-5f, 5f);
        float ry = Random.Range(-5f, 5f);
        float rz = Random.Range(-5f, 5f);

        prefab.transform.Rotate(rx, ry, rz);

        Destroy(prefab, 2f);
    }
}
