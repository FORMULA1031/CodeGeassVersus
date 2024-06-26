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
        //�v���C���[�p�̋@�̎擾
        if(KMF_Control == null)
        {
            KMF_Control = transform.GetComponent<KMF_Control>();
        }

        //NPC�p�̋@�̎擾
        if(Cpu_Control != null)
        {
            if(Cpu_Control.durable_value <= 0)
            {
                explosion_flag = true;
            }
        }
        //�@�̂̑ϋv�l��0�ɂȂ����ꍇ
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

    //��j����
    public void DestroyedControl()
    {
        time += Time.deltaTime;
        //��莞�ԑ�j����
        if(time >= 0.5f && !destroyed_flag)
        {
            //���@�̂ݎ��s����
            if (pv != null)
            {
                //�I�����C��
                if (pv.IsMine)
                {
                    pv.RPC(nameof(RpcDestroying), RpcTarget.AllBufferedViaServer);
                }
                //�I�����C��
                else if (SceneManager.GetActiveScene().name == "TrainingScene")
                {
                    RpcDestroying();
                }
            }
            //�I�����C��
            else if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                RpcDestroying();
            }
            destroyed_flag = true;
        }
    }

    //��j����
    [PunRPC]
    void RpcDestroying()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
