using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    [CustomEditor(typeof(GeneralSetting))]
    public class GeneralSettingsEditor : MetalSettingEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            HideScript(root);

            return root;
        }
    }
}
