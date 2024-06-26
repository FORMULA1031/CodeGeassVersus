using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurableValueUi_Control : MonoBehaviour
{
    public GameObject Player;
    int durable_value = 0;
    int durable_maxvalue = 0;

    private void FixedUpdate()
    {
        //���@�����݂���ꍇ
        if (Player != null)
        {
            durable_value = Player.GetComponent<KMF_Control>().durable_value;
            durable_maxvalue = Player.GetComponent<KMF_Control>().durable_maxvalue;
            gameObject.GetComponent<Text>().text = durable_value + "";
            ChangeColor();
        }
    }

    //�ϋv�l�pUI�̐F����
    void ChangeColor()
    {
        //2/3���傫���ꍇ��
        if (durable_value > (float)durable_maxvalue * (2.0f / 3.0f))
        {
            transform.GetComponent<Text>().color = new Color(1, 1, 1);
        }
        //1/3���傫���ꍇ��
        else if (durable_value <= durable_maxvalue * (2.0f / 3.0f) && durable_value > (float)durable_maxvalue * (1.0f / 3.0f))
        {
            transform.GetComponent<Text>().color = new Color(1, 1, 0);
        }
        //1/3�ȉ��̏ꍇ��
        if (durable_value <= (float)durable_maxvalue * (1.0f / 3.0f))
        {
            transform.GetComponent<Text>().color = new Color(1, 0, 0);
        }
    }
}
