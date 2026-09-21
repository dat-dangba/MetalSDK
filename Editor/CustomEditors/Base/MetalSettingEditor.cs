using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class MetalSettingEditor : UnityEditor.Editor
    {
        protected void HideScript(VisualElement root)
        {
            VisualElement scriptField = root.Q<PropertyField>("PropertyField:m_Script");
            if (scriptField != null)
            {
                scriptField.style.display = DisplayStyle.None;
            }
        }
    }
}
