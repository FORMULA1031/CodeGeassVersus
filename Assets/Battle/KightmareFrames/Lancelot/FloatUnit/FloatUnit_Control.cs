using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class FloatUnit_Control : MonoBehaviour
{
    GameObject Rancelot_obj;
    KMF_Control KMF_Control;
    Animator anim;
    bool boost_flag = false;
    bool jump_flag = false;
    ParticleSystem[] Jet_R = new ParticleSystem[3];
    ParticleSystem[] Jet_L = new ParticleSystem[3];
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        Rancelot_obj = transform.root.gameObject;
        KMF_Control = Rancelot_obj.GetComponent<KMF_Control>();
        anim = GetComponent<Animator>();
        Jet_R[0] = GameObject.Find("FloatUnit/Bone/Bone.001.R/Bone.001.R_end/JetEngine/Eff_Jet/par1_add").transform.GetComponent<ParticleSystem>();
        Jet_R[1] = GameObject.Find("FloatUnit/Bone/Bone.001.R/Bone.001.R_end/JetEngine/Eff_Jet/par2_alpha").transform.GetComponent<ParticleSystem>();
        Jet_R[2] = GameObject.Find("FloatUnit/Bone/Bone.001.R/Bone.001.R_end/JetEngine/Eff_Jet/shock1_alpha").transform.GetComponent<ParticleSystem>();
        Jet_L[0] = GameObject.Find("FloatUnit/Bone/Bone.001.L/Bone.001.L_end/JetEngine/Eff_Jet/par1_add").transform.GetComponent<ParticleSystem>();
        Jet_L[1] = GameObject.Find("FloatUnit/Bone/Bone.001.L/Bone.001.L_end/JetEngine/Eff_Jet/par2_alpha").transform.GetComponent<ParticleSystem>();
        Jet_L[2] = GameObject.Find("FloatUnit/Bone/Bone.001.L/Bone.001.L_end/JetEngine/Eff_Jet/shock1_alpha").transform.GetComponent<ParticleSystem>();
        Jet_R[0].Stop();
        Jet_R[1].Stop();
        Jet_R[2].Stop();
        Jet_L[0].Stop();
        Jet_L[1].Stop();
        Jet_L[2].Stop();
    }

    private void FixedUpdate()
    {
        if (pv != null)
        {
            if (pv.IsMine || SceneManager.GetActiveScene().name == "TrainingScene")
            {
                if (KMF_Control.anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Boost_Sky") ||
                    KMF_Control.anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Boost_Landing") ||
                    KMF_Control.anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|MEBoost") ||
                    KMF_Control.anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Jump"))
                {
                    if (!boost_flag && !jump_flag)
                    {
                        if (!KMF_Control.anim.GetCurrentAnimatorStateInfo(0).IsName("Lancelot|Jump"))
                        {
                            boost_flag = true;
                        }
                        Jet_R[0].Play();
                        Jet_R[1].Play();
                        Jet_R[2].Play();
                        Jet_L[0].Play();
                        Jet_L[1].Play();
                        Jet_L[2].Play();
                        jump_flag = true;
                    }
                }
                else
                {
                    Jet_R[0].Stop();
                    Jet_R[1].Stop();
                    Jet_R[2].Stop();
                    Jet_L[0].Stop();
                    Jet_L[1].Stop();
                    Jet_L[2].Stop();
                    boost_flag = false;
                    jump_flag = false;
                }
                anim.SetBool("Boost_Flag", boost_flag);
            }
        }
    }
}
