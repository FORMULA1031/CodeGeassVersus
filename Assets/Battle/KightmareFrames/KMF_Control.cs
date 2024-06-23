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
    Rigidbody rb;
    PhotonView pv;
    public float boost_amount;
    float walk_speed = 15;
    float boost_speed = 60;
    float jump_power = 80;
    float jumprug_time = 0;
    float landing_time = 0;
    float boostbutton_time = 0;
    float boostconsumed_time = 0;
    float slide_time = 0;
    float mainshooting_time = 0;
    public float mainshooting_currentreloadtime = 0;
    public float mainshooting_reloadtime;
    float subshooting_time = 0;
    public float subshooting_currentreloadtime = 0;
    public float subshooting_reloadtime;
    float subShooting_fightingvariants_time = 0;
    float specialshooting_time = 0;
    public float specialshooting_currentreloadtime = 0;
    public float specialshooting_reloadtime;
    float attack_time = 0;
    float attackfinish_time = 1.8f;
    float specialattack_time = 0;
    public float fightingcharge_time = 0;
    public float fightingcharge_maxtime;
    public float fightingcharge_currentreloadtime = 0;
    public float fightingcharge_reloadtime;
    public float floatunit_currenttime = 0;
    public float floatunit_time;
    float replacement_time = 0;
    float floatunit_presentlocation;
    float stagger_time = 0;
    float correctionfactor = 1;
    float correctionfactor_resettime = 0;
    float down_value = 0;
    float leverfront_time = 0;
    float leverback_time = 0;
    float leverright_time = 0;
    float leverleft_time = 0;
    float defense_time = 0;
    float defenselever_time = 0;
    float defenseing_time = 0;
    float step_time = 0;
    bool jump_flag = false;
    bool jumpmove_flag = false;
    bool rise_flag = false;
    bool leverinsert_flag = false;
    bool leverfront_flag = false;
    bool leverback_flag = false;
    bool leverright_flag = false;
    bool leverleft_flag = false;
    bool landing_flag = false;
    bool air_flag = false;
    bool boost_flag = false;
    bool slide_flag = false;
    bool stiffness_flag = false;
    bool incapableofaction_flag = false;
    bool mainshooting_flag = false;
    bool mainshootingfiring_flag = false;
    bool subshooting_flag = false;
    bool subshootingfiring_flag = false;
    bool subshooting_fightingvariants_flag = false;
    bool isinair_subshooting_fightingvariants_flag = false;
    bool attack_flag = false;
    bool attack1_flag = false;
    bool attack2_flag = false;
    bool attack3_flag = false;
    bool specialshooting_flag = false;
    bool specialshootinganimation_flag = false;
    bool specialshootingfiring_flag = false;
    public bool fightingchargeinput_flag = false;
    public bool floatunit_flag = false;
    bool replacement_flag = false;
    bool induction_flag = true;
    public bool inductionoff_flag = false;
    bool stagger_flag = false;
    public bool down_flag = false;
    bool underattack_flag = false;
    bool defense_flag = false;
    bool defenseing_flag = false;
    bool step_flag = false;
    bool stepjump_flag = false;
    bool jumpkey_pressing = false;
    bool hitstart_stay_flag = false;
    public bool type_groundrunnig;
    public int durable_value;
    public int durable_maxvalue;
    int boost_maxamount = 100;
    public int mainshooting_number;
    bool specialattack_flag = false;
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
        boost_amount = boost_maxamount;
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
            incapableofaction_flag = true;
        }
        if (correctionfactor < 1)
        {
            correctionfactor_resettime += Time.deltaTime;
            if (correctionfactor_resettime >= 3f)
            {
                correctionfactor = 1;
                down_value = 0;
            }
        }

        if (!(defense_flag || subshooting_fightingvariants_flag) && !rb.useGravity)
        {
            rb.useGravity = true;
        }
        if (!down_flag && !stagger_flag)
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
            if (!boost_flag)
            {
                RpcEffControl_NotBoostFlag();
            }
        }
        else
        {
            pv.RPC(nameof(RpcEffControl), RpcTarget.AllBufferedViaServer);
            if (!boost_flag)
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
        if (!stiffness_flag)
        {
            if (!incapableofaction_flag)
            {
                if (!landing_flag && !slide_flag)
                {
                    if (leverinsert_flag && !defense_flag && lastmove_name != "jump")
                    {
                        if (type_groundrunnig || !boost_flag)
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
                else if (slide_flag)
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
            if (down_flag || specialattack_flag || subshooting_fightingvariants_flag || attack_flag)
            {
            }
            else if (step_flag || stagger_flag)
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

    //レバー制御
    void MoveKeyControls()
    {
        float x = Input.GetAxis("Move_X");
        float z = Input.GetAxis("Move_Y") * -1;
        movingdirection = MainCamera.transform.forward * z + MainCamera.transform.right * x;
        if(x != 0 || z != 0)
        {
            leverinsert_flag = true;
            anim.SetBool("Walk", true);
        }
        else
        {
            leverinsert_flag = false;
            lastlever_name = "null";
            anim.SetBool("Walk", false);
        }
        movingdirection.Normalize();//斜めの距離が長くなるのを防ぎます
        if (!boost_flag && !slide_flag)
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * walk_speed;
        }
        if (!landing_flag)
        {
            DefenseAndStepControl(x, z);
        }
    }

    //ガードとステップ制御
    void DefenseAndStepControl(float x, float z)
    {
        if (Mathf.Abs(x) >= 0.8f || Mathf.Abs(z) >= 0.8f)
        {
            if (Mathf.Abs(x) > Mathf.Abs(z))
            {
                //右入力
                if (x > 0)
                {
                    if (lastlever_name == "null" && leverright_flag && leverright_time < 0.2f && !step_flag)
                    {
                        if ((!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "attack" || slide_flag) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * walk_speed;
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
                    leverright_time = 0;
                    leverright_flag = true;
                    leverfront_flag = false;
                    leverback_flag = false;
                    leverleft_flag = false;
                }
                //左入力
                else if (x < 0)
                {
                    if (lastlever_name == "null" && leverleft_flag && leverleft_time < 0.2f && !step_flag)
                    {
                        if ((!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "attack" || slide_flag) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.right * -walk_speed;
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
                    leverleft_time = 0;
                    leverleft_flag = true;
                    leverfront_flag = false;
                    leverback_flag = false;
                    leverright_flag = false;
                }
                defenselever_time = 2f;
            }
            else
            {
                //前入力
                if (z > 0)
                {
                    if (leverback_flag && !underattack_flag)
                    {
                        if (!defense_flag && !underattack_flag)
                        {
                            Vector3 direction;
                            defense_flag = true;
                            defense_time = 0;
                            defenselever_time = 0;
                            boostconsumed_time = 0;
                            stiffness_flag = true;
                            rb.useGravity = false;
                            underattack_flag = true;
                            boost_flag = false;
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
                    defenselever_time += Time.deltaTime;

                    if (lastlever_name == "null" && leverfront_flag && leverfront_time < 0.2f && !step_flag)
                    {
                        if ((!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "attack" || slide_flag) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * walk_speed;
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
                    leverfront_time = 0;
                    leverfront_flag = true;
                    leverback_flag = false;
                    leverleft_flag = false;
                    leverright_flag = false;
                }
                //後ろ入力
                else if (z < 0)
                {
                    if(lastlever_name == "null" && leverback_flag && leverback_time < 0.2f && !step_flag)
                    {
                        if ((!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "attack" || slide_flag) && boost_amount > 0)
                        {
                            step_move = MainCamera.transform.forward * -walk_speed;
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
                    leverback_time = 0;
                    leverback_flag = true;
                    leverfront_flag = false;
                    leverleft_flag = false;
                    leverright_flag = false;
                    defenselever_time = 2f;
                }
            }
        }
        else
        {
            defenselever_time = 2f;
        }

        //ステップ受付制御
        if (leverfront_flag || lastlever_name == "front")
        {
            leverfront_time += Time.deltaTime;
            if (leverfront_time >= 0.2f)
            {
                leverfront_flag = false;
            }
        }
        if (leverback_flag || lastlever_name == "back")
        {
            leverback_time += Time.deltaTime;
            if (leverback_time >= 0.2f)
            {
                leverback_flag = false;
            }
        }
        if (leverright_flag || lastlever_name == "right")
        {
            leverright_time += Time.deltaTime;
            if (leverright_time >= 0.2f)
            {
                leverright_flag = false;
            }
        }
        if (leverleft_flag || lastlever_name == "left")
        {
            leverleft_time += Time.deltaTime;
            if(leverleft_time >= 0.2f)
            {
                leverleft_flag = false;
            }
        }

        //ガード中
        if (defense_flag)
        {
            anim.SetBool("Defense", true);
            
            if ((defenselever_time >= 2f && !defenseing_flag) || incapableofaction_flag)
            {
                defense_time += Time.deltaTime;
            }

            //ガード終了
            if (defense_time >= 0.5f)
            {
                DefenseFinish();
            }

            //ブースト消費
            boostconsumed_time += Time.deltaTime;
            if(boostconsumed_time > 0.01f)
            {
                boost_amount -= 0.1f;
                boostconsumed_time = 0;
            }
        }

        if (defenseing_flag)
        {
            //反動
            stiffness_flag = false;
            defenseing_time += Time.deltaTime;
            if(defenseing_time <= 0.2f)
            {
                rb.velocity = new Vector3(defenserecoil.x, defenserecoil.y, defenserecoil.z);
            }
            else
            {
                stiffness_flag = true;
            }
            if(defenseing_time >= 0.5f)
            {
                defenseing_flag = false;
                defenseing_time = 0;
            }
        }

        //ステップ制御
        if (step_flag)
        {
            if (slide_flag)
            {
                SlideFinish();
            }

            step_time += Time.deltaTime;
            if(step_time < 0.1)
            {
                inductionoff_flag = true;
            }
            if (step_time >= 0.05f)
            {
                inductionoff_flag = false;
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
            if(step_time >= 0.3f)
            {
                if (!air_flag)
                {
                    boost_amount = boost_maxamount;
                }
                StepFinish();
            }
            lastmove_name = "step";
        }
    }

    void DefenseFinish()
    {
        defense_flag = false;
        anim.SetBool("Defense", false);
        stiffness_flag = false;
        underattack_flag = false;
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

    //ステップ開始
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
        stiffness_flag = true;
        step_flag = true;
        inductionoff_flag = true;
        step_time = 0;
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

    void StepAnimationSelect(string stepfront, string stepback, string stepright, string stepleft)
    {

        if (leverfront_flag)
        {
            anim.SetBool(stepfront, true);
        }
        if (leverback_flag)
        {
            anim.SetBool(stepback, true);
        }
        if (leverright_flag)
        {
            anim.SetBool(stepright, true);
        }
        if (leverleft_flag)
        {
            anim.SetBool(stepleft, true);
        }
    }

    //ステップ終了
    void StepFinish()
    {
        inductionoff_flag = false;
        stiffness_flag = false;
        step_flag = false;
        step_time = 0;
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

    void KMF_Rotation()
    {
        if (leverinsert_flag)
        {
            Vector3 diff = transform.position - new Vector3(latestPos.x, transform.position.y, latestPos.z);   //前回からどこに進んだかをベクトルで取得
            latestPos = transform.position;  //前回のPositionの更新
            if (diff != Vector3.zero)
            {
                float turning_angle = 0.1f;
                if(boost_flag)
                {
                    turning_angle = 0.02f;
                }
                else if (air_flag)
                {
                    turning_angle = 0;
                }
                Quaternion rotation = Quaternion.LookRotation (movingdirection);
                transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, turning_angle);
            }
        }
    }

    //ジャンプ＆ブースト開始時の方向転換
    void KMF_RapidRotation()
    {
        if (leverinsert_flag)
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

    void JumpKeyControls()
    {
        if (!incapableofaction_flag)
        {
            //ブースト中
            if (boost_flag)
            {
                Boosting();
            }
            //ジャンプ
            if (!jump_flag) { }
            else if (!type_groundrunnig && !replacement_flag)
            {
                if (jump_flag && !boost_flag && !jumpkey_pressing)
                {
                    if (!stiffness_flag || step_flag)
                    {
                        if (jumprug_time >= 0.3f)
                        {
                            rise_flag = false;
                            jump_flag = false;
                        }
                    }
                }
            }
        }

        //ジャンプ処理
        if (jump_flag && !boost_flag)
        {
            if (!stiffness_flag || step_flag)
            {
                //ジャンプ開始時に1度だけ実行
                if (jumpmove_flag)
                {
                    if (!step_flag)
                    {
                        KMF_RapidRotation();
                    }
                    else
                    {
                        stepjump_flag = true;
                    }
                    StepFinish();
                    jumpmove_flag = false;
                }

                //地走用
                if (type_groundrunnig)
                {
                    jumprug_time += Time.deltaTime;
                    if (jumprug_time >= 0.2f && !rise_flag)
                    {
                        JumpStart();
                    }
                    else if (jumprug_time <= 1.0f && jumprug_time >= 0.2f)
                    {
                        Jumping();
                        rise_flag = false;
                        jump_flag = false;
                    }
                }
                //ホバー用
                else
                {
                    jumprug_time += Time.deltaTime;
                    if (jumprug_time >= 0.2f && !rise_flag)
                    {
                        JumpStart();
                    }
                    else if (jumprug_time >= 0.2f)
                    {
                        Jumping();
                        boost_amount -= Time.deltaTime * 10f;
                    }
                }
            }
            else
            {
                if (underattack_flag || step_flag)
                {
                    jumprug_time += Time.deltaTime;
                    if (jumprug_time >= 0.2f && !rise_flag)
                        jump_flag = false;
                }
                else
                {
                    jump_flag = false;
                }
            }
        }
        else
        {
            anim.SetBool("Jump", false);
        }

        //ブースト終了
        if ((!leverinsert_flag && !Input.GetButton("Boost")) || incapableofaction_flag)
        {
            if (boost_flag)
            {
                BoostFinish();
                if (!air_flag)
                {
                    slide_flag = true;
                    underattack_flag = true;
                }
            }
        }

        //ズサ時間
        if (slide_flag)
        {
            anim.SetBool("Boost_Landing_Finish", true);
            slide_time += Time.deltaTime;
            if(slide_time > 1.5f)
            {
                SlideFinish();
                boost_amount = boost_maxamount;
            }
        }
    }

    //ジャンプ開始時の制御
    void JumpStart()
    {
        if (!landing_flag)
        {
            lastmove_name = "jump";
            anim.SetBool("Jump", true);
            anim.SetBool("Boost_Landing", false);
            boost_amount -= 10;
            rise_flag = true;
        }
    }

    //ジャンプ中の制御
    void Jumping()
    {
        if (!landing_flag)
        {
            Vector3 jump_direction;
            Vector3 jump_moving;

            jump_direction = gameObject.transform.up * 1;
            jump_direction.Normalize();//斜めの距離が長くなるのを防ぎます
            jump_moving = jump_direction * jump_power;
            movingvelocity = rb.velocity;
            if (stepjump_flag)
            {
                movingvelocity /= 2;
            }
            rb.velocity = new Vector3(movingvelocity.x, jump_moving.y, movingvelocity.z);
        }
    }

    void Boosting()
    {
        if (!type_groundrunnig)
        {
            rb.useGravity = false;
        }
        if (movingdirection != new Vector3(0, 0, 0) && leverinsert_flag)
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * boost_speed;
        }
        else
        {
            Vector3 boost_direction;
            boost_direction = transform.forward;
            boost_direction.Normalize();
            movingvelocity = boost_direction * boost_speed;
            rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
        }
        boostconsumed_time += Time.deltaTime;
        if (boostconsumed_time >= 0.1f)
        {
            boost_amount -= 2;
            boostconsumed_time = 0;
        }
    }

    //ブースト終了
    void BoostFinish()
    {
        boost_flag = false;
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

    //ズサ終了
    void SlideFinish()
    {
        anim.SetBool("Boost_Landing_Finish", false);
        slide_flag = false;
        slide_time = 0;
        incapableofaction_flag = false;
        underattack_flag = false;
    }

    //着地制御
    void LandingTime()
    {
        if (landing_flag && (!underattack_flag || lastmove_name == "mainshooting"))
        {
            anim.SetBool("Landing", true);
            landing_time += Time.deltaTime;
            if(landing_time >= 0.5f)
            {
                lastmove_name = "null";
                landing_flag = false;
                landing_time = 0;
                anim.SetBool("Landing", false);
                underattack_flag = false;
            }
        }
    }

    void Rig_Setting(string rig_name, GameObject _LockOnEnemy)
    {
        var sourceobject = gameObject.transform.Find(rig_name).gameObject.transform.GetComponent<MultiAimConstraint>().data.sourceObjects;
        sourceobject.SetTransform(0, _LockOnEnemy.transform);
        gameObject.transform.Find(rig_name).gameObject.transform.GetComponent<MultiAimConstraint>().data.sourceObjects = sourceobject;
    }

    ////////////////////////////////////////////////////////

    //射撃ボタン制御
    void ShootingKeyControls()
    {
        //メイン射撃中
        if (mainshooting_flag)
        {
            mainshooting_time += Time.deltaTime;
            if(mainshooting_time >= 0.23f && !mainshootingfiring_flag)
            {
                mainshooting_number--;
                mainshootingfiring_flag = true;
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
            if(mainshooting_time >= 1.0f)
            {
                MainShootingFinish();
                underattack_flag = false;
            }

            if(LockOnEnemy == null)
            {
                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
            }
        }

        //リロード
        if (mainshooting_maxnumber > mainshooting_number)
        {
            mainshooting_currentreloadtime += Time.deltaTime;
            if(mainshooting_currentreloadtime >= mainshooting_reloadtime)
            {
                mainshooting_number++;
                mainshooting_currentreloadtime = 0;
            }
        }
    }

    void MainShootingFinish()
    {
        mainshooting_flag = false;
        stiffness_flag = false;
        anim.SetBool("MainShooting", false);
        gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
    }

    //サブ射撃制御
    void SubShootingControls()
    {
        if (subshooting_flag)
        {
            subshooting_time += Time.deltaTime;
            if(subshooting_time >= 0.3f && !subshootingfiring_flag)
            {
                subshooting_number--;
                subshootingfiring_flag = true;
                if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    SlashHarken_Instance = Instantiate(SlashHarken, Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                else
                {
                    SlashHarken_Instance = PhotonNetwork.Instantiate("Prefab/SlashHarken(Object)", Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
                }
                Vector3 SlashHarkenAngle = SlashHarken_Instance.transform.eulerAngles;
                SlashHarkenAngle.y -= 180.0f; // ワールド座標を基準に、y軸を軸にした回転を10度に変更
                SlashHarkenAngle.z += 90.0f; // ワールド座標を基準に、z軸を軸にした回転を10度に変更
                SlashHarken_Instance.transform.eulerAngles = SlashHarkenAngle; // 回転角度を設定
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
            if (subshooting_time >= 0.8f && subshooting_time < 1.5f)
            {
                anim.SetBool("SubShooting_Start", false);
                anim.SetBool("SubShooting_Finish", true);
                if (SlashHarken_Instance != null)
                {
                    Destroy(SlashHarken_Instance);
                }
            }
            if (subshooting_time >= 1.5f)
            {
                SubShootingFinish();
                underattack_flag = false;
            }

            if(SlashHarken_Instance != null)
            {
                SlashHarken_Instance.transform.Find("SlashHarken/Bone.001/Bone.004").transform.position =BlazeLuminous.transform.position;
            }
        }

        //リロード
        if (subshooting_maxnumber > subshooting_number)
        {
            subshooting_currentreloadtime += Time.deltaTime;
            if (subshooting_currentreloadtime >= subshooting_reloadtime)
            {
                subshooting_number++;
                subshooting_currentreloadtime = 0;
            }
        }
    }

    void SubShootingFinish()
    {
        anim.SetBool("SubShooting_Start", false);
        anim.SetBool("SubShooting_Finish", false);
        subshooting_flag = false;
        stiffness_flag = false;
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        if (SlashHarken_Instance != null)
        {
            Destroy(SlashHarken_Instance);
        }
    }

    //特殊射撃ボタン制御
    void SpecialShootingControls()
    {
        if (!incapableofaction_flag
            && (!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
        {
            if (!specialshooting_flag)
            {

            }
            else if (specialshooting_flag && specialshootinganimation_flag)
            {
                anim.SetBool("SpecialShooting_Start", false);
                specialshootinganimation_flag = false;
            }
        }

        //特殊射撃中
        if (specialshooting_flag)
        {
            specialshooting_time += Time.deltaTime;
            if (specialshooting_time >= 0.6f && !specialshootingfiring_flag)
            {
                specialshooting_number--;
                specialshootingfiring_flag = true;
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
            if (specialshooting_time >= 1.5f)
            {
                SpecialShootingFinish();
            }
        }

        //リロード
        if (specialshooting_number == 0)
        {
            specialshooting_currentreloadtime += Time.deltaTime;
            if (specialshooting_currentreloadtime >= specialshooting_reloadtime)
            {
                specialshooting_number = specialshooting_maxnumber;
                specialshooting_currentreloadtime = 0;
            }
        }
    }

    void SpecialShootingFinish()
    {
        anim.SetBool("SpecialShooting", false);
        specialshooting_flag = false;
        stiffness_flag = false;
        underattack_flag = false;
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    //格闘ボタン制御
    void AttackControls()
    {
        Vector3 attack_direction;

        //格闘処理
        if (attack_flag)
        {
            attack_time += Time.deltaTime;
            if (attack_time <= 0.8f && (!attack1_flag && !attack2_flag && !attack3_flag))
            {
                if (induction_flag)
                {
                    if (LockOnEnemy != null)
                    {
                        gameObject.transform.LookAt(LockOnEnemy.transform);
                    }
                    attack_direction = gameObject.transform.forward * 1;
                    attack_direction.Normalize();//斜めの距離が長くなるのを防ぎます
                    attack_moving = attack_direction * boost_speed * 1f;
                    rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
                }
            }
            else if (attack_time > 0.8f && (!attack1_flag && !attack2_flag && !attack3_flag))
            {
                attack_time = 0;
                attackfinish_time = 1.0f;
                attack1_flag = true;
                anim.SetBool("Attack_Induction", false);
                anim.SetBool("Attack1", true);
                Rigidity();
            }
            else
            {
                attack_direction = gameObject.transform.forward * 1;
                attack_direction.Normalize();//斜めの距離が長くなるのを防ぎます
                attack_moving = attack_direction * 10f;
                rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
            }
            if (attack_time >= attackfinish_time)
            {
                AttackFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            }
        }

        //サブ射撃格闘派生処理
        if (subshooting_fightingvariants_flag)
        {
            subShooting_fightingvariants_time += Time.deltaTime;
            if (subShooting_fightingvariants_time <= 0.2f)
            {
                if (induction_flag && LockOnEnemy != null)
                {
                    gameObject.transform.LookAt(LockOnEnemy.transform);
                }
                attack_direction = gameObject.transform.forward * 1;
                attack_direction.Normalize();//斜めの距離が長くなるのを防ぎます
                attack_moving = attack_direction * boost_speed;
                rb.velocity = new Vector3(attack_moving.x, attack_moving.y, attack_moving.z);
                isinair_subshooting_fightingvariants_flag = air_flag;
            }
            if (subShooting_fightingvariants_time >= 2.0f)
            {
                SubShootingFightingVariantsFinish();
                if (!isinair_subshooting_fightingvariants_flag)
                {
                    landing_flag = true;
                    boost_amount = boost_maxamount;
                    incapableofaction_flag = false;
                }
            }
        }
    }

    public void StartAttack()
    {
        if (attack_flag)
        {
            if (attack_time < 0.8f && (!attack1_flag && !attack2_flag && !attack3_flag))
            {
                attack_time = 0;
                attackfinish_time = 1.0f;
                attack1_flag = true;
                anim.SetBool("Attack1", true);
                Rigidity();
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_Start"))
            {
                anim.SetBool("Attack_Induction", false);
            }
        }
    }

    //格闘終了
    void AttackFinish()
    {
        anim.SetBool("Attack_Induction", false);
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        attack_flag = false;
        attack1_flag = false;
        attack2_flag = false;
        attack3_flag = false;
        stiffness_flag = false;
        underattack_flag = false;
        attack_time = 0;
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        MVS_R.SetActive(false);
        MVS_L.SetActive(false);
        MVSSheathing_R.SetActive(true);
        MVSSheathing_L.SetActive(true);
        Eff_SpeedLine.SetActive(false);
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void SubShootingFightingVariantsFinish()
    {
        Varis_Normal.SetActive(true);
        Varis_FullPower.SetActive(false);
        anim.SetBool("SubShooting_FightingVariants", false);
        subshooting_fightingvariants_flag = false;
        stiffness_flag = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        underattack_flag = false;
        rb.useGravity = true;
        Leg_R.transform.GetComponent<BoxCollider>().enabled = false;
    }

    //特殊格闘ボタン制御
    void SpecialAttackControls()
    {
        if (specialattack_flag)
        {
            specialattack_time += Time.deltaTime;
            if (specialattack_time <= 0.4f)
            {
                rb.velocity = new Vector3(specialattack_moving.x, rb.velocity.y, specialattack_moving.z);
            }
            if (specialattack_time >= 1.2f)
            {
                Varis_Normal.SetActive(true);
                Varis_FullPower.SetActive(false);
                SpecialAttackFinish();
                underattack_flag = false;
            }
        }
    }

    //特殊格闘終了
    void SpecialAttackFinish()
    {
        anim.SetBool("SpecialAttack", false);
        specialattack_flag = false;
        stiffness_flag = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //格闘CS
    void FightingChargeControl()
    {
        if (fightingcharge_currentreloadtime < fightingcharge_reloadtime && !fightingchargeinput_flag && !floatunit_flag)
        {
            fightingcharge_currentreloadtime += Time.deltaTime;
            if(fightingcharge_currentreloadtime >= fightingcharge_reloadtime)
            {
                fightingchargeinput_flag = true;
            }
        }
        else if (Input.GetButtonUp("Attack") && fightingcharge_time >= fightingcharge_maxtime && !floatunit_flag &&
            !incapableofaction_flag && !underattack_flag && !landing_flag)
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
            fightingcharge_currentreloadtime = 0;
            fightingcharge_time = 0;
            floatunit_presentlocation = 0;
            fightingchargeinput_flag = false;
            floatunit_flag = true;
            BoostFinish();
            StepFinish();
            floatunit_currenttime = floatunit_time;
            type_groundrunnig = false;
            anim.SetBool("Replacement", true);
            replacement_flag = true;
            stiffness_flag = true;
            underattack_flag = true;
            lastmove_name = "fightingchargeshot";
        }
        else if (Input.GetButton("Attack") && fightingchargeinput_flag && !floatunit_flag)
        {
            fightingcharge_time += Time.deltaTime;
        }
        else if(fightingchargeinput_flag && !floatunit_flag)
        {
            fightingcharge_time -= Time.deltaTime;
        }

        fightingcharge_time = ChargeControl(fightingcharge_time, fightingcharge_maxtime);

        if (floatunit_flag || fightingchargeinput_flag)
        {
            if (_FloatUnit != null)
            {
                _FloatUnit.transform.localEulerAngles = new Vector3(77, 0, 0);
                float distance_two = Vector3.Distance(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.position);
                floatunit_presentlocation += (Time.deltaTime * 30f) / distance_two;
                _FloatUnit.transform.position =
                    Vector3.Lerp(Cockpit.transform.Find("InstanceFloatUnit").transform.position, Cockpit.transform.Find("FloatUnit_FinishPosition").transform.position, floatunit_presentlocation);
            }

            floatunit_currenttime -= Time.deltaTime;
            if(floatunit_currenttime <= 0 && floatunit_flag && !incapableofaction_flag && !underattack_flag && !landing_flag)
            {
                if (_FloatUnit != null)
                {
                    Destroy(_FloatUnit);
                }
                BoostFinish();
                StepFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                floatunit_flag = false;
                type_groundrunnig = true;
                anim.SetBool("Replacement", true);
                replacement_flag = true;
                stiffness_flag = true;
                underattack_flag = true;
                lastmove_name = "fightingchargeshot";
            }
        }

        ReplacementMove();
    }

    ////////////////////////////////////////

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

    void ReplacementMove()
    {
        if (replacement_flag)
        {
            replacement_time += Time.deltaTime;
            if (replacement_time >= 1.0f)
            {
                ReplacementFinish();
            }
        }
    }

    void ReplacementFinish()
    {
        replacement_time = 0;
        replacement_flag = false;
        stiffness_flag = false;
        underattack_flag = false;
        anim.SetBool("Replacement", false);
    }

    //重力オフ
    void GravityOff()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //足止め
    void Rigidity()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    //よろけ＆ダウン処理
    void Stagger_Control()
    {
        if (stagger_flag || down_flag)
        {
            if (!down_flag || !air_flag)
            {
                stagger_time += Time.deltaTime;
            }
            if(stagger_time >= 0.2f)
            {
                anim.SetBool("Stagger_Start", false);
            }
            if(stagger_time <= 0.2f)
            {
                Vector3 stagger_move;
                if (down_flag)
                {
                    stagger_move = other_forward * 0.3f;
                }
                else
                {
                    stagger_move = other_forward * 0.20f;
                }
                rb.velocity = new Vector3(stagger_move.x, rb.velocity.y, stagger_move.z);
            }
            if(stagger_time >= 1.0f && !down_flag)
            {
                incapableofaction_flag = false;
                stagger_flag = false;
                stiffness_flag = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                stagger_time = 0;
            }
            else if(stagger_time >= 1f && stagger_time < 2f && down_flag)
            {
                anim.SetBool("GetUp", true);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
            }
            else if(stagger_time >= 2f && down_flag)
            {
                incapableofaction_flag = false;
                stagger_flag = false;
                stiffness_flag = false;
                down_flag = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
                anim.SetBool("GetUp", false);
                stagger_time = 0;
                if (!air_flag)
                {
                    boost_amount = boost_maxamount;
                }
            }
        }
    }

    ////////////////////////////////////////////////////////////
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!incapableofaction_flag)
                    {
                        jumpkey_pressing = true;
                        //ジャンプ
                        if (!jump_flag)
                        {
                            BoostFinish();
                            jump_flag = true;
                            jumprug_time = 0;
                            boost_flag = false;
                            boostbutton_time = 0;
                            jumpmove_flag = true;
                            stepjump_flag = false;
                        }
                        //ブースト
                        else if (jump_flag)
                        {
                            boostbutton_time += Time.deltaTime;
                            if (boostbutton_time < 0.2f)
                            {
                                boost_flag = true;
                                jump_flag = false;
                                rise_flag = false;
                                anim.SetBool("Boost_Landing", true);
                                anim.SetBool("SubShooting_Start", false);
                                anim.SetBool("SpecialShooting_Start", false);
                                anim.SetBool("Boost_Landing_Finish", false);
                                slide_flag = false;
                                slide_time = 0;
                                MainShootingFinish();
                                SubShootingFinish();
                                SubShootingFightingVariantsFinish();
                                SpecialShootingFinish();
                                AttackFinish();
                                SpecialAttackFinish();
                                StepFinish();
                                ReplacementFinish();
                                boost_amount -= 15;
                                boostconsumed_time = 0;
                                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
                                Eff_SpeedLine.SetActive(true);
                                KMF_RapidRotation();
                                if (!type_groundrunnig)
                                {
                                    transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                }
                                else if (!air_flag)
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
    public void OffJump(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    jumpkey_pressing = false;
                }
            }
        }
    }

    public void OnMainShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!incapableofaction_flag && (!underattack_flag || lastmove_name == "subshooting"))
                    {
                        if (!mainshooting_flag)
                        {
                            if (mainshooting_number >= 1 && !landing_flag)
                            {
                                mainshooting_flag = true;
                                anim.SetBool("MainShooting", true);
                                mainshootingfiring_flag = false;
                                mainshooting_time = 0;
                                underattack_flag = true;
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
                                        stiffness_flag = true;
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
    public void OnSubShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!incapableofaction_flag && !underattack_flag)
                    {
                        if (!subshooting_flag)
                        {
                            if (subshooting_number >= 1 && !landing_flag)
                            {
                                StepFinish();
                                anim.SetBool("SubShooting_Start", true);
                                subshooting_flag = true;
                                subshootingfiring_flag = false;
                                subshooting_time = 0;
                                stiffness_flag = true;
                                underattack_flag = true;
                                Varis_Normal.SetActive(true);
                                Varis_FullPower.SetActive(false);

                                boost_flag = false;
                                anim.SetBool("Boost_Landing", false);
                                if (induction_flag && LockOnEnemy != null)
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
    public void OnSpecialShoot(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    if (!incapableofaction_flag
                    && (!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
                    {
                        if (Input.GetButtonDown("SpecialShooting") && !specialshooting_flag)
                        {
                            if (specialshooting_number >= 1 && !landing_flag)
                            {
                                StepFinish();
                                MainShootingFinish();
                                SpecialAttackFinish();
                                anim.SetBool("SpecialShooting", true);
                                anim.SetBool("SpecialShooting_Start", true);
                                specialshooting_flag = true;
                                specialshootingfiring_flag = false;
                                specialshooting_time = 0;
                                specialshootinganimation_flag = true;
                                stiffness_flag = true;
                                underattack_flag = true;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(true);

                                boost_flag = false;
                                anim.SetBool("Boost_Landing", false);

                                if (induction_flag && LockOnEnemy != null)
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
    public void OnFight(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    Vector3 attack_direction;

                    if (!incapableofaction_flag)
                    {
                        if (!landing_flag || underattack_flag)
                        {
                            if (!attack_flag && !attack1_flag && (!underattack_flag || lastmove_name == "specialattack"))
                            {
                                StepFinish();
                                SpecialAttackFinish();
                                anim.SetBool("Attack_Induction", true);
                                attack_flag = true;
                                stiffness_flag = true;
                                underattack_flag = true;
                                attack_time = 0;
                                attackfinish_time = 1.8f;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(false);
                                MVS_R.SetActive(true);
                                MVS_L.SetActive(true);
                                MVSSheathing_R.SetActive(false);
                                MVSSheathing_L.SetActive(false);
                                Eff_SpeedLine.SetActive(true);
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;

                                boost_flag = false;
                                anim.SetBool("Boost_Landing", false);
                                lastmove_name = "attack";
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                            }
                            else if (attack_flag && attack1_flag && !attack2_flag
                                && anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_01"))
                            {
                                anim.SetBool("Attack1", false);
                                anim.SetBool("Attack2", true);
                                attack2_flag = true;
                                attack_time = 0;
                                attackfinish_time = 1.0f;
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = true;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().power = 70;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().correctionfactor = 0.15f;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().down_value = 0.3f;
                            }
                            else if (attack2_flag
                                && anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Attack_02"))
                            {
                                anim.SetBool("Attack1", false);
                                anim.SetBool("Attack2", false);
                                anim.SetBool("Attack3", true);
                                attack3_flag = true;
                                attack2_flag = false;
                                attack_time = 0;
                                attackfinish_time = 1.0f;
                                MVS_R.transform.Find("MVS").GetComponent<BoxCollider>().enabled = false;
                                MVS_L.transform.Find("MVS").GetComponent<BoxCollider>().enabled = true;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().power = 80;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().correctionfactor = 0.12f;
                                MVS_L.transform.Find("MVS").GetComponent<Beam_Control>().down_value = 6;
                            }

                            //サブ射撃格闘派生
                            if (lastmove_name == "subshooting" && subshooting_flag && !subshooting_fightingvariants_flag)
                            {
                                SubShootingFinish();
                                anim.SetBool("SubShooting_FightingVariants", true);
                                anim.SetBool("SubShooting_Start", false);
                                subshooting_fightingvariants_flag = true;
                                stiffness_flag = true;
                                underattack_flag = true;
                                rb.useGravity = false;
                                subShooting_fightingvariants_time = 0;
                                Varis_Normal.SetActive(false);
                                Varis_FullPower.SetActive(false);
                                Leg_R.transform.GetComponent<BoxCollider>().enabled = true;

                                boost_flag = false;
                                anim.SetBool("Boost_Landing", false);

                                if (induction_flag && LockOnEnemy != null)
                                {
                                    gameObject.transform.LookAt(LockOnEnemy.transform);
                                }
                                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                lastmove_name = "subshooting_fightingvariants";
                                attack_direction = gameObject.transform.forward * 1;
                                attack_direction.Normalize();//斜めの距離が長くなるのを防ぎます
                                attack_moving = attack_direction * boost_speed;
                            }
                        }
                    }
                }
            }
        }
    }
    public void OnSpecialFight(InputAction.CallbackContext context)
    {
        if (pv != null || SceneManager.GetActiveScene().name == "TrainingScene")
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (context.phase == InputActionPhase.Performed)
                {
                    Vector3 specialattack_direction;
                    if (!incapableofaction_flag && !underattack_flag)
                    {
                        if (!landing_flag)
                        {
                            StepFinish();
                            anim.SetBool("SpecialAttack", true);
                            specialattack_flag = true;
                            specialattack_time = 0;
                            boost_amount -= 10;
                            stiffness_flag = true;
                            underattack_flag = true;
                            Varis_Normal.SetActive(false);
                            Varis_FullPower.SetActive(false);

                            boost_flag = false;
                            anim.SetBool("Boost_Landing", false);

                            if (induction_flag && LockOnEnemy != null)
                            {
                                gameObject.transform.LookAt(LockOnEnemy.transform);
                            }
                            lastmove_name = "specialattack";
                            specialattack_direction = gameObject.transform.forward * 1;
                            specialattack_direction.Normalize();//斜めの距離が長くなるのを防ぎます
                            specialattack_moving = specialattack_direction * boost_speed * 1.2f;
                        }
                    }
                }
            }
        }
    }

    ////////////////////////////////////////////////////////////

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

    [PunRPC]
    void StepLineDirection(Vector3 vector3)
    {
        Eff_StepLine.transform.localEulerAngles = vector3;
    }

    [PunRPC]
    void RpcEffControl_NotBoostFlag()
    {
        EffDust_R.GetComponent<ParticleSystem>().Stop();
        EffDust_L.GetComponent<ParticleSystem>().Stop();
    }

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

    [PunRPC]
    void EffSetActive(bool display_flag)
    {
        Eff_StepLine.SetActive(display_flag);
    }

    [PunRPC]
    void RpcEffDustPlay_Control()
    {
        EffDust_R.GetComponent<ParticleSystem>().Play();
        EffDust_L.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    void RpcEffDustStop_Control()
    {
        EffDust_R.GetComponent<ParticleSystem>().Stop();
        EffDust_L.GetComponent<ParticleSystem>().Stop();
    }

    [PunRPC]
    void BlazeLuminousExpand()
    {
        BlazeLuminous.SetActive(true);
    }

    [PunRPC]
    void BlazeLuminousClose()
    {
        BlazeLuminous.SetActive(false);
    }

    [PunRPC]
    void DurabilitySync(int _durable_value)
    {
        durable_value = _durable_value;
    }

    ////////////////////////////////////////////////////////////

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
                    jump_flag = false;
                    if (!boost_flag)
                    {
                        landing_flag = true;
                        boost_amount = boost_maxamount;
                        incapableofaction_flag = false;
                    }
                    else if (type_groundrunnig)
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
                    air_flag = false;

                    if (down_flag)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }

                    if (!type_groundrunnig && boost_flag)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                    }
                }
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {

        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    if (!step_flag && !jump_flag && !boost_flag && !slide_flag && !down_flag && !stagger_flag)
                    {
                        incapableofaction_flag = false;
                    }
                }
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (other.gameObject.CompareTag("Ground"))
                {
                    anim.SetBool("Air", true);
                    air_flag = true;
                }
            }
        }
    }

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

    private void OnTriggerStay(Collider other)
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (!hitstart_stay_flag)
                {
                    Hit_Control(other);
                }
            }
        }
    }

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
                            hitstart_stay_flag = false;
                        }
                    }
                }
            }
        }
    }

    void Hit_Control(Collider other)
    {
        if (other.gameObject.GetComponent<Beam_Control>() != null)
        {
            if (other.gameObject.GetComponent<Beam_Control>().OwnMachine != gameObject)
            {
                if (!down_flag)
                {
                    if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Untagged"))
                    {
                        other_forward = other.transform.position;
                        //ガード成功
                        if (defense_flag && Vector3.Angle(transform.forward, other.gameObject.transform.forward) >= 90)
                        {
                            defenseing_flag = true;
                            defenseing_time = 0;
                            Transform other_transform = other.transform;
                            other_transform.position = other_transform.forward * -2;
                            other_transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z);
                            transform.LookAt(other.transform);
                            defenserecoil = transform.forward * -30f;
                        }
                        else
                        {
                            hitstart_stay_flag = true;
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
                            durable_value -= Mathf.CeilToInt(other.gameObject.GetComponent<Beam_Control>().power * correctionfactor);
                            correctionfactor -= other.gameObject.GetComponent<Beam_Control>().correctionfactor;
                            correctionfactor_resettime = 0f;
                            if (correctionfactor < 0.1f)
                            {
                                correctionfactor = 0.1f;
                            }
                            down_value += other.gameObject.GetComponent<Beam_Control>().down_value;
                            if (down_value >= 6)
                            {
                                down_flag = true;
                                incapableofaction_flag = true;
                                stiffness_flag = true;
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
                                stagger_time = 0;
                                correctionfactor_resettime = 0;
                                down_value = 0;
                                if (air_flag)
                                {
                                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 180);
                                }
                            }
                            else
                            {
                                stagger_flag = true;
                                incapableofaction_flag = true;
                                stiffness_flag = true;
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
                                stagger_time = 0;
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
