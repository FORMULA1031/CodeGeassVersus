using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public GameObject Player_KMF;
    GameObject[] Enemys = new GameObject[3];
    Vector3 last_position = new Vector3(0, 0, 0);

    Vector3 m_position;   //�J�����̍��W
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
            //�G�l�~�[����v���C���[�ɐL�т�x�N�g�������߂܂��B
            Vector3 pos = Player_KMF.transform.position - Enemys[0].transform.position;
            //�x�N�g���𐳋K�����܂��B
            pos.Normalize();
            //�X�J���[���|���܂�
            pos *= 20.0f;
            //�J�������ǂꂾ���v���C���[�̍��W��荂������ݒ肵�܂��B
            pos.y = 10.0f;
            pos.y -= transform.eulerAngles.x / 100;
            pos.y -= (Vector3.Distance(transform.position, Enemys[0].transform.position) / 100f);
            //�v���C���[�̍��W�ɋ��߂��x�N�g���𑫂��āA�J�����̍��W�Ƃ��܂��B
            m_position = Player_KMF.transform.position + pos;
            transform.position = m_position;
            Vector3 enemy_position = new Vector3(Enemys[0].transform.position.x, Enemys[0].transform.position.y - 10f, Enemys[0].transform.position.z);
            // �^�[�Q�b�g�����̃x�N�g�����擾
            Vector3 relativePos = enemy_position - this.transform.position;
            // �������A��]���ɕϊ�
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            // ���݂̉�]���ƁA�^�[�Q�b�g�����̉�]����⊮����
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 1.0f);
        }
        else if(Player_KMF != null)
        {
            transform.position = Player_KMF.transform.position + last_position;
        }
    }
}
