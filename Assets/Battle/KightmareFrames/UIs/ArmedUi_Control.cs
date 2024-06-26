using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmedUi_Control : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] ArmedUi;

    // Update is called once per frame
    void Update()
    {
        //自機が存在する場合
        if (Player != null)
        {
            BulletNumberTextControl();
            BulletNumberGaugeControl();
            ChargeCaugeControl();
        }
    }

    //弾数を表示するテキストの制御
    void BulletNumberTextControl()
    {
        //弾数の数字を更新
        ArmedUi[0].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().mainshooting_number + "";
        ArmedUi[1].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().specialshooting_number + "";
        ArmedUi[2].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().subshooting_number + "";
        //格闘CSリロード中
        if (!Player.GetComponent<KMF_Control>().isFightingChargeInput && !Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text =
                (int)((Player.GetComponent<KMF_Control>().fightingChargeCurrentReloadTime / Player.GetComponent<KMF_Control>().fightingChargeReloadTime) * 100) + "";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 0);
        }
        //格闘CS発動中
        else if (Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text =
                (int)((Player.GetComponent<KMF_Control>().floatUnitCurrentTime / Player.GetComponent<KMF_Control>().floatUnitTime) * 100) + "";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 1);
        }
        //格闘CSチャージ中
        else if (Player.GetComponent<KMF_Control>().isFightingChargeInput)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text = "100";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 1);
        }
        TextColors(ArmedUi[0].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().mainshooting_number);
        TextColors(ArmedUi[1].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().specialshooting_number);
        TextColors(ArmedUi[2].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().subshooting_number);
    }

    //弾数テキストの色の制御
    void TextColors(GameObject Ui, int number)
    {
        //弾切れの場合
        if (number == 0)
        {
            Ui.GetComponent<Text>().color = new Color(255, 0, 0);
        }
        //弾が残っている場合
        else
        {
            Ui.GetComponent<Text>().color = new Color(255, 255, 255);
        }
    }

    //弾数用ゲージの制御
    void BulletNumberGaugeControl()
    {
        //メイン射撃の弾数が残っている場合
        if (Player.GetComponent<KMF_Control>().mainshooting_number > 0)
        {
            ArmedUi[0].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().mainshooting_number / (float)Player.GetComponent<KMF_Control>().mainshooting_maxnumber;
        }
        else
        {
            ArmedUi[0].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().mainShootingCurrentReloadTime / Player.GetComponent<KMF_Control>().mainShootingReloadTime;
        }
        //特殊射撃の弾数が残っている場合
        if (Player.GetComponent<KMF_Control>().specialshooting_number > 0)
        {
            ArmedUi[1].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().specialshooting_number / (float)Player.GetComponent<KMF_Control>().specialshooting_maxnumber;
        }
        else
        {
            ArmedUi[1].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().specialShootingCurrentReloadTime / Player.GetComponent<KMF_Control>().specialShootingReloadTime;
        }
        //サブ射撃の弾数が残っている場合
        if (Player.GetComponent<KMF_Control>().subshooting_number > 0)
        {
            ArmedUi[2].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().subshooting_number / (float)Player.GetComponent<KMF_Control>().subshooting_maxnumber;
        }
        else
        {
            ArmedUi[2].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().subShootingCurrentReloadTime / Player.GetComponent<KMF_Control>().subShootingReloadTime;
        }
        //格闘CSリロード中
        if (!Player.GetComponent<KMF_Control>().isFightingChargeInput && !Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().fightingChargeCurrentReloadTime / Player.GetComponent<KMF_Control>().fightingChargeReloadTime;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 0);
        }
        //格闘CS発動中
        else if (Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().floatUnitCurrentTime / Player.GetComponent<KMF_Control>().floatUnitTime;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 1);
        }
        //格闘CSチャージ中
        else
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount = 1;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 1);
        }
        ImageColors(ArmedUi[0].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().mainshooting_number);
        ImageColors(ArmedUi[1].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().specialshooting_number);
        ImageColors(ArmedUi[2].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().subshooting_number);
    }

    //武器の画像の色の制御
    void ImageColors(GameObject Ui, int number)
    {
        //弾数が残っていない場合
        if (number == 0)
        {
            Ui.GetComponent<Image>().color = new Color(255, 0, 0);
        }
        //弾数が残っている場合
        else
        {
            Ui.GetComponent<Image>().color = new Color(0, 200, 200);
        }
    }

    //チャージ制御
    void ChargeCaugeControl()
    {
        if (Player.GetComponent<KMF_Control>().isFightingChargeInput)
        {
            ArmedUi[3].transform.Find("ChargeGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().fightingChargeTime / Player.GetComponent<KMF_Control>().fightingChargeMaxTime;
        }
        else
        {
            ArmedUi[3].transform.Find("ChargeGauge_Outer/FillGauge").GetComponent<Image>().fillAmount = 0;
        }
    }
}
