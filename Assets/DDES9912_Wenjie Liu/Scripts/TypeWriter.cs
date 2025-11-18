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

    //How many lines can be written in total?
    public int lineMax = 10;
    private int lineNow = 0;
    public bool lineOver = false;

    //Number of characters that can be written in one line
    public int oneLineCount = 30;
    private int oneLineNow = 0;

    /// <summary>
    /// Is the line finished?
    /// </summary>
    public bool oneLineFull = false;
    public TextMeshPro paperText;

    public Transform writeRootTf, rollerTf;
    public Vector3 writeRootStartPos, newStrMoveDelta, rollerRotateDelta;

    //Is it currently in animation mode?
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

    public int paperUseCount = 0;
    public TextMeshProUGUI useCountText;

    public bool CanWriteNow()
    {
        bool canWrite = !isRollerAni && !oneLineFull && !lineOver;
        //Debug.Log($"can write :{canWrite}");
        return canWrite;
    }

    /// <summary>
    /// Pole strike
    /// </summary>
    public void PointDown(string info)
    {
        //Just like pressing a button, it needs to be interrupted when a new operation comes in.
        writeLineTf.DOKill();
        clickStr = (isLarge || isLockLarge) ? info : info.ToLower();
        writeLineTf.DOLocalRotate(downRotate, 0.05f).OnComplete(TriggerPaper);
    }

    /// <summary>
    /// Touching paper operation
    /// </summary>
    private void TriggerPaper()
    {
        //Writing
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
    /// Writing operation while the rollers run
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
    /// Complete a line
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
    /// Paper feed
    /// </summary>
    public void InPaper()
    {
        if (isInpapering || PaperTf.gameObject.activeSelf)
            return;
        isInpapering = true;
        ResetTypewriter();

        paperUseCount++;
        useCountText.text = paperUseCount.ToString();

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
    ///Printing complete, paper output
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
    /// Reset typewriter status
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
    /// quit
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }


    private float willRunTime = 0;

    /// <summary>
    /// New line content
    /// </summary>
    public void RunNewLine()
    {
        //To determine if the paper is used up and whether a new line can be started, check if the paper has run out.
        if (lineOver)
            return;

        //Enter the transition animation to prevent other button presses from triggering it.
        isRollerAni = true;

        //Check if a line has been written completely. If a newline is started before the line is finished, count the newline characters.
        if (!oneLineFull)
        {
            OverOneLine();
        }

        musicNow.PlayReturn();

        //Roller downwards
        rollerTf.DOLocalRotate(rollerRotateDelta, 0.1f);

        //Calculate the distance traveled while writing, and then calculate the time to return to the starting point, in order to maintain a reasonable movement speed.
        willRunTime = (writeRootStartPos.x - writeRootTf.localPosition.x) / 2f;

        //Allow time for the scrolling animation to prevent line breaks due to lack of animation.
        if (willRunTime < 0.1f)
            willRunTime = 0.1f;

        gearTf.DOLocalRotate(Vector3.zero, willRunTime);
        //The roller moves to the starting point
        writeRootTf.DOLocalMove(writeRootStartPos, willRunTime).OnComplete(() =>
        {
            //The roller rotates back
            rollerTf.DOLocalRotate(Vector3.zero, 0.1f);

            //The paper followed and moved upwards.
            PaperTf.DOLocalMove(PaperTf.localPosition + newLineUpDelta, 0.1f).OnComplete(() =>
            {
                oneLineFull = false;
                oneLineNow = 0;
                isRollerAni = false;

                //The move ended, and the text wrapped to a newline.
                writeNowInfo += "\n";
                //Debug.Log("???");
            });
        });
    }

    /// <summary>
    /// Raise the pole
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
    /// Capital bar Lift
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
    /// Down the capital bar
    /// </summary>
    public void PointMoveDown()
    {
        //transform.DOKill();
        writeLineTf.DOLocalMove(smallMove, 0.05f);
        isLarge = false;
        isLockLarge = false;
    }
}
