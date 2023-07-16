using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ActivePlayerManager : MonoBehaviour
{
    public delegate void SelectedPlayer(int value);
    public SelectedPlayer OnSelectedPlayer;
    [SerializeField] private PlayerMovements[] playersInScene;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera.Follow = playersInScene[0].transform;
    }

    private void OnSelectPlayer(InputValue value)
    {
        int pressed = (int)value.Get<float>();
        if (pressed != 0 && pressed <= playersInScene.Length)
        {
            virtualCamera.Follow = playersInScene[pressed - 1].transform;
            foreach (PlayerMovements player in playersInScene)
            {
                player.SetSelectedPlayer(pressed);
                OnSelectedPlayer?.Invoke(pressed);
            }
        }
    }

    public int GetNumberOfPlayersInScene() => playersInScene.Length;
}
