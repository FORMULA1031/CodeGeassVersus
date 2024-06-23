using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam_Control : MonoBehaviour
{
    Rigidbody rb;
    public float power;
    GameObject LockOnEnemy;
    bool direction_flag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (direction_flag)
        {
            gameObject.transform.LookAt(LockOnEnemy.transform);
            rb.AddForce(gameObject.transform.forward * power);
            direction_flag = false;
        }
    }

    public void LockOnEnemySetting(GameObject _LockOnEnemy)
    {
        LockOnEnemy = _LockOnEnemy;
        if (LockOnEnemy != null)
        {
            direction_flag = true;
        }
        else
        {
            rb.AddForce(gameObject.transform.forward * power);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
