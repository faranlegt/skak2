using System;
using UnityEngine;

public class SkakGlobals : MonoBehaviour
{
    private static readonly int ShadowHeightCoefficient = Shader.PropertyToID("_ShadowHeightCoefficient");
    private static readonly int HeightCoefficient = Shader.PropertyToID("_HeightCoefficient");
    private static readonly int ShadowColor = Shader.PropertyToID("_ShadowColor");
    
    public float shadowHeightCoefficient = 0;
    public float heightCoefficient = 1f;
    public Color shadowColor = Color.black;

    private void Awake()
    {
        SetShaderGlobals();
    }

    private void OnValidate()
    {
        SetShaderGlobals();
    }

    private void SetShaderGlobals()
    {
        Shader.SetGlobalFloat(ShadowHeightCoefficient, shadowHeightCoefficient);
        Shader.SetGlobalFloat(HeightCoefficient, heightCoefficient);
        Shader.SetGlobalColor(ShadowColor, shadowColor);
    }
}