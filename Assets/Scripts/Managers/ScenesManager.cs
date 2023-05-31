using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    private int BuildSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public void ReloadLevel() => SceneManager.LoadScene(BuildSceneIndex());
    public void LoadNextScene()
    {
        int sceneIndex = BuildSceneIndex();
        if (sceneIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            ReloadLevel();
        }
        if (Input.GetKeyDown("l"))
        {
            LoadNextScene();
        }
    }
}
