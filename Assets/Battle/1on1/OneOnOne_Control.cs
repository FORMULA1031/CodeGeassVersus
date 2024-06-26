using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class OneOnOne_Control : MonoBehaviour
{
    int[] team = { 6000, 6000 };
    public GameObject[] reswapn_position;
    public GameObject Selected_KMF;   //���炩���ߑI�������@��(�v���n�u)
    public GameObject Player; //�t�B�[���h�̑��݂��Ă���@��
    public int mine_number = 0;
    public GameObject MainCamera;
    public GameObject ArmedUI;
    public GameObject boostUI;
    public GameObject DurableValueUI;
    public GameObject[] WarPotentialGauge;
    public GameObject FinishPanel;
    bool[] relaunch_flag = { false, false };
    float[] relaunch_time = { 0, 0 };
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        IdentifyThePlayer();
        //�v���C���[�p�@�̂����݂���ꍇ
        if (Player.GetComponent<KMF_Control>() != null)
        {
            Player.GetComponent<KMF_Control>().enabled = false;
        }
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int num = 0; num < team.Length; num++)
        {
            WarPotentialGaugeControl(num);
            if (relaunch_flag[num])
            {
                relaunch_time[num] += Time.deltaTime;
                if (relaunch_time[num] >= 3)
                {
                    IdentifyThePlayer();
                    relaunch_flag[num] = false;
                    relaunch_time[num] = 0;
                }
            }
        }
    }

    //�@�̂̍ďo��
    void IdentifyThePlayer()
    {
        mine_number = PhotonNetwork.LocalPlayer.ActorNumber;
        Player = PhotonNetwork.Instantiate("Prefab/Lancelot_Player",
            reswapn_position[mine_number - 1].transform.position, reswapn_position[mine_number -1 ].transform.rotation);
        Player.GetComponent<PlayerID_Control>().ID = mine_number;
        MainCamera.GetComponent<Camera_Control>().Player_KMF = Player;
        ArmedUI.GetComponent<ArmedUi_Control>().Player = Player;
        boostUI.GetComponent<BoostUi_Control>().Player = Player;
        DurableValueUI.GetComponent<DurableValueUi_Control>().Player = Player;
    }

    //��̓Q�[�W�̍X�V�ƃQ�[���I���̔���
    void WarPotentialGaugeControl(int player_number)
    {
        if (WarPotentialGauge[player_number] != null)
        {
            WarPotentialGauge[player_number].GetComponent<Image>().fillAmount = (float)team[player_number] / 6000;

            //��̓Q�[�W�������Ȃ����ꍇ
            if (WarPotentialGauge[player_number].GetComponent<Image>().fillAmount <= 0)
            {
                if (FinishPanel != null)
                {
                    FinishPanel.GetComponent<FinishPanel_Control>().finish_flag = true;
                    //�����t���O�t�^
                    if (mine_number - 1 == player_number)
                    {
                        FinishPanel.GetComponent<FinishPanel_Control>().lose_flag = true;
                    }
                }
            }
        }
    }

    //���j���ꂽ�Ƃ��̏���
    public void ReLaunch(int player_number)
    {
        if (player_number != -1)
        {
            player_number -= 1;
            switch (player_number)
            {
                case 0:
                    pv.RPC(nameof(Team1CostDecreased), RpcTarget.AllBufferedViaServer);
                    break;
                case 1:
                    pv.RPC(nameof(Team2CostDecreased), RpcTarget.AllBufferedViaServer);
                    break;
            }
            //�ďo���\��
            if (WarPotentialGauge[player_number] != null)
            {
                if (WarPotentialGauge[player_number].GetComponent<Image>().fillAmount > 0)
                {
                    relaunch_flag[player_number] = true;
                }
            }
        }
    }

    //�`�[��1�̃R�X�g����
    [PunRPC]
    void Team1CostDecreased()
    {
        team[0] -= 2000;
    }

    //�`�[��2�̃R�X�g����
    [PunRPC]
    void Team2CostDecreased()
    {
        team[1] -= 2000;
    }
}
