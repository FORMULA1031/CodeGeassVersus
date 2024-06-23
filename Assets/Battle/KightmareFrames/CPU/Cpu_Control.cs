using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class Cpu_Control : MonoBehaviour
{
    Rigidbody rb;
    public float boost_amount;
    float walk_speed = 15;
    float boost_speed = 60;
    float jump_power = 50;
    float jumprug_time = 0;
    float landing_time = 0;
    float BoostButtonTime = 0;
    float boostconsumed_time = 0;
    float slide_time = 0;
    float mainshooting_time = 0;
    float mainshooting_reloadtime = 0;
    float subshooting_time = 0;
    float subshooting_reloadtime = 0;
    float subShooting_fightingvariants_time = 0;
    float specialshooting_time = 0;
    float specialshooting_reloadtime = 0;
    float attack_time = 0;
    float attackfinish_time = 1.8f;
    float specialattack_time = 0;
    float stagger_time = 0;
    float correctionfactor = 1;
    float correctionfactor_resettime = 0;
    float down_value = 0;
    float nextcpumove_time = 0;
    float x = 0;
    float z = 0;
    bool jump_flag = false;
    bool rise_flag = false;
    bool leverinsert_flag = false;
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
    bool induction_flag = true;
    public bool inductionoff_flag = false;
    bool stagger_flag = false;
    public bool down_flag = false;
    bool underattack_flag = false;
    bool defense_flag = false;
    bool step_flag = false;
    bool hitstart_stay_flag = false;
    bool cpu_jump = false;
    bool cpu_boost = false;
    bool cpu_shooting = false;
    bool cpu_fighting = false;
    bool cpu_subshooting = false;
    bool cpu_specialshooting = false;
    bool cpu_specialfighting = false;
    bool cpufighting_flag = false;
    bool cpumoving_flag = false;
    public int durable_value;
    public int durable_maxvalue;
    int boost_maxamount = 100;
    public int mainshooting_number;
    bool specialattack_flag = false;
    int mainshooting_maxnumber;
    public int subshooting_number;
    int subshooting_maxnumber;
    public int specialshooting_number;
    int specialshooting_maxnumber;
    string lastmove_name;
    Vector3 movingdirection;
    Vector3 movingvelocity;
    Vector3 attack_moving;
    Vector3 specialattack_moving;
    Vector3 inertia_direction;
    Vector3 other_forward;
    Animator anim;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        mainshooting_maxnumber = mainshooting_number;
        subshooting_maxnumber = subshooting_number;
        specialshooting_maxnumber = specialshooting_number;
        durable_value = durable_maxvalue;
        boost_amount = boost_maxamount;
    }

    // Update is called once per frame
    void Update()
    {
        if (LockOnEnemy == null)
        {
            LockOnEnemy = transform.GetComponent<PlayerID_Control>().LockOnEnemy;
            if (LockOnEnemy != null)
            {
                Rig_Setting("Lancelot/Rig 1/Waite_Rig", LockOnEnemy);
                Rig_Setting("Lancelot/Rig 1/Head_Rig", LockOnEnemy);
                Rig_Setting("Lancelot/Rig 1/Arm_Rig", LockOnEnemy);
                transform.GetComponent<RigBuilder>().Build();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cpumoving_flag = !cpumoving_flag;
            cpufighting_flag = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            cpufighting_flag = !cpufighting_flag;
            cpumoving_flag = false;
        }
        if (cpufighting_flag || cpumoving_flag)
        {
            nextcpumove_time += Time.deltaTime;
        }
        else
        {
            x = 0;
            z = 0;
            cpu_jump = false;
            cpu_boost = false;
            cpu_shooting = false;
            cpu_subshooting = false;
            cpu_specialshooting = false;
            cpu_fighting = false;
            cpu_specialfighting = false;
        }
        if(nextcpumove_time >= 1)
        {
            nextcpumove_time = 0;
            x = Random.Range(-1, 2);
            z = Random.Range(-1, 2);
            cpu_boost = Random_CpuControl(8, 10);
            if (cpu_boost)
            {
                cpu_jump = false;
            }
            else
            {
                cpu_jump = true;
            }
            if (!cpumoving_flag || cpufighting_flag)
            {
                cpu_shooting = Random_CpuControl(1, 2);
                cpu_fighting = Random_CpuControl(1, 5);
                if (cpu_fighting && cpu_jump)
                {
                    cpu_specialfighting = true;
                    cpu_specialshooting = false;
                    cpu_subshooting = false;
                    cpu_fighting = false;
                    cpu_jump = false;
                }
                else if (cpu_shooting && cpu_jump)
                {
                    cpu_specialshooting = true;
                    cpu_specialfighting = false;
                    cpu_subshooting = false;
                    cpu_shooting = false;
                    cpu_jump = false;
                }
                else if (cpu_shooting && cpu_fighting)
                {
                    cpu_subshooting = true;
                    cpu_specialshooting = false;
                    cpu_specialfighting = false;
                    cpu_shooting = false;
                    cpu_fighting = false;
                }
                else
                {
                    cpu_subshooting = false;
                    cpu_specialshooting = false;
                    cpu_specialfighting = false;
                }
            }
        }

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
        LandingTime();
        Stagger_Control();
    }

    void FixedUpdate()
    {
        if (!stiffness_flag)
        {
            if (!incapableofaction_flag)
            {
                if (!landing_flag && !slide_flag)
                {
                    if (leverinsert_flag && !defense_flag)
                    {
                        rb.velocity = new Vector3(movingvelocity.x, rb.velocity.y, movingvelocity.z);
                        KMF_Rotation();
                        InertiaControl();
                    }
                    else if (lastmove_name == "step")
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    }
                }
                else if (slide_flag)
                {
                    rb.velocity = transform.forward * (walk_speed);
                }
            }
        }
        else
        {
            if (down_flag || specialattack_flag || subshooting_fightingvariants_flag)
            {
            }
            else if (attack_flag || step_flag || stagger_flag)
            {
                GravityOff();
            }
            else
            {
                Rigidity();
            }
        }
    }

    bool Random_CpuControl(int molecule, int denominator)
    {
        if(Random.Range(1, denominator + 1) > molecule)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //���o�[����
    void MoveKeyControls()
    {
        movingdirection =  new Vector3(x, rb.velocity.y, z);
        if (x != 0 || z != 0)
        {
            leverinsert_flag = true;
            anim.SetBool("Walk", true);
        }
        else
        {
            leverinsert_flag = false;
            anim.SetBool("Walk", false);
        }
        movingdirection.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
        if (!boost_flag && !slide_flag)
        {
            if (!air_flag)
            {
                movingvelocity = movingdirection * walk_speed;
            }
        }
    }

    void KMF_Rotation()
    {
        if (leverinsert_flag)
        {
            Vector3 direction = new Vector3 (0, 0); //������ύX����
            if (x > 0)
            {
                direction = new Vector3(0, 90, 0);
                if(z > 0)
                {
                    direction = new Vector3(0, 45, 0);
                }
                else if(z < 0)
                {
                    direction = new Vector3(0, 135, 0);
                }
            }
            else if (x < 0)
            {
                direction = new Vector3(0, -90, 0);
                if (z > 0)
                {
                    direction = new Vector3(0, -45, 0);
                }
                else if (z < 0)
                {
                    direction = new Vector3(0, -135, 0);
                }
            }
            else if (z > 0)
            {
                direction = new Vector3(0, 0, 0);
            }
            else if (z < 0)
            {
                direction = new Vector3(0, 180, 0);
            }
            transform.eulerAngles = direction;
        }
    }

    void JumpKeyControls()
    {
        if (!incapableofaction_flag)
        {
            //�W�����v
            if (cpu_jump && !jump_flag)
            {
                BoostFinish();
                jump_flag = true;
                jumprug_time = 0;
                boost_flag = false;
                BoostButtonTime = 0;
            }
            //�u�[�X�g
            else if (cpu_boost && jump_flag)
            {
                BoostButtonTime += Time.deltaTime;
                if (BoostButtonTime < 0.2f)
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
                    boost_amount -= 15;
                    boostconsumed_time = 0;
                    gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
                }
            }

            //�W�����v����
            if (jump_flag && !boost_flag)
            {
                if (!stiffness_flag)
                {
                    jumprug_time += Time.deltaTime;
                    if (jumprug_time >= 0.2f && !rise_flag)
                    {
                        if (!landing_flag)
                        {
                            inertia_direction = gameObject.transform.forward;
                            lastmove_name = "jump";
                            anim.SetBool("Jump", true);
                            anim.SetBool("Boost_Landing", false);
                            boost_amount -= 10;
                            rise_flag = true;
                        }
                    }
                    else if (jumprug_time <= 1.0f && jumprug_time >= 0.2f)
                    {
                        if (!landing_flag)
                        {
                            Vector3 jump_direction;
                            Vector3 jump_moving;

                            jump_direction = gameObject.transform.up * 1;
                            jump_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                            jump_moving = jump_direction * jump_power;
                            rb.velocity = new Vector3(rb.velocity.x, jump_moving.y, rb.velocity.z);
                        }
                        rise_flag = false;
                        jump_flag = false;
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

            //�u�[�X�g��
            if (boost_flag)
            {
                if (movingdirection != new Vector3(0, 0, 0) && leverinsert_flag)
                {
                    movingvelocity = movingdirection * boost_speed;
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
        }

        //�u�[�X�g�I��
        if ((!leverinsert_flag && !cpu_boost) || incapableofaction_flag)
        {
            if (boost_flag)
            {
                BoostFinish();
                if (!air_flag)
                {
                    slide_flag = true;
                    underattack_flag = true;
                }
                else if (air_flag)
                {
                    inertia_direction = gameObject.transform.forward;
                }
            }
        }

        //�Y�T����
        if (slide_flag)
        {
            anim.SetBool("Boost_Landing_Finish", true);
            slide_time += Time.deltaTime;
            if (slide_time > 1.5f)
            {
                SlideFinish();
            }
        }
    }

    //�u�[�X�g�I��
    void BoostFinish()
    {
        boost_flag = false;
        lastmove_name = "boost";
        anim.SetBool("Boost_Landing", false);
    }

    //�Y�T�I��
    void SlideFinish()
    {
        anim.SetBool("Boost_Landing_Finish", false);
        slide_flag = false;
        slide_time = 0;
        if (lastmove_name != "step")
            boost_amount = boost_maxamount;
        incapableofaction_flag = false;
        underattack_flag = false;
    }

    //���n����
    void LandingTime()
    {
        if (landing_flag && !underattack_flag)
        {
            anim.SetBool("Landing", true);
            landing_time += Time.deltaTime;
            if (landing_time >= 0.5f)
            {
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

    //�ˌ��{�^������
    void ShootingKeyControls()
    {
        if (!incapableofaction_flag && (!underattack_flag || lastmove_name == "subshooting"))
        {
            if (cpu_shooting && !mainshooting_flag)
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
                    Vector3 myforward = transform.forward * 1.1f;
                    Vector3 myback = transform.forward * -1.1f;
                    if (LockOnEnemy != null)
                    {
                        //�U���������
                        if (Mathf.Abs(Vector3.Distance(LockOnEnemy.transform.position, myforward))
                            > Mathf.Abs(Vector3.Distance(LockOnEnemy.transform.position, myback)))
                        {
                            stiffness_flag = true;
                            gameObject.transform.LookAt(LockOnEnemy.transform);
                        }
                    }
                }
            }
        }

        //���C���ˌ���
        if (mainshooting_flag)
        {
            mainshooting_time += Time.deltaTime;
            if (mainshooting_time >= 0.23f && !mainshootingfiring_flag)
            {
                mainshooting_number--;
                mainshootingfiring_flag = true;
                GameObject _beam = Instantiate(Beam, Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
            }
            if (mainshooting_time >= 1.0f)
            {
                MainShootingFinish();
                underattack_flag = false;
            }

            if (LockOnEnemy == null)
            {
                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
            }
        }

        //�����[�h
        if (mainshooting_maxnumber > mainshooting_number)
        {
            mainshooting_reloadtime += Time.deltaTime;
            if (mainshooting_reloadtime >= 3f)
            {
                mainshooting_number++;
                mainshooting_reloadtime = 0;
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

    //�T�u�ˌ�����
    void SubShootingControls()
    {
        if (!incapableofaction_flag && !underattack_flag)
        {
            if (cpu_subshooting && !subshooting_flag)
            {
                if (subshooting_number >= 1 && !landing_flag)
                {
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
                }
            }
        }

        if (subshooting_flag)
        {
            subshooting_time += Time.deltaTime;
            if (subshooting_time >= 0.3f && !subshootingfiring_flag)
            {
                subshooting_number--;
                subshootingfiring_flag = true;
                SlashHarken_Instance = Instantiate(SlashHarken, Lancelot_ShashHarken.transform.position, Lancelot_ShashHarken.transform.rotation);
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
            if (subshooting_time >= 0.8f && subshooting_time < 1.5f)
            {
                anim.SetBool("SubShooting_Start", false);
                anim.SetBool("SubShooting_Finish", true);
            }
            if (subshooting_time >= 1.5f)
            {
                SubShootingFinish();
                underattack_flag = false;
            }
        }

        //�����[�h
        if (subshooting_maxnumber > subshooting_number)
        {
            subshooting_reloadtime += Time.deltaTime;
            if (subshooting_reloadtime >= 2f)
            {
                subshooting_number++;
                subshooting_reloadtime = 0;
            }
        }
    }

    void SubShootingFinish()
    {
        anim.SetBool("SubShooting_Finish", false);
        subshooting_flag = false;
        stiffness_flag = false;
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        Destroy(SlashHarken_Instance);
    }

    //����ˌ��{�^������
    void SpecialShootingControls()
    {
        if (!incapableofaction_flag
            && (!underattack_flag || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
        {
            if (cpu_specialshooting && !specialshooting_flag)
            {
                if (specialshooting_number >= 1 && !landing_flag)
                {
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
                }
            }
            else if (specialshooting_flag && specialshootinganimation_flag)
            {
                anim.SetBool("SpecialShooting_Start", false);
                specialshootinganimation_flag = false;
            }
        }

        //����ˌ���
        if (specialshooting_flag)
        {
            specialshooting_time += Time.deltaTime;
            if (specialshooting_time >= 0.6f && !specialshootingfiring_flag)
            {
                specialshooting_number--;
                specialshootingfiring_flag = true;
                GameObject _Beam_FullPower = Instantiate(Beam_FullPower, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.position, Varis_FullPower.transform.Find("Muzzle").gameObject.transform.rotation);
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

        //�����[�h
        if (specialshooting_number == 0)
        {
            specialshooting_reloadtime += Time.deltaTime;
            if (specialshooting_reloadtime >= 5f)
            {
                specialshooting_number = specialshooting_maxnumber;
                specialshooting_reloadtime = 0;
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

    //�i���{�^������
    void AttackControls()
    {
        Vector3 attack_direction;

        if (!incapableofaction_flag)
        {
            if (cpu_fighting)
            {
                if (!landing_flag || underattack_flag)
                {
                    if (!attack_flag && !attack1_flag && (!underattack_flag || lastmove_name == "specialattack"))
                    {
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

                    //�T�u�ˌ��i���h��
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

                        boost_flag = false;
                        anim.SetBool("Boost_Landing", false);

                        if (induction_flag && LockOnEnemy != null)
                        {
                            gameObject.transform.LookAt(LockOnEnemy.transform);
                        }
                        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                        lastmove_name = "subshooting_fightingvariants";
                        attack_direction = gameObject.transform.forward * 1;
                        attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                        attack_moving = attack_direction * boost_speed;
                        Leg_R.transform.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }

        //�i������
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
                    attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                    attack_moving = attack_direction * boost_speed * 1f;
                    rb.velocity = new Vector3(attack_moving.x, rb.velocity.y, attack_moving.z);
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
                attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                attack_moving = attack_direction * 10f;
                rb.velocity = new Vector3(attack_moving.x, rb.velocity.y, attack_moving.z);
            }
            if (attack_time >= attackfinish_time)
            {
                AttackFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            }
        }

        //�T�u�ˌ��i���h������
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
                attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
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

    //�i���I��
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

    //����i���{�^������
    void SpecialAttackControls()
    {
        Vector3 specialattack_direction;
        if (!incapableofaction_flag && !underattack_flag)
        {
            if (cpu_specialfighting && !landing_flag)
            {
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
                specialattack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                specialattack_moving = specialattack_direction * boost_speed * 1.2f;
            }
        }

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

    //����i���I��
    void SpecialAttackFinish()
    {
        anim.SetBool("SpecialAttack", false);
        specialattack_flag = false;
        stiffness_flag = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //��������
    void InertiaControl()
    {
        if (air_flag && !boost_flag)
        {
            if (lastmove_name == "jump" && !leverinsert_flag)
            {
                rb.velocity = new Vector3(inertia_direction.x * 20, rb.velocity.y, inertia_direction.y * 20);
            }
            if (lastmove_name == "boost")
            {
                rb.velocity = new Vector3(inertia_direction.x * boost_speed, rb.velocity.y, inertia_direction.y * boost_speed);
            }
            if (lastmove_name == "specialattack" && !specialattack_flag)
            {
                rb.velocity = new Vector3(inertia_direction.x * boost_speed * 2, rb.velocity.y, inertia_direction.y * boost_speed * 1.2f);
            }
        }
    }

    //�d�̓I�t
    void GravityOff()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    //���~��
    void Rigidity()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }

    //��낯
    void Stagger_Control()
    {
        if (stagger_flag || down_flag)
        {
            if (!down_flag || !air_flag)
            {
                stagger_time += Time.deltaTime;
            }
            if (stagger_time >= 0.2f)
            {
                anim.SetBool("Stagger_Start", false);
            }
            if (stagger_time <= 0.2f)
            {
                Vector3 stagger_move;
                if (down_flag)
                {
                    stagger_move = other_forward * 0.5f;
                }
                else
                {
                    stagger_move = other_forward * 0.25f;
                }
                rb.velocity = new Vector3(stagger_move.x, rb.velocity.y, stagger_move.z);
            }
            if (stagger_time >= 1.0f && !down_flag)
            {
                incapableofaction_flag = false;
                stagger_flag = false;
                stiffness_flag = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                stagger_time = 0;
            }
            else if (stagger_time >= 1f && stagger_time < 2f && down_flag)
            {
                anim.SetBool("GetUp", true);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
            }
            else if (stagger_time >= 2f && down_flag)
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
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
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
            air_flag = false;
            inertia_direction = new Vector3(0, 0, 0);

            if (down_flag)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (subshooting_fightingvariants_flag)
            {
                SubShootingFightingVariantsFinish();
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (attack_flag)
            {
                if (attack_time < 0.8f && (!attack1_flag && !attack2_flag && !attack3_flag))
                {
                    attack_time = 0;
                    attackfinish_time = 1.0f;
                    attack1_flag = true;
                    anim.SetBool("Attack_Induction", false);
                    anim.SetBool("Attack1", true);
                    Rigidity();
                }
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            if (!step_flag && !jump_flag && !boost_flag && !slide_flag && !down_flag && !stagger_flag)
            {
                incapableofaction_flag = false;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Air", true);
            air_flag = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit_Control(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hitstart_stay_flag)
        {
            Hit_Control(other);
        }
    }

    private void OnTriggerExit(Collider other)
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
                        //�K�[�h����
                        if (defense_flag && Vector3.Angle(transform.forward, other.gameObject.transform.forward) >= 90)
                        {
                            Transform other_transform = other.transform;
                            other_transform.position = other_transform.forward * -2;
                            other_transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z);
                            transform.LookAt(other.transform);
                        }
                        else
                        {
                            hitstart_stay_flag = true;
                            BoostFinish();
                            MainShootingFinish();
                            SubShootingFinish();
                            SubShootingFightingVariantsFinish();
                            SpecialShootingFinish();
                            AttackFinish();
                            SpecialAttackFinish();
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
                    other.GetComponent<Beam_Control>().ThisGameObjectDestroy();
                }
            }
        }
    }
}
