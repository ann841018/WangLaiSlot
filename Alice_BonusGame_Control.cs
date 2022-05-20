using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alice_BonusGame_Control : MonoBehaviour
{
    [SerializeField] GameObject Info_Panel;
    [SerializeField] GameObject[] Basket = new GameObject[12];
    [SerializeField] GameObject[] Animal_Pic = new GameObject[12];
    [SerializeField] GameObject[] Circle_All = new GameObject[12];
    [SerializeField] GameObject FreeSpin_Square;
    [SerializeField] GameObject Spin_Btn;
    public static bool Bonus_Prize;
    bool Open_Panel = true;
    float time = 1;
    int Result_Count;
    int Result_Index;
    int[] Result = new int[9];
    int[] Result_Num = new int[4] { 0, 0, 0, 0 };
    int[] Circle_Count = new int[4] { 0, 0, 0, 0 };

    void Start() { }// Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Open_Panel == true)
        {
            Info_Panel.SetActive(true);
            Spin_Btn.GetComponent<Button>().enabled = false;
            time = 1;
            for (int Num = 0; Num < 9; Num++)
            {
                Result[Num] = Random.Range(0, 4);//隨機出         
                Result_Num[Result[Num]] = Result_Num[Result[Num]] + 1;//記數
                if (Result[Num] != PhotonClient.newPhotonClient.BonusNum)
                {
                    if (Result_Num[Result[Num]] >= 3)
                    {
                        Result[Num] = PhotonClient.newPhotonClient.BonusNum;//超過就改結果
                        Result_Num[PhotonClient.newPhotonClient.BonusNum] = Result_Num[PhotonClient.newPhotonClient.BonusNum] + 1;
                    }
                }
                if (Result_Num[PhotonClient.newPhotonClient.BonusNum] == 3)//中3個
                {
                    Result_Index = Num + 1;// print("Index" + Result_Index);//要翻幾個
                    Result_Num[PhotonClient.newPhotonClient.BonusNum] = 10;
                }
                //print("BonusNum" + PhotonClient.newPhotonClient.BonusNum);//最終結果
                //print("Result" + Num + "=" + Result[Num]);//全部9個結果
            }
            Open_Panel = false;
        }
        if (time > -1) time = time - Time.deltaTime; if (time <= 0) { Info_Panel.SetActive(false); }//說明頁面
    }

    public void OpenBasket(int Basket_Num)
    {
        {
            int i = 0;
            while (i <= Result[Result_Count]) { if (i == Result[Result_Count]) { Circle_All[i * 3 + Circle_Count[i]].gameObject.SetActive(true); } i++; }//圈圈圖示
            Circle_Count[Result[Result_Count]] = Circle_Count[Result[Result_Count]] + 1;
        }
        Animal_Pic[Basket_Num].GetComponent<Animator>().SetInteger("Basket", Result[Result_Count]);
        Result_Count = Result_Count + 1;
        if (Result_Count == Result_Index) { Bonus_Prize = true; print("="); }//大獎頁面
    }

    public void ClosePanel()
    {
        for (int i = 0; i < 12; i++)//重置
        {
            Animal_Pic[i].GetComponent<Animator>().SetInteger("Basket", -1);
            Animal_Pic[i].gameObject.SetActive(false);
            Circle_All[i].gameObject.SetActive(false);
            Basket[i].GetComponent<Animator>().SetBool("Open", false);
            Basket[i].GetComponent<Button>().enabled = true;
            Basket[i].GetComponent<Gift_AnimationControl>().enabled = false;
        }
        PrizeSys.time = 0;
        Result_Num = new int[4] { 0, 0, 0, 0 };
        Circle_Count = new int[4] { 0, 0, 0, 0 };
        FreeSpin_Square.SetActive(false);
        Spin_Btn.GetComponent<Button>().enabled = true;
        Result_Count = 0;
        Result_Index = 0;
        Open_Panel = true;
        Bonus_Prize = false;
        gameObject.SetActive(false);
    }

    /*void OnGUI()
    {
        GUI.Label(new Rect(90, 510, 400, 20), "BonusNum" + PhotonClient.newPhotonClient.BonusNum);
        GUI.Label(new Rect(90, 530, 400, 20), "Index" + Result_Index);
        GUI.Label(new Rect(90, 550 ,400, 20), "Result_1 =" + Result[0]);
        GUI.Label(new Rect(90, 570, 400, 20), "Result_2 =" + Result[1]);
        GUI.Label(new Rect(90, 590, 400, 20), "Result_3 =" + Result[2]);
        GUI.Label(new Rect(90, 610, 400, 20), "Result_4 =" + Result[3]);
        GUI.Label(new Rect(90, 630, 400, 20), "Result_5 =" + Result[4]);
        GUI.Label(new Rect(90, 650, 400, 20), "Result_6 =" + Result[5]);
        GUI.Label(new Rect(90, 670, 400, 20), "Result_7 =" + Result[6]);
        GUI.Label(new Rect(90, 690, 400, 20), "Result_8 =" + Result[7]);
        GUI.Label(new Rect(90, 710, 400, 20), "Result_9 =" + Result[8]);
    }*/
}
