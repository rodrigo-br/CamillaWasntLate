using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    public static bool IsPaused { get; private set; } = false;
    InputPlayer inputPlayer;

    void Awake()
    {
        inputPlayer = new InputPlayer();
    }

    void OnEnable()
    {
        inputPlayer.Enable();
    }

    void Start()
    {
        inputPlayer.UI.Esc.performed += _ => SetPauseState(IsPaused);
    }

    void SetPauseState(bool currentState)
    {
        Pause(!currentState);
    }

    void OnDisable()
    {
        inputPlayer.Disable();
    }

    public void Pause(bool newState)
    {
        IsPaused = newState;
        Time.timeScale = IsPaused == true ? 0 : 1;
        pauseCanvas.SetActive(IsPaused);
    }
}
