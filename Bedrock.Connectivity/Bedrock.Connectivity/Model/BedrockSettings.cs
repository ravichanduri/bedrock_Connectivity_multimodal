using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bedrock.Connectivity.Model
{
    public class BedrockSettings
    {
        public Dictionary<string, ModelPreference> Models { get; set; } = new();
    }

    public class ModelPreference
    {
        public string Primary { get; set; } = string.Empty;
        public string Secondary { get; set; } = string.Empty;
    }
}
