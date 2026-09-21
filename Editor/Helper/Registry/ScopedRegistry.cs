using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metal.Editor
{
    [Serializable]
    public class ScopedRegistry
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("url")] public string URL { get; set; }

        [JsonProperty("scopes")] public List<string> Scopes { get; set; }
    }
}
