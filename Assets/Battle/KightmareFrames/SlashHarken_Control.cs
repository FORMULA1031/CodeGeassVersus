using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHarken_Control : MonoBehaviour
{
    Rigidbody rb;
    float power = 3000;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 worldAngle = transform.eulerAngles;
        worldAngle.y -= 180.0f; // ���[���h���W����ɁAy�������ɂ�����]��10�x�ɕύX
        worldAngle.z += 90.0f; // ���[���h���W����ɁAz�������ɂ�����]��10�x�ɕύX
        transform.eulerAngles = worldAngle; // ��]�p�x��ݒ�
        rb = GetComponent<Rigidbody>();
        rb.AddForce(gameObject.transform.up * -power);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
