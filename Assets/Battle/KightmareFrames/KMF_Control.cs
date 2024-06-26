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
        //自機のみ技入力可能にする
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
        //PhotonViewコンポーネントがある場合
        if (pv != null)
        {
            //自機もしくはトレモならキー入力可能
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

    //コントロール権限がある場合のUpdate処理
    void UpdateControl()
    {
        //耐久値が無くなると操作できなくする
        if (boost_amount <= 0)
        {
            isIncapableAction = true;
        }
        //ダメージ補正とダウン値のリセット
        if (correctionFactor < 1)
        {
            correctionFactorResetTime += Time.deltaTime;
            //3秒経過
            if (correctionFactorResetTime >= 3f)
            {
                Debug.Log("ダメージ補正とダウン値のリセット");
                correctionFactor = 1;
                downValue = 0;
            }
        }

        //特別な動作をしていない場合は重力を発生させる
        if (!(isDefense || isSubShootingFightingVariants) && !rb.useGravity)
        {
            Debug.Log("重力発生");
            rb.useGravity = true;
        }
        //ダウンまたはよろけていない場合
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

    //機体周辺の情報の更新
    void FirstSetting()
    {
        //Animatorコンポーネント取得
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        //オフライン用のエフェクト処理
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            RpcEffControl();
            if (!isBoost)
            {
                RpcEffControl_NotBoostFlag();
            }
        }
        //オンライン用のエフェクト処理
        else
        {
            pv.RPC(nameof(RpcEffControl), RpcTarget.AllBufferedViaServer);
            //ブーストしていない場合
            if (!isBoost)
            {
                pv.RPC(nameof(RpcEffControl_NotBoostFlag), RpcTarget.AllBufferedViaServer);
            }
        }
        //ロックオン対象がいない場合探す
        if (LockOnEnemy == null)
        {
            Debug.Log("自機のロックオン対象を捜索中");
            LockOnEnemy = transform.GetComponent<PlayerID_Control>().LockOnEnemy;
            //上半身のみ敵の方へ向けるときの処理
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
        //PhotonViewコンポーネントがある場合
        if (pv != null)
        {
            //自機の場合
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

    //コントロール可能の場合に処理できるFixedUpdete
    void FixedUpdateControl()
    {
        FirstSetting();
        //ダウン、よろけていない場合
        if (!isStiffness)
        {
            //何らかによってコントロール制限を掛けられていない場合
            if (!isIncapableAction)
            {
                //ズサ、着地硬直ではない場合
                if (!isLanding && !isSlide)
                {
                    //ジャンプを除いた移動する場合
                    if (isLeverInsert && !isDefense && lastmove_name != "jump")
                    {
                        //地走機体の場合
                        if (isTypeGroundRunning || !isBoost)
                        {
                            Debug.Log("地走移動");
                            rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
                        }
                        //ホバー機体の場合
                        else
                        {
                            Debug.Log("ホバー移動");
                            rb.velocity = new Vector3(movingvelocity.x, 0, movingvelocity.z);
                        }
                        //金属エフェクトの発生(オフライン)
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(true);
                        }
                        //金属エフェクトの発生(オンライン)
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, true);
                        }
                        KMF_Rotation();
                    }
                    //一つ前の行動がステップだった場合
                    else if (lastmove_name == "step")
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        //金属エフェクトのストップ(オフライン)
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(false);
                        }
                        //金属エフェクトのストップ(オンライン)
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                        }
                    }
                    else
                    {
                        //金属エフェクトのストップ(オフライン)
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcSparkControl(false);
                        }
                        //金属エフェクトのストップ(オンライン)
                        else
                        {
                            pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                        }
                    }
                }
                //ズサ中の場合
                else if (isSlide)
                {
                    //金属エフェクトのストップ(オフライン)
                    if (SceneManager.GetActiveScene().name == "TrainingScene")
                    {
                        RpcSparkControl(false);
                    }
                    //金属エフェクトのストップ(オンライン)
                    else
                    {
                        pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                    }
                }
                //本来傾かない軸のリセット
                gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            }
        }
        else
        {
            //特別な動作をしている場合
            if (isDown || isSpecialAttack || isSubShootingFightingVariants || isAttack)
            {
            }
            //ステップまたはよろけている場合
            else if (isStep || isStagger)
            {
                GravityOff();
                //金属エフェクトのストップ(オフライン)
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    RpcSparkControl(false);
                }
                //金属エフェクトのストップ(オンライン)
                else
                {
                    pv.RPC(nameof(RpcSparkControl), RpcTarget.AllBufferedViaServer, false);
                }
            }
            else
            {
                //硬直
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
        //レバーを入れている場合
        if(x != 0 || z != 0)
        {
            isLeverInsert = true;
            anim.SetBool("Walk", true);
        }
        //レバーNの場合
        else
        {
            isLeverInsert = false;
            lastlever_name = "null";
            anim.SetBool("Walk", false);
        }
        movingdirection.Normalize();
        //歩きの場合
        if (!isBoost && !isSlide)
        {
            Debug.Log("歩き調整");
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * WALK_SPEED;
        }
        //着地硬直でない場合
        if (!isLanding)
        {
            DefenseAndStepControl(x, z);
        }
    }

    //ガードとステップ用のレバーの制御
    void DefenseAndStepControl(float x, float z)
    {
        //レバーが入っている場合
        if (Mathf.Abs(x) >= 0.8f || Mathf.Abs(z) >= 0.8f)
        {
            //レバー横の方が強い場合
            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                //レバー右の場合
                if (x > 0)
                {
                    //ステップ右の受付タイミングの場合
                    if (lastlever_name == "null" && isLeverRight && leverRightTime < 0.2f && !isStep)
                    {
                        //ステップ可能な状態の場合
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * WALK_SPEED;
                            //ステップエフェクトの向き調整(オフライン)
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 90, 0);
                            }
                            //ステップエフェクトの向き調整(オンライン)
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
                    //ステップ左の受付タイミングの場合
                    if (lastlever_name == "null" && isLeverLeft && leverLeftTime < 0.2f && !isStep)
                    {
                        //ステップ可能な状態の場合
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * -WALK_SPEED;
                            //ステップエフェクトの向き調整(オフライン)
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y - 90, 0);
                            }
                            //ステップエフェクトの向き調整(オンライン)
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
            //レバー前後の場合
            else
            {
                //レバー前の場合
                if (z > 0)
                {
                    //盾構えが可能な場合
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
                            //盾の発生(オフライン)
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                BlazeLuminous.SetActive(true);
                            }
                            //盾の発生(オンライン)
                            else
                            {
                                pv.RPC(nameof(BlazeLuminousExpand), RpcTarget.AllBufferedViaServer);
                            }
                            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            //ロックオン対象がいる場合
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

                    //ステップ前の受付タイミングの場合
                    if (lastlever_name == "null" && isLeverFront && leverFrontTime < 0.2f && !isStep)
                    {
                        //ステップ可能な状態の場合
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * WALK_SPEED;
                            //ステップエフェクトの向き調整(オフライン)
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y, 0);
                            }
                            //ステップエフェクトの向き調整(オンライン)
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
                    //ステップ後ろの受付タイミングの場合
                    if (lastlever_name == "null" && isLeverBack && leverBackTime < 0.2f && !isStep)
                    {
                        //ステップ可能な状態の場合
                        if ((!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "attack" || isSlide) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * -WALK_SPEED;
                            //ステップエフェクトの向き調整(オフライン)
                            if (SceneManager.GetActiveScene().name == "TrainingScene")
                            {
                                Eff_StepLine.transform.localEulerAngles = new Vector3(0, MainCamera.transform.eulerAngles.y - transform.eulerAngles.y + 180, 0);
                            }
                            //ステップエフェクトの向き調整(オンライン)
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
            Debug.Log("ガード中");
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
            Debug.Log("ガード成功中");
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
            Debug.Log("ステップ中");
            //ズサ無くす
            if (isSlide)
            {
                SlideFinish();
            }

            stepTime += Time.deltaTime;
            //誘導切り時間
            if(stepTime < 0.1)
            {
                //[ToDo]オンライン中に誘導切り出来ない理由は変数同期していないと判明！？
                Debug.Log("誘導切り中");
                isInductionOff = true;
            }
            //ステップ中の挙動制御
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
            //ステップ終了
            if(stepTime >= 0.3f)
            {
                //地面に接地している場合(これ無いとなんかブースト回復できない)
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
        Debug.Log("ガード終了");
        isDefense = false;
        anim.SetBool("Defense", false);
        isStiffness = false;
        isUnderAttack = false;
        rb.useGravity = true;
        //盾収納(オフライン)
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            BlazeLuminous.SetActive(false);
        }
        //盾収納(オンライン)
        else
        {
            pv.RPC(nameof(BlazeLuminousClose), RpcTarget.AllBufferedViaServer);
        }
    }

    //ステップ開始時の処理
    void StepStart()
    {
        Debug.Log("ステップ開始");
        float KmfLookingAtCamera_rotation = Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y, 360))
            - Mathf.Abs(Mathf.Repeat(MainCamera.transform.localEulerAngles.y, 360));
        //マイナス角度を無くす
        if(KmfLookingAtCamera_rotation  < 0)
        {
            KmfLookingAtCamera_rotation += 360;
        }
        //自機が前を向いている場合
        if (KmfLookingAtCamera_rotation < 45f || KmfLookingAtCamera_rotation > 315f)
        {
            StepAnimationSelect("Step_Front", "Step_Back", "Step_Right", "Step_Left");
        }
        //自機が右を向いている場合
        else if(KmfLookingAtCamera_rotation >= 45f && KmfLookingAtCamera_rotation <= 135f)
        {
            StepAnimationSelect("Step_Left", "Step_Right", "Step_Front", "Step_Back");
        }
        //自機が後ろを向いている場合
        else if(KmfLookingAtCamera_rotation > 135f && KmfLookingAtCamera_rotation < 225f)
        {
            StepAnimationSelect("Step_Back", "Step_Front", "Step_Left", "Step_Right");
        }
        //自機が左を向いている場合
        else if(KmfLookingAtCamera_rotation >= 225f && KmfLookingAtCamera_rotation <= 315f)
        {
            StepAnimationSelect("Step_Right", "Step_Left", "Step_Back", "Step_Front");
        }

        float step_speed = 4.0f;
        //虹ステに設定
        if (lastmove_name == "attack")
        {
            step_speed = 5f;
            //オフライン
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcFightGradientChange_StepLine();
            }
            //オンライン
            else
            {
                pv.RPC(nameof(RpcFightGradientChange_StepLine), RpcTarget.AllBufferedViaServer);
            }
        }
        //普通のステップエフェクトに設定
        else
        {
            //オフライン
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcNormalGradientChange_StepLine();
            }
            //オンライン
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
        //ステップエフェクト可視化(オフライン)
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            Eff_StepLine.SetActive(true);
        }
        //ステップエフェクト可視化(オンライン)
        else
        {
            pv.RPC(nameof(EffSetActive), RpcTarget.AllBufferedViaServer, true);
        }
    }

    //ステップ中のアニメーション決め
    void StepAnimationSelect(string stepfront, string stepback, string stepright, string stepleft)
    {
        //レバー前入力
        if (isLeverFront)
        {
            anim.SetBool(stepfront, true);
        }
        //レバー後ろ入力
        if (isLeverBack)
        {
            anim.SetBool(stepback, true);
        }
        //レバー右入力
        if (isLeverRight)
        {
            anim.SetBool(stepright, true);
        }
        //レバー左入力
        if (isLeverLeft)
        {
            anim.SetBool(stepleft, true);
        }
    }

    //ステップ終了処理
    void StepFinish()
    {
        Debug.Log("ステップ終了");
        isInductionOff = false;
        isStiffness = false;
        isStep = false;
        stepTime = 0;
        lastmove_name = "null";
        //ステップエフェクト終了(オフライン)
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            Eff_StepLine.SetActive(false);
        }
        //ステップエフェクト終了(オンライン)
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
        //レバーを入れている場合
        if (isLeverInsert)
        {
            Vector3 diff = transform.position - new Vector3(latestPos.x, transform.position.y, latestPos.z);
            latestPos = transform.position;
            //機体が動いている場合
            if (diff != Vector3.zero)
            {
                Debug.Log("旋回中");
                float turning_angle = 0.1f;
                //ブースト中の旋回性能
                if(isBoost)
                {
                    turning_angle = 0.02f;
                }
                //空中時の旋回性能
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
        //レバーを入れている場合
        if (isLeverInsert)
        {
            Debug.Log("向き急速変更");
            float x = Input.GetAxis("Move_X");
            float z = Input.GetAxis("Move_Y") * -1;
            Vector2 snapped = SnapAngle(new Vector2(x, z), 45f * Mathf.Deg2Rad);
            Vector3 direction = MainCamera.transform.eulerAngles;
            //レバー右の場合
            if (snapped.x > 0.38f)
            {
                //レバー右上の場合
                if (snapped.y <= 0.86f && snapped.y > 0.38f)
                {
                    direction.y += 45;
                }
                //レバー右下の場合
                else if (snapped.y >= -0.86f && snapped.y < -0.38f)
                {
                    direction.y += 135;
                }
                //自機を右向きにする
                else
                {
                    direction.y += 90;
                }
            }
            //レバー左の場合
            else if (snapped.x < -0.38f)
            {
                //レバー左上の場合
                if (snapped.y <= 0.86f && snapped.y > 0.38f)
                {
                    direction.y += -45;
                }
                //レバー左下の場合
                else if (snapped.y >= -0.86f && snapped.y < -0.38f)
                {
                    direction.y += -135;
                }
                //自機を左向きにする
                else
                {
                    direction.y += -90;
                }
            }
            //自機を前向きにする
            else if (snapped.y > 0.38f)
            {
                direction.y += 0;
            }
            //自機を後ろ向きにする
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
        //入力可能な場合
        if (!isIncapableAction)
        {
            //ブースト中だった場合
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
                    //よろけまたはステップ中ではない場合
                    if (!isStiffness || isStep)
                    {
                        //ジャンプ終了までのディレイ
                        if (jumpRugTime >= 0.3f)
                        {
                            isRise = false;
                            isJump = false;
                        }
                    }
                }
            }
        }

        //ジャンプ処理
        if (isJump && !isBoost)
        {
            //よろけまたはステップではない場合
            if (!isStiffness || isStep)
            {
                //ブースト開始1回のみ実行
                if (isJumpMove)
                {
                    //ステップ中ではない場合
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
                    //ジャンプするまでのラグ
                    if (jumpRugTime >= 0.2f && !isRise)
                    {
                        JumpStart();
                    }
                    //ジャンプ中
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
                    //ジャンプするまでのラグ
                    if (jumpRugTime >= 0.2f && !isRise)
                    {
                        JumpStart();
                    }
                    //ブースト消費
                    else if (jumpRugTime >= 0.2f)
                    {
                        Jumping();
                        boost_amount -= Time.deltaTime * 10f;
                    }
                }
            }
            else
            {
                //私にも分からん
                if (isUnderAttack || isStep)
                {
                    Debug.Log("クリックして存在意義を教えて！");
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
            Debug.Log("ブースト終了");
            //ブースト中の場合
            if (isBoost)
            {
                BoostFinish();
                //接地している場合
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
            Debug.Log("ズサ中");
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
        //着地硬直でない場合
        if (!isLanding)
        {
            Debug.Log("ジャンプ開始");
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
        //着地硬直でない場合
        if (!isLanding)
        {
            Debug.Log("ジャンプ中");
            Vector3 jump_direction;
            Vector3 jump_moving;

            jump_direction = gameObject.transform.up * 1;
            jump_direction.Normalize();
            jump_moving = jump_direction * JUMP_POWER;
            movingvelocity = rb.velocity;
            //フワジャンの場合(兄ちゃんが直したいって言ってたやつかも！！！)
            if (isStepJump)
            {
                Debug.Log("フワジャン");
                movingvelocity /= 2;
            }
            rb.velocity = new Vector3(movingvelocity.x, jump_moving.y, movingvelocity.z);
        }
    }

    //ブースト中の処理
    void Boosting()
    {
        Debug.Log("ブースト中");
        //ホバー機体の場合重力なくす
        if (!isTypeGroundRunning)
        {
            rb.useGravity = false;
        }
        //レバーを入れたブーストダッシュ
        if (movingdirection != new Vector3(0, 0, 0) && isLeverInsert)
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * BOOST_SPEED;
        }
        //レバーを入れないブーストダッシュ
        else
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * BOOST_SPEED;
            rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
        }
        boostConsumedTime += Time.deltaTime;
        //ブースト消費
        if (boostConsumedTime >= 0.1f)
        {
            boost_amount -= 2;
            boostConsumedTime = 0;
        }
    }

    //ブースト終了の処理
    void BoostFinish()
    {
        Debug.Log("ブースト終了");
        isBoost = false;
        rb.useGravity = true;
        lastmove_name = "boost";
        Eff_SpeedLine.SetActive(false);
        //砂煙エフェクトの終了(オフライン)
        if (SceneManager.GetActiveScene().name == "TrainingScene")
        {
            RpcEffDustStop_Control();
        }
        //砂煙エフェクトの終了(オンライン)
        else
        {
            pv.RPC(nameof(RpcEffDustStop_Control), RpcTarget.AllBufferedViaServer);
        }
        anim.SetBool("Boost_Landing", false);
    }

    //ズサ終了の処理
    void SlideFinish()
    {
        Debug.Log("ズサ終了");
        anim.SetBool("Boost_Landing_Finish", false);
        isSlide = false;
        slideTime = 0;
        isIncapableAction = false;
        isUnderAttack = false;
    }

    //着地硬直処理
    void LandingTime()
    {
        //着地硬直の条件がそろった場合
        if (isLanding && (!isUnderAttack || lastmove_name == "mainshooting"))
        {
            Debug.Log("着地硬直中");
            anim.SetBool("Landing", true);
            landingTime += Time.deltaTime;
            //着地硬直終了
            if(landingTime >= 0.5f)
            {
                Debug.Log("着地硬直終了");
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
            //ビーム発射
            if(mainShootingTime >= 0.23f && !isMainShootingFiring)
            {
                Debug.Log("メインビーム発射");
                mainshooting_number--;
                isMainShootingFiring = true;
                GameObject _beam;
                //ビーム生成(オフライン)
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    _beam = Instantiate(Beam, Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                //ビーム生成(オンライン)
                else
                {
                    _beam = PhotonNetwork.Instantiate("Prefab/Beam(Green)", Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                //ビームに自機とロックオン対象を設定
                if (LockOnEnemy != null)
                {
                    _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                //ビームに自機を設定
                else
                {
                    _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
            }
            //メイン射撃終了
            if(mainShootingTime >= 1.0f)
            {
                MainShootingFinish();
                isUnderAttack = false;
            }

            //上半身を敵の方向へ向かない
            if(LockOnEnemy == null)
            {
                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
            }
        }

        //メイン射撃の弾数が最大量より少ない場合
        if (mainshooting_maxnumber > mainshooting_number)
        {
            mainShootingCurrentReloadTime += Time.deltaTime;
            //メイン射撃のリロード処理
            if (mainShootingCurrentReloadTime >= mainShootingReloadTime)
            {
                Debug.Log("メイン射撃リロード");
                mainshooting_number++;
                mainShootingCurrentReloadTime = 0;
            }
        }
    }

    //メイン射撃終了処理
    void MainShootingFinish()
    {
        Debug.Log("メイン射撃終了");
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
            //サブ射撃発射
            if(subShootingTime >= 0.3f && !isSubShootingFiring)
            {
                Debug.Log("サブ射撃発射");
                subshooting_number--;
                isSubShootingFiring = true;
                //スラッシュハーケン生成(オフライン)
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    SlashHarken_Instance = Instantiate(SlashHarken, Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                //スラッシュハーケン生成(オンライン)
                else
                {
                    SlashHarken_Instance = PhotonNetwork.Instantiate("Prefab/SlashHarken(Object)", Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                Vector3 SlashHarkenAngle = SlashHarken_Instance.transform.eulerAngles;
                SlashHarkenAngle.y -= 180.0f;
                SlashHarkenAngle.z += 90.0f;
                SlashHarken_Instance.transform.eulerAngles = SlashHarkenAngle; //スラッシュハーケンの角度調整
                //スラッシュハーケンに自機とロックオン対象を設定
                if (LockOnEnemy != null)
                {
                    SlashHarken_Instance.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                //スラッシュハーケンに自機を設定
                else
                {
                    SlashHarken_Instance.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
                lastmove_name = "subshooting";
            }
            //サブ射撃終了動作開始
            if (subShootingTime >= 0.8f && subShootingTime < 1.5f)
            {
                Debug.Log("サブ射撃終了動作開始");
                anim.SetBool("SubShooting_Start", false);
                anim.SetBool("SubShooting_Finish", true);
                //スラッシュハーケンがある場合
                if (SlashHarken_Instance != null)
                {
                    Destroy(SlashHarken_Instance);
                }
            }
            //サブ射撃終了
            if (subShootingTime >= 1.5f)
            {
                SubShootingFinish();
                isUnderAttack = false;
            }

            //スラッシュハーケンの有線処理([ToDo]これ同期しないと対戦相手無線に見える)
            if(SlashHarken_Instance != null)
            {
                SlashHarken_Instance.transform.Find("SlashHarken/Bone.001/Bone.004").transform.position = BlazeLuminous.transform.position;
            }
        }

        //サブ射撃の弾数が最大量より少ない場合
        if (subshooting_maxnumber > subshooting_number)
        {
            subShootingCurrentReloadTime += Time.deltaTime;
            //サブ射撃のリロード処理
            if (subShootingCurrentReloadTime >= subShootingReloadTime)
            {
                Debug.Log("サブ射撃リロード");
                subshooting_number++;
                subShootingCurrentReloadTime = 0;
            }
        }
    }

    //サブ射撃終了処理
    void SubShootingFinish()
    {
        Debug.Log("サブ射撃終了");
        anim.SetBool("SubShooting_Start", false);
        anim.SetBool("SubShooting_Finish", false);
        isSubShooting = false;
        isStiffness = false;
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //スラッシュハーケンが残っている場合
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
            //特殊射撃アニメーション移行終了
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
            //特殊射撃発射
            if (specialShootingTime >= 0.6f && !isSpecialShootingFiring)
            {
                Debug.Log("特殊射撃発射");
                specialshooting_number--;
                isSpecialShootingFiring = true;
                GameObject _Beam_FullPower;
                //特殊射撃ビーム生成(オフライン)
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    _Beam_FullPower = Instantiate(Beam_FullPower, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.position, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                //特殊射撃ビーム生成(オンライン)
                else
                {
                    _Beam_FullPower = PhotonNetwork.Instantiate("Prefab/Beam_Full", Varis_FullPower.transform.Find("Muzzle").gameObject.transform.position, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.rotation);
                }
                //特殊射撃ビームに自機とロックオン対象を設定
                if (LockOnEnemy != null)
                {
                    _Beam_FullPower.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
                }
                //特殊射撃ビームに自機を設定
                else
                {
                    _Beam_FullPower.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, null);
                }
            }
            //特殊射撃終了
            if (specialShootingTime >= 1.5f)
            {
                SpecialShootingFinish();
            }
        }

        //特殊射撃の弾数が無くなった場合
        if (specialshooting_number == 0)
        {
            specialShootingCurrentReloadTime += Time.deltaTime;
            //特殊射撃リロード処理
            if (specialShootingCurrentReloadTime >= specialShootingReloadTime)
            {
                Debug.Log("特殊射撃リロード完了");
                specialshooting_number = specialshooting_maxnumber;
                specialShootingCurrentReloadTime = 0;
            }
        }
    }

    //特殊射撃終了処理
    void SpecialShootingFinish()
    {
        Debug.Log("特殊射撃終了");
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
            //格闘初動に移行
            if (attackTime <= 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                //誘導している場合
                if (isInduction)
                {
                    Debug.Log("格闘(初動)");
                    //ロックオン対象がいる場合
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
            //格闘1打目に移行
            else if (attackTime > 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                Debug.Log("格闘(1打目)");
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
            //格闘終了
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
            //サブ格闘派生中
            if (subShootingFightingVariantsTime <= 0.2f)
            {
                Debug.Log("サブ格闘派生中");
                //誘導
                if (isInduction && LockOnEnemy != null)
                {
                    gameObject.transform.LookAt(LockOnEnemy.transform);
                }
                attack_direction = gameObject.transform.forward * 1;
                attack_direction.Normalize();
                attack_moving = attack_direction * BOOST_SPEED;
                rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
                isSubShootingFightingVariantsInAir = isAir;
            }
            //サブ格闘派生終了
            if (subShootingFightingVariantsTime >= 2.0f)
            {
                SubShootingFightingVariantsFinish();
                //サブ格闘派生中に接地していた場合
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
        //格闘中の場合
        if (isAttack)
        {
            Debug.Log("格闘開始");
            //格闘1打目に移行
            if (attackTime < 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
            {
                Debug.Log("格闘1打目に移行");
                attackTime = 0;
                attackFinishTime = 1.0f;
                isAttack1 = true;
                anim.SetBool("Attack1", true);
                Rigidity();
            }
            //格闘初動モーションだった場合
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_Start"))
            {
                anim.SetBool("Attack_Induction", false);
            }
        }
    }

    //格闘終了処理
    void AttackFinish()
    {
        Debug.Log("格闘終了");
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
        Debug.Log("サブ格闘派生終了");
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
        //特殊格闘中の場合
        if (isSpecialAttack)
        {
            specialAttackTime += Time.deltaTime;
            //特殊格闘中の動作
            if (specialAttackTime <= 0.4f)
            {
                Debug.Log("特殊格闘中");
                rb.velocity = new Vector3(specialattack_moving.x, rb.velocity.y, specialattack_moving.z);
            }
            //特殊格闘終了
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
        Debug.Log("特殊格闘終了");
        anim.SetBool("SpecialAttack", false);
        isSpecialAttack = false;
        isStiffness = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //格闘CS処理
    void FightingChargeControl()
    {
        //格闘CSリロード中
        if (fightingChargeCurrentReloadTime < fightingChargeReloadTime && !isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeCurrentReloadTime += Time.deltaTime;
            //格闘CSリロード完了
            if(fightingChargeCurrentReloadTime >= fightingChargeReloadTime)
            {
                Debug.Log("格闘CSリロード完了");
                isFightingChargeInput = true;
            }
        }
        //格闘CS発動([ToDo]入力方法がInputSystemを使っていない為仁が格闘振れない)
        else if (Input.GetButtonUp("Attack") && fightingChargeTime >= fightingChargeMaxTime && !isFloatUnit &&
            !isIncapableAction && !isUnderAttack && !isLanding)
        {
            Debug.Log("格闘CS発動");
            Vector3 InstanceFloatUnit_postion = Cockpit.transform.Find("InstanceFloatUnit").transform.position;
            Quaternion InstanceFloatUnit_rotation = transform.rotation;
            //フロートユニット生成(オフライン)
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                _FloatUnit = Instantiate(FloatUnit, InstanceFloatUnit_postion, InstanceFloatUnit_rotation);
            }
            //フロートユニット生成(オンライン)
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
        //格闘CSチャージ中
        else if (Input.GetButton("Attack") && isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeTime += Time.deltaTime;
        }
        //格闘CS減少中
        else if(isFightingChargeInput && !isFloatUnit)
        {
            fightingChargeTime -= Time.deltaTime;
        }

        fightingChargeTime = ChargeControl(fightingChargeTime, fightingChargeMaxTime);

        //フロートユニット制御
        if (isFloatUnit || isFightingChargeInput)
        {
            //フロートユニット装着するための移動
            if (_FloatUnit != null)
            {
                _FloatUnit.transform.localEulerAngles = new Vector3(77, 0, 0);
                float distance_two = Vector3.Distance(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.position);
                floatUnitPresentLocation += (Time.deltaTime * 30f) / distance_two;
                _FloatUnit.transform.position =
                    Vector3.Lerp(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.Find("FloatUnit_FinishPosition").transform.position, floatUnitPresentLocation);
            }

            floatUnitCurrentTime -= Time.deltaTime;
            //フロートユニット装着終了
            if(floatUnitCurrentTime <= 0 && isFloatUnit && !isIncapableAction && !isUnderAttack && !isLanding)
            {
                //フロートユニットがある場合
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
        //チャージ量のカンスト
        if(currenttime > maxtime)
        {
            return maxtime;
        }
        //チャージ量無くす
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
        //換装中の場合
        if (isReplacement)
        {
            replacementTime += Time.deltaTime;
            //換装終了
            if (replacementTime >= 1.0f)
            {
                ReplacementFinish();
            }
        }
    }

    //換装終了処理
    void ReplacementFinish()
    {
        Debug.Log("換装終了");
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
        Debug.Log("硬直中");
        rb.velocity = new Vector3(0, 0, 0);
    }

    //よろけとダウン時の処理
    void Stagger_Control()
    {
        //よろけまたはダウン中の場合
        if (isStagger || isDown)
        {
            //ダウンまたは空中にいる場合
            if (!isDown || !isAir)
            {
                staggerTime += Time.deltaTime;
            }
            //よろけモーション移行受付終了
            if(staggerTime >= 0.2f)
            {
                anim.SetBool("Stagger_Start", false);
            }
            //よろけまたはダウン中
            if(staggerTime <= 0.2f)
            {
                Vector3 stagger_move;
                //ダウンの場合
                if (isDown)
                {
                    Debug.Log("ダウン中");
                    stagger_move = other_forward * 0.3f;
                }
                //よろけの場合
                else
                {
                    Debug.Log("よろけ中");
                    stagger_move = other_forward * 0.20f;
                }
                rb.velocity = new Vector3(stagger_move.x, rb.velocity.y, stagger_move.z);
            }
            //よろけ終了
            if(staggerTime >= 1.0f && !isDown)
            {
                Debug.Log("よろけ終了");
                isIncapableAction = false;
                isStagger = false;
                isStiffness = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                staggerTime = 0;
            }
            //ダウン中の起き上がり開始
            else if(staggerTime >= 1f && staggerTime < 2f && isDown)
            {
                anim.SetBool("GetUp", true);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
            }
            //ダウン終了
            else if(staggerTime >= 2f && isDown)
            {
                Debug.Log("ダウン終了");
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
                //接地している場合
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("ジャンプボタンを押しました");
                    //行動可能の場合
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
                            //ブースト受付時間以内の場合
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
                                //ホバー機体の場合
                                if (!isTypeGroundRunning)
                                {
                                    transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                }
                                //接地している場合
                                else if (!isAir)
                                {
                                    //砂煙エフェクト開始(オフライン)
                                    if (SceneManager.GetActiveScene().name == "TrainingScene")
                                    {
                                        RpcEffDustPlay_Control();
                                    }
                                    //砂煙エフェクト開始(オンライン)
                                    else
                                    {
                                        pv.RPC(nameof(RpcEffDustPlay_Control), RpcTarget.AllBufferedViaServer);
                                    }
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("メイン射撃ボタンを押しました");
                    //メイン射撃可能な状態な場合
                    if (!isIncapableAction && (!isUnderAttack || lastmove_name == "subshooting"))
                    {
                        if (!isMainShooting)
                        {
                            //弾数が残っていて着地硬直していない場合
                            if (mainshooting_number >= 1 && !isLanding)
                            {
                                isMainShooting = true;
                                anim.SetBool("MainShooting", true);
                                isMainShootingFiring = false;
                                mainShootingTime = 0;
                                isUnderAttack = true;
                                Varis_Normal.SetActive(true);
                                Varis_FullPower.SetActive(false);
                                //ロックオン対象がいる場合
                                if (LockOnEnemy != null)
                                {
                                    gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 1;
                                }
                                //サブメインキャンセルした場合
                                if (lastmove_name == "subshooting")
                                {
                                    anim.SetBool("SubShooting_Start", false);
                                    SubShootingFinish();
                                }
                                lastmove_name = "mainshooting";
                                //振り向き撃ち
                                if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(MainCamera.transform.localEulerAngles.y + 180, 360) - 180)) >= 100f)
                                {
                                    //ロックオン対象がいる場合
                                    if (LockOnEnemy != null)
                                    {
                                        Debug.Log("振り向き撃ち");
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("サブ射撃ボタンを押しました");
                    //サブ射撃可能な状態の場合
                    if (!isIncapableAction && !isUnderAttack)
                    {
                        if (!isSubShooting)
                        {
                            //サブ射撃の弾数が残っていて着地硬直していない場合
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
                                //ロックオン対象がいる場合
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("特殊射撃ボタンを押しました");
                    //特殊射撃可能な状態の場合
                    if (!isIncapableAction
                        && (!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
                    {
                        if (!isSpecialShooting)
                        {
                            //特殊射撃の弾数が残っていて着地硬直していない場合
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

                                //ロックオン対象がいる場合
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("格闘ボタンを押しました");
                    Vector3 attack_direction;

                    if (!isIncapableAction)
                    {
                        //格闘可能な状態の場合
                        if (!isLanding || isUnderAttack)
                        {
                            //格闘初動に移行
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
                            //格闘2打目に移行
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
                            //格闘3打目に移行
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

                                //ロックオン対象がいる場合
                                if (isInduction && LockOnEnemy != null)
                                {
                                    gameObject.transform.LookAt(LockOnEnemy.transform);
                                }
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                lastmove_name = "subshooting_fightingvariants";
                                attack_direction = gameObject.transform.forward * 1;
                                attack_direction.Normalize();
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
        //コントロール権限がある場合
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //一度だけ実行
                if (context.phase == InputActionPhase.Performed)
                {
                    Debug.Log("特殊格闘ボタンを押しました");
                    Vector3 specialattack_direction;
                    //特殊格闘可能な状態の場合
                    if (!isIncapableAction && !isUnderAttack)
                    {
                        //着地硬直していない場合
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

                            //ロックオン対象がいる場合
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
        //金属エフェクトが存在する場合
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
        //ゲームが開始された場合
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
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //地面に着地
                if (other.gameObject.CompareTag("Ground") && anim != null)
                {
                    Debug.Log("地面に着地");
                    anim.SetBool("Jump", false);
                    anim.SetBool("Air", false);
                    isJump = false;
                    //ブースト中でない場合
                    if (!isBoost)
                    {
                        isLanding = true;
                        boost_amount = BOOST_MAX;
                        isIncapableAction = false;
                    }
                    //地走機体の場合
                    else if (isTypeGroundRunning)
                    {
                        //オフライン
                        if (SceneManager.GetActiveScene().name == "TrainingScene")
                        {
                            RpcEffDustPlay_Control();
                        }
                        //オンライン
                        else
                        {
                            pv.RPC(nameof(RpcEffDustPlay_Control), RpcTarget.AllBufferedViaServer);
                        }
                    }
                    isAir = false;

                    //ダウンした場合角度調整
                    if (isDown)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }

                    //ホバー機体でブースト中の場合浮かす
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
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //地面に接地している場合
                if (other.gameObject.CompareTag("Ground"))
                {
                    //行動可能な状態にする
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
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //地面から離れた場合
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
        //コントロール権限がある場合
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
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //自機が被弾した場合一度だけ実行
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
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //自機と接触したオブジェクトにBeam_Controlコンポートがある場合
                if (other.gameObject.GetComponent<Beam_Control>() != null)
                {
                    //接触したのが自機が生成したオブジェクトでない場合
                    if (other.gameObject.GetComponent<Beam_Control>().OwnMachine != gameObject)
                    {
                        //接触したオブジェクトに攻撃判定がある場合
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
        //接触したオブジェクトがBeam_Controlコンポーネントがある場合
        if (other.gameObject.GetComponent<Beam_Control>() != null)
        {
            //接触したオブジェクトが自機が生成したオブジェクトでない場合
            if (other.gameObject.GetComponent<Beam_Control>().OwnMachine != gameObject)
            {
                //ダウンしていない場合
                if (!isDown)
                {
                    //接触したオブジェクトに攻撃判定がある場合
                    if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Untagged"))
                    {
                        other_forward = other.transform.position;
                        //ガード成功時の反動
                        if (isDefense && Vector3.Angle(transform.forward, other.gameObject.transform.forward) >= 90)
                        {
                            Debug.Log("ガード成功時の反動");
                            isDefending = true;
                            defendingTime = 0;
                            Transform other_transform = other.transform;
                            other_transform.position = other_transform.forward * -2;
                            other_transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z);
                            transform.LookAt(other.transform);
                            defenserecoil = transform.forward * -30f;
                        }
                        //被弾したとき
                        else
                        {
                            Debug.Log("被弾しました");
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
                            //ダメージ補正は0.1未満にならない
                            if (correctionFactor < 0.1f)
                            {
                                correctionFactor = 0.1f;
                            }
                            downValue += other.gameObject.GetComponent<Beam_Control>().downValue;
                            //ダウン値が最大になった場合
                            if (downValue >= 6)
                            {
                                Debug.Log("ダウン");
                                isDown = true;
                                isIncapableAction = true;
                                isStiffness = true;
                                Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
                                //遠隔攻撃の場合
                                if (other.gameObject.CompareTag("Bullet"))
                                {
                                    //後ろへダウン
                                    if (Vector3.Distance(hitPos, gameObject.transform.forward * 1.1f) > Vector3.Distance(hitPos, gameObject.transform.forward * -1.1f))
                                    {
                                        anim.SetBool("DownBack", true);
                                    }
                                    //前へダウン
                                    else
                                    {
                                        anim.SetBool("DownFront", true);
                                    }
                                }
                                //格闘攻撃の場合
                                else if (other.gameObject.CompareTag("Untagged"))
                                {
                                    //後ろへダウン
                                    if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(other.transform.root.localEulerAngles.y + 180, 360) - 180)) >= 90f)
                                    {
                                        anim.SetBool("DownBack", true);
                                    }
                                    //前へダウン
                                    else
                                    {
                                        anim.SetBool("DownFront", true);
                                    }
                                }
                                staggerTime = 0;
                                correctionFactorResetTime = 0;
                                downValue = 0;
                                //空中にいる場合
                                if (isAir)
                                {
                                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 180);
                                }
                            }
                            //よろけ処理
                            else
                            {
                                Debug.Log("よろけ");
                                isStagger = true;
                                isIncapableAction = true;
                                isStiffness = true;
                                Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
                                //遠隔攻撃の場合
                                if (other.gameObject.CompareTag("Bullet"))
                                {
                                    //後ろへよろける
                                    if (Vector3.Distance(hitPos, gameObject.transform.forward * 1.1f) > Vector3.Distance(hitPos, gameObject.transform.forward * -1.1f))
                                    {
                                        anim.SetBool("StaggerBack_Landing", true);
                                    }
                                    //前へよろける
                                    else
                                    {
                                        anim.SetBool("StaggerFront_Landing", true);
                                    }
                                }
                                //格闘攻撃の場合
                                else if (other.gameObject.CompareTag("Untagged"))
                                {
                                    //後ろへよろける
                                    if (Mathf.Abs(Mathf.Abs(Mathf.Repeat(transform.localEulerAngles.y + 180, 360) - 180) - Mathf.Abs(Mathf.Repeat(other.transform.root.localEulerAngles.y + 180, 360) - 180)) >= 90f)
                                    {
                                        anim.SetBool("StaggerBack_Landing", true);
                                    }
                                    //前へよろける
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

                    //耐久値は0未満にならない
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
