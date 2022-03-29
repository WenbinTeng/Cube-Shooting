using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private float healTotal;
    private float healPoint;
    private float healTimer;
    private float healIntvl;
    private bool healFlag;

    private float movingSpd;
    private float groundGrv;
    private float groundDst;
    private bool groundedFlag;
    private Vector3 playerVelocity;
    public Transform groundChk;
    public LayerMask groundMsk;

    private int gameScore;

    private float ammoTotal;
    private float ammoCurrt;
    private float fireTimer;
    private float fireIntvl;
    private float hurtTimer;
    private float hurtIntvl;

    private float chillTimer;
    private float chillIntvl;
    private float crossTimer;
    private float crossIntvl;

    private Vector3 transmitPosition;

    public GameObject transmitEffect;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        healTotal = 100f;
        healPoint = 100f;
        healTimer = 0.0f;
        healIntvl = 2.0f;
        healFlag = false;

        movingSpd = 5.0f;
        groundGrv = -20f;
        groundDst = 0.5f;

        gameScore = 0;

        ammoTotal = 100;
        ammoCurrt = 100;
        fireTimer = 0.0f;
        fireIntvl = 0.1f;
        hurtTimer = 0.0f;
        hurtIntvl = 0.1f;

        chillTimer = 0.0f;
        chillIntvl = 0.1f;
        crossTimer = 0.0f;
        crossIntvl = 5.0f;

        transmitPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        FallFunc();

        MoveFunc();

        RecoverManagement();

        ActivateShootFunc();

        ActivateCrossFunc();

    }

    public void AcceptGoalFunc(int goal)
    {
        if (!healFlag)
        {
            gameScore += goal;
        }
    }

    public void AcceptHurtFunc(int hurt)
    {
        if (Time.time > hurtTimer)
        {
            healPoint -= hurt;
            healTimer = Time.time + healIntvl;
            hurtTimer = Time.time + hurtIntvl;
        }
    }

    public float getGoal()
    {
        return this.gameScore;
    }

    public float getHeal()
    {
        return this.healPoint;
    }

    public float getAmmoRate()
    {
        return ammoCurrt / ammoTotal < 1 ? ammoCurrt / ammoTotal : 1;
    }

    public float getHealRate()
    {
        return healPoint / healTotal > 0 ? healPoint / healTotal : 0;
    }

    private void FallFunc()
    {
        groundedFlag = Physics.CheckBox(groundChk.position, Vector3.one * groundDst, transform.rotation, groundMsk);
        
        if (groundedFlag)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                playerVelocity.y = Mathf.Sqrt(-6f * groundGrv);
            }
        }

        playerVelocity.y += groundGrv * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void MoveFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        Vector3 moveDirect = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            moveDirect -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirect += Vector3.forward;
        }

        characterController.Move(moveDirect * movingSpd * Time.deltaTime);
    }

    private void OverFunc()
    {
        if (!healFlag)
        {
            healFlag = true;

            movingSpd = 0;

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);

            BroadcastMessage("HideMe");
        }
    }

    private void RecoverManagement()
    {
        if (Time.time > healTimer)
        {
            healPoint += 5f * Time.deltaTime;

            if (healPoint > healTotal)
            {
                healPoint = healTotal;
            }
        }

        if (healPoint <= 0)
        {
            OverFunc();
        }
    }

    private void ActivateShootFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > fireTimer && ammoCurrt > 0)
            {
                fireTimer = Time.time + fireIntvl;

                gameObject.GetComponentInChildren<Gun>().Shoot();

                ammoCurrt--;

                Camera.main.GetComponent<CamShake>().StartFunc(0.1f, 0.3f);
            }
        }
        else
        {
            ammoCurrt += ammoTotal * 0.2f * Time.deltaTime;

            if (ammoCurrt > ammoTotal)
            {
                ammoCurrt = ammoTotal;
            }
        }
    }

    private void ActivateCrossFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        crossTimer += Time.deltaTime;
        chillTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (crossTimer > crossIntvl)
            {
                crossTimer = 0;
                chillTimer = 0;

                transmitPosition.z = -transform.position.z;

                GameObject srcPrefab = Instantiate(transmitEffect, transform.position, transform.rotation);
                srcPrefab.transform.Rotate(0, 180, 0);
                Destroy(srcPrefab, 2f);

                transform.position = transmitPosition;

                GameObject dstPrefab = Instantiate(transmitEffect, transform.position, transform.rotation);
                dstPrefab.transform.Rotate(0, 180, 0);
                Destroy(dstPrefab, 2f);
            }
        }

        if (chillTimer < chillIntvl)
        {
            transform.position = transmitPosition;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (playerVelocity.y > 0 && hit.gameObject.tag == "Wall")
        {
            playerVelocity.y = 0;
        }
    }
}