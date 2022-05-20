using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lv_Control : MonoBehaviour
{
    [SerializeField] GameObject DontHaveMoney;//金幣不足
    //[SerializeField] GameObject ExpBarOnPlayerPanel;//
    [SerializeField] GameObject LvUpAnim;//升等動畫
    [SerializeField] GameObject[] LvUp_Num = new GameObject[2];//升等數字
    [SerializeField] Image[] LvUp_Num_Sprite = new Image[3];//升等數字

    [SerializeField] Text LvTextOnPlayerPanel;//玩家設定上的文字
    [SerializeField] Text LvText;//大廳上的文字
    [SerializeField] Sprite[] Lv_Num = new Sprite[10];

    public static bool LevelUp;

    // Use this for initialization
    void Start()
    {
        //ExpBarOnPlayerPanel.transform.localPosition = new Vector3(-250, 0, 0);//進度條原始位置
    }

    // Update is called once per frame
    void Update()
    {
        //ExpBarOnPlayerPanel.transform.localPosition = new Vector3(250 * PhotonClient.newPhotonClient.iEXP / needExp - 250, 0, 0);//進度條位置
        LvText.text = PhotonClient.newPhotonClient.iLV.ToString();//主畫面上的Lv
        LvTextOnPlayerPanel.text = "Lv "+PhotonClient.newPhotonClient.iLV.ToString();//PlayerPanel上的Lv
        if (SlotTurnSys.CanTurn == false)
        {
            if (LevelUp == true)
            {
                //PhotonClient.newPhotonClient.iLV = PhotonClient.newPhotonClient.iLV + 1;//升等
                LvUpAnim.SetActive(true);//升等動畫
                if (PhotonClient.newPhotonClient.iLV < 10)
                {
                    LvUp_Num[0].SetActive(false);
                    LvUp_Num[1].SetActive(false);
                    LvUp_Num[2].SetActive(true);
                    LvUp_Num_Sprite[2].sprite = Lv_Num[PhotonClient.newPhotonClient.iLV];//升等數字
                }
                else if (PhotonClient.newPhotonClient.iLV >= 10 && PhotonClient.newPhotonClient.iLV < 100)
                {
                    LvUp_Num[0].SetActive(false);
                    LvUp_Num[1].SetActive(true);
                    LvUp_Num[2].SetActive(true);
                    LvUp_Num_Sprite[1].sprite = Lv_Num[(int)PhotonClient.newPhotonClient.iLV / 10];//升等數字
                    LvUp_Num_Sprite[2].sprite = Lv_Num[(int)PhotonClient.newPhotonClient.iLV % 10];//升等數字
                }
                else if (PhotonClient.newPhotonClient.iLV >= 100)
                {
                    LvUp_Num[0].SetActive(true);
                    LvUp_Num[1].SetActive(true);
                    LvUp_Num[2].SetActive(true);
                    LvUp_Num_Sprite[0].sprite = Lv_Num[(int)PhotonClient.newPhotonClient.iLV / 100];//升等數字
                    LvUp_Num_Sprite[1].sprite = Lv_Num[(int)PhotonClient.newPhotonClient.iLV % 100 / 10];//升等數字
                    LvUp_Num_Sprite[2].sprite = Lv_Num[(int)PhotonClient.newPhotonClient.iLV % 100 % 10];//升等數字
                }
                LevelUp = false;
            }
        }
        else if (SlotTurnSys.CanTurn == true)
        {
            LvUpAnim.SetActive(false);
        }
    }
}