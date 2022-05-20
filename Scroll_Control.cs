using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll_Control : MonoBehaviour
{
    [SerializeField]RectTransform Content;//整個要滑的物件
    [SerializeField]Toggle[] EventSquare;//單個公告
    [SerializeField]int MaxNum;

    float InputOldPositionX;//按下去的x座標
    float InputNewPositionX;//按下去的x座標
    int SquareDis;//兩個公告的距離
    int MinSquareNum;//最小距離
    bool dragging;//正在拖移
    bool OldPos;
    bool CanDrag;

    // Use this for initialization
    void Start()
    {
        OldPos = true;
        CanDrag = true;
        int SquareLengh = EventSquare.Length;//物件長度
        SquareDis = (int) Mathf.Abs(EventSquare[1].GetComponent<RectTransform>().anchoredPosition.x- EventSquare[0].GetComponent<RectTransform>().anchoredPosition.x);//兩個物件距離多少
    }
    // Update is called once per frame
    void Update()
    {
        if (dragging == true)
        {
            if (CanDrag == true)
            {
                if (InputNewPositionX > InputOldPositionX)//往右滑
                {
                    if (MinSquareNum > 0) { MinSquareNum = MinSquareNum - 1; }//頁面變左邊那格
                    EventSquare[MinSquareNum].isOn = true;//那格的圈圈變色
                    CanDrag = false;//只能滑一格
                }
                else if (InputNewPositionX < InputOldPositionX)//往左滑
                {
                    if (MinSquareNum < MaxNum){ MinSquareNum = MinSquareNum + 1; }//頁面變右邊那格
                    EventSquare[MinSquareNum].isOn = true;//那格的圈圈變色
                    CanDrag = false;//只能滑一格
                }
            }
        }else if (dragging==false) {LerpToSquare(MinSquareNum * -SquareDis);}//沒有在拖曳才跑位置
   }

    void LerpToSquare(int position)//移動頁面
    {
        float newX = Mathf.Lerp(Content.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, Content.anchoredPosition.y);
        Content.anchoredPosition = newPosition;
    }

    public void StatrDrag()
    {
        dragging = true;//有在拖曳
        if (OldPos == true)//按下去
        {
            if (Input.GetMouseButton(0)) { InputOldPositionX = Input.mousePosition.x; }//滑鼠控制
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { InputOldPositionX = Input.GetTouch(0).position.x; }//觸控控制
            OldPos = false;
        }
        if (OldPos == false)//移動後
        {
            if (Input.GetMouseButton(0)) { InputNewPositionX = Input.mousePosition.x; }//滑鼠控制
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { InputNewPositionX = Input.GetTouch(0).position.x; }//觸控控制
        }
    }
    public void EndDrag() {
        dragging = false;//沒有在拖曳
        OldPos = true;//按下去
        CanDrag = true;//可以拖曳
    }
}
