using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioClip walkingSFXClip;
    [SerializeField] AudioClip jumpingSFXClip;
    [SerializeField] AudioClip landingSFXClip;
    AudioSource myAudioSource;
    IEnumerator walkingClipRoutine;
    float musicVolume = 0.5f;
    float sfxVolume = 0.4f;

    protected override void Awake()
    {
        base.Awake();

        myAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        myAudioSource.volume = musicVolume;
        myAudioSource.Play();
    }

    void PlaySFXClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, sfxVolume);
    }

    public void PlayJumpingClip() => PlaySFXClip(jumpingSFXClip);

    public void PlayLandingClip() => PlaySFXClip(landingSFXClip);

    public void PlayWalkingClip()
    {
        if (walkingClipRoutine == null)
        {
            walkingClipRoutine = WalkingClipRoutine();
            StartCoroutine(walkingClipRoutine);
        }
    }

    public void StopWalkingClip()
    {
        if (walkingClipRoutine != null)
        {
            StopCoroutine(walkingClipRoutine);
            walkingClipRoutine = null;
        }
    }

    IEnumerator WalkingClipRoutine()
    {
        PlaySFXClip(walkingSFXClip);
        yield return new WaitForSeconds(walkingSFXClip.length);
    }
}
