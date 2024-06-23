using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarPotentialGauge_Control : MonoBehaviour
{
    Image[] Team1_Gauges = new Image[6];
    Image[] Team2_Gauges = new Image[6];
    BattleGame_Control BattleGame_Control;

    // Start is called before the first frame update
    void Start()
    {
        BattleGame_Control = GameObject.Find("EventSystem").GetComponent<BattleGame_Control>();
    }

    private void FixedUpdate()
    {
        
    }
}
