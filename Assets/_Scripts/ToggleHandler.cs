using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHandler : MonoBehaviour
{
    private bool textureToggle = false, toggleBlur = false;
    private Material skyboxMaterial;
    [SerializeField] private List<SkinnedMeshRenderer> distortedMesh = new List<SkinnedMeshRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        skyboxMaterial = RenderSettings.skybox;
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the value of the "_textureToggle" property of all materials with the shader "LokiToon" to 1 when the "T" key is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (textureToggle)
            {
                foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
                {
                    material.SetFloat("_textureToggle", 1);
                }
                RenderSettings.skybox = skyboxMaterial;
                RenderSettings.fog = true;
                textureToggle = false;
            }
            else
            {
                foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
                {
                    material.SetFloat("_textureToggle", 0);
                }
                RenderSettings.skybox = null;
                RenderSettings.fog = false;
                textureToggle = true;
            }
        }
        //Toggles off the blur effect by setting blur strength to 0 when the "B" key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (toggleBlur)
            {
                foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
                {
                    material.SetFloat("_blurStrength", 0.012f);
                }
                foreach (SkinnedMeshRenderer x in distortedMesh)
                {
                    x.material.SetFloat("_distortionStrength", 2.0f);
                }
                toggleBlur = false;
            }
            else
            {
                foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
                {
                    material.SetFloat("_blurStrength", 0f);
                }
                foreach (SkinnedMeshRenderer x in distortedMesh)
                {
                    x.material.SetFloat("_distortionStrength", 0.0f);
                }
                toggleBlur = true;
            }
        }
    }
}
