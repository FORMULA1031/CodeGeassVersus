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
        worldAngle.y -= 180.0f; // ワールド座標を基準に、y軸を軸にした回転を10度に変更
        worldAngle.z += 90.0f; // ワールド座標を基準に、z軸を軸にした回転を10度に変更
        transform.eulerAngles = worldAngle; // 回転角度を設定
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
