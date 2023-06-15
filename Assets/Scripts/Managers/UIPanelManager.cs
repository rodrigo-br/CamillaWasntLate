using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField] GameObject players;
    Transform[] childs = new Transform[3];
    void Start()
    {
        FillchildsTranforms();
        SetMaterialColor();
    }

    private void SetMaterialColor()
    {
        int i = 0;
        foreach (Transform child in players.transform)
        {
            Color spriteColor = PickPlayerColor(child);
            Image newImage = childs[i].GetComponent<Image>();
            newImage.material = new Material(newImage.material);
            newImage.material.color = spriteColor;
            i++;
        }
    }

    private void FillchildsTranforms()
    {
        int i = 0;
        foreach (Transform child in this.transform)
        {
            childs[i] = child;
            i++;
        }
    }

    private Color PickPlayerColor(Transform child)
    {
        SpriteRenderer sr = child.GetComponentInChildren<SpriteRenderer>();
        return sr.sprite.texture.GetPixel((int)(sr.bounds.size.x / 2), (int)sr.bounds.size.y / 2);
    }
}
