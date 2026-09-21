using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metal.Editor
{
    public static class MetalServicesEditor
    {
        private static Dictionary<Type, BaseEditorSetting> EditorSettings = new();

        public static string GetToken()
        {
            string token = GetSetting<GeneralSetting>().SDKToken;
            if (!string.IsNullOrEmpty(token)) return token;
            MetalLog.Log("Chưa nhập token");
            return "";
        }

        public static T GetSetting<T>() where T : BaseEditorSetting
        {
            if (EditorSettings.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.SETTINGS_FOLDER}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            if (t != null && !EditorSettings.ContainsKey(t.GetType()))
            {
                EditorSettings.Add(t.GetType(), t);
            }

            return t;
        }

        public static T GetSetting<T>(string name) where T : BaseEditorSetting
        {
            if (EditorSettings.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.SETTINGS_FOLDER}/{name}";
            T t = Resources.Load<T>(path);
            if (t != null && !EditorSettings.ContainsKey(t.GetType()))
            {
                EditorSettings.Add(t.GetType(), t);
            }

            return t;
        }
    }
}
