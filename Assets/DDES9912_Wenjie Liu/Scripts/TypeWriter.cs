using DG.Tweening;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public Transform writeLineTf;
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

    public Transform writeRootTf, rollerTf;
    public Vector3 writeRootStartPos, newStrMoveDelta, rollerRotateDelta;

    //正在动画状态？
    public bool isRollerAni = false;

    private string clickStr = "";
    private string writeNowInfo = "";

    //public GameObject overGo;

    public MusicNow musicNow;

    public Transform gearTf;
    public Vector3 gearRotateDelta;

    public Transform newPaperTf;
    public Transform newPaperPosStart, newPaperPos1;
    public Transform paperWillPos, paperWritePos, paperOutPos;
    public GameObject paperPrefab;
    public Transform endPos;

    public bool CanWriteNow()
    {
        bool canWrite = !isRollerAni && !oneLineFull && !lineOver;
        //Debug.Log($"can write :{canWrite}");
        return canWrite;
    }

    /// <summary>
    /// 杆子打击
    /// </summary>
    public void PointDown(string info)
    {
        //跟按键一样，需要打断，新的操作过来的话
        writeLineTf.DOKill();
        clickStr = (isLarge || isLockLarge) ? info : info.ToLower();
        writeLineTf.DOLocalRotate(downRotate, 0.05f).OnComplete(TriggerPaper);
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

        gearTf.DOLocalRotate(gearTf.localEulerAngles + gearRotateDelta, 0.05f);
        writeRootTf.DOLocalMove(writeRootTf.localPosition + newStrMoveDelta, 0.05f);
        if (oneLineNow >= oneLineCount)
        {
            oneLineFull = true;
            OverOneLine();
        }
    }

    /// <summary>
    /// 完成一行
    /// </summary>
    private void OverOneLine()
    {
        lineNow++;
        if (lineNow >= lineMax)
        {
            lineOver = true;
            DoOverAni();
        }
    }

    private void DoOverAni()
    {
        OutPaper();
        //PaperTf.DOLocalRotate(overRotate, 0.3f);
        //PaperTf.DOLocalMove(overShowPos, 0.5f).OnComplete(() => {
        //    overGo.SetActive(true);
        //});
    }

    private bool isInpapering = false;
    /// <summary>
    /// 进纸
    /// </summary>
    public void InPaper()
    {
        if (isInpapering || PaperTf.gameObject.activeSelf)
            return;
        isInpapering = true;
        ResetTypewriter();
        newPaperTf.localPosition = newPaperPosStart.localPosition;
        newPaperTf.localEulerAngles = newPaperPosStart.localEulerAngles;
        newPaperTf.gameObject.SetActive(true);
        newPaperTf.DOMove(newPaperPos1.position, 0.3f).OnComplete(() => {
            newPaperTf.DORotate(paperWillPos.eulerAngles, 0.2f);
            newPaperTf.DOMove(paperWillPos.position, 0.3f).OnComplete(() => {
                newPaperTf.DOMove(paperWritePos.position, 0.2f).OnComplete(() => {
                    newPaperTf.gameObject.SetActive(false);
                    PaperTf.gameObject.SetActive(true);
                    isInpapering = false;
                });
            });
        });
    }

    /// <summary>
    /// 打印完毕，出纸
    /// </summary>
    public void OutPaper()
    {
        if (!PaperTf.gameObject.activeSelf)
            return;
        PaperTf.gameObject.SetActive(false);
        PaperPrefab outPaper = Instantiate(paperPrefab, paperWritePos.position, PaperTf.rotation).GetComponent<PaperPrefab>();
        outPaper.SetInfo(writeNowInfo, paperWillPos, paperOutPos, endPos);
    }

    /// <summary>
    /// 重置打字机
    /// </summary>
    public void ResetTypewriter()
    {
        writeNowInfo = "";
        paperText.text = writeNowInfo;
        lineNow = 0;
        oneLineNow = 0;
        lineOver = false;
        oneLineFull = false;
        //overGo.SetActive(false);

        gearTf.DOLocalRotate(Vector3.zero, 0.2f);
        writeRootTf.DOLocalMove(writeRootStartPos, 0.2f);
        PaperTf.DOLocalMove(paperStartPos, 0.2f);
        PaperTf.DOLocalRotate(Vector3.zero, 0.2f);

        //gearTf.localEulerAngles = Vector3.zero;
        //writeRootTf.localPosition = writeRootStartPos;
        //PaperTf.localPosition = paperStartPos;
        //PaperTf.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 退出
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }


    private float willRunTime = 0;

    /// <summary>
    /// 换一行新的内容
    /// </summary>
    public void RunNewLine()
    {
        //判断纸用完没，是否能继续换行
        if (lineOver)
            return;

        //进入换行动画，防止别的按钮按下触发
        isRollerAni = true;

        //判断是否写完了一行，没写完直接换行的话，计数换行
        if (!oneLineFull)
        {
            OverOneLine();
        }

        musicNow.PlayReturn();

        //滚筒下滚
        rollerTf.DOLocalRotate(rollerRotateDelta, 0.1f);

        //计算写字移动距离，然后计算回到起点时间，为了保持移动速度合理性
        willRunTime = (writeRootStartPos.x - writeRootTf.localPosition.x) / 2f;

        //给滚筒的动画预留时间，防止没动画直接换行
        if (willRunTime < 0.1f)
            willRunTime = 0.1f;

        gearTf.DOLocalRotate(Vector3.zero, willRunTime);
        //滚筒移动到起点
        writeRootTf.DOLocalMove(writeRootStartPos, willRunTime).OnComplete(() =>
        {
            //滚筒转动回去
            rollerTf.DOLocalRotate(Vector3.zero, 0.1f);

            //纸张跟随向上移动
            PaperTf.DOLocalMove(PaperTf.localPosition + newLineUpDelta, 0.1f).OnComplete(() =>
            {
                oneLineFull = false;
                oneLineNow = 0;
                isRollerAni = false;

                //移动结束，text换行
                writeNowInfo += "\n";
                //Debug.Log("???");
            });
        });
    }

    /// <summary>
    /// 杆子抬起
    /// </summary>
    public void PointUp()
    {
        writeLineTf.DOKill();
        writeLineTf.DOLocalRotate(upRotate, 0.05f);

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
        writeLineTf.DOLocalMove(largeMove, 0.05f).OnComplete(() =>
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
        writeLineTf.DOLocalMove(smallMove, 0.05f);
        isLarge = false;
        isLockLarge = false;
    }
}
