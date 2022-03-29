using Lean.Transition;
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

    private float gameScore;

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
    void Start()
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

        playerVelocity = Vector3.zero;

        gameScore = 0;

        ammoTotal = 50;
        ammoCurrt = 50;
        fireTimer = 0.0f;
        fireIntvl = 0.1f;
        hurtTimer = 0.0f;
        hurtIntvl = 0.1f;

        chillTimer = 0.0f;
        chillIntvl = 0.1f;
        crossTimer = 0.0f;
        crossIntvl = 1.0f;

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

    public void AccepthurtFunc(int hurt)
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
        groundedFlag = Physics.CheckBox(groundChk.position, groundDst * Vector3.one, Quaternion.identity, groundMsk);

        if (groundedFlag)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
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

        if (Input.GetKey(KeyCode.W))
        {
            moveDirect += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirect -= Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirect -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirect += Vector3.right;
        }

        characterController.Move(moveDirect * movingSpd * Time.deltaTime);
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

                transmitPosition = transform.position;

                int area = (int)((transform.rotation.eulerAngles.y + 360) % 360) / 45;

                switch (area)
                {
                    case 0:
                    case 7:
                        transmitPosition += new Vector3(0f, 0f, 10f);
                        break;
                    case 1:
                    case 2:
                        transmitPosition += new Vector3(10f, 0f, 0f);
                        break;
                    case 3:
                    case 4:
                        transmitPosition -= new Vector3(0f, 0f, 10f);
                        break;
                    case 5:
                    case 6:
                        transmitPosition -= new Vector3(10f, 0f, 0f);
                        break;
                }

                if (transmitPosition.x > -20 && transmitPosition.x < 20 && transmitPosition.z > -20 & transmitPosition.z < 20)
                {
                    GameObject srcPrefab = Instantiate(transmitEffect, transform.position, transform.rotation);
                    srcPrefab.transform.Rotate(0, 180, 0);
                    Destroy(srcPrefab, 2f);

                    transform.position = transmitPosition;

                    GameObject dstPrefab = Instantiate(transmitEffect, transform.position, transform.rotation);
                    dstPrefab.transform.Rotate(0, 180, 0);
                    Destroy(dstPrefab, 2f);
                }
                else
                {
                    transmitPosition = transform.position;
                }
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