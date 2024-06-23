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
    public bool fighting_flag;  //���̃I�u�W�F�N�g���i����
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
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (!fighting_flag && LockOnEnemy != null)
                {
                    if (direction_flag)
                    {
                        gameObject.transform.LookAt(LockOnEnemy.transform);
                        direction_flag = false;
                        moving = transform.forward * speed;
                    }

                    if (LockOnEnemy.GetComponent<KMF_Control>())
                    {
                        if (LockOnEnemy.GetComponent<KMF_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    else if (LockOnEnemy.GetComponent<Cpu_Control>())
                    {
                        if (LockOnEnemy.GetComponent<Cpu_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    Induction_time += Time.deltaTime;
                    if (Induction_time >= Induction_cooltime && LockOnEnemy != null && isInduction)
                    {
                        direction = transform.eulerAngles;
                        transform.LookAt(LockOnEnemy.transform);
                        moving = transform.forward * speed;
                        moving.x /= induction_x;
                        moving.z /= induction_z;
                        moving.y /= induction_y;
                        if (!changedirection_flag)
                        {
                            transform.eulerAngles = direction;
                        }
                        Induction_time = 0;
                    }

                    OwnMachinAndBeam_distance = Vector3.Distance(OwnMachine.transform.position, gameObject.transform.position);
                    OwnMachinAndEnemy_distance = Vector3.Distance(OwnMachine.transform.position, LockOnEnemy.transform.position) - 6f;
                    if (OwnMachinAndEnemy_distance <= OwnMachinAndBeam_distance)
                    {
                        isInduction = false;
                    }
                }
                else
                {
                    moving = transform.forward * speed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (pv != null)
        {
            if (pv.IsMine)
            {
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

    public void LockOnEnemySetting(GameObject _OwnMachine, GameObject _LockOnEnemy)
    {
        OwnMachine = _OwnMachine;
        LockOnEnemy = _LockOnEnemy;
        if (LockOnEnemy != null)
        {
            direction_flag = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("OccurrenceDistance"))
        {

        }
        else
        {
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

    public void ThisGameObjectDestroy()
    {
        if (!fighting_flag)
        {
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                Destroy(gameObject);
            }
            else
            {
                pv.RPC(nameof(RpcGameObjectDestroy), RpcTarget.AllBufferedViaServer);
            }
        }
    }

    [PunRPC]
    void RpcGameObjectDestroy()
    {
        Destroy(gameObject);
    }
}
