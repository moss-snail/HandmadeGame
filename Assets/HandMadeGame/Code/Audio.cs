using System.Collections;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Singleton;
    private float AudioVolume;
    private AudioSource[] MusicSources;
    private AudioSource SfxSource, SfxSourcePitched;
    private int ActiveMusicSourceIndex;
    private bool MusicCrossfadeActive;

    private void Awake()
    {
        Singleton = this;

        // Create two music audio sources, this will allow the script to cross-fade between two songs
        MusicSources = new AudioSource[2];

        // This generates two new gameobjects and attaches an AudioSource component to it
        for (int i = 0; i < 2; i++)
        {
            GameObject NewMusicSource = new GameObject("MusicSource" + (i + 1));
            MusicSources[i] = NewMusicSource.AddComponent<AudioSource>();
            MusicSources[i].loop = true;
            NewMusicSource.transform.parent = transform;
        }

        // Create a new gameobject with AudioSource to handle sound effects
        GameObject NewSfxSource = new GameObject("SFXSource");
        SfxSource = NewSfxSource.AddComponent<AudioSource>();
        NewSfxSource.transform.parent = transform;

        // Create a new gameobject with AudioSource to handle sound effects
        GameObject NewSfxSourcePitched = new GameObject("SFXSourcePitched");
        SfxSourcePitched = NewSfxSourcePitched.AddComponent<AudioSource>();
        NewSfxSourcePitched.transform.parent = transform;
    }

    // This will force stop all sound effects
    public void StopAllSFX() => SfxSource.Stop();

    public void StopMusic(float _fade = 0)
    {
        ActiveMusicSourceIndex = 1 - ActiveMusicSourceIndex;
        StartCoroutine(AnimateMusicCrossfade(_fade));
    }

    public static void SetAudioVolume(int Vol) => Audio.Singleton.SetAudioVolumeSingle(Vol);
    public void SetAudioVolumeSingle(int Vol)
    {
        Vol = Mathf.Clamp(Vol, 0, 10);
        AudioVolume = (float)Vol / 10;
        if (!MusicCrossfadeActive)
        {
            MusicSources[ActiveMusicSourceIndex].volume = AudioVolume;
            MusicSources[1 - ActiveMusicSourceIndex].volume = 0;
        }
    }

    // Returns the currently playing song
    public static AudioClip CurrentSong() { return Audio.Singleton.CurrentSongSingle(); }
    public AudioClip CurrentSongSingle() { return MusicSources[ActiveMusicSourceIndex].clip; }
    public float CurrentSongPosition() { return MusicSources[ActiveMusicSourceIndex].time; }
    public void PauseCurrentSong() { MusicSources[ActiveMusicSourceIndex].Pause(); }
    public void UnpauseCurrentSong() { MusicSources[ActiveMusicSourceIndex].UnPause(); }

    // This will load the referenced AudioClip and crossfade between them.
    // This also starts the music at the same timestamp as the currently playing song.
    // Great for music that has different layers!
    public static void PlayMusic(AudioClip _clip, float _fade = 1) => Audio.Singleton.PlayMusicSingle(_clip, _fade);
    public void PlayMusicSingle(AudioClip _clip, float _fade = 1)
    {
        // If the requested audio is different from the currently playing audio, start fade
        if (MusicSources[ActiveMusicSourceIndex].clip != _clip)
        {
            ActiveMusicSourceIndex = 1 - ActiveMusicSourceIndex;
            MusicSources[ActiveMusicSourceIndex].clip = _clip;

            // If the current song has progressed longer than the new song's length, the new song will start at 0
            if (MusicSources[ActiveMusicSourceIndex].clip.length < MusicSources[1 - ActiveMusicSourceIndex].time)
            {
                MusicSources[ActiveMusicSourceIndex].timeSamples = 0;
            }
            else
            {
                MusicSources[ActiveMusicSourceIndex].timeSamples = MusicSources[1 - ActiveMusicSourceIndex].timeSamples;
                MusicSources[ActiveMusicSourceIndex].Play(); // Hard code Play() here, helps keep things in sync
            }

            MusicSources[ActiveMusicSourceIndex].Play();
            StartCoroutine(AnimateMusicCrossfade(_fade));
        }
    }

    // Plays a one-shot sound
    public static void PlaySound(AudioClip _clip) { Audio.Singleton.PlaySoundSingle(_clip); }
    public void PlaySoundSingle(AudioClip _clip) { SfxSource.PlayOneShot(_clip, AudioVolume); }

    public static void PlaySoundPitched(AudioClip _clip, float PitchMod) { Audio.Singleton.PlaySoundPitchedSingle(_clip, PitchMod); }
    public void PlaySoundPitchedSingle(AudioClip _clip, float PitchMod)
    {
        SfxSourcePitched.pitch = PitchMod;
        SfxSourcePitched.PlayOneShot(_clip, AudioVolume);
    }

    public static void StopSFX() { Audio.Singleton.StopSFXSingle(); }
    public void StopSFXSingle() { SfxSource.Stop(); }

    // Small coroutine that crossfades the currently playing song with the new one
    private IEnumerator AnimateMusicCrossfade(float _duration)
    {
        MusicCrossfadeActive = true;
        float Percent = 0;
        while (Percent < 1)
        {
            Percent += Time.deltaTime / _duration;
            MusicSources[ActiveMusicSourceIndex].volume = Mathf.Lerp(0, AudioVolume, Percent);
            MusicSources[1 - ActiveMusicSourceIndex].volume = Mathf.Lerp(AudioVolume, 0, Percent);
            yield return new WaitForFixedUpdate();
        }
        MusicCrossfadeActive = false;
        MusicSources[1 - ActiveMusicSourceIndex].Stop();
    }
}