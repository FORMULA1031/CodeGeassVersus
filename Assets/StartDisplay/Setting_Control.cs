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

    // Start is called before the first frame update
    void Start()
    {
        StartDisplay_Control = GetComponent<StartDisplay_Control>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if(action_number == currentnumber)
            {

                ActionPanels[action_number].GetComponent<Image>().color = new Color(1f, 0.78f, 0f);
            }
            else
            {
                ActionPanels[action_number].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
    }

    void SelectingAction()
    {
        float x = Input.GetAxis("Move_X");
        float y = Input.GetAxis("Move_Y") * -1;

        if(x < 0.8f && x > -0.8f && y < 0.8f && y >-0.8f)
        {
            moved_flag = false;
        }

        if (!moved_flag)
        {
            if (x >= 0.8f)
            {
                if (currentnumber <= 3)
                {
                    currentnumber += 4;
                }
                moved_flag = true;
            }
            else if (x <= -0.8f)
            {
                if (currentnumber >= 4 && currentnumber != 8)
                {
                    currentnumber -= 4;
                }
                moved_flag = true;
            }
            if (y >= 0.8f)
            {
                if (currentnumber != 0 && currentnumber != 4)
                {
                    currentnumber -= 1;
                }
                moved_flag = true;
            }
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

    void DicisionAction()
    {
        if (Input.GetButtonDown("SubShooting"))
        {
            if (ActionPanels[currentnumber].GetComponent<RebindUI>() != null)
            {
                if (!ActionPanels[currentnumber].GetComponent<RebindUI>().rebinding_flag)
                {
                    ActionPanels[currentnumber].GetComponent<RebindUI>().StartRebinding();
                }
            }
            else
            {
                StartDisplay_Control.CloseKeyConfigPanel();
            }
        }
    }
}
