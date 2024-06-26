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
        //�ݒ胁�j���[�ɓ������ꍇ
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
            //�I�𒆃{�^���̐F
            if(action_number == currentnumber)
            {

                ActionPanels[action_number].GetComponent<Image>().color = new Color(1f, 0.78f, 0f);
            }
            //�I�𒆂ł͂Ȃ��ꍇ�̃{�^���̐F
            else
            {
                ActionPanels[action_number].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
    }

    //�I������{�^������
    void SelectingAction()
    {
        float x = Input.GetAxis("Move_X");
        float y = Input.GetAxis("Move_Y") * -1;

        //���o�[N
        if(x < 0.8f && x > -0.8f && y < 0.8f && y >-0.8f)
        {
            moved_flag = false;
        }

        //���͌���������ꍇ
        if (!moved_flag)
        {
            //�E
            if (x >= 0.8f)
            {
                if (currentnumber <= 3)
                {
                    currentnumber += 4;
                }
                moved_flag = true;
            }
            //��
            else if (x <= -0.8f)
            {
                if (currentnumber >= 4 && currentnumber != 8)
                {
                    currentnumber -= 4;
                }
                moved_flag = true;
            }
            //��
            if (y >= 0.8f)
            {
                if (currentnumber != 0 && currentnumber != 4)
                {
                    currentnumber -= 1;
                }
                moved_flag = true;
            }
            //��
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

    //����{�^�����������ꍇ
    void DicisionAction()
    {
        if (Input.GetButtonDown("SubShooting"))
        {
            //����Ƀ{�^�����߂����Ȃ�
            if (close_flag)
            {
                if (ActionPanels[currentnumber].GetComponent<RebindUI>() != null)
                {
                    //�Z�ɑ΂���{�^�����ߊJ�n
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
        //�{�^�����߂ł���悤�ɂ���
        else if (Input.GetButtonUp("SubShooting"))
        {
            close_flag = true;
        }
    }
}
