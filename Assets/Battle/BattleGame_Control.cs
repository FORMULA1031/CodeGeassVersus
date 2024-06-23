using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGame_Control : MonoBehaviour
{
    int[] team = { 6000, 6000 };
    public GameObject[] reswapn_position;
    public GameObject[] Selected_KMF;   //あらかじめ選択した機体(プレハブ)
    public GameObject[] Player = new GameObject[2]; //フィールドの存在している機体
    int mine_number = 0;
    public GameObject MainCamera;
    public GameObject ArmedUI;
    public GameObject boostUI;
    public GameObject DurableValueUI;
    public GameObject[] WarPotentialGauge;
    public GameObject FinishPanel;
    bool[] relaunch_flag = { false, false };
    float[] relaunch_time = { 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        for (int num = 0; num < team.Length; num++) {
            IdentifyThePlayer(num);
            if(Player[num].GetComponent<KMF_Control>() != null)
            {
                Player[num].GetComponent<KMF_Control>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int num = 0; num <team.Length; num++)
        {
            if (relaunch_flag[num])
            {
                relaunch_time[num] += Time.deltaTime;
                if(relaunch_time[num] >= 3)
                {
                    IdentifyThePlayer(num);
                    relaunch_flag[num] = false;
                    relaunch_time[num] = 0;
                }
            }
        }
    }

    void IdentifyThePlayer(int num)
    {
        Player[num] = Instantiate(Selected_KMF[num], reswapn_position[num].transform.position, reswapn_position[num].transform.rotation);
        Player[num].GetComponent<PlayerID_Control>().ID = num;
        if(mine_number == num)
        {
            MainCamera.GetComponent<Camera_Control>().Player_KMF = Player[num];
            ArmedUI.GetComponent<ArmedUi_Control>().Player = Player[num];
            boostUI.GetComponent<BoostUi_Control>().Player = Player[num];
            DurableValueUI.GetComponent<DurableValueUi_Control>().Player = Player[num];
        }
    }

    public void ReLaunch(int player_number)
    {
        if (player_number != -1)
        {
            team[player_number] -= 2000;
            if (WarPotentialGauge[player_number] != null)
            {
                WarPotentialGauge[player_number].GetComponent<Image>().fillAmount = (float)team[player_number] / 6000;

                if (WarPotentialGauge[player_number].GetComponent<Image>().fillAmount <= 0)
                {
                    if (FinishPanel != null)
                    {
                        FinishPanel.GetComponent<FinishPanel_Control>().finish_flag = true;
                        if (mine_number == player_number)
                        {
                            FinishPanel.GetComponent<FinishPanel_Control>().lose_flag = true;
                        }
                    }
                }
                else
                {
                    relaunch_flag[player_number] = true;
                }
            }
        }
    }
}
