using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin_Control : MonoBehaviour
{
    [SerializeField] GameObject StopBtn;//停止按鈕
    [SerializeField] GameObject DontHaveMoney;//停止按鈕
    [SerializeField] AudioSource Bet_Sound;//停止按鈕

    float time;
    public static bool StopAuto;//停止自動

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SlotTurnSys.AutoPlay == true)//開自動
        {
            if (PrizeSys.PrizeNum == -1)
            {
                if (time >= 1.5f)
                {
                    Bet(); PhotonClient.newPhotonClient.iBet = Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line;//持續下注
                    Auto_Control.AutoNum[Auto_Control.SwitchNum] = Auto_Control.AutoNum[Auto_Control.SwitchNum] - 1;
                    time = 0;
                }
            }
            if (SlotTurnSys.CanTurn == true)//如果正在轉
            {
                StopBtn.SetActive(true);//顯示停止按鈕
            }
            else if (SlotTurnSys.CanTurn == false)//沒再轉的時候
            {
                time = time + Time.deltaTime;
                if (Auto_Control.AutoNum[Auto_Control.SwitchNum] <= 0)//如果次數用完
                {
                    StopBtn.SetActive(false);
                    SlotTurnSys.AutoPlay = false;
                    Auto_Control.SwitchNum = 0;
                }
                if (StopAuto == true)//有按過停止鍵
                {
                    Auto_Control.SwitchNum = 0;
                    SlotTurnSys.AutoPlay = false;
                    StopBtn.SetActive(false);//停止按鈕隱藏
                    StopAuto = false;
                }
            }
        }
        else if (SlotTurnSys.AutoPlay == false)
        {
            if (SlotTurnSys.CanTurn == true)//如果正在轉
            {
                StopBtn.SetActive(true);//顯示停止按鈕
            }
            else if (SlotTurnSys.CanTurn == false)//沒再轉的時候
            {
                StopBtn.SetActive(false);//停止按鈕隱藏
            }
        }
    }

    public void Bet()
    {
        if (PhotonClient.newPhotonClient.ServerConnected)
        {
            if (PhotonClient.newPhotonClient.getUserMoney >= Bet_Control.BetNum[Bet_Control.SwitchNum]* Bet_Control.Line)
            {
                Bet_Sound.Play(); PhotonClient.newPhotonClient.iBet = Bet_Control.BetNum[Bet_Control.SwitchNum] * Bet_Control.Line;
                PhotonClient.newPhotonClient.SendBet(PlayerPrefs.GetInt("SceneNum"));
                if ((PlayerPrefs.GetInt("SceneNum") != 5)&& (PlayerPrefs.GetInt("SceneNum") != 6)) SlotTurnSys.StartTurn();//Wild一輪5
                if ((PlayerPrefs.GetInt("SceneNum") == 5)|| (PlayerPrefs.GetInt("SceneNum") == 6)) SlotTurnSysS.StartTurn();//Wild兩輪45

                SlotTurnSys.ChangeFakeSlotImage = true;
                if (Auto_Control.AutoNum[Auto_Control.SwitchNum] != 0)
                {
                    if (SlotTurnSys.AutoPlay == false) { Auto_Control.AutoNum[Auto_Control.SwitchNum] = Auto_Control.AutoNum[Auto_Control.SwitchNum] - 1; }
                    SlotTurnSys.AutoPlay = true;
                }                
            }
            else
            {
                DontHaveMoney.SetActive(true);
                PhotonClient.newPhotonClient.isBetSent = false;
                SlotTurnSys.AutoPlay = false;
            }
        }
    }
    public void Stop() { StopAuto = true; }
}