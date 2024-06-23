using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown_Control : MonoBehaviour
{
    float countdown_time = 0;
    float countdown_scale = 0;
    GameObject StandeyPanel;
    GameObject CountDownText;
    GameObject StartText;
    GameObject Players;
    GameObject Canvas;
    GameObject LockOnMarkerCanvas;
    public bool countdown_flag = true;
    AudioSource AudioSource;
    public AudioClip countdown_se;

    // Start is called before the first frame update
    void Start()
    {
        StandeyPanel = gameObject.transform.Find("StandeyPanel").gameObject;
        StandeyPanel.transform.localScale = new Vector3(1, 0, 1);
        CountDownText = StandeyPanel.transform.Find("CountDownText").gameObject;
        StartText = gameObject.transform.Find("StartText").gameObject;
        StartText.transform.localScale = new Vector3(0, 0, 1);
        Canvas = GameObject.Find("Canvas");
        Canvas.SetActive(false);
        LockOnMarkerCanvas = GameObject.Find("LockOnMarkerCanvas/RockOnMarker_UI");
        LockOnMarkerCanvas.SetActive(false);
        AudioSource = GetComponent<AudioSource>();
        AudioSource.PlayOneShot(countdown_se);
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown_flag)
        {
            countdown_time += Time.deltaTime;
            countdown_scale = countdown_time;
            if (countdown_scale > 1)
            {
                countdown_scale = 1;
            }
            StandeyPanel.transform.localScale = new Vector3(1, countdown_scale, 1);
            CountDownText.GetComponent<Text>().text = "<" + (4 - (int)countdown_time) + ">";

            if (countdown_time >= 4 && countdown_time < 5)
            {
                StandeyPanel.transform.localScale = new Vector3(1, 0, 1);
                StartText.transform.localScale = new Vector3((countdown_time - 4) * 3, (countdown_time - 4) * 3, 1);
            }
            if (countdown_time >= 5)
            {
                StandeyPanel.transform.localScale = new Vector3(1, 0, 1);
                StartText.transform.localScale = new Vector3(0, 0, 1);
                Canvas.SetActive(true);
                LockOnMarkerCanvas.SetActive(true);
                if (GameObject.Find("EventSystem").GetComponent<BattleGame_Control>() != null)
                {
                    Players = GameObject.Find("EventSystem").GetComponent<BattleGame_Control>().Player[0];
                }
                else if(GameObject.Find("EventSystem").GetComponent<OneOnOne_Control>() != null)
                {
                    Players = GameObject.Find("EventSystem").GetComponent<OneOnOne_Control>().Player;
                }
                if (Players != null)
                {
                    if (Players.GetComponent<KMF_Control>() != null)
                    {
                        Players.GetComponent<KMF_Control>().enabled = true;
                    }
                }
                countdown_flag = false;
            }
        }
    }
}
