using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class Cpu_Control : MonoBehaviour
{
    Rigidbody rb;
    public float boost_amount;
    const float WALK_SPEED = 15;
    const float BOOST_SPEED = 60;
    const float JUMP_POWER = 50;
    float jumpRugTime = 0;
    float landingTime = 0;
    float boostButtonTime = 0;
    float boostConsumedTime = 0;
    float slideTime = 0;
    float mainShootingTime = 0;
    float mainShootingReloadTime = 0;
    float subShootingTime = 0;
    float subShootingReloadTime = 0;
    float subShootingFightingVariantsTime = 0;
    float specialShootingTime = 0;
    float specialShootingReloadTime = 0;
    float attackTime = 0;
    float attackFinishTime = 1.8f;
    float specialAttackTime = 0;
    float staggerTime = 0;
    float correctionFactor = 1;
    float correctionFactorResetTime = 0;
    float downValue = 0;
    float nextcpumove_time = 0;
    float x = 0;
    float z = 0;
    bool isJump = false;
    bool isRise = false;
    bool isLeverInsert = false;
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
    bool isInduction = true;
    public bool isInductionOff = false;
    bool isStagger = false;
    public bool isDown = false;
    bool isUnderAttack = false;
    bool isDefense = false;
    bool isStep = false;
    bool isHitStartStay = false;
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
    int BOOST_MAX = 100;
    public int mainshooting_number;
    bool isSpecialAttack = false;
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
        boost_amount = BOOST_MAX;
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
        LandingTime();
        Stagger_Control();
    }

    void FixedUpdate()
    {
        if (!isStiffness)
        {
            if (!isIncapableAction)
            {
                if (!isLanding && !isSlide)
                {
                    if (isLeverInsert && !isDefense)
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
                else if (isSlide)
                {
                    rb.velocity = transform.forward * (WALK_SPEED);
                }
            }
        }
        else
        {
            if (isDown || isSpecialAttack || isSubShootingFightingVariants)
            {
            }
            else if (isAttack || isStep || isStagger)
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
            isLeverInsert = true;
            anim.SetBool("Walk", true);
        }
        else
        {
            isLeverInsert = false;
            anim.SetBool("Walk", false);
        }
        movingdirection.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
        if (!isBoost && !isSlide)
        {
            if (!isAir)
            {
                movingvelocity = movingdirection * WALK_SPEED;
            }
        }
    }

    void KMF_Rotation()
    {
        if (isLeverInsert)
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
        if (!isIncapableAction)
        {
            //�W�����v
            if (cpu_jump && !isJump)
            {
                BoostFinish();
                isJump = true;
                jumpRugTime = 0;
                isBoost = false;
                boostButtonTime = 0;
            }
            //�u�[�X�g
            else if (cpu_boost && isJump)
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
                    boost_amount -= 15;
                    boostConsumedTime = 0;
                    gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
                }
            }

            //�W�����v����
            if (isJump && !isBoost)
            {
                if (!isStiffness)
                {
                    jumpRugTime += Time.deltaTime;
                    if (jumpRugTime >= 0.2f && !isRise)
                    {
                        if (!isLanding)
                        {
                            inertia_direction = gameObject.transform.forward;
                            lastmove_name = "jump";
                            anim.SetBool("Jump", true);
                            anim.SetBool("Boost_Landing", false);
                            boost_amount -= 10;
                            isRise = true;
                        }
                    }
                    else if (jumpRugTime <= 1.0f && jumpRugTime >= 0.2f)
                    {
                        if (!isLanding)
                        {
                            Vector3 jump_direction;
                            Vector3 jump_moving;

                            jump_direction = gameObject.transform.up * 1;
                            jump_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                            jump_moving = jump_direction * JUMP_POWER;
                            rb.velocity = new Vector3(rb.velocity.x, jump_moving.y, rb.velocity.z);
                        }
                        isRise = false;
                        isJump = false;
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

            //�u�[�X�g��
            if (isBoost)
            {
                if (movingdirection != new Vector3(0, 0, 0) && isLeverInsert)
                {
                    movingvelocity = movingdirection * BOOST_SPEED;
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
        }

        //�u�[�X�g�I��
        if ((!isLeverInsert && !cpu_boost) || isIncapableAction)
        {
            if (isBoost)
            {
                BoostFinish();
                if (!isAir)
                {
                    isSlide = true;
                    isUnderAttack = true;
                }
                else if (isAir)
                {
                    inertia_direction = gameObject.transform.forward;
                }
            }
        }

        //�Y�T����
        if (isSlide)
        {
            anim.SetBool("Boost_Landing_Finish", true);
            slideTime += Time.deltaTime;
            if (slideTime > 1.5f)
            {
                SlideFinish();
            }
        }
    }

    //�u�[�X�g�I��
    void BoostFinish()
    {
        isBoost = false;
        lastmove_name = "boost";
        anim.SetBool("Boost_Landing", false);
    }

    //�Y�T�I��
    void SlideFinish()
    {
        anim.SetBool("Boost_Landing_Finish", false);
        isSlide = false;
        slideTime = 0;
        if (lastmove_name != "step")
            boost_amount = BOOST_MAX;
        isIncapableAction = false;
        isUnderAttack = false;
    }

    //���n����
    void LandingTime()
    {
        if (isLanding && !isUnderAttack)
        {
            anim.SetBool("Landing", true);
            landingTime += Time.deltaTime;
            if (landingTime >= 0.5f)
            {
                isLanding = false;
                landingTime = 0;
                anim.SetBool("Landing", false);
                isUnderAttack = false;
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
        if (!isIncapableAction && (!isUnderAttack || lastmove_name == "subshooting"))
        {
            if (cpu_shooting && !isMainShooting)
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
                    Vector3 myforward = transform.forward * 1.1f;
                    Vector3 myback = transform.forward * -1.1f;
                    if (LockOnEnemy != null)
                    {
                        //�U���������
                        if (Mathf.Abs(Vector3.Distance(LockOnEnemy.transform.position, myforward))
                            > Mathf.Abs(Vector3.Distance(LockOnEnemy.transform.position, myback)))
                        {
                            isStiffness = true;
                            gameObject.transform.LookAt(LockOnEnemy.transform);
                        }
                    }
                }
            }
        }

        //���C���ˌ���
        if (isMainShooting)
        {
            mainShootingTime += Time.deltaTime;
            if (mainShootingTime >= 0.23f && !isMainShootingFiring)
            {
                mainshooting_number--;
                isMainShootingFiring = true;
                GameObject _beam = Instantiate(Beam, Varis_Normal.transform.Find("Muzzle").gameObject.transform.position, Varis_Normal.transform.Find("Muzzle").gameObject.transform.rotation);
                _beam.GetComponent<Beam_Control>().LockOnEnemySetting(gameObject, LockOnEnemy);
            }
            if (mainShootingTime >= 1.0f)
            {
                MainShootingFinish();
                isUnderAttack = false;
            }

            if (LockOnEnemy == null)
            {
                gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
            }
        }

        //�����[�h
        if (mainshooting_maxnumber > mainshooting_number)
        {
            mainShootingReloadTime += Time.deltaTime;
            if (mainShootingReloadTime >= 3f)
            {
                mainshooting_number++;
                mainShootingReloadTime = 0;
            }
        }
    }

    void MainShootingFinish()
    {
        isMainShooting = false;
        isStiffness = false;
        anim.SetBool("MainShooting", false);
        gameObject.transform.Find("Lancelot/Rig 1").GetComponent<Rig>().weight = 0;
    }

    //�T�u�ˌ�����
    void SubShootingControls()
    {
        if (!isIncapableAction && !isUnderAttack)
        {
            if (cpu_subshooting && !isSubShooting)
            {
                if (subshooting_number >= 1 && !isLanding)
                {
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
                }
            }
        }

        if (isSubShooting)
        {
            subShootingTime += Time.deltaTime;
            if (subShootingTime >= 0.3f && !isSubShootingFiring)
            {
                subshooting_number--;
                isSubShootingFiring = true;
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
            if (subShootingTime >= 0.8f && subShootingTime < 1.5f)
            {
                anim.SetBool("SubShooting_Start", false);
                anim.SetBool("SubShooting_Finish", true);
            }
            if (subShootingTime >= 1.5f)
            {
                SubShootingFinish();
                isUnderAttack = false;
            }
        }

        //�����[�h
        if (subshooting_maxnumber > subshooting_number)
        {
            subShootingReloadTime += Time.deltaTime;
            if (subShootingReloadTime >= 2f)
            {
                subshooting_number++;
                subShootingReloadTime = 0;
            }
        }
    }

    void SubShootingFinish()
    {
        anim.SetBool("SubShooting_Finish", false);
        isSubShooting = false;
        isStiffness = false;
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        Destroy(SlashHarken_Instance);
    }

    //����ˌ��{�^������
    void SpecialShootingControls()
    {
        if (!isIncapableAction
            && (!isUnderAttack || lastmove_name == "mainshooting" || lastmove_name == "specialattack"))
        {
            if (cpu_specialshooting && !isSpecialShooting)
            {
                if (specialshooting_number >= 1 && !isLanding)
                {
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
                }
            }
            else if (isSpecialShooting && isSpecialShootingAnimation)
            {
                anim.SetBool("SpecialShooting_Start", false);
                isSpecialShootingAnimation = false;
            }
        }

        //����ˌ���
        if (isSpecialShooting)
        {
            specialShootingTime += Time.deltaTime;
            if (specialShootingTime >= 0.6f && !isSpecialShootingFiring)
            {
                specialshooting_number--;
                isSpecialShootingFiring = true;
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
            if (specialShootingTime >= 1.5f)
            {
                SpecialShootingFinish();
            }
        }

        //�����[�h
        if (specialshooting_number == 0)
        {
            specialShootingReloadTime += Time.deltaTime;
            if (specialShootingReloadTime >= 5f)
            {
                specialshooting_number = specialshooting_maxnumber;
                specialShootingReloadTime = 0;
            }
        }
    }

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

    //�i���{�^������
    void AttackControls()
    {
        Vector3 attack_direction;

        if (!isIncapableAction)
        {
            if (cpu_fighting)
            {
                if (!isLanding || isUnderAttack)
                {
                    if (!isAttack && !isAttack1 && (!isUnderAttack || lastmove_name == "specialattack"))
                    {
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

                    //�T�u�ˌ��i���h��
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
                        Leg_R.transform.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }

        //�i������
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
                    attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                    attack_moving = attack_direction * BOOST_SPEED * 1f;
                    rb.velocity = new Vector3(attack_moving.x, rb.velocity.y, attack_moving.z);
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
                attack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                attack_moving = attack_direction * 10f;
                rb.velocity = new Vector3(attack_moving.x, rb.velocity.y, attack_moving.z);
            }
            if (attackTime >= attackFinishTime)
            {
                AttackFinish();
                transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            }
        }

        //�T�u�ˌ��i���h������
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

    //�i���I��
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
        gameObject.transform.eulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

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

    //����i���{�^������
    void SpecialAttackControls()
    {
        Vector3 specialattack_direction;
        if (!isIncapableAction && !isUnderAttack)
        {
            if (cpu_specialfighting && !isLanding)
            {
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
                specialattack_direction.Normalize();//�΂߂̋����������Ȃ�̂�h���܂�
                specialattack_moving = specialattack_direction * BOOST_SPEED * 1.2f;
            }
        }

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

    //����i���I��
    void SpecialAttackFinish()
    {
        anim.SetBool("SpecialAttack", false);
        isSpecialAttack = false;
        isStiffness = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    //��������
    void InertiaControl()
    {
        if (isAir && !isBoost)
        {
            if (lastmove_name == "jump" && !isLeverInsert)
            {
                rb.velocity = new Vector3(inertia_direction.x * 20, rb.velocity.y, inertia_direction.y * 20);
            }
            if (lastmove_name == "boost")
            {
                rb.velocity = new Vector3(inertia_direction.x * BOOST_SPEED, rb.velocity.y, inertia_direction.y * BOOST_SPEED);
            }
            if (lastmove_name == "specialattack" && !isSpecialAttack)
            {
                rb.velocity = new Vector3(inertia_direction.x * BOOST_SPEED * 2, rb.velocity.y, inertia_direction.y * BOOST_SPEED * 1.2f);
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
        if (isStagger || isDown)
        {
            if (!isDown || !isAir)
            {
                staggerTime += Time.deltaTime;
            }
            if (staggerTime >= 0.2f)
            {
                anim.SetBool("Stagger_Start", false);
            }
            if (staggerTime <= 0.2f)
            {
                Vector3 stagger_move;
                if (isDown)
                {
                    stagger_move = other_forward * 0.5f;
                }
                else
                {
                    stagger_move = other_forward * 0.25f;
                }
                rb.velocity = new Vector3(stagger_move.x, rb.velocity.y, stagger_move.z);
            }
            if (staggerTime >= 1.0f && !isDown)
            {
                isIncapableAction = false;
                isStagger = false;
                isStiffness = false;
                anim.SetBool("StaggerFront_Landing", false);
                anim.SetBool("StaggerBack_Landing", false);
                staggerTime = 0;
            }
            else if (staggerTime >= 1f && staggerTime < 2f && isDown)
            {
                anim.SetBool("GetUp", true);
                anim.SetBool("DownFront", false);
                anim.SetBool("DownBack", false);
            }
            else if (staggerTime >= 2f && isDown)
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
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
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
            isAir = false;
            inertia_direction = new Vector3(0, 0, 0);

            if (isDown)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (isSubShootingFightingVariants)
            {
                SubShootingFightingVariantsFinish();
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isAttack)
            {
                if (attackTime < 0.8f && (!isAttack1 && !isAttack2 && !isAttack3))
                {
                    attackTime = 0;
                    attackFinishTime = 1.0f;
                    isAttack1 = true;
                    anim.SetBool("Attack_Induction", false);
                    anim.SetBool("Attack1", true);
                    Rigidity();
                }
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            if (!isStep && !isJump && !isBoost && !isSlide && !isDown && !isStagger)
            {
                isIncapableAction = false;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Air", true);
            isAir = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit_Control(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isHitStartStay)
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
                    isHitStartStay = false;
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
                if (!isDown)
                {
                    if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Untagged"))
                    {
                        other_forward = other.transform.position;
                        //�K�[�h����
                        if (isDefense && Vector3.Angle(transform.forward, other.gameObject.transform.forward) >= 90)
                        {
                            Transform other_transform = other.transform;
                            other_transform.position = other_transform.forward * -2;
                            other_transform.position = new Vector3(other.transform.position.x, other.transform.position.y + 5f, other.transform.position.z);
                            transform.LookAt(other.transform);
                        }
                        else
                        {
                            isHitStartStay = true;
                            BoostFinish();
                            MainShootingFinish();
                            SubShootingFinish();
                            SubShootingFightingVariantsFinish();
                            SpecialShootingFinish();
                            AttackFinish();
                            SpecialAttackFinish();
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
                    other.GetComponent<Beam_Control>().ThisGameObjectDestroy();
                }
            }
        }
    }
}
