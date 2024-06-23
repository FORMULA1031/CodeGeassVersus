using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel_Control : MonoBehaviour
{
    float finishpanel_time = 0;
    float finishpanel_scale = 0;
    GameObject BackGroundPanel_LOSE;
    GameObject BackGroundPanel_WIN;
    public GameObject FadePanel;
    public bool finish_flag = false;
    public bool lose_flag = false;
    AudioSource AudioSource;
    public AudioClip win_se;
    public AudioClip lose_se;
    bool finishse_flag = true;

    // Start is called before the first frame update
    void Start()
    {
        BackGroundPanel_LOSE = gameObject.transform.Find("BackGroundPanel_Lose").gameObject;
        BackGroundPanel_LOSE.transform.localScale = new Vector3(1, 0, 1);
        BackGroundPanel_WIN = gameObject.transform.Find("BackGroundPanel_Win").gameObject;
        BackGroundPanel_WIN.transform.localScale = new Vector3(1, 0, 1);
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lose_flag)
        {
            BackGroundPanel_LOSE.SetActive(true);
            BackGroundPanel_WIN.SetActive(false);
        }
        else
        {
            BackGroundPanel_LOSE.SetActive(false);
            BackGroundPanel_WIN.SetActive(true);
        }

        if (finish_flag)
        {
            finishpanel_time += Time.deltaTime;
            finishpanel_scale = finishpanel_time;
            if(finishpanel_scale > 1)
            {
                finishpanel_scale = 1;
            }
            BackGroundPanel_LOSE.transform.localScale = new Vector3(1, finishpanel_scale, 1);
            BackGroundPanel_WIN.transform.localScale = new Vector3(1, finishpanel_scale, 1);

            if (finishpanel_time >= 3)
            {
                BackGroundPanel_LOSE.transform.localScale = new Vector3(1, 0, 1);
                BackGroundPanel_WIN.transform.localScale = new Vector3(1, 0, 1);
            }
            if(finishpanel_time >= 4)
            {
                FadePanel.transform.GetComponent<FadeManager>().Out = true;
                FadePanel.transform.GetComponent<FadeManager>().scenename = "StartDisplayScene";
            }

            if (finishse_flag)
            {
                if (lose_flag)
                {
                    AudioSource.PlayOneShot(lose_se);
                }
                else
                {
                    AudioSource.PlayOneShot(win_se);
                }
                finishse_flag = false;
            }
        }
    }
}
