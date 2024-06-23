using UnityEngine;

public class BigExplosion_Control : MonoBehaviour
{
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 1)
        {
            Destroy(gameObject);
        }
    }
}
