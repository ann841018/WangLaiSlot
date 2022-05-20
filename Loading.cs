using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI
using UnityEngine.SceneManagement;//切場景

public class Loading : MonoBehaviour
{
    public GameObject LoadingLine;//進度條
    public Text LoadingNumber;//進度條%數
    
    // Use this for initialization
    void Start ()
    {
        LoadingLine.transform.localPosition = new Vector3 (-400, 0, 0);//進度條初始位置
        PlayerPrefs.SetInt("SceneNum", 2);
        StartCoroutine(DisplayLoadingScreen());//開始讀取場景
    }

    private IEnumerator DisplayLoadingScreen()
    {
        int displayProgress = 0;//顯示出來的數值
        int toProgress = 0;//場景讀取的數值
        AsyncOperation op = SceneManager.LoadSceneAsync(1);//讀取場景的資料
        op.allowSceneActivation = false;//進度先讓他不會切過去
        while (op.progress < 0.9f)//90%的時候
        {
            toProgress = (int)op.progress * 100;//讀取數值
            while (displayProgress < toProgress)
            {
                ++displayProgress;//顯示出來的數值
                SetLoadingPercentage(displayProgress);//設定進度條資訊
                yield return new WaitForEndOfFrame();//一幀一幀跑
            }
        }

        toProgress = 100;//如果跑完了
        if(NickName.WrongNickName == false)
        while (displayProgress < toProgress)//90-100%
        {
            ++displayProgress;//顯示出來的數值
            SetLoadingPercentage(displayProgress);//設定進度條資訊
            yield return new WaitForEndOfFrame();//一幀一幀跑
        }
        op.allowSceneActivation = true;//進度條跑完才可以切場景
    }

    void SetLoadingPercentage(float percent)//回傳數值
    {
        LoadingLine.transform.localPosition = new Vector3(percent*8-400, 0, 0);//移動進度條
        LoadingNumber.text = percent.ToString() + " %";//顯示進度條數值%數
    }
}
