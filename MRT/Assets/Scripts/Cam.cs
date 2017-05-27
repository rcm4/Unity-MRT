using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Cam : MonoBehaviour {

    public enum RT
    {
        Depth  = 0,
        Color  = 1,
        Normal = 2,
        WorldNormal = 3
    }
    
    public RT rt;
    public Shader MRTShader;

    private Camera sourceCamera;
    private Camera tempCamera;

    private RenderTexture[] rts;
    private RenderBuffer[] colorBuffers;
    private RenderTexture depthBuffer;

    void Start () {

        this.sourceCamera = this.GetComponent<Camera>();
        tempCamera = new GameObject().AddComponent<Camera>();
        tempCamera.enabled = false;

        this.rts = new RenderTexture[4] {
            new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 0, RenderTextureFormat.Default),
            new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 0, RenderTextureFormat.Default),
            new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 0, RenderTextureFormat.Default),
            new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 0, RenderTextureFormat.Default)
        };

        rts[0].Create();
        rts[1].Create();
        rts[2].Create();
        rts[3].Create();

        this.colorBuffers = new RenderBuffer[4] { rts[0].colorBuffer, rts[1].colorBuffer, rts[2].colorBuffer, rts[3].colorBuffer };

        this.depthBuffer = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 24, RenderTextureFormat.Depth);
        this.depthBuffer.Create();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        tempCamera.CopyFrom(sourceCamera);
        tempCamera.SetTargetBuffers(this.colorBuffers, this.depthBuffer.depthBuffer);
        tempCamera.cullingMask = 1 << LayerMask.NameToLayer("Lerpz");
        tempCamera.RenderWithShader(MRTShader, "");

        Graphics.Blit(rts[rt.GetHashCode()], destination);

        
    }

    void OnDestroy()
    {
        rts[0].Release();
        rts[1].Release();
        rts[2].Release();
        rts[3].Release();

        depthBuffer.Release();
    }
}
