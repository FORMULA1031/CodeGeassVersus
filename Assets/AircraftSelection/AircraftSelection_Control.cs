using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftSelection_Control : MonoBehaviour
{
    bool selectkmf_flag = false;
    public GameObject FadePanel;
    AudioSource AudioSource;
    public AudioClip selectkmf_se;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //決定ボタンを押した場合
        if (Input.GetButtonDown("SubShooting"))
        {
            //ボタンは一度しか押せない
            if (!selectkmf_flag)
            {
                FadePanel.transform.GetComponent<FadeManager>().Out = true;
                FadePanel.transform.GetComponent<FadeManager>().In = false;
                FadePanel.transform.GetComponent<FadeManager>().scenename = "TrainingScene";
                selectkmf_flag = true;
                AudioSource.PlayOneShot(selectkmf_se);
            }
        }
    }
}
