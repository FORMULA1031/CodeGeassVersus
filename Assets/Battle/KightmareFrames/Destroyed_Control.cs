using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Destroyed_Control : MonoBehaviour
{
    public GameObject Explosion;
    float time = 0;
    bool explosion_flag = false;
    bool destroyed_flag = false;
    KMF_Control KMF_Control;
    Cpu_Control Cpu_Control;
    PhotonView pv;

    private void Start()
    {
        Cpu_Control = transform.GetComponent<Cpu_Control>();
        pv = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        //プレイヤー用の機体取得
        if(KMF_Control == null)
        {
            KMF_Control = transform.GetComponent<KMF_Control>();
        }

        //NPC用の機体取得
        if(Cpu_Control != null)
        {
            if(Cpu_Control.durable_value <= 0)
            {
                explosion_flag = true;
            }
        }
        //機体の耐久値が0になった場合
        if (KMF_Control != null)
        {
            if (KMF_Control.durable_value <= 0)
            {
                explosion_flag = true;
            }
        }
        if (explosion_flag)
        {
            DestroyedControl();
        }
    }

    //大破制御
    public void DestroyedControl()
    {
        time += Time.deltaTime;
        //一定時間大破する
        if(time >= 0.5f && !destroyed_flag)
        {
            //自機のみ実行する
            if (pv != null)
            {
                //オンライン
                if (pv.IsMine)
                {
                    pv.RPC(nameof(RpcDestroying), RpcTarget.AllBufferedViaServer);
                }
                //オンライン
                else if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    RpcDestroying();
                }
            }
            //オンライン
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcDestroying();
            }
            destroyed_flag = true;
        }
    }

    //大破同期
    [PunRPC]
    void RpcDestroying()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
