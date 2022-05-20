using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Mail_Control : MonoBehaviour
{
    #region//宣告
    //寄件資訊
    string NickName_Str = "";//暱稱
    string Title_Str = "";//標題文字
    string Info_Str = "";//內容文字
    int[] Max_Mail = new int[8] { 0, 5,10,15,25,50,100,100 };//最大交易金額
    int[] Max_Money = new int[8] { 5000, 50000, 100000, 500000, 1000000, 2000000, 2000000, 2000000 };//最大交易金額
    int switch_Money;//切換金額=Max/10
    bool getReturn;
    public int TotalSendMail;
    public int TotalSendMoney;

    [SerializeField] Text Nickname;
    [SerializeField] Text Title;
    [SerializeField] Text Info;
    [SerializeField] Text Money;
    [SerializeField] Text HintMail;
    [SerializeField] Text HintMoney;

    //收件資訊
    [SerializeField] Text Get_NickName_Str;//暱稱
    [SerializeField] Text Get_Title_Str;//標題文字
    [SerializeField] Text Get_Info_Str;//內容文字
    [SerializeField] Text Get_Money_Str;//錢文字
    [SerializeField] Text Get_Show_Money_Str;//錢文字
    [SerializeField] GameObject Get_Button;//領取按鈕

    #endregion

    void Start() {}// Start is called before the first frame update
    void Update(){ if (getReturn == true) {
            if(PhotonClient.newPhotonClient.SendSuccessCode==62|| PhotonClient.newPhotonClient.SendSuccessCode == 66)
            SendSuccess(); }  }// Update is called once per frame
    public void OpenPanel()//打開信箱頁面會讀取的
    {
        switch_Money = Max_Money[PhotonClient.newPhotonClient.iVIPLv] / 10;
        Money.text = switch_Money.ToString("#,##0");
        HintMail.text = TotalSendMail.ToString("#,##0") + " / " + Max_Mail[PhotonClient.newPhotonClient.iVIPLv].ToString("#,##0");
        HintMoney.text = TotalSendMoney.ToString("#,##0") + " / " + Max_Money[PhotonClient.newPhotonClient.iVIPLv].ToString("#,##0");
        if (TotalSendMail >= Max_Mail[PhotonClient.newPhotonClient.iVIPLv]) HintMail.color = new Color(1, 0.5f, 0);
        else HintMail.color = new Color(1, 1, 1);
        if (TotalSendMoney >= Max_Money[PhotonClient.newPhotonClient.iVIPLv]) HintMoney.color = new Color(1, 0.5f, 0);
        else HintMail.color = new Color(1, 1, 1);
        PhotonClient.newPhotonClient.CheckMail();
    }

    #region//這邊都是寫信的功能
    public void InputNickName() {NickName_Str = Nickname.text.ToString(); }//輸入暱稱
    public void InputTitle() { Title_Str = Title.text.ToString(); }//輸入標題
    public void InputInfo(){Info_Str = Info.text.ToString();}//輸入內容
    public void Plus()//加錢
    {
        //if (switch_Money > PhotonClient.newPhotonClient.getUserMoney)
        {
            switch_Money = switch_Money + (Max_Money[PhotonClient.newPhotonClient.iVIPLv] / 10);
            if (switch_Money > Max_Money[PhotonClient.newPhotonClient.iVIPLv]) switch_Money = Max_Money[PhotonClient.newPhotonClient.iVIPLv];
            Money.text = switch_Money.ToString("#,##0");
        }
    }
    public void Miner()//減錢
    {
        switch_Money = switch_Money - (Max_Money[PhotonClient.newPhotonClient.iVIPLv] / 10);
        if (switch_Money < 0) switch_Money = 0;
        Money.text = switch_Money.ToString("#,##0");
    }
    public void Send()//寄信
    {
        if (NickName_Str != "" && Title_Str != "") {
            int Nick_len = NickName_Str.Length;
            int Title_len = Title_Str.Length;
            int Info_len = Info_Str.Length;

            int Nick_byte_len = System.Text.Encoding.Default.GetBytes(NickName_Str).Length;
            int Title_byte_len = System.Text.Encoding.Default.GetBytes(Title_Str).Length;
            int Info_byte_len = System.Text.Encoding.Default.GetBytes(Info_Str).Length;

            if (Nick_len != Nick_byte_len) Nick_len = (Nick_len + Nick_byte_len) / 2;
            if (Title_len != Title_byte_len) Title_len = (Title_len + Title_byte_len) / 2;
            if (Info_len != Info_byte_len) Info_len = (Info_len + Info_byte_len) / 2;

            if (Nick_len > 12 || Nick_len < 4) { Get_Show_Money_Str.text = ("收件人暱稱字數不合規定\n中文2-6個字，英數半形4-12個字"); }
            else if (Title_len > 12) { Get_Show_Money_Str.text = ("信件標題字數不合規定\n中文最多6個字"); }
            else if (Info_len > 100) { Get_Show_Money_Str.text = ("信件內容字數不合規定\n中文最多50個字"); }
            else {
                PhotonClient.newPhotonClient.SendMail(switch_Money, NickName_Str, Title_Str, Info_Str);
                getReturn = true;
            }
        }
        else 
        {
            if (NickName_Str == "") Get_Show_Money_Str.text = "收件人暱稱不能為空白";
            else if (Title_Str == "") Get_Show_Money_Str.text = "信件標題不能為空白";
        }

    }
    public void SendSuccess()//寄件成功
    {
        if (PhotonClient.newPhotonClient.SendSuccess == true)
        {
            Get_Show_Money_Str.text = "寄件成功";
            PhotonClient.newPhotonClient.CheckMail();
        }
        if (PhotonClient.newPhotonClient.SendSuccess == false) {
            Get_Show_Money_Str.text = "寄件失敗";
        }
        PhotonClient.newPhotonClient.SendSuccessCode = 0;

       getReturn = false;
    }
    #endregion

    public void Check(){PhotonClient.newPhotonClient.CheckMail();Get(); }//重收信箱
    public void Get()//收信
    {
        if (PhotonClient.newPhotonClient.Mail_Num == 0){//沒信    
            Get_NickName_Str.text = "寄件人暱稱：";
            Get_Title_Str.text = "信件標題：";
            Get_Info_Str.text = "目前沒有信件";
            Get_Money_Str.gameObject.SetActive(false);
            Get_Button.gameObject.SetActive(true);
            Get_Button.GetComponent<Button>().enabled = false;
        } else//有信
        {
            Get_NickName_Str.text = "寄件人暱稱：" + PhotonClient.newPhotonClient.Mail_Name;
            Get_Title_Str.text = "信件標題：" + PhotonClient.newPhotonClient.Mail_Title;
            Get_Info_Str.text = PhotonClient.newPhotonClient.Mail_Info;
            Get_Button.GetComponent<Button>().enabled = true;
            if (PhotonClient.newPhotonClient.Mail_Money == 0)//信沒錢
            {
                Get_Button.gameObject.SetActive(false);
                Get_Money_Str.gameObject.SetActive(false);
                Get_Show_Money_Str.text = "信件已刪除 ";
            }
            else//信有錢
            {
                Get_Button.gameObject.SetActive(true);
                Get_Money_Str.gameObject.SetActive(true);
                Get_Money_Str.text = "$" + PhotonClient.newPhotonClient.Mail_Money.ToString("#,##0");
                Get_Show_Money_Str.text = "已領取 $" + PhotonClient.newPhotonClient.Mail_Money.ToString("#,##0");
               
            }
        }
    }

    public void Delete()//刪除
    {
        PhotonClient.newPhotonClient.GetMail(PhotonClient.newPhotonClient.Mail_ID);
        PhotonClient.newPhotonClient.CheckMail();
    }
}
