using UnityEngine;

public class BackgroundMusicTest : MonoBehaviour
{
    public AudioClip Music;

    void Start()
    {
        Audio.SetAudioVolume(10);
        Audio.PlayMusic(Music);
    }
}
