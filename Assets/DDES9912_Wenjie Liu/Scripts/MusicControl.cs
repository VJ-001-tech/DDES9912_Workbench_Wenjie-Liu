using TMPro;
using UnityEngine;
using static UnityEngine.SpriteMask;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;

    public AudioClip downClip;
    public AudioClip upClip;
    public AudioClip huaClip;

    public AudioSource mainSource;
    public AudioClip goodClip;
    public AudioClip badClip;

    public Animator lightAni;

    private int goodCount, badCount;
    public TextMeshProUGUI goodCountText, badCountText;
    public TextMeshPro aimText;
    private string aimStr;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        aimStr = aimText.text.Replace(" ", "").Replace("\n", "").Replace("\t", "");
    }

    public void GetPaper(string info)
    {
        string nowStr = info.Replace(" ", "").Replace("\n", "").Replace("\t", "");
        if(aimStr.Equals(nowStr))
        {
            Good();
        }
        else
        {
            Bad();
        }
    }

    private void Good()
    {
        lightAni.SetTrigger("good");
        mainSource.PlayOneShot(goodClip);
        goodCount++;
        goodCountText.text = goodCount.ToString();
    }

    private void Bad()
    {
        lightAni.SetTrigger("bad");
        mainSource.PlayOneShot(badClip);
        badCount++;
        badCountText.text = badCount.ToString();
    }

}
