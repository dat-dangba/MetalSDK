using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class InstallPackageVisualElement : VisualElement
    {
        public InstallPackageVisualElement(string sdkName, Action clickEvent)
        {
            Label label = new Label($"{sdkName} is not installed.")
            {
                style =
                {
                    height = 30,
                    backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f),
                    color = Color.red,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    unityTextAlign = TextAnchor.MiddleCenter,
                }
            };
            Add(label);
            Button installButton = new Button(clickEvent)
            {
                text = $"Install {sdkName}",
                style =
                {
                    marginTop = 10
                }
            };
            Add(installButton);
        }
    }
}
