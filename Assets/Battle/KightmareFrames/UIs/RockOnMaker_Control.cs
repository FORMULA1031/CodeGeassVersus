using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RockOnMaker_Control : MonoBehaviour
{
    public GameObject target;
    Vector3 targetPoint;
    public Image EnemyDurableBar;
    public Sprite RockOnMaker_Red;
    public Sprite RockOnMaker_Yellow;

    private void FixedUpdate()
    {
        //ロックオン対象が存在する場合
        if (target != null)
        {
            //ロックオンマーカー表示
            gameObject.GetComponent<Image>().enabled = true;
            foreach (Transform child in gameObject.transform)
            {
                if (child.GetComponent<Image>() != null)
                {
                    child.gameObject.SetActive(true);
                }
            }
            targetPoint = Camera.main.WorldToScreenPoint(target.transform.position);
            transform.GetComponent<RectTransform>().position = targetPoint;
            //ロックオン対象の耐久値を表示(オフライン)
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<Cpu_Control>().durable_value / (float)target.GetComponent<Cpu_Control>().durable_maxvalue;
            }
            //ロックオン対象の耐久値を表示(オンライン)
            else
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<KMF_Control>().durable_value / (float)target.GetComponent<KMF_Control>().durable_maxvalue;
            }
            RockOnMaker_Color();
        }
        else
        {
            //ロックオンマーカー非表示
            gameObject.GetComponent<Image>().enabled = false;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            //ロックオン対象を探す(オフライン)
            if (GameObject.Find("Lancelot_Enemy(Clone)") != null)
            {
                target = GameObject.Find("Lancelot_Enemy(Clone)");
            }
        }
    }

    //ロックオンマーカーの色制御
    void RockOnMaker_Color()
    {
        //オフライン
        if (target.GetComponent<Cpu_Control>() != null)
        {
            //敵のダウン中
            if (target.GetComponent<Cpu_Control>().isDown)
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Yellow;
            }
            else
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Red;
            }
        }
        //オンライン
        else if (target.GetComponent<KMF_Control>() != null)
        {
            //敵のダウン中
            if (target.GetComponent<KMF_Control>().isDown)
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Yellow;
            }
            else
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Red;
            }
        }
    }
}
