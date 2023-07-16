using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsManager : MonoBehaviour
{
    public delegate void EndLevel();
    public EndLevel OnEndLevel; 
    ExitPortal[] exitPortals;
    int portalsOn = 0;

    void Awake()
    {
        exitPortals = GetComponentsInChildren<ExitPortal>();
    }

    void OnEnable()
    {
        foreach (ExitPortal portal in exitPortals)
        {
            portal.OnPortalChange += PortalChanged;
        }
    }

    void PortalChanged(int value)
    {
        portalsOn += value;   
        if (portalsOn == exitPortals.Length)
        {
            OnEndLevel?.Invoke();
        }
    }

    void OnDisable() 
    {
        foreach(ExitPortal portal in exitPortals)
        {
            portal.OnPortalChange -= PortalChanged;
        }
    } 
}
