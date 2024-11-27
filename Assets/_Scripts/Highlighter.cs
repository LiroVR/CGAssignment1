using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{

    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers = new List<SkinnedMeshRenderer>();
    private WaitForSeconds timerWait;
    // Start is called before the first frame update

    void Start()
    {
        timerWait = new WaitForSeconds(2);
    }

    public void Detected()
    {
        Highlight();
        StartCoroutine(Timer());
    }
    void Highlight()
    {
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.SetColor("_Color", new Color(0f,0f,0f,1f));
            }
        }
    }

    void UnHighlight()
    {
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.SetColor("_Color", new Color(0f,0f,0f,0f));
            }
        }
    }
    IEnumerator Timer()
    {
        yield return timerWait;
        UnHighlight();
    }
}
