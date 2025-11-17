using DG.Tweening;
using UnityEngine;

public class ClickCapPoint : ClickBase
{
    public TypeWriter typeWriter;
    /// <summary>
    /// 按一下下去，再按下抬起效果开关
    /// </summary>
    public bool isLockTog = false;

    /// <summary>
    /// 是否大写打开
    /// </summary>
    public bool isUp = false;
    public Transform rootTf;
    public Vector3 downRotate, upRotate;

    public override void OnMouseDownClick()
    {
        if (!typeWriter.CanWriteNow())
            return;

        if (isLockTog)
        {
            if (isUp)
                ClickUp();
            else
                ClickDown();
        }
        else
        {
            ClickDown();
        }
    }

    public override void OnMouseUpClick()
    {
        if (!typeWriter.CanWriteNow())
            return;

        if (!isLockTog && !isUp)
            ClickUp();
    }

    /// <summary>
    /// 按下
    /// </summary>
    public void ClickDown()
    {
        if (!isLockTog && typeWriter.isLockLarge)
            return;
        musicNow.PlayDown();
        rootTf.DOKill();
        rootTf.DOLocalRotate(downRotate, 0.1f).OnComplete(() => {
            //按下去，大写打开
            isUp = true;
        });

        typeWriter.PointMoveUp(isLockTog);
    }

    /// <summary>
    /// 松手弹起
    /// </summary>
    public void ClickUp()
    {
        rootTf.DOKill();
        rootTf.DOLocalRotate(upRotate, 0.1f);
        musicNow.PlayUp();
        if (isUp)
            isUp = false;

        if (isLockTog)
        {
            typeWriter.PointMoveDown();
        }
    }


}
