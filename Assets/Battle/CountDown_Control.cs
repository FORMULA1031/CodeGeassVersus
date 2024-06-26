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
    public bool countisDown = true;
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
        //�J�E���g�_�E���J�n
        if (countisDown)
        {
            countdown_time += Time.deltaTime;
            countdown_scale = countdown_time;
            //�J�E���g�_�E���p��UI�̑傫������
            if (countdown_scale > 1)
            {
                countdown_scale = 1;
            }
            StandeyPanel.transform.localScale = new Vector3(1, countdown_scale, 1);
            CountDownText.GetComponent<Text>().text = "<" + (4 - (int)countdown_time) + ">";

            //�X�^���o�CUI�̐���
            if (countdown_time >= 4 && countdown_time < 5)
            {
                StandeyPanel.transform.localScale = new Vector3(1, 0, 1);
                StartText.transform.localScale = new Vector3((countdown_time - 4) * 3, (countdown_time - 4) * 3, 1);
            }
            //�J�E���g�_�E���I��
            if (countdown_time >= 5)
            {
                StandeyPanel.transform.localScale = new Vector3(1, 0, 1);
                StartText.transform.localScale = new Vector3(0, 0, 1);
                Canvas.SetActive(true);
                LockOnMarkerCanvas.SetActive(true);
                //�I�t���C���p�̎��@�̎擾
                if (GameObject.Find("EventSystem").GetComponent<BattleGame_Control>() != null)
                {
                    Players = GameObject.Find("EventSystem").GetComponent<BattleGame_Control>().Player[0];
                }
                //�I�����C���p�̎��@�̎擾
                else if(GameObject.Find("EventSystem").GetComponent<OneOnOne_Control>() != null)
                {
                    Players = GameObject.Find("EventSystem").GetComponent<OneOnOne_Control>().Player;
                }
                //�@�̂̐�����\�ɂ���
                if (Players != null)
                {
                    if (Players.GetComponent<KMF_Control>() != null)
                    {
                        Players.GetComponent<KMF_Control>().enabled = true;
                    }
                }
                countisDown = false;
            }
        }
    }
}
