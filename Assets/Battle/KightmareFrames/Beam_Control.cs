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
            //自機が生成したオブジェクトの場合
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
        //自機が生成したオブジェクトの場合
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                //遠隔攻撃の場合
                if (!fighting_flag && LockOnEnemy != null)
                {
                    //一度だけ相手の方向へ向く
                    if (direction_flag)
                    {
                        gameObject.transform.LookAt(LockOnEnemy.transform);
                        direction_flag = false;
                        moving = transform.forward * speed;
                    }

                    //誘導切りされたか確認(対プレイヤー)
                    if (LockOnEnemy.GetComponent<KMF_Control>())
                    {
                        if (LockOnEnemy.GetComponent<KMF_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    //誘導切りされたか確認(対NPC)
                    else if (LockOnEnemy.GetComponent<Cpu_Control>())
                    {
                        if (LockOnEnemy.GetComponent<Cpu_Control>().isInductionOff)
                        {
                            isInduction = false;
                        }
                    }
                    Induction_time += Time.deltaTime;
                    //一定時間ごとに誘導する
                    if (Induction_time >= Induction_cooltime && LockOnEnemy != null && isInduction)
                    {
                        direction = transform.eulerAngles;
                        transform.LookAt(LockOnEnemy.transform);
                        moving = transform.forward * speed;
                        moving.x /= induction_x;
                        moving.z /= induction_z;
                        moving.y /= induction_y;
                        //自オブジェクトの向きを固定
                        if (!changedirection_flag)
                        {
                            transform.eulerAngles = direction;
                        }
                        Induction_time = 0;
                    }

                    OwnMachinAndBeam_distance = Vector3.Distance(OwnMachine.transform.position, gameObject.transform.position);
                    OwnMachinAndEnemy_distance = Vector3.Distance(OwnMachine.transform.position, LockOnEnemy.transform.position) - 6f;
                    //ロックオン対象が自機より自オブジェクトと近い場合誘導を切る
                    if (OwnMachinAndEnemy_distance <= OwnMachinAndBeam_distance)
                    {
                        isInduction = false;
                    }
                }
                //誘導していない場合の移動設定
                else
                {
                    moving = transform.forward * speed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //自機が生成したオブジェクトの場合
        if (pv != null)
        {
            if (pv.IsMine)
            {
                //遠隔攻撃の場合移動する
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

    //自機とロックオン対象を設定する
    public void LockOnEnemySetting(GameObject _OwnMachine, GameObject _LockOnEnemy)
    {
        OwnMachine = _OwnMachine;
        LockOnEnemy = _LockOnEnemy;
        //ロックオン対象いる場合
        if (LockOnEnemy != null)
        {
            direction_flag = true;
        }
    }

    //自オブジェクトがオブジェクトと接触した場合
    private void OnTriggerEnter(Collider other)
    {
        //特に反応しないオブジェクト
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("OccurrenceDistance"))
        {

        }
        //それ以外の場合自オブジェクトを破棄する
        else
        {
            //遠隔攻撃の場合
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

    //KMFと接触した時の自オブジェクトの破棄
    public void ThisGameObjectDestroy()
    {
        //遠隔攻撃の場合
        if (!fighting_flag)
        {
            //オフライン
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                Destroy(gameObject);
            }
            //オンライン
            else
            {
                pv.RPC(nameof(RpcGameObjectDestroy), RpcTarget.AllBufferedViaServer);
            }
        }
    }

    //自オブジェクト破棄の同期
    [PunRPC]
    void RpcGameObjectDestroy()
    {
        Destroy(gameObject);
    }
}
