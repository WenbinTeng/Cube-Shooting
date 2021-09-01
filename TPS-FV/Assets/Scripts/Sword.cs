using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject swordPrefab;
    private float freshTimer;
    private float freshIntvl;
    private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        freshTimer = 0f;
        freshIntvl = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        freshTimer += Time.deltaTime;
    }

    public void Hold()
    {
        if (freshTimer > freshIntvl)
        {
            if (prefab == null)
            {
                freshTimer = 0;

                prefab = Instantiate(swordPrefab, transform.position, transform.rotation, transform);
                prefab.GetComponent<BulletMovement>().setDmg(20);
                prefab.GetComponent<BulletMovement>().setSpd(00);
                Destroy(prefab, 2f);
            }
        }
    }
}
