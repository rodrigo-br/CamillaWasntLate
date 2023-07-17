using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; } = false;
    InputPlayer inputPlayer;
    // [SerializeField] GameObject menu;

    void Awake()
    {
        inputPlayer = new InputPlayer();
        // menu.GetComponentInChildren<Button>().onClick.AddListener(SetPauseState);
    }

    void OnEnable()
    {
        inputPlayer.Enable();
    }

    void Start()
    {
        inputPlayer.UI.Esc.performed += _ => SetPauseState();
    }

    void SetPauseState()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void OnDisable()
    {
        inputPlayer.Disable();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        IsPaused = true;
        // menu.SetActive(IsPaused);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        IsPaused = false;
        // menu.SetActive(IsPaused);
    }
}
