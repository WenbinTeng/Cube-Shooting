using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public GameObject impactEffect;
    public GameObject muzzleEffect;
    private int bulletSpd;
    private int bulletDmg;
    private float collisionTimer;
    private float collisionIntvl;

    // Start is called before the first frame update
    void Start()
    {
        collisionTimer = 0.00f;
        collisionIntvl = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * bulletSpd * Time.deltaTime, Space.World);
    }

    public void setFlr()
    {
        GameObject effect = Instantiate(muzzleEffect, transform.position, transform.rotation);
        effect.transform.Rotate(0, 270f, 0);
        Destroy(effect, 2f);
    }

    public void setDmg(int dmg)
    {
        this.bulletDmg = dmg;
    }

    public void setSpd(int spd)
    {
        this.bulletSpd = spd;
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionTimer += Time.deltaTime;

        if (collisionTimer > collisionIntvl && other.gameObject.tag != "FX")
        {
            collisionTimer = 0;
            collisionIntvl = 0;

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            effect.transform.Rotate(0, 180f, 0);
            Destroy(effect, 2f);

            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponentInParent<PlayerController>().AcceptHurtFunc(bulletDmg);
            }
            else if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponentInParent<EnemyController>().AcceptHurtFunc(bulletDmg);
            }
            else if (other.gameObject.tag == "Guard")
            {
                other.gameObject.GetComponentInParent<GuardController>().AcceptHurtFunc(bulletDmg);
            }
            else if (other.gameObject.tag == "Blast")
            {
                other.gameObject.GetComponent<Blast>().ExplodeFunc();
            }
            else if (other.gameObject.tag == "Flash")
            {
                other.gameObject.GetComponent<Flash>().FlashFunc();
            }

            Destroy(gameObject);
        }
    }
}
