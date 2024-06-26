using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUi_Control : MonoBehaviour
{
    public GameObject Player;
    float boost_amount;

    // Update is called once per frame
    void Update()
    {
        //���@�����݂���ꍇ
        if (Player != null)
        {
            boost_amount = Player.GetComponent<KMF_Control>().boost_amount / 100;
            gameObject.transform.Find("FillGauge").GetComponent<Image>().fillAmount = boost_amount;
        }
        //�I�[�o�[�q�[�g�̏ꍇ
        if(boost_amount <= 0)
        {
            gameObject.transform.Find("BackGround_Outer").GetComponent<Image>().color = new Color(255, 0, 0);
        }
        else
        {
            gameObject.transform.Find("BackGround_Outer").GetComponent<Image>().color = new Color(255, 255, 255);
        }
    }
}
