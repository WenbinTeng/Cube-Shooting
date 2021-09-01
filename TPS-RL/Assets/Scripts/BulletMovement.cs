using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public GameObject impactEffect;
    public GameObject muzzleEffect;
    private int bulletSpd;
    private int bullecDmg;
    private float collisionTimer;
    private float collisionIntvl;

    // Start is called before the first frame update
    void Start()
    {
        collisionTimer = 0.00f;
        collisionIntvl = 0.00f;

        GameObject effect = Instantiate(muzzleEffect, transform.position, transform.rotation);
        effect.transform.Rotate(0, 270, 0);
        Destroy(effect, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * bulletSpd * Time.deltaTime, Space.World);
    }

    public void setDmg(int dmg)
    {
        this.bullecDmg = dmg;
    }

    public void setSpd(int spd)
    {
        this.bulletSpd = spd;
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionTimer += Time.deltaTime;

        if (collisionTimer > collisionIntvl && other.gameObject.tag != "Bullet")
        {
            collisionTimer = 0;
            collisionIntvl = 0;

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            effect.transform.Rotate(Vector3.up, 180f);
            Destroy(effect, 1f);

            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponentInParent<EnemyController>().AcceptHurtFunc(bullecDmg);
                other.gameObject.GetComponentInParent<Rigidbody>().AddForce(300 * transform.forward, ForceMode.Force);
            }
            if (other.gameObject.tag == "Guard")
            {
                other.gameObject.GetComponentInParent<GuardController>().AcceptHurtFunc(bullecDmg);
                other.gameObject.GetComponentInParent<Rigidbody>().AddForce(300 * transform.forward, ForceMode.Force);
            }

            Destroy(gameObject);
        }
    }
}
