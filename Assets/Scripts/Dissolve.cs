using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] float dissolveSpeed = 1f;
    [SerializeField] PortalsManager portalsManager;
    SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        portalsManager.OnEndLevel += PlayDissolve;
    }
    
    void OnDisable()
    {
        portalsManager.OnEndLevel -= PlayDissolve;
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
        yield return new WaitForSeconds(0.1f);
        ScenesManager.Instance.LoadNextScene();
    }
}
