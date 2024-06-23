using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    float red, green, blue, alfa;

    public bool Out = false;
    public bool In = false;

    public string scenename;

    Image fadeImage;                //ƒpƒlƒ‹

    void Start()
    {
        transform.GetComponent<Image>().enabled = true;
        fadeImage = GetComponent<Image>();
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    void Update()
    {
        if (In)
        {
            FadeIn();
        }

        if (Out)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        alfa -= Time.deltaTime;
        Alpha();
        if (alfa <= 0)
        {
            In = false;
        }
    }

    void FadeOut()
    {
        fadeImage.enabled = true;
        alfa += Time.deltaTime;
        Alpha();
        if (alfa >= 1)
        {
            SceneManager.LoadScene(scenename);
        }
    }

    void Alpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}