using DG.Tweening;
using TMPro;
using UnityEngine;

public class PaperPrefab : MonoBehaviour
{
    public TextMeshPro infoText;
    public float speed;

    public void SetInfo(string info, Transform willPos, Transform outPos, Transform overTf)
    {
        infoText.text = info;
        gameObject.SetActive(true);

        transform.DOMove(willPos.position, 0.2f).OnComplete(() =>
        {
            transform.DORotate(outPos.eulerAngles, 0.3f);
            transform.DOMove(outPos.position, 0.5f).OnComplete(() =>
            {
                transform.DOLocalMove(overTf.position, Vector3.Distance(overTf.position, transform.position) / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    MusicControl.instance.GetPaper(info);
                    Destroy(gameObject);
                });
            });
        });
    }

}
