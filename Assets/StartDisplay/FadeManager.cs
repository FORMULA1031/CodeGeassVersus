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

    Image fadeImage;                //パネル

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
        //フェードイン
        if (In)
        {
            FadeIn();
        }

        //フェードアウト
        if (Out)
        {
            FadeOut();
        }
    }

    //フェードイン
    void FadeIn()
    {
        alfa -= Time.deltaTime;
        Alpha();
        if (alfa <= 0)
        {
            In = false;
        }
    }
    
    //フェードアウト
    void FadeOut()
    {
        fadeImage.enabled = true;
        alfa += Time.deltaTime;
        Alpha();
        //シーン遷移
        if (alfa >= 1)
        {
            SceneManager.LoadScene(scenename);
        }
    }

    //色の調整
    void Alpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}