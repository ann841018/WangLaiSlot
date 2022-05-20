using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auto_Control : MonoBehaviour
{
    [SerializeField] Text AutoText;//代打文字

    public static int SwitchNum = 0;
    public static int[] AutoNum = new int[11] { 0,10,20,30,50,100,200,300,500,1000,99999};

    int[] AutoNumOri = new int[11] { 0, 10, 20, 30, 50, 100, 200, 300, 500, 1000, 99999 };


    // Use this for initialization
    void Start() {SwitchNum = 0;}

    // Update is called once per frame
    void Update() {

        if (SwitchNum != 10 && SwitchNum != 0) {
            if (SlotTurnSys.AutoPlay == true)//自動打的時候
            {
                if (AutoNum[SwitchNum] == 0) { SwitchNum = 0; }//自動次數結束
            }
            AutoText.text = "代打 " + AutoNum[SwitchNum].ToString();
        }
        else if (SwitchNum == 10) { AutoText.text = "代打 ∞"; }
        else if (SwitchNum == 0) { AutoText.text = "無代打"; }
    }

    public void ChangeAutoNum()
    {
        SwitchNum = SwitchNum + 1;//代打次數切換
        if (SwitchNum >= 11) SwitchNum = 0;
        AutoNum[SwitchNum] = AutoNumOri[SwitchNum];
    }
}
