using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceSize_Control : MonoBehaviour
{
    float size = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        size += Time.deltaTime * 2f;
        if(size > 1)
        {
            size = 1;
        }
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(size, size, size);
    }
}
