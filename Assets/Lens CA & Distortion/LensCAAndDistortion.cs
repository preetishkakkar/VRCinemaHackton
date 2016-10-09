using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu ("Image Effects/Lens CA And Distortion")]
public class LensCAAndDistortion : MonoBehaviour {
	#region Variables
	public Shader LensShader;
	[Range(-10.0f, 10.0f)] public float RedCyan = 0.0f;
	[Range(-10.0f, 10.0f)] public float GreenMagenta = 0.0f;
	[Range(-10.0f, 10.0f)] public float BlueYellow = 0.0f;
	public bool TrimExtents = false;
	public Texture2D TrimTexture = null;
	[Range(-1.0f, 1.0f)] public float Zoom = 0.0f;
	[Range(-5.0f, 5.0f)] public float BarrelDistortion = 0.0f;
	
	private Material curMaterial;
	#endregion
	
	#region Properties
	Material material
	{
		get
		{
			if(curMaterial == null)
			{
				curMaterial = new Material(LensShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;	
			}
			return curMaterial;
		}
	}
	#endregion
	
	void Start () 
	{
		if(!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	
	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture) 
	{
		if(LensShader != null)
		{
			material.SetFloat("_RedCyan", RedCyan * 10.0f); 
			material.SetFloat("_GreenMagenta", GreenMagenta * 10.0f);
			material.SetFloat("_BlueYellow", BlueYellow * 10.0f);
			material.SetFloat("_Zoom", 0.0f - Zoom);
			material.SetFloat("_BarrelDistortion", BarrelDistortion);
			material.SetTexture ("_BorderTex", TrimTexture);
			
			if (TrimExtents == true)
			{
				material.SetInt("_BorderOnOff", 1);
			}
			else
			{
				material.SetInt("_BorderOnOff", 0);
			}
			
			Graphics.Blit(sourceTexture, destTexture, material); 
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
		
		
	}
	
	
	void Update () 
	{

	}
	
	
	void OnDisable ()
	{
		if(curMaterial)
		{
			DestroyImmediate(curMaterial);
		}
		
	}
	
	
}