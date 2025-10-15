using UnityEngine;
using DG.Tweening;
using TMPro;

public class WriteLine : MonoBehaviour
{
    public static WriteLine instance;

    public Vector3 downRotate, upRotate;

    public Vector3 largeMove, smallMove;

    public bool isLarge = false;
    public bool isLockLarge = false;

    public ClickCapPoint[] OnceCapPoints;

    public Transform PaperTf;
    public Vector3 paperStartPos, newLineUpDelta, overShowPos, overRotate;

    //一共能写几行
    public int lineMax = 10;
    private int lineNow = 0;
    public bool lineOver = false;

    //一行的能写的数量
    public int oneLineCount = 30;
    private int oneLineNow = 0;

    /// <summary>
    /// 一行写完了吗
    /// </summary>
    public bool oneLineFull = false;
    public TextMeshPro paperText;

    public Transform writeRootTf,rollerTf;
    public Vector3 writeRootStartPos, newStrMoveDelta;

    //正在动画状态？
    public bool isRollerAni = false;

    private string clickStr = "";
    private string writeNowInfo = "";


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool CanWriteNow()
    {
        bool canWrite = !isRollerAni && !oneLineFull && !lineOver;
        Debug.Log($"can write :{canWrite}");
        return canWrite;
    }

    /// <summary>
    /// 杆子打击
    /// </summary>
    public void PointDown(string info)
    {
        //跟按键一样，需要打断，新的操作过来的话
        transform.DOKill();
        clickStr = (isLarge || isLockLarge) ? info : info.ToLower();
        transform.DOLocalRotate(downRotate, 0.05f).OnComplete(TriggerPaper);
    }

    /// <summary>
    /// 触碰到纸操作
    /// </summary>
    private void TriggerPaper()
    {
        //写字
        WriteStr();

        //单次大写，回杆
        if (!isLockLarge && isLarge)
        {
            foreach (var capPoint in OnceCapPoints)
            {
                capPoint.ClickUp();
            }
        }
    }

    /// <summary>
    /// 写字操作，同时滚筒运行
    /// </summary>
    private void WriteStr()
    {
        writeNowInfo += clickStr;
        paperText.text = writeNowInfo;
        oneLineNow++;

        writeRootTf.DOLocalMove(writeRootTf.localPosition + newStrMoveDelta, 0.05f);
        if (oneLineNow >= oneLineCount)
        {
            oneLineFull = true;
        }
    }

    /// <summary>
    /// 杆子抬起
    /// </summary>
    public void PointUp()
    {
        transform.DOKill();
        transform.DOLocalRotate(upRotate, 0.05f);

        if (!isLockLarge && isLarge)
        {
            PointMoveDown();
        }
    }

    /// <summary>
    /// 大写抬杆
    /// </summary>
    public void PointMoveUp(bool isLock = false)
    {
        //transform.DOKill();
        transform.DOLocalMove(largeMove, 0.05f).OnComplete(() =>
        {
            isLarge = true;
            isLockLarge = isLock;
        });
    }

    /// <summary>
    /// 大写下杆
    /// </summary>
    public void PointMoveDown()
    {
        //transform.DOKill();
        transform.DOLocalMove(smallMove, 0.05f);
        isLarge = false;
        isLockLarge = false;
    }

}
