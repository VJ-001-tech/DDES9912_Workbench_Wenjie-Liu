using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;

    public AudioClip downClip;
    public AudioClip upClip;
    public AudioClip huaClip;

    private void Awake()
    {
        instance = this;
    }

}
