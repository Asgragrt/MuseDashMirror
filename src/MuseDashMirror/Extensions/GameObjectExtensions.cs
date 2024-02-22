using UnityEngine.UI;

namespace MuseDashMirror.Extensions;

/// <summary>
///     GameObject Extension Methods
/// </summary>
[Logger]
public static partial class GameObjectExtensions
{
    /// <summary>
    ///     Set the parent of a GameObject
    /// </summary>
    /// <param name="gameObject">GameObject</param>
    /// <param name="parent">Parent</param>
    public static void SetParent(this GameObject gameObject, GameObject parent) => gameObject.transform.SetParent(parent.transform);

    /// <summary>
    ///     Set the text of a GameObject with a Text component
    /// </summary>
    /// <param name="gameObject">GameObject</param>
    /// <param name="text">Text</param>
    public static void SetText(this GameObject gameObject, string text)
    {
        if (gameObject.GetComponent<Text>() == null)
        {
            Logger.Error($"GameObject {gameObject} does not have a Text component");
            return;
        }

        gameObject.GetComponent<Text>().text = text;
    }

    /// <summary>
    ///     Set the Text component of a GameObject
    /// </summary>
    /// <param name="gameObject">GameObject</param>
    /// <param name="textParameters">Text Parameters</param>
    public static void SetTextComponent(this GameObject gameObject, TextParameters textParameters)
    {
        var textComponent = gameObject.GetComponent<Text>() ?? gameObject.AddComponent<Text>();
        textComponent.text = textParameters.Text;
        textComponent.font = textParameters.Font;
        textComponent.fontSize = textParameters.FontSize;
        textComponent.color = textParameters.Color;
        textComponent.alignment = textParameters.Alignment;
    }

    /// <summary>
    ///     Set the RectTransform of a GameObject
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="transformParameters"></param>
    public static void SetRectTransform(this GameObject gameObject, TransformParameters transformParameters)
    {
        var rectTransform = gameObject.GetComponent<RectTransform>();
        if (transformParameters.IsAutoSize)
        {
            var contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            rectTransform.sizeDelta = transformParameters.SizeDelta;
            rectTransform.localScale = transformParameters.LocalScale;
        }

        if (transformParameters.IsLocalPosition)
        {
            rectTransform.localPosition = transformParameters.Position;
        }
        else
        {
            rectTransform.position = transformParameters.Position;
        }
    }
}