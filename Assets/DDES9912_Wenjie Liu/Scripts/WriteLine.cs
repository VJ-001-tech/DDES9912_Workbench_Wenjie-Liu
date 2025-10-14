using UnityEngine;
using DG.Tweening;

public class WriteLine : MonoBehaviour
{
    public static WriteLine instance;

    public Vector3 downRotate, upRotate;

    public Vector3 largeMove, smallMove;

    public bool isLarge = false;
    public bool isLockLarge = false;

    public ClickCapPoint[] OnceCapPoints;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    /// <summary>
    /// 杆子打击
    /// </summary>
    public void PointDown()
    {
        //跟按键一样，需要打断，新的操作过来的话
        transform.DOKill();
        transform.DOLocalRotate(downRotate, 0.1f).OnComplete(TriggerPaper);
    }

    /// <summary>
    /// 触碰到纸操作
    /// </summary>
    private void TriggerPaper()
    {
        //写字


        //单次大写，回杆
        if(!isLockLarge && isLarge)
        {
            foreach (var capPoint in OnceCapPoints)
            {
                capPoint.ClickUp();
            }
        }
    }

    /// <summary>
    /// 杆子抬起
    /// </summary>
    public void PointUp()
    {
        transform.DOKill();
        transform.DOLocalRotate(upRotate, 0.1f);

        if(!isLockLarge && isLarge)
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
        transform.DOLocalMove(largeMove, 0.1f).OnComplete(() =>
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
        transform.DOLocalMove(smallMove, 0.1f);
        isLarge = false;
    }

}
