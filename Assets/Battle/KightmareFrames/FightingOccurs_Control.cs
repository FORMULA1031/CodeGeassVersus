using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingOccurs_Control : MonoBehaviour
{
    KMF_Control KMF_Control;

    // Start is called before the first frame update
    void Start()
    {
        KMF_Control = transform.root.gameObject.GetComponent<KMF_Control>();
    }

    //���̃I�u�W�F�N�g�ƐڐG�����ꍇ
    private void OnTriggerStay(Collider other)
    {
        //�G�ƐڐG�����ꍇ�i��1�Ŗڂ��J�n����
        if (other.gameObject.CompareTag("Player") && KMF_Control != null)
        {
            KMF_Control.StartAttack();
        }
    }
}
