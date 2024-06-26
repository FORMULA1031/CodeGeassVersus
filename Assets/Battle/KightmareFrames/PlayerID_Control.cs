using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerID_Control : MonoBehaviour
{
    public int ID = -1;
    GameObject EventSystem;
    public GameObject LockOnEnemy;
    GameObject RockOnMaker;
    RockOnMaker_Control RockOnMaker_Control;
    int player_maxnumber;
    bool relaunch_flag = true;
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem = GameObject.Find("EventSystem");
        //プレイヤーの最大人数取得
        if (EventSystem.GetComponent<BattleGame_Control>() != null)
        {
            player_maxnumber = EventSystem.GetComponent<BattleGame_Control>().Selected_KMF.Length;
        }
        pv = GetComponent<PhotonView>();
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine)
            {
                //タイマンの場合
                if (EventSystem.GetComponent<OneOnOne_Control>() != null)
                {
                    player_maxnumber = 2;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RockOnMakerSearch();
        //コントロール権限がある場合
        if (pv != null)
        {
            if (pv.IsMine)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int num = 0; num < players.Length; num++)
                {
                    if (EventSystem.GetComponent<OneOnOne_Control>() != null)
                    {
                        if (players[num].GetComponent<PlayerID_Control>() != null)
                        {
                            //自機ではない場合ロックオン対象とする
                            if (ID != players[num].GetComponent<PlayerID_Control>().ID)
                            {
                                LockOnEnemy = players[num];
                                if (RockOnMaker_Control != null)
                                {
                                    RockOnMaker_Control.target = LockOnEnemy;
                                }
                            }
                        }
                    }
                }
            }
            //オフライン
            else if (EventSystem.GetComponent<BattleGame_Control>() != null)
            {
                LockOnEnemy = GameObject.Find("Lancelot_Enemy(Clone)");
                //ロックオン対象決定
                if (RockOnMaker_Control != null)
                {
                    RockOnMaker_Control.target = LockOnEnemy;
                }
            }
        }
    }

    //ロックオンマーカーを探す
    void RockOnMakerSearch()
    {
        if (RockOnMaker == null)
        {
            RockOnMaker = GameObject.Find("LockOnMarkerCanvas/RockOnMarker_UI");
            //ロックオンマーカーが見つかった場合
            if (RockOnMaker != null)
            {
                if (RockOnMaker.activeSelf)
                {
                    RockOnMaker_Control = RockOnMaker.GetComponent<RockOnMaker_Control>();
                }
            }
        }
    }

    //自オブジェクトが破壊された場合
    private void OnDestroy()
    {
        //コントロール権限がある場合
        if (pv != null)
        {
            //オンライン
            if (pv.IsMine)
            {
                //再出撃申請する
                if (EventSystem != null && relaunch_flag)
                {
                    if (EventSystem.GetComponent<OneOnOne_Control>() != null)
                    {
                        EventSystem.GetComponent<OneOnOne_Control>().ReLaunch(ID);
                    }
                    relaunch_flag = false;
                }
            }
        }
        else
        {
            //オフライン
            if (EventSystem != null && relaunch_flag)
            {
                if (EventSystem.GetComponent<BattleGame_Control>() != null)
                {
                    EventSystem.GetComponent<BattleGame_Control>().ReLaunch(ID);
                }
                relaunch_flag = false;
            }
        }
    }
}
