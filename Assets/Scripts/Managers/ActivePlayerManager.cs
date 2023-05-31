using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ActivePlayerManager : MonoBehaviour
{
    [SerializeField] private Movement[] playersInScene;
    [SerializeField] CinemachineStateDrivenCamera sdCamera;

    void Start()
    {
        sdCamera.Follow = playersInScene[0].transform;
    }

    private void OnSelectPlayer(InputValue value)
    {
        int pressed = (int)value.Get<float>();
        if (pressed != 0)
        {
            sdCamera.Follow = playersInScene[pressed - 1].transform;
            foreach (Movement player in playersInScene)
            {
                player.SetSelectedPlayer(pressed);
            }
        }
    }
}
