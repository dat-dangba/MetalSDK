using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class BaseVisualElement : VisualElement
    {
        protected void DrawSetting<T>() where T : BaseEditorSetting
        {
            ScriptableObject setting = MetalServicesEditor.GetSetting<T>();
            DrawSetting(setting);
        }

        protected void DrawSetting(ScriptableObject setting)
        {
            SerializedObject settingSerialized = new SerializedObject(setting);
            InspectorElement settingInspector = new InspectorElement(settingSerialized);
            settingInspector.Bind(settingSerialized);
            settingInspector.style.paddingLeft = 0;
            settingInspector.style.paddingRight = 0;
            settingInspector.style.paddingTop = 0;
            settingInspector.style.paddingBottom = 0;
            Add(settingInspector);
        }

        protected void HideScript()
        {
            VisualElement scriptField = this.Q<PropertyField>("PropertyField:m_Script");
            if (scriptField != null)
            {
                scriptField.style.display = DisplayStyle.None;
            }
        }
    }
}
