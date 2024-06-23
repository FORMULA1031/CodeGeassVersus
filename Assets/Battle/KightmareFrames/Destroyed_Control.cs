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
        if(KMF_Control == null)
        {
            KMF_Control = transform.GetComponent<KMF_Control>();
        }

        if(Cpu_Control != null)
        {
            if(Cpu_Control.durable_value <= 0)
            {
                explosion_flag = true;
            }
        }
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

    public void DestroyedControl()
    {
        time += Time.deltaTime;
        if(time >= 0.5f && !destroyed_flag)
        {
            if (pv != null)
            {
                if (pv.IsMine)
                {
                    pv.RPC(nameof(RpcDestroying), RpcTarget.AllBufferedViaServer);
                }
                else if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    RpcDestroying();
                }
            }
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcDestroying();
            }
            destroyed_flag = true;
        }
    }

    [PunRPC]
    void RpcDestroying()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
