using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


[RequireComponent(typeof(Camera)),
]
public class PostProcesser : MonoBehaviour
{
    [Header("Pixel Shader")]
    public ShaderState pixelState = ShaderState.On;
    public bool dynamicPixelSize = false;
    public int screenHeight = 192;
    public float pixelsPerUnit = 24f;

    [Range(1f / 32f, 1)] public float zoom = 0.125f;

    [Header("Outline Shader")]
    public ShaderState outlineState = ShaderState.On;
    public Color outlineColor = Color.black;
    public float colorShift = 0.25f;
    public float depthThreshold = 0.02f;
    public float normalThreshold = 0.05f;
    public Vector3 normalEdgeBias = Vector3.one;
    public float angleThreshold = 0.5f;
    public int angleFactorScale = 7;

    public enum ShaderState
    {
        On,
        Off,
        Debug,
    }

    void OnEnable()
    {
        Camera.main.depthTextureMode = DepthTextureMode.DepthNormals | DepthTextureMode.Depth;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        var outlineMaterial = CoreUtils.CreateEngineMaterial("Custom/PixelPerfectOutline");
        outlineMaterial.SetFloat("_DepthThreshold", depthThreshold);
        outlineMaterial.SetFloat("_AngleThreshold", angleThreshold);
        outlineMaterial.SetFloat("_AngleFactorScale", angleFactorScale);
        outlineMaterial.SetFloat("_NormalThreshold", normalThreshold);
        outlineMaterial.SetVector("_NormalEdgeBias", normalEdgeBias);
        outlineMaterial.SetInteger("_DebugOutline", outlineState == ShaderState.Debug ? 1 : 0);
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_ColorShift", colorShift);

        var pixelScreenHeight = screenHeight;

        if (dynamicPixelSize)
        {
            pixelScreenHeight = (int)(1 / zoom * pixelsPerUnit);
        }

        var pixelScreenWidth = (int)(pixelScreenHeight * Camera.main.aspect + 0.5f);
        var tempTex = RenderTexture.GetTemporary(src.descriptor);
        var screenSize = new Vector2(Screen.width, Screen.height);

        if (pixelState == ShaderState.On)
        {
            src.filterMode = FilterMode.Point;
            tempTex.Release();
            tempTex.height = pixelScreenHeight;
            tempTex.width = pixelScreenWidth;
            tempTex.filterMode = FilterMode.Point;
            tempTex.Create();
            screenSize = new Vector2(pixelScreenWidth, pixelScreenHeight);
        }
        else
        {
            src.filterMode = FilterMode.Bilinear;
            tempTex.filterMode = FilterMode.Bilinear;
            tempTex.Release();
            tempTex.Create();
        }

        outlineMaterial.SetVector("_ScreenSize", screenSize);
        if (outlineState != ShaderState.Off)
        {
            Graphics.Blit(src, tempTex, outlineMaterial);
            Graphics.Blit(tempTex, dest);
        }
        else
        {
            Graphics.Blit(src, tempTex);
            Graphics.Blit(tempTex, dest);
        }

        RenderTexture.ReleaseTemporary(tempTex);
        Graphics.SetRenderTarget(dest);
    }
}
