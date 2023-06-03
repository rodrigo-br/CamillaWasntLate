using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] float dissolveSpeed = 1f;
    SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            PlayDissolve();
        }
    }

    public void PlayDissolve()
    {
        dissolveMaterial.SetFloat("_Dissolve", 0);
        mySpriteRenderer.material = new Material(dissolveMaterial);
        StartCoroutine(DissolveCoroutine());
    }

    IEnumerator DissolveCoroutine()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * dissolveSpeed;
            mySpriteRenderer.material.SetFloat("_Dissolve", t);
            yield return null;
        }
    }
}
