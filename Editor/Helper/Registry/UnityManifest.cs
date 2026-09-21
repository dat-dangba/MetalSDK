using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metal.Editor
{
    [Serializable]
    public class UnityManifest
    {
        [JsonProperty("scopedRegistries")] public List<ScopedRegistry> ScopedRegistries { get; set; }

        [JsonProperty("dependencies")] public Dictionary<string, string> Dependencies { get; set; }
    }
}
