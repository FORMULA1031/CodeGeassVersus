using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDisplay_Control : MonoBehaviour
{
    public GameObject text;
    public GameObject HostPanel;
    public GameObject GestPanel;
    public GameObject TrainingPanel;
    public GameObject SettingPanel;
    public GameObject SelectingPanel;
    public GameObject FadePanel;
    public GameObject SearchPanel;
    public GameObject KeyConfigPanel;
    float time = 0;
    float panelopen_time = 0;
    bool disappearing_flag = false;
    bool panel_open = false;
    bool keyconfigpanelopen_flag = true;
    bool selected_host = false;
    bool selected_gest = false;
    public bool selected_setting = false;
    string selectmenu = "host";
    AudioSource AudioSource;
    public AudioClip showpanel_se;
    public AudioClip selected_se;
    NetworkManager NetworkManager;

    // Start is called before the first frame update
    void Start()
    {
        HostPanel.transform.localScale = new Vector3(HostPanel.transform.localScale.x, 0f, HostPanel.transform.localScale.z);
        GestPanel.transform.localScale = new Vector3(GestPanel.transform.localScale.x, 0f, GestPanel.transform.localScale.z);
        TrainingPanel.transform.localScale = new Vector3(TrainingPanel.transform.localScale.x, 0f, TrainingPanel.transform.localScale.z);
        SettingPanel.transform.localScale = new Vector3(SettingPanel.transform.localScale.x, 0f, SettingPanel.transform.localScale.z);
        SelectingPanel.transform.localScale = new Vector3(SelectingPanel.transform.localScale.x, 0f, SelectingPanel.transform.localScale.z);
        SearchPanel.transform.localScale = new Vector3(SearchPanel.transform.localScale.x, 0f, SearchPanel.transform.localScale.z);
        KeyConfigPanel.transform.localScale = new Vector3(KeyConfigPanel.transform.localScale.x, 0f, KeyConfigPanel.transform.localScale.z);
        AudioSource = GetComponent<AudioSource>();
        NetworkManager = GameObject.Find("EventSystem").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TextControl();
        PanelOpen();
        ChangeSelectMenu();

        //Œˆ’è
        if (Input.GetButtonDown("SubShooting"))
        {
            if (panel_open && !selected_host && !selected_gest && !selected_setting)
            {
                switch (selectmenu) {
                    case "host":
                        SearchPanel.transform.localScale = new Vector3(SearchPanel.transform.localScale.x, 1f, SearchPanel.transform.localScale.z);
                        AudioSource.PlayOneShot(selected_se);
                        selected_host = true;
                        NetworkManager.CreateAndJoinRoom();
                        break;
                    case "gest":
                        AudioSource.PlayOneShot(selected_se);
                        selected_gest = true;
                        NetworkManager.JoinRandomRoom();
                        break;
                    case "training":
                        FadePanel.transform.GetComponent<FadeManager>().Out = true;
                        FadePanel.transform.GetComponent<FadeManager>().scenename = "AircraftSelectionScene";
                        AudioSource.PlayOneShot(selected_se);
                        break;
                    case "setting":
                        if (keyconfigpanelopen_flag)
                        {
                            KeyConfigPanel.transform.localScale = new Vector3(KeyConfigPanel.transform.localScale.x, 1f, KeyConfigPanel.transform.localScale.z);
                            AudioSource.PlayOneShot(selected_se);
                            selected_setting = true;
                            Debug.Log("fasfadfda");
                            keyconfigpanelopen_flag = false;
                        }
                        break;
                }
            }
            else if (!panel_open)
            {
                panel_open = true;
                AudioSource.PlayOneShot(showpanel_se);
            }
        }
        if (Input.GetButtonUp("SubShooting"))
        {
            keyconfigpanelopen_flag = true;
        }

            //–ß‚é
            if (Input.GetButtonDown("Boost"))
        {
            if(panel_open && selected_host)
            {
                SearchPanel.transform.localScale = new Vector3(SearchPanel.transform.localScale.x, 0f, SearchPanel.transform.localScale.z);
                AudioSource.PlayOneShot(selected_se);
                selected_host = false;
                NetworkManager.LeaveRoom();
            }
            else if(panel_open && selected_gest)
            {
                AudioSource.PlayOneShot(selected_se);
                selected_gest = false;
                NetworkManager.JoinLobby();
            }
            else if (panel_open && selected_setting)
            {
                //CloseKeyConfigPanel();
            }
        }
    }

    void TextControl()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            disappearing_flag = !disappearing_flag;
            time = 0;
        }
        if (disappearing_flag)
        {
            text.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255, 1.0f - time);
        }
        else
        {
            text.transform.Find("Text").GetComponent<Text>().color = new Color(255, 255, 255, time);
        }

        if (panel_open)
        {
            text.SetActive(false);
        }
    }

    void PanelOpen()
    {
        if (panel_open && panelopen_time <= 0.25f)
        {
            panelopen_time += Time.deltaTime;
            HostPanel.transform.localScale = new Vector3(HostPanel.transform.localScale.x, panelopen_time * 4, HostPanel.transform.localScale.z);
            GestPanel.transform.localScale = new Vector3(GestPanel.transform.localScale.x, panelopen_time * 4, GestPanel.transform.localScale.z);
            TrainingPanel.transform.localScale = new Vector3(TrainingPanel.transform.localScale.x, panelopen_time * 4, TrainingPanel.transform.localScale.z);
            SettingPanel.transform.localScale = new Vector3(SettingPanel.transform.localScale.x, panelopen_time * 4, SettingPanel.transform.localScale.z);
            SelectingPanel.transform.localScale = new Vector3(SelectingPanel.transform.localScale.x, panelopen_time * 4, SelectingPanel.transform.localScale.z);
        }
        else if(panel_open && panelopen_time > 0.25f)
        {
            HostPanel.transform.localScale = new Vector3(HostPanel.transform.localScale.x, 1f, HostPanel.transform.localScale.z);
            GestPanel.transform.localScale = new Vector3(GestPanel.transform.localScale.x, 1f, GestPanel.transform.localScale.z);
            TrainingPanel.transform.localScale = new Vector3(TrainingPanel.transform.localScale.x, 1f, TrainingPanel.transform.localScale.z);
            SettingPanel.transform.localScale = new Vector3(SettingPanel.transform.localScale.x, 1f, SettingPanel.transform.localScale.z);
            SelectingPanel.transform.localScale = new Vector3(SelectingPanel.transform.localScale.x, 1f, SelectingPanel.transform.localScale.z);
        }
    }

    void ChangeSelectMenu()
    {
        if (panel_open && !selected_host && !selected_gest && !selected_setting)
        {
            float x = Input.GetAxis("Move_X");
            float y = Input.GetAxis("Move_Y") * -1;
            switch (selectmenu)
            {
                case "host":
                    if (x > 0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(180, 100, 0f);
                        selectmenu = "gest";
                    }
                    else if (y < -0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, -100, 0f);
                        selectmenu = "setting";
                    }
                    break;
                case "gest":
                    if (x < -0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, 100, 0f);
                        selectmenu = "host";
                    }
                    else if (y < -0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(180, -100, 0f);
                        selectmenu = "training";
                    }
                    break;
                case "training":
                    if (x < -0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, -100, 0f);
                        selectmenu = "setting";
                    }
                    else if (y > 0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(180, 100, 0f);
                        selectmenu = "gest";
                    }
                    break;
                case "setting":
                    if (x > 0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(180, -100, 0f);
                        selectmenu = "training";
                    }
                    else if (y > 0.5f)
                    {
                        SelectingPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(-180, 100, 0f);
                        selectmenu = "host";
                    }
                    break;
            }
        }
    }

    public void CloseKeyConfigPanel()
    {
        KeyConfigPanel.transform.localScale = new Vector3(KeyConfigPanel.transform.localScale.x, 0f, KeyConfigPanel.transform.localScale.z);
        AudioSource.PlayOneShot(selected_se);
        selected_setting = false;
        NetworkManager.LeaveRoom();
        keyconfigpanelopen_flag = false;
    }
}
