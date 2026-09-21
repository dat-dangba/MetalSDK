using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metal
{
    public static class MetalServices
    {
        private static Dictionary<Type, BaseBuildConfig> BuildConfigs = new();

        public static T GetBuildConfig<T>() where T : BaseBuildConfig
        {
            if (BuildConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.BUILD_CONFIG_FOLDER}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            if (t != null && !BuildConfigs.ContainsKey(t.GetType()))
            {
                BuildConfigs.Add(t.GetType(), t);
            }

            return t;
        }

        public static T GetBuildConfig<T>(string name) where T : BaseBuildConfig
        {
            if (BuildConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.BUILD_CONFIG_FOLDER}/{name}";
            T t = Resources.Load<T>(path);
            if (t != null && !BuildConfigs.ContainsKey(t.GetType()))
            {
                BuildConfigs.Add(t.GetType(), t);
            }

            return t;
        }
    }
}
