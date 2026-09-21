using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Metal
{
    public class BaseBuildConfig : ScriptableObject
    {
        public FieldInfo[] GetBuildFields()
        {
            return GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).ToArray();
        }
    }
}
