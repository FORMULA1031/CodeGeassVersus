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
        //���b�N�I���Ώۂ����݂���ꍇ
        if (target != null)
        {
            //���b�N�I���}�[�J�[�\��
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
            //���b�N�I���Ώۂ̑ϋv�l��\��(�I�t���C��)
            if (SceneManager.GetActiveScene().name == "TrainingScene")
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<Cpu_Control>().durable_value / (float)target.GetComponent<Cpu_Control>().durable_maxvalue;
            }
            //���b�N�I���Ώۂ̑ϋv�l��\��(�I�����C��)
            else
            {
                EnemyDurableBar.fillAmount = (float)target.GetComponent<KMF_Control>().durable_value / (float)target.GetComponent<KMF_Control>().durable_maxvalue;
            }
            RockOnMaker_Color();
        }
        else
        {
            //���b�N�I���}�[�J�[��\��
            gameObject.GetComponent<Image>().enabled = false;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            //���b�N�I���Ώۂ�T��(�I�t���C��)
            if (GameObject.Find("Lancelot_Enemy(Clone)") != null)
            {
                target = GameObject.Find("Lancelot_Enemy(Clone)");
            }
        }
    }

    //���b�N�I���}�[�J�[�̐F����
    void RockOnMaker_Color()
    {
        //�I�t���C��
        if (target.GetComponent<Cpu_Control>() != null)
        {
            //�G�̃_�E����
            if (target.GetComponent<Cpu_Control>().isDown)
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Yellow;
            }
            else
            {
                transform.GetComponent<Image>().sprite = RockOnMaker_Red;
            }
        }
        //�I�����C��
        else if (target.GetComponent<KMF_Control>() != null)
        {
            //�G�̃_�E����
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
