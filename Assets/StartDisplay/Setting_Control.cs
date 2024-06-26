using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Control : MonoBehaviour
{
    StartDisplay_Control StartDisplay_Control;
    public GameObject[] ActionPanels;
    int currentnumber = 0;
    bool moved_flag = false;
    bool close_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        StartDisplay_Control = GetComponent<StartDisplay_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        //設定メニューに入った場合
        if (StartDisplay_Control.selected_setting)
        {
            SelectingAction();
            DicisionAction();
        }
    }

    private void FixedUpdate()
    {
        for(int action_number = 0; action_number < 9; action_number++)
        {
            //選択中ボタンの色
            if(action_number == currentnumber)
            {

                ActionPanels[action_number].GetComponent<Image>().color = new Color(1f, 0.78f, 0f);
            }
            //選択中ではない場合のボタンの色
            else
            {
                ActionPanels[action_number].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
    }

    //選択するボタン決め
    void SelectingAction()
    {
        float x = Input.GetAxis("Move_X");
        float y = Input.GetAxis("Move_Y") * -1;

        //レバーN
        if(x < 0.8f && x > -0.8f && y < 0.8f && y >-0.8f)
        {
            moved_flag = false;
        }

        //入力権限がある場合
        if (!moved_flag)
        {
            //右
            if (x >= 0.8f)
            {
                if (currentnumber <= 3)
                {
                    currentnumber += 4;
                }
                moved_flag = true;
            }
            //左
            else if (x <= -0.8f)
            {
                if (currentnumber >= 4 && currentnumber != 8)
                {
                    currentnumber -= 4;
                }
                moved_flag = true;
            }
            //上
            if (y >= 0.8f)
            {
                if (currentnumber != 0 && currentnumber != 4)
                {
                    currentnumber -= 1;
                }
                moved_flag = true;
            }
            //下
            else if (y <= -0.8f)
            {
                if (currentnumber != 3 && currentnumber != 8)
                {
                    currentnumber += 1;
                }
                else if(currentnumber == 3)
                {
                    currentnumber = 8;
                }
                moved_flag = true;
            }
        }
    }

    //決定ボタンを押した場合
    void DicisionAction()
    {
        if (Input.GetButtonDown("SubShooting"))
        {
            //勝手にボタン決めをしない
            if (close_flag)
            {
                if (ActionPanels[currentnumber].GetComponent<RebindUI>() != null)
                {
                    //技に対するボタン決め開始
                    if (!ActionPanels[currentnumber].GetComponent<RebindUI>().rebinding_flag)
                    {
                        ActionPanels[currentnumber].GetComponent<RebindUI>().StartRebinding();
                    }
                }
                else
                {
                    StartDisplay_Control.CloseKeyConfigPanel();
                    close_flag = false;
                }
            }
        }
        //ボタン決めできるようにする
        else if (Input.GetButtonUp("SubShooting"))
        {
            close_flag = true;
        }
    }
}
