using UnityEngine;
using DG.Tweening;

public class ClickPoint : ClickBase
{
    //public Animator animator;

    public Transform rootTf;
    public Vector3 downRotate, upRotate;


    //public void ClickBt()
    //{
    //    animator.SetTrigger("down");
    //}

    //public void ClickBt()
    //{
    //    //按钮旋转下去，然后再起来
    //    rootTf.DOLocalRotate(downRotate, 0.15f).OnComplete(() => {
    //        rootTf.DOLocalRotate(downRotate, 0.1f).OnComplete(() => {
    //            rootTf.DOLocalRotate(Vector3.zero, 0.1f);
    //        });
    //    });
    //}

    public override void OnMouseDownClick()
    {
        ClickDown();
    }

    public override void OnMouseUpClick()
    {
        ClickUp();
    }

    /// <summary>
    /// 按下
    /// </summary>
    public void ClickDown()
    {
        rootTf.DOKill();
        rootTf.DOLocalRotate(downRotate, 0.1f);

        WriteLine.instance.PointDown();
    }

    /// <summary>
    /// 松手弹起
    /// </summary>
    public void ClickUp()
    {
        rootTf.DOKill();
        rootTf.DOLocalRotate(upRotate, 0.1f);

        WriteLine.instance.PointUp();
    }

}
