using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;


[ExecuteInEditMode]
public class DomeController : MonoBehaviour
{
    // The DomeController is a convenience class that allows users of the Dome Projection prefab to tweak
    // the parameters of the dome projection without having to drill down to the DomeProjection script that
    // is attached to the projection camera.

    //----------------------------------------------------------------------------------------------------
    // DEFAULTS AND LIMITS
    //----------------------------------------------------------------------------------------------------

    public const float WorldCameraDefPitch = -90.0f;
    public const float WorldCameraMinPitch = -120.0f;
    public const float WorldCameraMaxPitch = +120.0f;

    public const float WorldCameraDefRoll  =    0.0f;
    public const float WorldCameraMinRoll  = -180.0f;
    public const float WorldCameraMaxRoll  = +180.0f;

    public const int DefDomeAngleProjection = 210;
    public const int MinDomeAngleProjection = 120;
    public const int MaxDomeAngleProjection = 360;

    public const float DefBackFadeIntensity     =  0.1f;
    public const float DefCrescentFadeIntensity =  0.5f;
    public const float DefCrescentFadeRadius    =  0.8f;
    public const float DefCrescentFadeOffset    = -0.2f;

	public const int DomeAngleProjectionIncrement = 5;
	public const float RotationIncrement = 5.0f;
	public const float FadeIntensityIncrement = 0.1f;
	public const float FadeRadiusIncrement = 0.1f;
	public const float FadeOffsetIncrement = 0.1f;

	//----------------------------------------------------------------------------------------------------
	// ERROR MESSAGES
	//----------------------------------------------------------------------------------------------------

	private const string ProjectionCameraMissingError = "DomeController: required component \"Projection Camera\" is missing from Dome Projector asset! Reverting the Dome Projector prefab will probably fix this error.";
    private const string ProjectionCameraNotMainCamera = "DomeController: \"Projection Camera\" is not the main camera! Most likely, you still need to delete the \"Main Camera\" that was added when Unity created the scene.";
    private const string WorldCameraMissingError = "DomeController: required component \"World Camera\" is missing from the Dome Projector asset! Reverting the Dome Projector prefab will probably fix this error.";
	private const string FPSTextMissingWarning = "DomeController: \"FPS Text\" not set! Please drag a Text into the \"FPS Text\" slot of the DomeController to enable FPS display!";
	private const string FPSCanvasMissingError = "DomeController: required component \"FPS Canvas\" is missing! Reverting the Dome Projector prefab will probably fix this error.";

    //----------------------------------------------------------------------------------------------------
    // ENUMS
    //----------------------------------------------------------------------------------------------------

    // Anti-aliasing types supported by the dome projection renderer.
    public enum AntiAliasingType
    {
        Off,                    // No anti-aliasing
        SSAA_2X,                // 2X super sampling
        SSAA_4X,                // 4X super sampling
    }

    // Cubemap sizes supported by the dome projection renderer.
    public enum CubeMapType
    {
        Cube512    = 512,       // Cubemap with 512x512 faces
        Cube1024   = 1024,      // Cubemap with 1024x1024 faces
        Cube2048   = 2048,      // etc.
        Cube4096   = 4096,
        Cube8192   = 8192,
    }

    //----------------------------------------------------------------------------------------------------
    // EDITOR PROPERTIES
    //----------------------------------------------------------------------------------------------------

    [Range(WorldCameraMinPitch, WorldCameraMaxPitch)]
    [Tooltip("Controls the pitch (x-axis rotation) of the world camera.")] 
    public float cameraOrientationAngle = WorldCameraDefPitch;

    [Range(WorldCameraMinRoll, WorldCameraMaxRoll)]
    [Tooltip("Controls the roll (z-axis rotation) of the world camera.")]
    public float DomeNorthOrientation = WorldCameraDefRoll;

    // Field of view of the fisheye 'lens' used during the dome projection.
    [Range(MinDomeAngleProjection, MaxDomeAngleProjection)]
    [Tooltip("Controls the field of view angle of the fisheye lens.")]
    public int DomeAngleProjection = DefDomeAngleProjection;

    // Size of the cubemaps captured from the scene.
    // Larger cubemaps == higher quality
    // Smaller cubemaps == better performance
    [Tooltip("Sets the size of the cubemap captured from the world camera. Increase the cubemap size to improve the quality of the final image; decrease the cubemap size for more performance.")]
    public CubeMapType cubeMapType = CubeMapType.Cube1024;

    // Anti-aliasing type used when rendering the dome projection
    [Tooltip("Sets the anti-aliasing level used while rendering the fisheye image. Supports 2X and 4X super-sampling.")]
    public AntiAliasingType antiAliasingType = AntiAliasingType.SSAA_2X;


	//----------------------------------------------------------------------------------------------------
	// PUBLIC
	//----------------------------------------------------------------------------------------------------

	public Camera projectionCamera { get { return m_projectionCamera; } }

    public Camera worldCamera { get { return m_worldCamera; } }

    //----------------------------------------------------------------------------------------------------
    // UNITY EVENTS
    //----------------------------------------------------------------------------------------------------
       
	void Start()
    {
        // One-time initialization of the dome controller.
        //
        // The below code has been designed to be 'automatic'; i.e. the user doesn't have to do anything
        // to correctly setup the dome projection asset. However, for the code to work correctly, it is
        // required that the Dome Projection game object (to which this MonoBehaviour is attached) has
        // two child cameras:
        //
        // - A camera named "Projection Camera", which should have the MainCamera tag
        // - A camera named "World Camera", which should NOT have the MainCamera taf
        //
        // Both cameras are present in the default Dome Projection prefab.

        // Fetch projection camera from child.
        Transform projectionCameraTrans = transform.Find("camProjection");
        if (projectionCameraTrans == null)
            Debug.LogError(ProjectionCameraMissingError);
        m_projectionCamera = projectionCameraTrans.GetComponent<Camera>();
        if (m_projectionCamera == null)
            Debug.LogError(ProjectionCameraMissingError);

        // Check if projection camera is the main camera.
        if (Camera.main != m_projectionCamera)
            Debug.LogError(ProjectionCameraNotMainCamera);

        // Fetch world camera from child.
        Transform worldCameraTrans = transform.Find("camWorld");
        if (worldCameraTrans == null)
            Debug.LogError(WorldCameraMissingError);
        m_worldCamera = worldCameraTrans.GetComponent<Camera>();
        if (m_worldCamera == null)
            Debug.LogError(WorldCameraMissingError);


		// Save initial pitch and roll.
		m_initialWorldCameraPitch = cameraOrientationAngle;
		m_initialWorldCameraRoll = DomeNorthOrientation;

	}

	void Update()
	{
		// Keeps the DomeAngleProjection and orientation of the world camera up-to-date.

		if (m_worldCamera == null)
            return;

        // Ensure DomeAngleProjection stays within a valid range.
        DomeAngleProjection = Mathf.Clamp(DomeAngleProjection, MinDomeAngleProjection, MaxDomeAngleProjection);

        // Apply world camera pitch and roll.
        cameraOrientationAngle = Mathf.Clamp(cameraOrientationAngle, WorldCameraMinPitch, WorldCameraMaxPitch);
		DomeNorthOrientation  = Mathf.Clamp(DomeNorthOrientation,  WorldCameraMinRoll,  WorldCameraMaxRoll);
        m_worldCamera.transform.localRotation = Quaternion.Euler(new Vector3(cameraOrientationAngle, 0, DomeNorthOrientation));
	}

	//----------------------------------------------------------------------------------------------------
	// PRIVATE
	//----------------------------------------------------------------------------------------------------


	private void SaveSettingsToFile(string filename)
	{
		using (StreamWriter stream = new StreamWriter(filename))
		{
			stream.WriteLine(string.Format("cameraOrientationAngle = {0}", cameraOrientationAngle));
			stream.WriteLine(string.Format("DomeNorthOrientation = {0}", DomeNorthOrientation));
			stream.WriteLine(string.Format("DomeAngleProjection = {0}", DomeAngleProjection));
			stream.WriteLine(string.Format("cubeMapType = {0}", (int) cubeMapType));
			stream.WriteLine(string.Format("antiAliasingType = {0}", antiAliasingType.ToString()));
			stream.WriteLine(string.Format("vSync = {0}", QualitySettings.vSyncCount));
		}
	}
		

	private void LoadSettingsFromFile(string filename)
	{

		if (File.Exists(filename))
		{
			string[] lines = File.ReadAllLines(filename);
			foreach (string line in lines)
			{
				if (line.IndexOf('=') == -1)
					continue;
				string[] parts = line.Split('=');
				if (parts.Length < 2)
					continue;
				string setting = parts[0].Trim();
				string value = parts[1].Trim();

				if (string.Compare(setting, "cameraOrientationAngle", true) == 0)
				{
					if (!float.TryParse(value, out cameraOrientationAngle))
						cameraOrientationAngle = WorldCameraDefPitch ;
					cameraOrientationAngle = Mathf.Clamp(cameraOrientationAngle, WorldCameraMinPitch, WorldCameraMaxPitch) - 90f;
				}
				else if (string.Compare(setting, "DomeNorthOrientation", true) == 0)
				{
					if (!float.TryParse(setting, out DomeNorthOrientation))
						DomeNorthOrientation = WorldCameraDefRoll;
					DomeNorthOrientation = Mathf.Clamp(DomeNorthOrientation, WorldCameraMinRoll, WorldCameraMaxRoll);
				}
				else if (string.Compare(setting, "cubeMapType", true) == 0)
				{
					int cubeMapSize;
					if (int.TryParse(value, out cubeMapSize))
					{
						switch (cubeMapSize)
						{
							case 512: cubeMapType = CubeMapType.Cube512; break;
							case 1024: cubeMapType = CubeMapType.Cube1024; break;
							case 2048: cubeMapType = CubeMapType.Cube2048; break;
							case 4096: cubeMapType = CubeMapType.Cube4096; break;
							case 8192: cubeMapType = CubeMapType.Cube8192; break;
							default: goto case 1024;
						}
					}
					else
						cubeMapType = CubeMapType.Cube1024;
				}
				else if (string.Compare(setting, "antiAliasingType", true) == 0)
				{
					if (string.Compare(value, "Off", true) == 0)
						antiAliasingType = AntiAliasingType.Off;
					else if (string.Compare(value, "SSAA_4X", true) == 0)
						antiAliasingType = AntiAliasingType.SSAA_4X;
					else
						antiAliasingType = AntiAliasingType.SSAA_2X;
				}
				else if (string.Compare(setting, "vSync", true) == 0)
				{
					int vSyncCount = 0;
					if (!int.TryParse(value, out vSyncCount))
						vSyncCount = 1;
					if (vSyncCount != 0 && vSyncCount != 1)
						vSyncCount = 1;
					QualitySettings.vSyncCount = vSyncCount;
				}
			}
		}
	}
		
	// The projection camera is the camera that is used to render the final fisheye image.
	private Camera m_projectionCamera;

    // The world camera is the camera that is used to capture a cubemap of the scene every frame.
    private Camera m_worldCamera;

	// Camera properties
	private float m_initialWorldCameraPitch = 0.0f;
	private float m_initialWorldCameraRoll = 0.0f;

}
