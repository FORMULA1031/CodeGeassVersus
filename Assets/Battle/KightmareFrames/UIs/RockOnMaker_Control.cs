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
        if (target != null)
        {
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
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<Cpu_Control>().durable_value / (float)target.GetComponent<Cpu_Control>().durable_maxvalue;
            }
            else
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<KMF_Control>().durable_value / (float)target.GetComponent<KMF_Control>().durable_maxvalue;
            }
            RockOnMaker_Color();
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            if (GameObject.Find("Lancelot_Enemy(Clone)") != null)
            {
                target = GameObject.Find("Lancelot_Enemy(Clone)");
            }
        }
    }

    void RockOnMaker_Color()
    {
        if (target.GetComponent<Cpu_Control>() != null)
        {
            if (target.GetComponent<Cpu_Control>().down_flag)
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Yellow;
            }
            else
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Red;
            }
        }
        else if (target.GetComponent<KMF_Control>() != null)
        {
            if (target.GetComponent<KMF_Control>().down_flag)
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
