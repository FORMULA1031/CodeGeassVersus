using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using Photon.Pun;
using UnityEngine.InputSystem;

public class KMF_Control : MonoBehaviour
{
    /***********
        定数
    ***********/
    const float WALK_SPEED   = 15;
    const float BOOST_SPEED  = 60;
    const float JUMP_POWER   = 80;
    const int BOOST_MAX      = 100;
    /*********************
        時間管理用変数
    *********************/
    float jumpRugTime = 0;
    float landingTime = 0;
    float boostButtonTime = 0;
    float boostConsumedTime = 0;
    float slideTime = 0;
    float mainShootingTime = 0;
    public float mainShootingCurrentReloadTime = 0;
    public float mainShootingReloadTime;
    float subShootingTime = 0;
    public float subShootingCurrentReloadTime = 0;
    public float subShootingReloadTime;
    float subShootingFightingVariantsTime = 0;
    float specialShootingTime = 0;
    public float specialShootingCurrentReloadTime = 0;
    public float specialShootingReloadTime;
    float attackTime = 0;
    float attackFinishTime = 1.8f;
    float specialAttackTime = 0;
    public float fightingChargeTime = 0;
    public float fightingChargeMaxTime;
    public float fightingChargeCurrentReloadTime = 0;
    public float fightingChargeReloadTime;
    public float floatUnitCurrentTime = 0;
    public float floatUnitTime;
    float replacementTime = 0;
    float floatUnitPresentLocation;
    float staggerTime = 0;
    float correctionFactor = 1;
    float correctionFactorResetTime = 0;
    float downValue = 0;
    float leverFrontTime = 0;
    float leverBackTime = 0;
    float leverRightTime = 0;
    float leverLeftTime = 0;
    float defenseTime = 0;
    float defenseLeverTime = 0;
    float defendingTime = 0;
    float stepTime = 0;
    /*********************
        フラグ用変数
    *********************/
    public bool isFightingChargeInput = false;
    public bool isFloatUnit = false;
    public bool isInductionOff = false;
    public bool isDown = false;
    public bool isTypeGroundRunning;
    bool isJump = false;
    bool isJumpMove = false;
    bool isRise = false;
    bool isLeverInsert = false;
    bool isLeverFront = false;
    bool isLeverBack = false;
    bool isLeverRight = false;
    bool isLeverLeft = false;
    bool isLanding = false;
    bool isAir = false;
    bool isBoost = false;
    bool isSlide = false;
    bool isStiffness = false;
    bool isIncapableAction = false;
    bool isMainShooting = false;
    bool isMainShootingFiring = false;
    bool isSubShooting = false;
    bool isSubShootingFiring = false;
    bool isSubShootingFightingVariants = false;
    bool isSubShootingFightingVariantsInAir = false;
    bool isAttack = false;
    bool isAttack1 = false;
    bool isAttack2 = false;
    bool isAttack3 = false;
    bool isSpecialShooting = false;
    bool isSpecialShootingAnimation = false;
    bool isSpecialShootingFiring = false;
    bool isReplacement = false;
    bool isInduction = true;
    bool isStagger = false;
    bool isUnderAttack = false;
    bool isDefense = false;
    bool isDefending = false;
    bool isStep = false;
    bool isStepJump = false;
    bool isJumpKeyPressing = false;
    bool isHitStartStay = false;
    bool isSpecialAttack = false;

    Rigidbody rb;
    PhotonView pv;
    public float boost_amount;
    
    public int durable_value;
    public int durable_maxvalue;
    public int mainshooting_number;
    public int mainshooting_maxnumber;
    public int subshooting_number;
    public int subshooting_maxnumber;
    public int specialshooting_number;
    public int specialshooting_maxnumber;
    string lastmove_name;
    string lastlever_name;
    Vector3 movingdirection;
    Vector3 movingvelocity;
    Vector3 latestPos;
    Vector3 attack_moving;
    Vector3 specialattack_moving;
    Vector3 defenserecoil;
    Vector3 step_move;
    Vector3 other_forward;
    public Animator anim;
    GameObject MainCamera;
    public GameObject LockOnEnemy;
    public GameObject Varis_Normal;
    public GameObject Beam;
    public GameObject SlashHarken;
    GameObject SlashHarken_Instance;
    public GameObject Lancelot_ShashHarken;
    public GameObject Varis_FullPower;
    public GameObject Beam_FullPower;
    public GameObject MVS_R;
    public GameObject MVS_L;
    public GameObject MVSSheathing_R;
    public GameObject MVSSheathing_L;
    public GameObject BlazeLuminous;
    public GameObject Leg_R;
    public GameObject Spark_R;
    public GameObject Spark_L;
    public GameObject EffDust_R;
    public GameObject EffDust_L;
    public GameObject Cockpit;
    public GameObject FloatUnit;
    GameObject _FloatUnit;
    GameObject Eff_SpeedLine;
    GameObject Eff_StepLine;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        MainCamera = GameObject.Find("Main Camera");
        mainshooting_maxnumber = mainshooting_number;
        subshooting_maxnumber = subshooting_number;
        specialshooting_maxnumber = specialshooting_number;
        durable_value = durable_maxvalue;
        boost_amount = BOOST_MAX;
        Eff_SpeedLine = transform.Find("Eff_SpeedLine").gameObject;
        Eff_StepLine = transform.Find("Eff_StepLine").gameObject;
        Eff_SpeedLine.SetActive(false);
        Eff_StepLine.SetActive(false);
        EffDust_R.GetComponent<ParticleSystem>().Stop();
        EffDust_L.GetComponent<ParticleSystem>().Stop();
        if(SceneManager.GetActiveScene().name == "TrainingScene" || pv.IsMine)
        {
            GetComponent<PlayerInput>().enabled = true;
        }
        else if(!pv.IsMine)
        {
            Destroy(GetComponent<Rigidbody>());
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv != null)
        {
            if (pv.IsMine)
            {
                UpdateControl();
            }
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                UpdateControl();
            }
        }
    }

    void UpdateControl()
    {
        FirstSetting();

        if (boost_amount <= 0)
        {
            isIncapableAction = true;
        }
        if (correctionFactor < 1)
        {
            correctionFactorResetTime += Time.deltaTime;
            if (correctionFactorResetTime >= 3f)
            {
                correctionFactor = 1;
                downValue = 0;
            }
        }

        if (!(isDefense || isSubShootingFightingVariants) && !rb.useGravity)
        {
            rb.useGravity = true;
        }
        if (!isDown && !isStagger)
        {
            MoveKeyControls();
            JumpKeyControls();
        }
        ShootingKeyControls();
        SubShootingControls();
        SpecialShootingControls();
        AttackControls();
        SpecialAttackControls();
        FightingChargeControl();
        LandingTime();
        Stagger_Control();
    }

    void FirstSetting()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            RpcEffControl();
            if (!isBoost)
            {
                RpcEffControl_NotBoostFlag();
            }
        }
        else
        {
            pv.RPC(nameof(RpcEffControl), RpcTarget.AllBufferedViaServer);
            if (!isBoost)
            {
                pv.RPC(nameof(RpcEffControl_NotBoostFlag), RpcTarget.AllBufferedViaServer);
            }
        }
        if (LockOnEnemy == null)
        {
            LockOnEnemy = transform.GetComponent<PlayerID_Control>().LockOnEnemy;
            if (LockOnEnemy != null)
            {
                /*Rig_Setting("Lancelot/Rig 1/Waite_Rig", LockOnEnemy);
                Rig_Setting("Lancelot/Rig 1/Head_Rig", LockOnEnemy);
                Rig_Setting("Lancelot/Rig 1/Arm_Rig", LockOnEnemy);
                transform.GetComponent<RigBuilder>().Build();*/
            }
        }
    }

/// <summary>
/// ///////////////////////////////////////////////////////////
/// </summary>

    void FixedUpdate()
    {
        if (pv != null)
        {
            if (pv.IsMine)
            {
                FixedUpdateControl();
            }
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                FixedUpdateControl();
            }
        }
    }

    void FixedUpdateControl()
    {
        if (!isStiffness)
        {
            if (!isIncapableAction)
            {
                if (!isLanding && !isSlide)
                {
                    if (isLeverInsert && !isDefense && lastmove_name != "jump")
                    {
                        if (isTypeGroundRunning || !isBoost)
                        {
                            rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
                        }
                        else
                        {
                            rb.velocity = new Vector3(movingvelocity.x, 0, movingvelocity.z);
                        }
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(true);
                        }
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, true);
                        }
                        KMF_Rotation();
                    }
                    else if (lastmove_name == "step")
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(false);
                        }
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                        }
                    }
                    else
                    {
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(false);
                        }
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                        }
                    }
                }
                else if (isSlide)
                {
                    if (SceneManager.GetActiveScene().name == "TrainingScene")
                    {
                        RpcSparkControl(false);
                    }
                    else
                    {
                        pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                    }
                }
                gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            }
        }
        else
        {
            if (isDown || isSpecialAttack || isSubShootingFightingVariants || isAttack)
            {
            }
            else if (isStep || isStagger)
            {
                GravityOff();
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    RpcSparkControl(false);
                }
                else
                {
                    pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                }
            }
            else
            {
                Rigidity();
            }
        }
    }

    //移動方向と歩く処理
    void MoveKeyControls()
    {
        float x = Input.GetAxis("Move_X");
        float z = Input.GetAxis("Move_Y") * -1;
        movingdirection = MainCamera.transform.forward * z + MainCamera.transform.right * x;
        if(x != 0 || z != 0)
        {
            isLeverInsert = true;
            anim.SetBool("Walk", true);
        }
        else
        {
            isLeverInsert = false;
            lastlever_name = "null";
            anim.SetBool("Walk", false);
        }
        movingdirection.Normalize();
        if (!isBoost && !isSlide)
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * WALK_SPEED;
        }
        if (!isLanding)
        {
            DefenseAndStepControl(x, z);
        }
    }

    //レバーの制御
    void DefenseAndStepControl(float x, float z)
    {
        if (Mathf.Abs(x) >= 0.8f || Mathf.Abs(z) >= 0.8f)
        {
            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                //レバー右の場合
                if (x > 0)
                {
                    if (lastlever_name == "null" && isLeverRight && leverRightTime < 0.2f && !isStep)
                    {
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * WALK_SPEED;
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 90, 0);
                            }
                            else
                            {
                                pv.RPC(nameof(StepLineDirection), RpcTarget.AllBufferedViaServer, new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 90, 0));
                            }
                                step_move.y = 0;
                            StepStart();
                        }
                    }
                    lastlever_name = "right";
                    leverRightTime = 0;
                    isLeverRight = true;
                    isLeverFront = false;
                    isLeverBack = false;
                    isLeverLeft = false;
                }
                //レバー左の場合
                else if (x < 0)
                {
                    if (lastlever_name == "null" && isLeverLeft && leverLeftTime < 0.2f && !isStep)
                    {
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * -WALK_SPEED;
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y - 90, 0);
                            }
                            else
                            {
                                pv.RPC(nameof(StepLineDirection), RpcTarget.AllBufferedViaServer, new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y - 90, 0));
                            }
                            step_move.y = 0;
                            StepStart();
                        }
                    }
                    lastlever_name = "left";
                    leverLeftTime = 0;
                    isLeverLeft = true;
                    isLeverFront = false;
                    isLeverBack = false;
                    isLeverRight = false;
                }
                defenseLeverTime = 2f;
            }
            else
            {
                //レバー前の場合
                if (z > 0)
                {
                    if (isLeverBack && !isUnderAttack)
                    {
                        if (!isDefense && !isUnderAttack)
                        {
                            Vector3 direction;
                            isDefense = true;
                            defenseTime = 0;
                            defenseLeverTime = 0;
                            boostConsumedTime = 0;
                            isStiffness = true;
                            rb.useGravity = false;
                            isUnderAttack = true;
                            isBoost = false;
                            anim.SetBool("Boost_Landing", false);
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                BlazeLuminous.SetActive(true);
                            }
                            else
                            {
                                pv.RPC(nameof(BlazeLuminousExpand), RpcTarget.AllBufferedViaServer);
                            }
                            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            if (LockOnEnemy != null)
                            {
                                transform.LookAt(LockOnEnemy.transform);
                            }
                            direction = transform.eulerAngles;
                            direction.x = 0;
                            direction.z = 0;
                            transform.eulerAngles = direction;
                        }
                    }
                    defenseLeverTime += Time.deltaTime;

                    if (lastlever_name == "null" && isLeverFront && leverFrontTime < 0.2f && !isStep)
                    {
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * WALK_SPEED;
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y, 0);
                            }
                            else
                            {
                                pv.RPC(nameof(StepLineDirection), RpcTarget.AllBufferedViaServer, new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y, 0));
                            }
                            step_move.y = 0;
                            StepStart();
                        }
                    }
                    lastlever_name = "front";
                    leverFrontTime = 0;
                    isLeverFront = true;
                    isLeverBack = false;
                    isLeverLeft = false;
                    isLeverRight = false;
                }
                //レバー下の場合
                else if (z < 0)
                {
                    if(lastlever_name == "null" && isLeverBack && leverBackTime < 0.2f && !isStep)
                    {
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * -WALK_SPEED;
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 180, 0);
                            }
                            else
                            {
                                pv.RPC(nameof(StepLineDirection), RpcTarget.AllBufferedViaServer, new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 180, 0));
                            }
                            step_move.y = 0;
                            StepStart();
                        }
                    }
                    lastlever_name = "back";
                    leverBackTime = 0;
                    isLeverBack = true;
                    isLeverFront = false;
                    isLeverLeft = false;
                    isLeverRight = false;
                    defenseLeverTime = 2f;
                }
            }
        }
        else
        {
            defenseLeverTime = 2f;
        }

        //ステップ受付時間
        if (isLeverFront || lastlever_name == "front")
        {
            leverFrontTime += Time.deltaTime;
            if (leverFrontTime >= 0.2f)
            {
                isLeverFront = false;
            }
        }
        if (isLeverBack || lastlever_name == "back")
        {
            leverBackTime += Time.deltaTime;
            if (leverBackTime >= 0.2f)
            {
                isLeverBack = false;
            }
        }
        if (isLeverRight || lastlever_name == "right")
        {
            leverRightTime += Time.deltaTime;
            if (leverRightTime >= 0.2f)
            {
                isLeverRight = false;
            }
        }
        if (isLeverLeft || lastlever_name == "left")
        {
            leverLeftTime += Time.deltaTime;
            if(leverLeftTime >= 0.2f)
            {
                isLeverLeft = false;
            }
        }

        //ガード構え中の制御
        if (isDefense)
        {
            anim.SetBool("Defense", true);
            
            if ((defenseLeverTime >= 2f && !isDefending) || isIncapableAction)
            {
                defenseTime += Time.deltaTime;
            }

            //ガード終了
            if (defenseTime >= 0.5f)
            {
                DefenseFinish();
            }

            //ブースト消費
            boostConsumedTime += Time.deltaTime;
            if(boostConsumedTime > 0.01f)
            {
                boost_amount -= 0.1f;
                boostConsumedTime = 0;
            }
        }

        //ガード成功中の処理
        if (isDefending)
        {
            isStiffness = false;
            defendingTime += Time.deltaTime;
            if(defendingTime <= 0.2f)
            {
                rb.velocity = new Vector3(defenserecoil.x, defenserecoil.y, defenserecoil.z);
            }
            else
            {
                isStiffness = true;
            }
            if(defendingTime >= 0.5f)
            {
                isDefending = false;
                defendingTime = 0;
            }
        }

        //ステップ処理
        if (isStep)
        {
            if (isSlide)
            {
                SlideFinish();
            }

            stepTime += Time.deltaTime;
            if(stepTime < 0.1)
            {
                isInductionOff = true;
            }
            if (stepTime >= 0.05f)
            {
                isInductionOff = false;
                if (anim.GetBool("Step_Front"))
                {
                    anim.SetBool("Step_Back", false);
                    anim.SetBool("Step_Right", false);
                    anim.SetBool("Step_Left", false);
                }
                else if (anim.GetBool("Step_Back"))
                {
                    anim.SetBool("Step_Front", false);
                    anim.SetBool("Step_Right", false);
                    anim.SetBool("Step_Left", false);
                }
                else if (anim.GetBool("Step_Right"))
                {
                    anim.SetBool("Step_Front", false);
                    anim.SetBool("Step_Back", false);
                    anim.SetBool("Step_Left", false);
                }
                else if (anim.GetBool("Step_Left"))
                {
                    anim.SetBool("Step_Front", false);
                    anim.SetBool("Step_Back", false);
                    anim.SetBool("Step_Right", false);
                }
                rb.velocity = step_move;
            }
            if(stepTime >= 0.3f)
            {
                if (!isAir)
                {
                    boost_amount = BOOST_MAX;
                }
                StepFinish();
            }
            lastmove_name = "step";
        }
    }

    //ガード終了処理
    void DefenseFinish()
    {
        isDefense = false;
        anim.SetBool("Defense", false);
        isStiffness = false;
        isUnderAttack = false;
        rb.useGravity = true;
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            BlazeLuminous.SetActive(false);
        }
        else
        {
            pv.RPC(nameof(BlazeLuminousClose), RpcTarget.AllBufferedViaServer);
        }
    }

    //ステップ開始時の処理
    void StepStart()
    {
        float KmfLookingAtCamera_rotation = Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y, 360))
            - Mathf.Abs(Mathf.Repeat(MainCamera.transform.localEulerAngles.y, 360));
        if(KmfLookingAtCamera_rotation  < 0)
        {
            KmfLookingAtCamera_rotation += 360;
        }

        if (KmfLookingAtCamera_rotation < 45f || KmfLookingAtCamera_rotation > 315f)
        {
            StepAnimationSelect("Step_Front", "Step_Back", "Step_Right", "Step_Left");
        }
        else if(KmfLookingAtCamera_rotation >= 45f && KmfLookingAtCamera_rotation <= 135f)
        {
            StepAnimationSelect("Step_Left", "Step_Right", "Step_Front", "Step_Back");
        }
        else if(KmfLookingAtCamera_rotation > 135f && KmfLookingAtCamera_rotation < 225f)
        {
            StepAnimationSelect("Step_Back", "Step_Front", "Step_Left", "Step_Right");
        }
        else if(KmfLookingAtCamera_rotation >= 225f && KmfLookingAtCamera_rotation <= 315f)
        {
            StepAnimationSelect("Step_Right", "Step_Left", "Step_Back", "Step_Front");
        }

        float step_speed = 4.0f;
        if (lastmove_name == "attack")
        {
            step_speed = 5f;
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcFightGradientChange_StepLine();
            }
            else
            {
                pv.RPC(nameof(RpcFightGradientChange_StepLine), RpcTarget.AllBufferedViaServer);
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcNormalGradientChange_StepLine();
            }
            else
            {
                pv.RPC(nameof(RpcNormalGradientChange_StepLine), RpcTarget.AllBufferedViaServer);
            }
        }
        step_move *= step_speed;

        AttackFinish();
        BoostFinish();
        isStiffness = true;
        isStep = true;
        isInductionOff = true;
        stepTime = 0;
        boost_amount -= 20;
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            Eff_StepLine.SetActive(true);
        }
        else
        {
            pv.RPC(nameof(EffSetActive), RpcTarget.AllBufferedViaServer, true);
        }
    }

    //ステップ中のアニメーション決め
    void StepAnimationSelect(string stepfront, string stepback, string stepright, string stepleft)
    {

        if (isLeverFront)
        {
            anim.SetBool(stepfront, true);
        }
        if (isLeverBack)
        {
            anim.SetBool(stepback, true);
        }
        if (isLeverRight)
        {
            anim.SetBool(stepright, true);
        }
        if (isLeverLeft)
        {
            anim.SetBool(stepleft, true);
        }
    }

    //ステップ終了処理
    void StepFinish()
    {
        isInductionOff = false;
        isStiffness = false;
        isStep = false;
        stepTime = 0;
        lastmove_name = "null";
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            Eff_StepLine.SetActive(false);
        }
        else
        {
            pv.RPC(nameof(EffSetActive), RpcTarget.AllBufferedViaServer, false);
        }
        anim.SetBool("Step_Front", false);
        anim.SetBool("Step_Back", false);
        anim.SetBool("Step_Right", false);
        anim.SetBool("Step_Left", false);
    }

    //機体の向き制御
    void KMF_Rotation()
    {
        if (isLeverInsert)
        {
            Vector3 diff = transform.position - new Vector3(latestPos.x, transform.position.y, latestPos.z);
            latestPos = transform.position;
            if (diff != Vector3.zero)
            {
                float turning_angle = 0.1f;
                if(isBoost)
                {
                    turning_angle = 0.02f;
                }
                else if (isAir)
                {
                    turning_angle = 0;
                }
                Quaternion rotation = Quaternion.LookRotation (movingdirection);
                transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, turning_angle);
            }
        }
    }

    //機体の急な向きの更新時に使用
    void KMF_RapidRotation()
    {
        if (isLeverInsert)
        {
            float x = Input.GetAxis("Move_X");
            float z = Input.GetAxis("Move_Y") * -1;
            Vector2 snapped = SnapAngle(new Vector2(x, z), 45f * Mathf.Deg2Rad);
            Vector3 direction = MainCamera.transform.eulerAngles;
            if (snapped.x > 0.38f)
            {
                if (snapped.y <= 0.86f && snapped.y > 0.38f)
                {
                    direction.y += 45;
                }
                else if (snapped.y >= -0.86f && snapped.y < -0.38f)
                {
                    direction.y += 135;
                }
                else
                {
                    direction.y += 90;
                }
            }
            else if (snapped.x < -0.38f)
            {
                if (snapped.y <= 0.86f && snapped.y > 0.38f)
                {
                    direction.y += -45;
                }
                else if (snapped.y >= -0.86f && snapped.y < -0.38f)
                {
                    direction.y += -135;
                }
                else
                {
                    direction.y += -90;
                }
            }
            else if (snapped.y > 0.38f)
            {
                direction.y += 0;
            }
            else if (snapped.y < -0.38f)
            {
                direction.y += 180;
            }
            transform.eulerAngles = new Vector3(0, direction.y, 0);
        }
    }

    //レバー方向を取得
    Vector2 SnapAngle(Vector2 vector, float angleSize)
    {
        var angle = Mathf.Atan2(vector.y, vector.x);

        var index = Mathf.RoundToInt(angle / angleSize);
        var snappedAngle = index * angleSize;
        var magnitude = vector.magnitude;
        return new Vector2(
            Mathf.Cos(snappedAngle) * magnitude,
            Mathf.Sin(snappedAngle) * magnitude);
    }

    //ジャンプボタン処理
    void JumpKeyControls()
    {
        if (!isIncapableAction)
        {
            if (isBoost)
            {
                Boosting();
            }
            //ホバー機体の上昇終了処理
            if (!isJump) { }
            else if (!isTypeGroundRunning && !isReplacement)
            {
                if (isJump && !isBoost && !isJumpKeyPressing)
                {
                    if (!isStiffness || isStep)
                    {
                        if (jumpRugTime >= 0.3f)
                        {
                            isRise = false;
                            isJump = false;
                        }
                    }
                }
            }
        }

        //ブースト処理
        if (isJump && !isBoost)
        {
            if (!isStiffness || isStep)
            {
                //ブースト開始1回のみ実行
                if (isJumpMove)
                {
                    if (!isStep)
                    {
                        KMF_RapidRotation();
                    }
                    else
                    {
                        isStepJump = true;
                    }
                    StepFinish();
                    isJumpMove = false;
                }

                //地走機体の場合
                if (isTypeGroundRunning)
                {
                    jumpRugTime += Time.deltaTime;
                    if (jumpRugTime >= 0.2f && !isRise)
                    {
                        JumpStart();
                    }
                    else if (jumpRugTime <= 1.0f && jumpRugTime >= 0.2f)
                    {
                        Jumping();
                        isRise = false;
                        isJump = false;
                    }
                }
                //ホバー機体の場合
                else
                {
                    jumpRugTime += Time.deltaTime;
                    if (jumpRugTime >= 0.2f && !isRise)
                    {
                        JumpStart();
                    }
                    else if (jumpRugTime >= 0.2f)
                    {
                        Jumping();
                        boost_amount -= Time.deltaTime * 10f;
                    }
                }
            }
            else
            {
                if (isUnderAttack || isStep)
                {
                    jumpRugTime += Time.deltaTime;
                    if (jumpRugTime >= 0.2f && !isRise)
                        isJump = false;
                }
                else
                {
                    isJump = false;
                }
            }
        }
        else
        {
            anim.SetBool("Jump", false);
        }

        //ブースト終了
        if ((!isLeverInsert && !Input.GetButton("Boost")) || isIncapableAction)
        {
            if (isBoost)
            {
                BoostFinish();
                if (!isAir)
                {
                    isSlide = true;
                    isUnderAttack = true;
                }
            }
        }

        //ズサ中の処理
        if (isSlide)
        {
            anim.SetBool("Boost_Landing_Finish", true);
            slideTime += Time.deltaTime;
            if(slideTime > 1.5f)
            {
                SlideFinish();
                boost_amount = BOOST_MAX;
            }
        }
    }

    //ジャンプ開始の処理
    void JumpStart()
    {
        if (!isLanding)
        {
            lastmove_name = "jump";
            anim.SetBool("Jump", true);
            anim.SetBool("Boost_Landing", false);
            boost_amount -= 10;
            isRise = true;
        }
    }

    //ジャンプ中の処理
    void Jumping()
    {
        if (!isLanding)
        {
            Vector3 jump_direction;
            Vector3 jump_moving;

            jump_direction = gameObject.transform.up * 1;
            jump_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
            jump_moving = jump_direction * JUMP_POWER;
            movingvelocity = rb.velocity;
            if (isStepJump)
            {
                movingvelocity /= 2;
            }
            rb.velocity = new Vector3(movingvelocity.x, jump_moving.y, movingvelocity.z);
        }
    }

    //ブースト中の処理
    void Boosting()
    {
        if (!isTypeGroundRunning)
        {
            rb.useGravity = false;
        }
        if (movingdirection != new Vector3(0, 0, 0) && isLeverInsert)
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * BOOST_SPEED;
        }
        else
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * BOOST_SPEED;
            rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
        }
        boostConsumedTime += Time.deltaTime;
        if (boostConsumedTime >= 0.1f)
        {
            boost_amount -= 2;
            boostConsumedTime = 0;
        }
    }

    //ブースト終了の処理
    void BoostFinish()
    {
        isBoost = false;
        rb.useGravity = true;
        lastmove_name = "boost";
        Eff_SpeedLine.SetActive(false);
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            RpcEffDustStop_Control();
        }
        else
        {
            pv.RPC(nameof(RpcEffDustStop_Control), RpcTarget.AllBufferedViaServer);
        }
        //EffDust_R.GetComponent<ParticleSystem>().Stop();
        //EffDust_L.GetComponent<ParticleSystem>().Stop();
        anim.SetBool("Boost_Landing", false);
    }

    //ズサ終了の処理
    void SlideFinish()
    {
        anim.SetBool("Boost_Landing_Finish", false);
        isSlide = false;
        slideTime = 0;
        isIncapableAction = false;
        isUnderAttack = false;
    }

    //着地硬直処理
    void LandingTime()
    {
        if (isLanding && (!isUnderAttack || lastmove_name == "mainshooting"))
        {
            anim.SetBool("Landing", true);
            landingTime += Time.deltaTime;
            if(landingTime >= 0.5f)
            {
                lastmove_name = "null";
                isLanding = false;
                landingTime = 0;
                anim.SetBool("Landing", false);
                isUnderAttack = false;
            }
        }
    }

    //上半身のみ相手の方向へ向く処理
    void Rig_Setting(string rig_name, GameObject _LockOnEnemy)
    {
        var sourceobject = gameObject.transform.Find(rig_name).gameObject.transform.GetComponent<MultiAimConstraint>().data.sourceObjects;
        sourceobject.SetTransform(0, _LockOnEnemy.transform);
        gameObject.transform.Find(rig_name).gameObject.transform.GetComponent<MultiAimConstraint>().data.sourceObjects = sourceobject;
    }

    ////////////////////////////////////////////////////////

    //メイン射撃ボタンの処理
    void ShootingKeyControls()
    {
        //メイン射撃開始処理
        if (isMainShooting)
        {
            mainShootingTime += Time.deltaTime;
            if(mainShootingTime >= 0.23f && !isMainShootingFiring)
            {
                mainshooting_number--;
                isMainShootingFiring = true;
                GameObject _beam;
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    _beam = Instantiate(Beam, Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                else
                {
                    _beam = PhotonNetwork.Instantiate("Prefab/Beam(Green)", Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                if (LockOnEnemy != null)
                {
                    _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                else
                {
                    _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
            }
            if(mainShootingTime >= 1.0f)
            {
                MainShootingFinish();
                isUnderAttack = false;
            }

            if(LockOnEnemy == null)
            {
                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
            }
        }

        //メイン射撃のリロード処理
        if (mainshooting_maxnumber > mainshooting_number)
        {
            mainShootingCurrentReloadTime += Time.deltaTime;
            if(mainShootingCurrentReloadTime >= mainShootingReloadTime)
            {
                mainshooting_number++;
                mainShootingCurrentReloadTime = 0;
            }
        }
    }

    //メイン射撃終了処理
    void MainShootingFinish()
    {
        isMainShooting = false;
        isStiffness = false;
        anim.SetBool("MainShooting", false);
        gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
    }

    //サブ射撃ボタンの処理
    void SubShootingControls()
    {
        //サブ射撃の開始処理
        if (isSubShooting)
        {
            subShootingTime += Time.deltaTime;
            if(subShootingTime >= 0.3f && !isSubShootingFiring)
            {
                subshooting_number--;
                isSubShootingFiring = true;
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    SlashHarken_Instance = Instantiate(SlashHarken, Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                else
                {
                    SlashHarken_Instance = PhotonNetwork.Instantiate("Prefab/SlashHarken(Object)", Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                Vector3 SlashHarkenAngle = SlashHarken_Instance.transform.eulerAngles;
                SlashHarkenAngle.y -= 180.0f; // ���[���h���W����ɁAy�������ɂ�����]��10�x�ɕύX
                SlashHarkenAngle.z += 90.0f; // ���[���h���W����ɁAz�������ɂ�����]��10�x�ɕύX
                SlashHarken_Instance.transform.eulerAngles = SlashHarkenAngle; // ��]�p�x��ݒ�
                if (LockOnEnemy != null)
                {
                    SlashHarken_Instance.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                else
                {
                    SlashHarken_Instance.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
                lastmove_name = "subshooting";
            }
            if (subShootingTime >= 0.8f && subShootingTime < 1.5f)
            {
                anim.SetBool("SubShooting_Start", false);
                anim.SetBool("SubShooting_Finish", true);
                if (SlashHarken_Instance != null)
                {
                    Destroy(SlashHarken_Instance);
                }
            }
            if (subShootingTime >= 1.5f)
            {
                SubShootingFinish();
                isUnderAttack = false;
            }

            if(SlashHarken_Instance != null)
            {
                SlashHarken_Instance.transform.Find("SlashHarken/Bone.001/Bone.004").transform.position =BlazeLuminous.transform.position;
            }
        }

        //サブ射撃のリロード処理
        if (subshooting_maxnumber > subshooting_number)
        {
            subShootingCurrentReloadTime += Time.deltaTime;
            if (subShootingCurrentReloadTime >= subShootingReloadTime)
            {
                subshooting_number++;
                subShootingCurrentReloadTime = 0;
            }
        }
    }

    //サブ射撃終了処理
    void SubShootingFinish()
    {
        anim.SetBool("SubShooting_Start", false);
        anim.SetBool("SubShooting_Finish", false);
        isSubShooting = false;
        isStiffness = false;
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        if (SlashHarken_Instance != null)
        {
            Destroy(SlashHarken_Instance);
        }
    }

    //特殊射撃ボタン制御
    void SpecialShootingControls()
    {
        //特殊射撃開始処理
        if (!isIncapableAction
            && (!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
        {
            if (!isSpecialShooting)
            {

            }
            else if (isSpecialShooting && isSpecialShootingAnimation)
            {
                anim.SetBool("SpecialShooting_Start", false);
                isSpecialShootingAnimation = false;
            }
        }

        //特殊射撃中
        if (isSpecialShooting)
        {
            specialShootingTime += Time.deltaTime;
            if (specialShootingTime >= 0.6f && !isSpecialShootingFiring)
            {
                specialshooting_number--;
                isSpecialShootingFiring = true;
                GameObject _Beam_FullPower;
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    _Beam_FullPower = Instantiate(Beam_FullPower, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.position, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                else
                {
                    _Beam_FullPower = PhotonNetwork.Instantiate("Prefab/Beam_Full", Varis_FullPower.transform.Find("Muzzle").gameObject.transform.position, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                if (LockOnEnemy != null)
                {
                    _Beam_FullPower.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                else
                {
                    _Beam_FullPower.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
            }
            if (specialShootingTime >= 1.5f)
            {
                SpecialShootingFinish();
            }
        }

        //特殊射撃リロード処理
        if (specialshooting_number == 0)
        {
            specialShootingCurrentReloadTime += Time.deltaTime;
            if (specialShootingCurrentReloadTime >= specialShootingReloadTime)
            {
                specialshooting_number = specialshooting_maxnumber;
                specialShootingCurrentReloadTime = 0;
            }
        }
    }

    //特殊射撃終了処理
    void SpecialShootingFinish()
    {
        anim.SetBool("SpecialShooting", false);
        isSpecialShooting = false;
        isStiffness = false;
        isUnderAttack = false;
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    //格闘ボタンの制御
    void AttackControls()
    {
        Vector3 attack_direction;

        //格闘開始処理
        if (isAttack)
        {
            attackTime += Time.deltaTime;
            if (attackTime <= 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                if (isInduction)
                {
                    if (LockOnEnemy != null)
                    {
                        gameObject.transform.LookAt(LockOnEnemy.transform);
                    }
                    attack_direction = gameObject.transform.forward * 1;
                    attack_direction.Normalize();
                    attack_moving = attack_direction * BOOST_SPEED * 1f;
                    rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
                }
            }
            else if (attackTime > 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                attackTime = 0;
                attackFinishTime = 1.0f;
                isAttack1 = true;
                anim.SetBool("Attack_Induction", false);
                anim.SetBool("Attack1", true);
                Rigidity();
            }
            else
            {
                attack_direction = gameObject.transform.forward * 1;
                attack_direction.Normalize();
                attack_moving = attack_direction * 10f;
                rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
            }
            if (attackTime >= attackFinishTime)
            {
                AttackFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            }
        }

        //サブ格闘派生開始
        if (isSubShootingFightingVariants)
        {
            subShootingFightingVariantsTime += Time.deltaTime;
            if (subShootingFightingVariantsTime <= 0.2f)
            {
                if (isInduction && LockOnEnemy != null)
                {
                    gameObject.transform.LookAt(LockOnEnemy.transform);
                }
                attack_direction = gameObject.transform.forward * 1;
                attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                attack_moving = attack_direction * BOOST_SPEED;
                rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
                isSubShootingFightingVariantsInAir = isAir;
            }
            if (subShootingFightingVariantsTime >= 2.0f)
            {
                SubShootingFightingVariantsFinish();
                if (!isSubShootingFightingVariantsInAir)
                {
                    isLanding = true;
                    boost_amount = BOOST_MAX;
                    isIncapableAction = false;
                }
            }
        }
    }

    //格闘開始時の処理
    public void StartAttack()
    {
        if (isAttack)
        {
            if (attackTime < 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                attackTime = 0;
                attackFinishTime = 1.0f;
                isAttack1 = true;
                anim.SetBool("Attack1", true);
                Rigidity();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_Start"))
            {
                anim.SetBool("Attack_Induction", false);
            }
        }
    }

    //格闘終了処理
    void AttackFinish()
    {
        anim.SetBool("Attack_Induction", false);
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        isAttack = false;
        isAttack1 = false;
        isAttack2 = false;
        isAttack3 = false;
        isStiffness = false;
        isUnderAttack = false;
        attackTime = 0;
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        MVS_R.SetActive(false);
        MVS_L.SetActive(false);
        MVSSheathing_R.SetActive(true);
        MVSSheathing_L.SetActive(true);
        Eff_SpeedLine.SetActive(false);
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    //サブ格闘派生終了処理
    void SubShootingFightingVariantsFinish()
    {
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        anim.SetBool("SubShooting_FightingVariants", false);
        isSubShootingFightingVariants = false;
        isStiffness = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        isUnderAttack = false;
        rb.useGravity = true;
        Leg_R.transform.GetComponent<BoxCollider>().enabled = false;
    }

    //特殊格闘ボタンの処理
    void SpecialAttackControls()
    {
        if (isSpecialAttack)
        {
            specialAttackTime += Time.deltaTime;
            if (specialAttackTime <= 0.4f)
            {
                rb.velocity = new Vector3(specialattack_moving.x, rb.velocity.y, specialattack_moving.z);
            }
            if (specialAttackTime >= 1.2f)
            {
                Varis_Normal.SetActive(true);
                Varis_FullPower.SetActive(false);
                SpecialAttackFinish();
                isUnderAttack = false;
            }
        }
    }

    //特殊格闘終了処理
    void SpecialAttackFinish()
    {
        anim.SetBool("SpecialAttack", false);
        isSpecialAttack = false;
        isStiffness = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //格闘CS処理
    void FightingChargeControl()
    {
        if (fightingChargeCurrentReloadTime < fightingChargeReloadTime && !isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeCurrentReloadTime += Time.deltaTime;
            if(fightingChargeCurrentReloadTime >= fightingChargeReloadTime)
            {
                isFightingChargeInput = true;
            }
        }
        else if (Input.GetButtonUp("Attack") && fightingChargeTime >= fightingChargeMaxTime && !isFloatUnit &&
            !isIncapableAction && !isUnderAttack && !isLanding)
        {
            Vector3 InstanceFloatUnit_postion = Cockpit.transform.Find("InstanceFloatUnit").transform.position;
            Quaternion InstanceFloatUnit_rotation = transform.rotation;
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                _FloatUnit = Instantiate(FloatUnit, InstanceFloatUnit_postion, InstanceFloatUnit_rotation);
            }
            else
            {
                _FloatUnit = PhotonNetwork.Instantiate("Prefab/FloatUnit", InstanceFloatUnit_postion, InstanceFloatUnit_rotation);
            }
            _FloatUnit.transform.parent = Cockpit.transform;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            fightingChargeCurrentReloadTime = 0;
            fightingChargeTime = 0;
            floatUnitPresentLocation = 0;
            isFightingChargeInput = false;
            isFloatUnit = true;
            BoostFinish();
            StepFinish();
            floatUnitCurrentTime = floatUnitTime;
            isTypeGroundRunning = false;
            anim.SetBool("Replacement", true);
            isReplacement = true;
            isStiffness = true;
            isUnderAttack = true;
            lastmove_name = "fightingchargeshot";
        }
        else if (Input.GetButton("Attack") && isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeTime += Time.deltaTime;
        }
        else if(isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeTime -= Time.deltaTime;
        }

        fightingChargeTime = ChargeControl(fightingChargeTime, fightingChargeMaxTime);

        if (isFloatUnit || isFightingChargeInput)
        {
            if (_FloatUnit != null)
            {
                _FloatUnit.transform.localEulerAngles = new Vector3(77, 0, 0);
                float distance_two = Vector3.Distance(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.position);
                floatUnitPresentLocation += (Time.deltaTime * 30f) / distance_two;
                _FloatUnit.transform.position =
                    Vector3.Lerp(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.Find("FloatUnit_FinishPosition").transform.position, floatUnitPresentLocation);
            }

            floatUnitCurrentTime -= Time.deltaTime;
            if(floatUnitCurrentTime <= 0 && isFloatUnit && !isIncapableAction && !isUnderAttack && !isLanding)
            {
                if (_FloatUnit != null)
                {
                    Destroy(_FloatUnit);
                }
                BoostFinish();
                StepFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                isFloatUnit = false;
                isTypeGroundRunning = true;
                anim.SetBool("Replacement", true);
                isReplacement = true;
                isStiffness = true;
                isUnderAttack = true;
                lastmove_name = "fightingchargeshot";
            }
        }

        ReplacementMove();
    }

    ////////////////////////////////////////

    //チャージゲージの範囲制限
    float ChargeControl(float currenttime, float maxtime)
    {
        if(currenttime > maxtime)
        {
            return maxtime;
        }
        else if(currenttime < 0)
        {
            return 0;
        }
        else
        {
            return currenttime;
        }
    }

    //換装中の処理
    void ReplacementMove()
    {
        if (isReplacement)
        {
            replacementTime += Time.deltaTime;
            if (replacementTime >= 1.0f)
            {
                ReplacementFinish();
            }
        }
    }

    //換装終了処理
    void ReplacementFinish()
    {
        replacementTime = 0;
        isReplacement = false;
        isStiffness = false;
        isUnderAttack = false;
        anim.SetBool("Replacement", false);
    }

    //重力を無くす
    void GravityOff()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //硬直
    void Rigidity()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    //よろけとダウン時の処理
    void Stagger_Control()
    {
        if (isStagger || isDown)
        {
            if (!isDown || !isAir)
            {
                staggerTime += Time.deltaTime;
            }
            if(staggerTime >= 0.2f)
            {
                anim.SetBool("Stagger_Start", false);
            }
            if(staggerTime <= 0.2f)
            {
                Vector3 stagger_move;
                if (isDown)
                {
                    stagger_move = other_forward * 0.3f;
                }
                else
                {
                    stagger_move = other_forward * 0.20f;
                }
                rb.velocity = new Vector3(stagger_move.x, rb.velocity.y, stagger_move.z);
            }
            if(staggerTime >= 1.0f && !isDown)
            {
                isIncapableAction = false;
                isStagger = false;
                isStiffness = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                staggerTime = 0;
            }
            else if(staggerTime >= 1f && staggerTime < 2f && isDown)
            {
                anim.SetBool("GetUp", true);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
            }
            else if(staggerTime >= 2f && isDown)
            {
                isIncapableAction = false;
                isStagger = false;
                isStiffness = false;
                isDown = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
                anim.SetBool("GetUp", false);
                staggerTime = 0;
                if (!isAir)
                {
                    boost_amount = BOOST_MAX;
                }
            }
        }
    }

    ////////////////////////////////////////////////////////////
    
    //ジャンプボタンが押された場合
    public void OnJump(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!isIncapableAction)
                    {
                        isJumpKeyPressing = true;
                        //ジャンプ開始
                        if (!isJump)
                        {
                            BoostFinish();
                            isJump = true;
                            jumpRugTime = 0;
                            isBoost = false;
                            boostButtonTime = 0;
                            isJumpMove = true;
                            isStepJump = false;
                        }
                        //ブースト開始
                        else if (isJump)
                        {
                            boostButtonTime += Time.deltaTime;
                            if (boostButtonTime < 0.2f)
                            {
                                isBoost = true;
                                isJump = false;
                                isRise = false;
                                anim.SetBool("Boost_Landing", true);
                                anim.SetBool("SubShooting_Start", false);
                                anim.SetBool("SpecialShooting_Start", false);
                                anim.SetBool("Boost_Landing_Finish", false);
                                isSlide = false;
                                slideTime = 0;
                                MainShootingFinish();
                                SubShootingFinish();
                                SubShootingFightingVariantsFinish();
                                SpecialShootingFinish();
                                AttackFinish();
                                SpecialAttackFinish();
                                StepFinish();
                                ReplacementFinish();
                                boost_amount -= 15;
                                boostConsumedTime = 0;
                                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
                                Eff_SpeedLine.SetActive(true);
                                KMF_RapidRotation();
                                if (!isTypeGroundRunning)
                                {
                                    transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                }
                                else if (!isAir)
                                {
                                    if (SceneManager.GetActiveScene().name == "TrainingScene")
                                    {
                                        RpcEffDustPlay_Control();
                                    }
                                    else
                                    {
                                        pv.RPC(nameof(RpcEffDustPlay_Control), RpcTarget.AllBufferedViaServer);
                                    }
                                    //EffDust_R.GetComponent<ParticleSystem>().Play();
                                    //EffDust_L.GetComponent<ParticleSystem>().Play();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //ジャンプボタンが離した場合
    public void OffJump(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    isJumpKeyPressing = false;
                }
            }
        }
    }

    //メイン射撃ボタンが押された場合
    public void OnMainShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!isIncapableAction && (!isUnderAttack || lastmove_name == "subshooting"))
                    {
                        if (!isMainShooting)
                        {
                            if (mainshooting_number >= 1 && !isLanding)
                            {
                                isMainShooting = true;
                                anim.SetBool("MainShooting", true);
                                isMainShootingFiring = false;
                                mainShootingTime = 0;
                                isUnderAttack = true;
                                Varis_Normal.SetActive(true);
                                Varis_FullPower.SetActive(false);
                                if (LockOnEnemy != null)
                                {
                                    gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 1;
                                }
                                if (lastmove_name == "subshooting")
                                {
                                    anim.SetBool("SubShooting_Start", false);
                                    SubShootingFinish();
                                }
                                lastmove_name = "mainshooting";
                                //振り向き撃ち
                                if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(MainCamera.transform.localEulerAngles.y + 180, 360) - 180)) >= 100f)
                                {
                                    if (LockOnEnemy != null)
                                    {
                                        isStiffness = true;
                                        gameObject.transform.LookAt(LockOnEnemy.transform);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //サブ射撃ボタンが押された場合
    public void OnSubShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!isIncapableAction && !isUnderAttack)
                    {
                        if (!isSubShooting)
                        {
                            if (subshooting_number >= 1 && !isLanding)
                            {
                                StepFinish();
                                anim.SetBool("SubShooting_Start", true);
                                isSubShooting = true;
                                isSubShootingFiring = false;
                                subShootingTime = 0;
                                isStiffness = true;
                                isUnderAttack = true;
                                Varis_Normal.SetActive(true);
                                Varis_FullPower.SetActive(false);

                                isBoost = false;
                                anim.SetBool("Boost_Landing", false);
                                if (isInduction && LockOnEnemy != null)
                                {
                                    gameObject.transform.LookAt(LockOnEnemy.transform);
                                }
                                lastmove_name = "subshooting_start";
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            }
                        }
                    }
                }
            }
        }
    }

    //特殊射撃ボタンが押された場合
    public void OnSpecialShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!isIncapableAction
                    && (!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
                    {
                        if (Input.GetButtonDown("SpecialShooting") && !isSpecialShooting)
                        {
                            if (specialshooting_number >= 1 && !isLanding)
                            {
                                StepFinish();
                                MainShootingFinish();
                                SpecialAttackFinish();
                                anim.SetBool("SpecialShooting", true);
                                anim.SetBool("SpecialShooting_Start", true);
                                isSpecialShooting = true;
                                isSpecialShootingFiring = false;
                                specialShootingTime = 0;
                                isSpecialShootingAnimation = true;
                                isStiffness = true;
                                isUnderAttack = true;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(true);

                                isBoost = false;
                                anim.SetBool("Boost_Landing", false);

                                if (isInduction && LockOnEnemy != null)
                                {
                                    gameObject.transform.LookAt(LockOnEnemy.transform);
                                }
                                lastmove_name = "specialshooting";
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            }
                        }
                    }
                }
            }
        }
    }

    //格闘ボタンが押された場合
    public void OnFight(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    Vector3 attack_direction;

                    if (!isIncapableAction)
                    {
                        if (!isLanding || isUnderAttack)
                        {
                            if (!isAttack && !isAttack1 && (!isUnderAttack || lastmove_name == "specialattack"))
                            {
                                StepFinish();
                                SpecialAttackFinish();
                                anim.SetBool("Attack_Induction", true);
                                isAttack = true;
                                isStiffness = true;
                                isUnderAttack = true;
                                attackTime = 0;
                                attackFinishTime = 1.8f;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(false);
                                MVS_R.SetActive(true);
                                MVS_L.SetActive(true);
                                MVSSheathing_R.SetActive(false);
                                MVSSheathing_L.SetActive(false);
                                Eff_SpeedLine.SetActive(true);
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;

                                isBoost = false;
                                anim.SetBool("Boost_Landing", false);
                                lastmove_name = "attack";
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            }
                            else if (isAttack && isAttack1 && !isAttack2
                                && anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_01"))
                            {
                                anim.SetBool("Attack1", false);
                                anim.SetBool("Attack2", true);
                                isAttack2 = true;
                                attackTime = 0;
                                attackFinishTime = 1.0f;
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = true;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().power = 70;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().correctionFactor = 0.15f;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().downValue = 0.3f;
                            }
                            else if (isAttack2
                                && anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_02"))
                            {
                                anim.SetBool("Attack1", false);
                                anim.SetBool("Attack2", false);
                                anim.SetBool("Attack3", true);
                                isAttack3 = true;
                                isAttack2 = false;
                                attackTime = 0;
                                attackFinishTime = 1.0f;
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = true;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().power = 80;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().correctionFactor = 0.12f;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().downValue = 6;
                            }

                            //サブ格闘派生開始処理
                            if (lastmove_name == "subshooting" && isSubShooting && !isSubShootingFightingVariants)
                            {
                                SubShootingFinish();
                                anim.SetBool("SubShooting_FightingVariants", true);
                                anim.SetBool("SubShooting_Start", false);
                                isSubShootingFightingVariants = true;
                                isStiffness = true;
                                isUnderAttack = true;
                                rb.useGravity = false;
                                subShootingFightingVariantsTime = 0;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(false);
                                Leg_R.transform.GetComponent<BoxCollider>().enabled = true;

                                isBoost = false;
                                anim.SetBool("Boost_Landing", false);

                                if (isInduction && LockOnEnemy != null)
                                {
                                    gameObject.transform.LookAt(LockOnEnemy.transform);
                                }
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                lastmove_name = "subshooting_fightingvariants";
                                attack_direction = gameObject.transform.forward * 1;
                                attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                                attack_moving = attack_direction * BOOST_SPEED;
                            }
                        }
                    }
                }
            }
        }
    }

    //特殊格闘ボタンが押された場合
    public void OnSpecialFight(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    Vector3 specialattack_direction;
                    if (!isIncapableAction && !isUnderAttack)
                    {
                        if (!isLanding)
                        {
                            StepFinish();
                            anim.SetBool("SpecialAttack", true);
                            isSpecialAttack = true;
                            specialAttackTime = 0;
                            boost_amount -= 10;
                            isStiffness = true;
                            isUnderAttack = true;
                            Varis_Normal.SetActive(false);
                            Varis_FullPower.SetActive(false);

                            isBoost = false;
                            anim.SetBool("Boost_Landing", false);

                            if (isInduction && LockOnEnemy != null)
                            {
                                gameObject.transform.LookAt(LockOnEnemy.transform);
                            }
                            lastmove_name = "specialattack";
                            specialattack_direction = gameObject.transform.forward * 1;
                            specialattack_direction.Normalize();
                            specialattack_moving = specialattack_direction * BOOST_SPEED * 1.2f;
                        }
                    }
                }
            }
        }
    }

    ////////////////////////////////////////////////////////////

    //金属エフェクトと砂煙エフェクトのアクティブ設定
    [PunRPC]
    void RpcEffControl()
    {
        if (Spark_R != null)
        {
            Spark_R.SetActive(true);
            Spark_L.SetActive(true);
        }
        if (EffDust_R != null)
        {
            EffDust_R.SetActive(true);
            EffDust_L.SetActive(true);
        }
    }

    //金属エフェクトの制御
    [PunRPC]
    void RpcSparkControl(bool start_flag)
    {
        if (start_flag)
        {
            Spark_R.GetComponent<ParticleSystem>().Play();
            Spark_L.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Spark_R.GetComponent<ParticleSystem>().Stop();
            Spark_L.GetComponent<ParticleSystem>().Stop();
        }
    }

    //ステップラインの方向設定
    [PunRPC]
    void StepLineDirection(Vector3 vector3)
    {
        Eff_StepLine.transform.localEulerAngles = vector3;
    }

    //砂煙エフェクト終了
    [PunRPC]
    void RpcEffControl_NotBoostFlag()
    {
        EffDust_R.GetComponent<ParticleSystem>().Stop();
        EffDust_L.GetComponent<ParticleSystem>().Stop();
    }

    //虹ステップ色にする
    [PunRPC]
    void RpcFightGradientChange_StepLine()
    {
        Gradient colorkey = new Gradient();
        colorkey.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(0, 255, 255), 0f), new GradientColorKey(new Color(255, 255, 0), 0.3f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });

        var col = Eff_StepLine.transform.Find("parline1_alpha").GetComponent<ParticleSystem>().colorOverLifetime;
        col.color = colorkey;
    }

    //通常ステップ色にする
    [PunRPC]
    void RpcNormalGradientChange_StepLine()
    {
        Gradient colorkey = new Gradient();
        colorkey.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(255, 255, 255), 0f), new GradientColorKey(new Color(255, 255, 255), 0.3f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });

        var col = Eff_StepLine.transform.Find("parline1_alpha").GetComponent<ParticleSystem>().colorOverLifetime;
        col.color = colorkey;
    }

    //ステップエフェクトの表示設定
    [PunRPC]
    void EffSetActive(bool display_flag)
    {
        Eff_StepLine.SetActive(display_flag);
    }

    //砂煙エフェクト開始
    [PunRPC]
    void RpcEffDustPlay_Control()
    {
        EffDust_R.GetComponent<ParticleSystem>().Play();
        EffDust_L.GetComponent<ParticleSystem>().Play();
    }

    //砂煙エフェクト終了
    [PunRPC]
    void RpcEffDustStop_Control()
    {
        EffDust_R.GetComponent<ParticleSystem>().Stop();
        EffDust_L.GetComponent<ParticleSystem>().Stop();
    }

    //盾出現
    [PunRPC]
    void BlazeLuminousExpand()
    {
        BlazeLuminous.SetActive(true);
    }

    //盾収納
    [PunRPC]
    void BlazeLuminousClose()
    {
        BlazeLuminous.SetActive(false);
    }

    //耐久値同期
    [PunRPC]
    void DurabilitySync(int _durable_value)
    {
        durable_value = _durable_value;
    }

    ////////////////////////////////////////////////////////////

    //コライダー当たった場合
    private void OnCollisionEnter(Collision other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.CompareTag("Ground") && anim != null)
                {
                    anim.SetBool("Jump", false);
                    anim.SetBool("Air", false);
                    isJump = false;
                    if (!isBoost)
                    {
                        isLanding = true;
                        boost_amount = BOOST_MAX;
                        isIncapableAction = false;
                    }
                    else if (isTypeGroundRunning)
                    {
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcEffDustPlay_Control();
                        }
                        else
                        {
                            pv.RPC(nameof(RpcEffDustPlay_Control), RpcTarget.AllBufferedViaServer);
                        }
                        //EffDust_R.GetComponent<ParticleSystem>().Play();
                        //EffDust_L.GetComponent<ParticleSystem>().Play();
                    }
                    isAir = false;

                    if (isDown)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }

                    if (!isTypeGroundRunning && isBoost)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                    }
                }
            }
        }
    }

    //コライダー当たっている場合
    private void OnCollisionStay(Collision other)
    {

        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    if (!isStep && !isJump && !isBoost && !isSlide && !isDown && !isStagger)
                    {
                        isIncapableAction = false;
                    }
                }
            }
        }
    }

    //コライダー離れた場合
    private void OnCollisionExit(Collision other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    anim.SetBool("Air", true);
                    isAir = true;
                }
            }
        }
    }

    //トリガー当たった場合
    private void OnTriggerEnter(Collider other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                Hit_Control(other);
            }
        }
    }

    //トリガー当たっている場合
    private void OnTriggerStay(Collider other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (!isHitStartStay)
                {
                    Hit_Control(other);
                }
            }
        }
    }

    //トリガー離れた場合
    private void OnTriggerExit(Collider other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.GetComponent<Beam_Control>() != null)
                {
                    if (other.gameObject.GetComponent<Beam_Control>().OwnMachine != gameObject)
                    {
                        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Untagged"))
                        {
                            isHitStartStay = false;
                        }
                    }
                }
            }
        }
    }

    //被弾処理
    void Hit_Control(Collider other)
    {
        if (other.gameObject.GetComponent<Beam_Control>() != null)
        {
            if (other.gameObject.GetComponent<Beam_Control>().OwnMachine != gameObject)
            {
                if (!isDown)
                {
                    if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Untagged"))
                    {
                        other_forward = other.transform.position;
                        //ガード成功の場合
                        if (isDefense && Vector3.Angle(transform.forward, other.gameObject.transform.forward) >= 90)
                        {
                            isDefending = true;
                            defendingTime = 0;
                            Transform other_transform = other.transform;
                            other_transform.position = other_transform.forward * -2;
                            other_transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z);
                            transform.LookAt(other.transform);
                            defenserecoil = transform.forward * -30f;
                        }
                        else
                        {
                            isHitStartStay = true;
                            BoostFinish();
                            DefenseFinish();
                            StepFinish();
                            MainShootingFinish();
                            SubShootingFinish();
                            SubShootingFightingVariantsFinish();
                            SpecialShootingFinish();
                            AttackFinish();
                            SpecialAttackFinish();
                            ReplacementFinish();
                            durable_value -= Mathf.CeilToInt(other.gameObject.GetComponent<Beam_Control>().power * correctionFactor);
                            correctionFactor -= other.gameObject.GetComponent<Beam_Control>().correctionFactor;
                            correctionFactorResetTime = 0f;
                            if (correctionFactor < 0.1f)
                            {
                                correctionFactor = 0.1f;
                            }
                            downValue += other.gameObject.GetComponent<Beam_Control>().downValue;
                            if (downValue >= 6)
                            {
                                isDown = true;
                                isIncapableAction = true;
                                isStiffness = true;
                                Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
                                if (other.gameObject.CompareTag("Bullet"))
                                {
                                    if (Vector3.Distance(hitPos, gameObject.transform.forward * 1.1f) > Vector3.Distance(hitPos, gameObject.transform.forward * -1.1f))
                                    {
                                        anim.SetBool("DownBack", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("DownFront", true);
                                    }
                                }
                                else if (other.gameObject.CompareTag("Untagged"))
                                {
                                    if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(other.transform.root.localEulerAngles.y + 180, 360) - 180)) >= 90f)
                                    {
                                        anim.SetBool("DownBack", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("DownFront", true);
                                    }
                                }
                                staggerTime = 0;
                                correctionFactorResetTime = 0;
                                downValue = 0;
                                if (isAir)
                                {
                                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 180);
                                }
                            }
                            else
                            {
                                isStagger = true;
                                isIncapableAction = true;
                                isStiffness = true;
                                Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
                                if (other.gameObject.CompareTag("Bullet"))
                                {
                                    if (Vector3.Distance(hitPos, gameObject.transform.forward * 1.1f) > Vector3.Distance(hitPos, gameObject.transform.forward * -1.1f))
                                    {
                                        anim.SetBool("StaggerBack_Landing", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("StaggerFront_Landing", true);
                                    }
                                }
                                else if (other.gameObject.CompareTag("Untagged"))
                                {
                                    if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(other.transform.root.localEulerAngles.y + 180, 360) - 180)) >= 90f)
                                    {
                                        anim.SetBool("StaggerBack_Landing", true);
                                    }
                                    else
                                    {
                                        anim.SetBool("StaggerFront_Landing", true);
                                    }
                                }
                                anim.SetBool("Stagger_Start", true);
                                staggerTime = 0;
                            }
                        }
                    }

                    if (durable_value < 0)
                    {
                        durable_value = 0;
                    }
                    pv.RPC(nameof(DurabilitySync), RpcTarget.AllBufferedViaServer, durable_value);
                    other.GetComponent<Beam_Control>().ThisGameObjectDestroy();
                }
            }
        }
    }
}
