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
        if (Player != null)
        {
            BulletNumberTextControl();
            BulletNumberGaugeControl();
            ChargeCaugeControl();
        }
    }

    void BulletNumberTextControl()
    {
        ArmedUi[0].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().mainshooting_number + "";
        ArmedUi[1].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().specialshooting_number + "";
        ArmedUi[2].transform.Find("BulletNumberText").GetComponent<Text>().text = Player.GetComponent<KMF_Control>().subshooting_number + "";
        if (!Player.GetComponent<KMF_Control>().isFightingChargeInput && !Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text =
                (int)((Player.GetComponent<KMF_Control>().fightingChargeCurrentReloadTime / Player.GetComponent<KMF_Control>().fightingChargeReloadTime) * 100) + "";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 0);
        }
        else if (Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text =
                (int)((Player.GetComponent<KMF_Control>().floatUnitCurrentTime / Player.GetComponent<KMF_Control>().floatUnitTime) * 100) + "";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 1);
        }
        else if (Player.GetComponent<KMF_Control>().isFightingChargeInput)
        {
            ArmedUi[3].transform.Find("BulletNumberText").GetComponent<Text>().text = "100";
            TextColors(ArmedUi[3].transform.Find("BulletNumberText").gameObject, 1);
        }
        TextColors(ArmedUi[0].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().mainshooting_number);
        TextColors(ArmedUi[1].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().specialshooting_number);
        TextColors(ArmedUi[2].transform.Find("BulletNumberText").gameObject, Player.GetComponent<KMF_Control>().subshooting_number);
    }

    void TextColors(GameObject Ui, int number)
    {
        if (number == 0)
        {
            Ui.GetComponent<Text>().color = new Color(255, 0, 0);
        }
        else
        {
            Ui.GetComponent<Text>().color = new Color(255, 255, 255);
        }
    }

    void BulletNumberGaugeControl()
    {
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
        if (!Player.GetComponent<KMF_Control>().isFightingChargeInput && !Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().fightingChargeCurrentReloadTime / Player.GetComponent<KMF_Control>().fightingChargeReloadTime;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 0);
        }
        else if (Player.GetComponent<KMF_Control>().isFloatUnit)
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount =
                Player.GetComponent<KMF_Control>().floatUnitCurrentTime / Player.GetComponent<KMF_Control>().floatUnitTime;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 1);
        }
        else
        {
            ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").GetComponent<Image>().fillAmount = 1;
            ImageColors(ArmedUi[3].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, 1);
        }
        ImageColors(ArmedUi[0].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().mainshooting_number);
        ImageColors(ArmedUi[1].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().specialshooting_number);
        ImageColors(ArmedUi[2].transform.Find("BulletNumberGauge_Outer/FillGauge").gameObject, Player.GetComponent<KMF_Control>().subshooting_number);
    }

    void ImageColors(GameObject Ui, int number)
    {
        if (number == 0)
        {
            Ui.GetComponent<Image>().color = new Color(255, 0, 0);
        }
        else
        {
            Ui.GetComponent<Image>().color = new Color(0, 200, 200);
        }
    }

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
