﻿namespace MuseDashMirror.UIComponents;

/// <summary>
///     Methods for creating canvas and related components
/// </summary>
[Logger]
public static partial class CanvasUtils
{
    /// <summary>
    ///     Cache for cameras
    /// </summary>
    private static readonly Dictionary<string, Camera> CameraCache = new();

    /// <summary>
    ///     Get camera by dimension
    /// </summary>
    /// <param name="cameraDimension">TwoD or ThreeD</param>
    /// <returns>Camera</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Camera GetCamera(CameraDimension cameraDimension)
    {
        return cameraDimension switch
        {
            CameraDimension.TwoD => GetCamera("Camera_2D"),
            CameraDimension.ThreeD => GetCamera("Camera_3D"),
            _ => throw new ArgumentOutOfRangeException(nameof(cameraDimension), cameraDimension, null)
        };
    }

    /// <summary>
    ///     Get camera by name
    /// </summary>
    /// <param name="cameraName">Camera Name</param>
    /// <returns>Camera</returns>
    public static Camera GetCamera(string cameraName)
    {
        if (CameraCache.TryGetValue(cameraName, out var camera))
        {
            return camera;
        }

        camera = GetGameObject(cameraName).GetComponent<Camera>();
        if (camera == null)
        {
            Logger.Error($"Camera with name {cameraName} is not found");
            return camera;
        }

        CameraCache[cameraName] = camera;
        return camera;
    }

    /// <summary>
    ///     Create a ScreenSpaceOverlay canvas
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateOverlayCanvas(string canvasName) =>
        CreateCanvas(canvasName, RenderMode.ScreenSpaceOverlay, null, null);

    /// <summary>
    ///     Create a ScreenSpaceCamera canvas with specified camera
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <param name="cameraName">Camera Name</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateCameraCanvas(string canvasName, string cameraName) =>
        CreateCameraCanvas(canvasName, cameraName, null);

    /// <summary>
    ///     Create a ScreenSpaceCamera canvas with specified camera dimension
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <param name="cameraDimension">TwoD or ThreeD</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateCameraCanvas(string canvasName, CameraDimension cameraDimension) =>
        CreateCameraCanvas(canvasName, cameraDimension, null);

    /// <summary>
    ///     Create a ScreenSpaceCamera canvas with specified camera and parent
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <param name="cameraName">Camera Name</param>
    /// <param name="parent">Parent GameObject</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateCameraCanvas(string canvasName, string cameraName, GameObject parent)
    {
        var camera = GetCamera(cameraName);
        var canvas = CreateCanvas(canvasName, RenderMode.ScreenSpaceCamera, camera, parent);
        return canvas;
    }

    /// <summary>
    ///     Create a ScreenSpaceCamera canvas with specified camera dimension and parent
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <param name="cameraDimension">TwoD or ThreeD</param>
    /// <param name="parent">Parent GameObject</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateCameraCanvas(string canvasName, CameraDimension cameraDimension, GameObject parent)
    {
        var camera = GetCamera(cameraDimension);
        var canvas = CreateCanvas(canvasName, RenderMode.ScreenSpaceCamera, camera, parent);
        return canvas;
    }

    /// <summary>
    ///     Create canvas base method
    /// </summary>
    /// <param name="canvasName">Canvas Name</param>
    /// <param name="renderMode">Render Mode</param>
    /// <param name="camera">Camera GameObject</param>
    /// <param name="parent">Parent GameObject</param>
    /// <returns>Canvas GameObject</returns>
    public static GameObject CreateCanvas(string canvasName, RenderMode renderMode, Camera camera, GameObject parent)
    {
        var canvasGameObject = new GameObject(canvasName);
        var canvas = canvasGameObject.AddComponent<Canvas>();
        var canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
        canvasGameObject.AddComponent<GraphicRaycaster>();
        canvas.renderMode = renderMode;

        if (renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return canvasGameObject;
        }

        canvas.worldCamera = camera;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0f;

        if (parent != null)
        {
            canvasGameObject.SetParent(parent);
        }

        GameObjectCache[canvasName] = canvasGameObject;
        return canvasGameObject;
    }

    [ExitScene]
    private static void ClearCache(object e, SceneEventArgs args) => CameraCache?.Clear();
}