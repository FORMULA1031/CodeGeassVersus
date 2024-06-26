using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Beam_Control : MonoBehaviour
{
    Rigidbody rb;

    public int power;
    public float correctionFactor;
    public float downValue;
    public float speed;
    public float induction_x;
    public float induction_y;
    public float induction_z;
    float Induction_time = 0;
    float Induction_cooltime = 0.1f;
    public GameObject OwnMachine;
    GameObject LockOnEnemy;
    public bool changedirection_flag;
    public bool fighting_flag;
    bool direction_flag = false;
    bool isInduction = true;
    Vector3 direction;
    Vector3 moving;
    float OwnMachinAndEnemy_distance;
    float OwnMachinAndBeam_distance;
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv != null)
        {
            //���@�����������I�u�W�F�N�g�̏ꍇ
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                rb = GetComponent<Rigidbody>();
            }
            else
            {
                Destroy(rb);
            }
        }
    }

    private void Update()
    {
        //���@�����������I�u�W�F�N�g�̏ꍇ
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //���u�U���̏ꍇ
                if (!fighting_flag && LockOnEnemy != null)
                {
                    //��x��������̕����֌���
                    if (direction_flag)
                    {
                        gameObject.transform.LookAt(LockOnEnemy.transform);
                        direction_flag = false;
                        moving = transform.forward * speed;
                    }

                    //�U���؂肳�ꂽ���m�F(�΃v���C���[)
                    if (LockOnEnemy.GetComponent<KMF_Control>())
                    {
                        if (LockOnEnemy.GetComponent<KMF_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    //�U���؂肳�ꂽ���m�F(��NPC)
                    else if (LockOnEnemy.GetComponent<Cpu_Control>())
                    {
                        if (LockOnEnemy.GetComponent<Cpu_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    Induction_time += Time.deltaTime;
                    //��莞�Ԃ��ƂɗU������
                    if (Induction_time >= Induction_cooltime && LockOnEnemy != null && isInduction)
                    {
                        direction = transform.eulerAngles;
                        transform.LookAt(LockOnEnemy.transform);
                        moving = transform.forward * speed;
                        moving.x /= induction_x;
                        moving.z /= induction_z;
                        moving.y /= induction_y;
                        //���I�u�W�F�N�g�̌������Œ�
                        if (!changedirection_flag)
                        {
                            transform.eulerAngles = direction;
                        }
                        Induction_time = 0;
                    }

                    OwnMachinAndBeam_distance = Vector3.Distance(OwnMachine.transform.position, gameObject.transform.position);
                    OwnMachinAndEnemy_distance = Vector3.Distance(OwnMachine.transform.position, LockOnEnemy.transform.position) - 6f;
                    //���b�N�I���Ώۂ����@��莩�I�u�W�F�N�g�Ƌ߂��ꍇ�U����؂�
                    if (OwnMachinAndEnemy_distance <= OwnMachinAndBeam_distance)
                    {
                        isInduction = false;
                    }
                }
                //�U�����Ă��Ȃ��ꍇ�̈ړ��ݒ�
                else
                {
                    moving = transform.forward * speed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //���@�����������I�u�W�F�N�g�̏ꍇ
        if (pv != null)
        {
            if (pv.IsMine)
            {
                //���u�U���̏ꍇ�ړ�����
                if (!fighting_flag)
                {
                    rb.velocity = new Vector3(moving.x, moving.y, moving.z);
                }
            }
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (!fighting_flag)
                {
                    rb.velocity = new Vector3(moving.x, moving.y, moving.z);
                }
            }
        }
    }

    //���@�ƃ��b�N�I���Ώۂ�ݒ肷��
    public void LockOnEnemySetting(GameObject _OwnMachine, GameObject _LockOnEnemy)
    {
        OwnMachine = _OwnMachine;
        LockOnEnemy = _LockOnEnemy;
        //���b�N�I���Ώۂ���ꍇ
        if (LockOnEnemy != null)
        {
            direction_flag = true;
        }
    }

    //���I�u�W�F�N�g���I�u�W�F�N�g�ƐڐG�����ꍇ
    private void OnTriggerEnter(Collider other)
    {
        //���ɔ������Ȃ��I�u�W�F�N�g
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("OccurrenceDistance"))
        {

        }
        //����ȊO�̏ꍇ���I�u�W�F�N�g��j������
        else
        {
            //���u�U���̏ꍇ
            if (!fighting_flag)
            {
                Destroy(gameObject);
            }
        }
        /*if (pv != null)
        {
            if (pv.IsMine)
            {
                if (!fighting_flag)
                {
                    if (other.gameObject != OwnMachine && !other.gameObject.CompareTag("Bullet")
                        && other.gameObject.GetComponent<FightingOccurs_Control>() == null)
                    {
                        pv.RPC(nameof(ThisGameObjectDestroy), RpcTarget.AllBufferedViaServer);
                    }
                }
            }
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (!fighting_flag)
                {
                    if (other.gameObject != OwnMachine && !other.gameObject.CompareTag("Bullet")
                        && other.gameObject.GetComponent<FightingOccurs_Control>() == null)
                    {
                        ThisGameObjectDestroy();
                    }
                }
            }
        }*/
    }

    //KMF�ƐڐG�������̎��I�u�W�F�N�g�̔j��
    public void ThisGameObjectDestroy()
    {
        //���u�U���̏ꍇ
        if (!fighting_flag)
        {
            //�I�t���C��
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                Destroy(gameObject);
            }
            //�I�����C��
            else
            {
                pv.RPC(nameof(RpcGameObjectDestroy), RpcTarget.AllBufferedViaServer);
            }
        }
    }

    //���I�u�W�F�N�g�j���̓���
    [PunRPC]
    void RpcGameObjectDestroy()
    {
        Destroy(gameObject);
    }
}
