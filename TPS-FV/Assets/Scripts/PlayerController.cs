using Unity.UIWidgets.ui;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private float healPoint;
    private float healTotal;
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

    private float gameLvl;
    private float goalAll;
    private float goalCur;
    private float goalNxt;
    private float upgradeTimer;
    private float upgradeIntvl;

    private int weaponCode;
    private float ammoAll;
    private float ammoCur;
    private float fireTimer;
    private float fireIntvl;
    private float hurtTimer;
    private float hurtIntvl;

    public GameObject explodePrefab;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();

        healPoint = 100f;
        healTotal = 100f;
        healTimer = 0.0f;
        healIntvl = 2.0f;
        healFlag = false;

        movingSpd = 5.0f;
        groundGrv = -20f;
        groundDst = 0.5f;

        playerVelocity = Vector3.zero;

        gameLvl = 0;

        goalAll = 0f;
        goalCur = 0f;
        goalNxt = 5f;

        upgradeTimer = 0f;
        upgradeIntvl = 1f;

        weaponCode = 0;
        ammoAll = 10f;
        ammoCur = 10f;

        fireTimer = 0.0f;
        fireIntvl = 0.3f;

        hurtTimer = 0.0f;
        hurtIntvl = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

        FallDownFunc();

        MovementFunc();

        RotationFunc();

        UpgradeManagement();

        RecoverManagement();

        ActivateShootFunc();

    }

    public void AcceptGoalFunc(int score)
    {
        if (!healFlag)
        {
            goalAll += score;
            goalCur += score;
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

    public int getGameLvl()
    {
        return (int)gameLvl;
    }

    public int getGoalAll()
    {
        return (int)goalAll;
    }

    public float getAmmoRate()
    {
        return ammoCur / ammoAll < 1f ? ammoCur / ammoAll : 1f;
    }

    public float getGoalRate()
    {
        return goalCur / goalNxt < 1f ? goalCur / goalNxt : 1f;
    }

    public float getHealRate()
    {
        return healPoint / healTotal > 0f ? healPoint / healTotal : 0f;
    }

    private void FallDownFunc()
    {
        groundedFlag = Physics.CheckBox(groundChk.position, groundDst * Vector3.one, Quaternion.identity, groundMsk);

        if (groundedFlag)
        {
            if (playerVelocity.y < 0f)
            {
                playerVelocity.y = 0f;
            }
        }

        playerVelocity.y += groundGrv * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void MovementFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        Vector3 moveDirect = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirect += Vector3.forward + Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirect += Vector3.forward - Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirect -= Vector3.forward + Vector3.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirect -= Vector3.forward - Vector3.right;
        }

        characterController.Move(moveDirect * movingSpd * Time.deltaTime);
    }

    private void RotationFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Time.timeScale >= 0.01f)
            {
                Vector3 mousePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mousePoint - transform.position), 0.2f);
            }
        }
    }

    private void UpgradeManagement()
    {
        if (goalCur >= goalNxt)
        {
            if (upgradeTimer > upgradeIntvl)
            {
                upgradeTimer = 0;

                goalCur -= (int)goalNxt / 1f;
                goalNxt += (int)goalAll % 25 == 0 ? 5 : 0;
                gameLvl++;

                ResettingArgsFunc();
            }
            else
            {
                upgradeTimer += Time.deltaTime;
            }
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

    private void OverFunc()
    {
        if (!healFlag)
        {
            healFlag = true;

            GameObject effect = Instantiate(explodePrefab, transform.position, transform.rotation);
            effect.transform.Rotate(-90f, 0, 0);
            Destroy(effect, 5f);

            movingSpd = 0;

            BroadcastMessage("HideMe");
        }
    }

    private void ActivateShootFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            ammoCur += ammoAll * 0.2f * Time.deltaTime;

            if (ammoCur > ammoAll)
            {
                ammoCur = ammoAll;
            }
        }

        switch (weaponCode)
        {
            case 0:
                if (Input.GetButtonDown("Fire1"))
                {
                    if (Time.time > fireTimer && ammoCur > 0)
                    {
                        fireTimer = Time.time + fireIntvl;
                        gameObject.GetComponentInChildren<Handgun>().Shoot();
                        ammoCur--;

                        Camera.main.GetComponent<CamShake>().StartFunc(0.1f, 0.3f);
                    }
                }
                break;
            case 1:
                if (Input.GetButtonDown("Fire1"))
                {
                    if (Time.time > fireTimer && ammoCur > 0)
                    {
                        fireTimer = Time.time + fireIntvl;
                        gameObject.GetComponentInChildren<Shotgun>().Shoot();
                        ammoCur--;

                        Camera.main.GetComponent<CamShake>().StartFunc(0.1f, 0.3f);
                    }
                }
                break;
            case 2:
                if (Input.GetMouseButton(0))
                {
                    if (Time.time > fireTimer && ammoCur > 0)
                    {
                        fireTimer = Time.time + fireIntvl;
                        gameObject.GetComponentInChildren<Submachine>().Shoot();
                        ammoCur--;

                        Camera.main.GetComponent<CamShake>().StartFunc(0.05f, 1f);
                    }

                    movingSpd = Mathf.Lerp(movingSpd, 7.5f, Time.deltaTime / 2);
                }
                else
                {
                    movingSpd = Mathf.Lerp(movingSpd, 5.0f, Time.deltaTime);
                }
                break;
            case 3:
                if (Input.GetMouseButton(0))
                {
                    if (Time.time > fireTimer && ammoCur > 0)
                    {
                        fireTimer = Time.time + fireIntvl;
                        gameObject.GetComponentInChildren<Mulmachine>().Shoot();
                        ammoCur--;

                        Camera.main.GetComponent<CamShake>().StartFunc(0.05f, 1f);
                    }

                    movingSpd = Mathf.Lerp(movingSpd, 2.5f, Time.deltaTime * 2);
                }
                else
                {

                    movingSpd = Mathf.Lerp(movingSpd, 5.0f, Time.deltaTime);
                }
                break;
            case 4:
                if (Input.GetButtonDown("Fire1"))
                {
                    if (Time.time > fireTimer && ammoCur > 0)
                    {
                        fireTimer = Time.time + fireIntvl;
                        gameObject.GetComponentInChildren<Plasma>().Shoot();
                        ammoCur--;
                    }
                }
                movingSpd = Mathf.Lerp(movingSpd, 5.0f, Time.deltaTime);
                break;
        }
    }

    private void ResettingArgsFunc()
    {
        if (healFlag || Time.timeScale <= 0)
        {
            return;
        }

        weaponCode = (int)gameLvl % 5;

        switch (weaponCode)
        {
            case 0:
                fireIntvl = 0.5f - gameLvl * 0.03f;
                fireIntvl = fireIntvl > 0.2f ? fireIntvl : 0.2f;
                ammoAll = 20 + gameLvl * 1f;
                ammoCur = ammoAll;
                break;
            case 1:
                fireIntvl = 0.5f - gameLvl * 0.03f;
                fireIntvl = fireIntvl > 0.2f ? fireIntvl : 0.2f;
                ammoAll = 20 + gameLvl * 1f;
                ammoCur = ammoAll;
                break;
            case 2:
                fireIntvl = 0.1f - gameLvl * 0.002f;
                fireIntvl = fireIntvl > 0.05f ? fireIntvl : 0.05f;
                ammoAll = 50 + gameLvl * 5f;
                ammoCur = ammoAll;
                break;
            case 3:
                fireIntvl = 0.1f - gameLvl * 0.002f;
                fireIntvl = fireIntvl > 0.05f ? fireIntvl : 0.05f;
                ammoAll = 50 + gameLvl * 5f;
                ammoCur = ammoAll;
                break;
            case 4:
                fireIntvl = 0.8f - gameLvl * 0.030f;
                fireIntvl = fireIntvl > 0.30f ? fireIntvl : 0.30f;
                ammoAll = 10 + gameLvl * 1f;
                ammoCur = ammoAll;
                break;
        }
    }
}