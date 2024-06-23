using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public GameObject Player_KMF;
    GameObject[] Enemys = new GameObject[3];
    Vector3 last_position = new Vector3(0, 0, 0);

    Vector3 m_position;   //カメラの座標
    GameObject EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        //Player_KMF = GameObject.Find("Lancelot_Player");
        EventSystem = GameObject.Find("EventSystem");
    }

    private void FixedUpdate()
    {
        if (Enemys[0] == null)
        {
            if (EventSystem.GetComponent<BattleGame_Control>() != null)
            {
                Enemys[0] = GameObject.Find("Lancelot_Enemy(Clone)");
            }
            else if (EventSystem.GetComponent<OneOnOne_Control>() != null)
            {
                Enemys[0] = Player_KMF.GetComponent<PlayerID_Control>().LockOnEnemy;
                /*GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int num = 0; num < players.Length; num++)
                {
                    if (players[num].GetComponent<PlayerID_Control>() != null)
                    {
                        if (EventSystem.GetComponent<OneOnOne_Control>().mine_number != players[num].GetComponent<PlayerID_Control>().ID)
                        {
                            Enemys[0] = players[num];
                        }
                    }
                }*/
            }
        }

        if (Enemys[0] != null && Player_KMF != null)
        {
            last_position = transform.position - Player_KMF.transform.position;
            //エネミーからプレイヤーに伸びるベクトルを求めます。
            Vector3 pos = Player_KMF.transform.position - Enemys[0].transform.position;
            //ベクトルを正規化します。
            pos.Normalize();
            //スカラーを掛けます
            pos *= 20.0f;
            //カメラがどれだけプレイヤーの座標より高いかを設定します。
            pos.y = 10.0f;
            pos.y -= transform.eulerAngles.x / 100;
            pos.y -= (Vector3.Distance(transform.position, Enemys[0].transform.position) / 100f);
            //プレイヤーの座標に求めたベクトルを足して、カメラの座標とします。
            m_position = Player_KMF.transform.position + pos;
            transform.position = m_position;
            Vector3 enemy_position = new Vector3(Enemys[0].transform.position.x, Enemys[0].transform.position.y - 10f, Enemys[0].transform.position.z);
            // ターゲット方向のベクトルを取得
            Vector3 relativePos = enemy_position - this.transform.position;
            // 方向を、回転情報に変換
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 1.0f);
        }
        else if(Player_KMF != null)
        {
            transform.position = Player_KMF.transform.position + last_position;
        }
    }
}
