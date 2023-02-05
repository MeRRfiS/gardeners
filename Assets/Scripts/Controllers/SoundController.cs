using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour,IController
{
    private static SoundController inctanse;

    private AudioSource audio;
    [SerializeField] private List<AudioClip> hits;
    [SerializeField] private AudioClip collect;
    [SerializeField] private AudioClip dialog;

    [SerializeField] private AudioSource music;

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        inctanse = this;
        audio = GetComponent<AudioSource>();

        Status = LoadStatusEnum.IsLoaded;
    }

    public void PlayHit()
    {
        var rand = Random.Range(0, hits.Count);
        audio.PlayOneShot(hits[rand]);
    }

    public void PlayCollect()
    {
        audio.PlayOneShot(collect);
    }

    public void PlayDialog()
    {
        audio.PlayOneShot(dialog);
    }

    public void MuteSound()
    {
        audio.volume = 0;
        music.volume = 0;
    }

    public void UnMuteSound()
    {
        audio.volume = 0.5f;
        music.volume = 0.3f;
    }

    public static SoundController GetInctanse()
    {
        return inctanse;
    }
}
