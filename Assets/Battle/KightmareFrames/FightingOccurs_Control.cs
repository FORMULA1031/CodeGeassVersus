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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && KMF_Control != null)
        {
            KMF_Control.StartAttack();
        }
    }
}
