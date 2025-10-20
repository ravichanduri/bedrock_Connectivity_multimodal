using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bedrock.Connectivity.Services
{
    public interface IBedrockModelSelector
    {
        Task<string> GetModelForTypeAsync(string inputType);
    }
}
