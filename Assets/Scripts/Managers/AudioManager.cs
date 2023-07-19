using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioClip walkingSFXClip;
    [SerializeField] AudioClip jumpingSFXClip;
    [SerializeField] AudioClip landingSFXClip;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    AudioSource myAudioSource;
    IEnumerator walkingClipRoutine;

    protected override void Awake()
    {
        base.Awake();

        myAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        musicSlider.onValueChanged.AddListener(_ => myAudioSource.volume = musicSlider.value);
        myAudioSource.volume = musicSlider.value;
        myAudioSource.Play();
    }

    void PlaySFXClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, sfxSlider.value);
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
