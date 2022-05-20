using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSys : MonoBehaviour
{
    [SerializeField] GameObject PrizePanel;//大獎
    [SerializeField] GameObject Slot_Panel;
    [SerializeField] GameObject Wild_Panel;
    [SerializeField] GameObject FreeSpin_Panel;
    [SerializeField] GameObject FreeSpin_Btn;

    [SerializeField] Animator MoneyFly;

    [SerializeField] Text GetMoney_text;//贏分的文字
    [SerializeField] Text Win_text;//大獎文字
    [SerializeField] Text Free_text;//免費轉

    [SerializeField] Button Bet_Btn;
    [SerializeField] Button Lobby_Btn;
    
    // Use this for initialization
    void Start(){ Bet_Control.SwitchNum = 0; }

    // Update is called once per frame
    void Update()
    {
        if (SlotTurnSys.CanTurn == true)
        {
            Lobby_Btn.enabled = false;
            GetMoney_text.text = "線數 "+ Bet_Control.Line +" 線注 " + (Bet_Control.BetNum[Bet_Control.SwitchNum]).ToString("#,##0");
            MoneyFly.SetBool("MoneyFly", false);
            if (CookGirl_BonusGame_Control.CanCountDown == false)
            {
                if (PhotonClient.newPhotonClient.iLVNum != 0)
                {
                    FreeSpin_Btn.GetComponent<Button>().enabled = false;
                    Free_text.text = "免費 " + PhotonClient.newPhotonClient.iLVNum.ToString();
                }
                else if (PhotonClient.newPhotonClient.iLVNum == 0) FreeSpin_Btn.SetActive(false);
            }
        }
        else if (SlotTurnSys.CanTurn == false)
        {
            Lobby_Btn.enabled = true;
            FreeSpin_Btn.GetComponent<Button>().enabled = true;
            if (PhotonClient.newPhotonClient.iAllFG != 1)//如果不是關卡
            {
                if (Alice_BonusGame_Control.Bonus_Prize == false)
                {
                    if (PhotonClient.newPhotonClient.Line_Money > 0)
                    {
                        GetMoney_text.text = "恭喜中獎   贏分 " + PhotonClient.newPhotonClient.Line_Money.ToString("#,##0");
                        MoneyFly.SetBool("MoneyFly", true);
                        if (PhotonClient.newPhotonClient.Line_Money / Bet_Control.BetNum[Bet_Control.SwitchNum] < 10) PrizePanel.SetActive(false);
                    }
                    else if (PhotonClient.newPhotonClient.Line_Money == 0)
                    {
                        GetMoney_text.text = "線數 " + Bet_Control.Line + " 線注 " + (Bet_Control.BetNum[Bet_Control.SwitchNum]).ToString("#,##0");
                        MoneyFly.SetBool("MoneyFly", false);
                        PrizePanel.SetActive(false);
                    }
                }
                else if (Alice_BonusGame_Control.Bonus_Prize == true)
                {
                    GetMoney_text.text = "恭喜中獎   贏分 " + PhotonClient.newPhotonClient.Bonus_Money.ToString("#,##0");
                }
            }
            if ((PhotonClient.newPhotonClient.iLVNum == 0)&&(PhotonClient.newPhotonClient.Bonus_Money==0))//如果不是關卡
            {
                Slot_Panel.SetActive(true);
                FreeSpin_Panel.SetActive(false);
                FreeSpin_Btn.SetActive(false);
                Wild_Panel.SetActive(false);
                Bet_Btn.enabled = true;
            }
        }
    }
}
