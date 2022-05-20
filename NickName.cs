using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickName : MonoBehaviour
{
    [SerializeField] Button Change_Btn;//更換暱稱按鈕
    [SerializeField] Text Change_Taxt;//暱稱錯誤
    [SerializeField] Text NickName_Text;//暱稱UI
    [SerializeField] Text Wrong_Hint_Taxt;//暱稱錯誤
    [SerializeField] GameObject Creat_Panel;//創建帳號頁面
    [SerializeField] GameObject Loading_Panel;//讀取頁面
    [SerializeField] GameObject WrongNickName_Panel;//暱稱錯誤頁面

    public static bool WrongNickName;
    public static bool OK;
    float time;
    string NickName_Str;//暱稱名

    void FixedUpdate()
    {
        if (WrongNickName == true) { RepeatNickName(); }
        else if (WrongNickName == false)
        {
            if (PlayerPrefs.GetInt("SceneNum") == 0)
            {
                if (PhotonClient.newPhotonClient.getNickname != "NewUser0001" && PhotonClient.newPhotonClient.getNickname != "")
                {
                    if (OK == true) { Loading_Panel.SetActive(true); PlayerPrefs.SetInt("haveID", 1); }
                    else if (OK == false) { PlayerPrefs.SetInt("haveID", 0); NicknameLong();}
                }else if (PhotonClient.newPhotonClient.getNickname == "NewUser0001") { RepeatNickName(); }
            }else if (PlayerPrefs.GetInt("SceneNum") != 0&& PlayerPrefs.GetInt("SceneNum") != 2)
            {
                if (PlayerPrefs.GetInt("ChangeNickName") == 1)//暱稱只能更換一次
                {
                    Change_Btn.enabled = false;
                    Change_Taxt.text = "已更換";
                }
            }
        }
    }
    public void NicknameLong()
    {
        NickName_Str = NickName_Text.text.ToString();
        string[] unable = { " ", ",", ".", "/", "<", ">", "?", ";", ":", "[", "]", "{", "}", "|", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "`", "~" };
        int byte_len = System.Text.Encoding.Default.GetBytes(NickName_Str).Length;//中文3單位英數1單位
        int len = NickName_Str.Length;//中文1單位英數1單位
        if (byte_len != len) len = (byte_len + len) / 2;

        if (len > 12 || len < 4)//字數錯誤
        {
            Creat_Panel.SetActive(false);
            WrongNickName_Panel.SetActive(true);
            Wrong_Hint_Taxt.text = ("字數不合規定\n中文2-6個字，英數半形4-12個字");
        }else {
            for (int i = 0; i < 30; i++)
            {
                if (NickName_Str.Contains(unable[i]))//暱稱包含無效符號
                {
                    Creat_Panel.SetActive(false);
                    WrongNickName_Panel.SetActive(true);
                    if (i == 0) Wrong_Hint_Taxt.text = ("暱稱內容不合規定\n符號 空格 不可使用");
                    else Wrong_Hint_Taxt.text = ("暱稱內容不合規定\n符號 " + unable[i] + " 不可使用");
                    break;
                }
                else
                {
                    if (i == 29)//暱稱可以用
                    {
                        if (PlayerPrefs.GetInt("SceneNum") == 0)
                        {
                            if (WrongNickName == false)
                            {
                                PhotonClient.newPhotonClient.SetUserNickName(NickName_Str);
                                OK = true;
                            }
                        }
                        else
                        {
                            if (PlayerPrefs.GetInt("ChangeNickName") == 0)
                            {
                                PhotonClient.newPhotonClient.SetUserNickName(NickName_Str);
                                if (NickName_Str == NickName_Text.text.ToString()) PlayerPrefs.SetInt("ChangeNickName", 1);
                            }
                        }
                        Creat_Panel.SetActive(false);
                    }
                }
            }
        }
    }

    public static void MistakeNickName() { WrongNickName = true; OK = false; }
    public void RepeatNickName()
    {
        if (WrongNickName == true)
        {
            if (PlayerPrefs.GetInt("SceneNum") == 0 ) { Loading_Panel.SetActive(false); }
            Creat_Panel.SetActive(false);
            WrongNickName_Panel.SetActive(true);
            Wrong_Hint_Taxt.text = ("暱稱重複");
            WrongNickName = false;
        }
        if (PhotonClient.newPhotonClient.getNickname == NickName_Str) { WrongNickName_Panel.SetActive(false); }
    }
}

