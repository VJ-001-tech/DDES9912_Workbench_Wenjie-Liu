using UnityEngine;

public class MusicNow : MonoBehaviour
{
    public AudioSource source;

    public void PlayDown()
    {
        source.PlayOneShot(MusicControl.instance.downClip);
    }

    public void PlayUp()
    {
        source.PlayOneShot(MusicControl.instance.upClip);
    }

    public void PlayReturn()
    {
        source.PlayOneShot(MusicControl.instance.huaClip);
    }

}
