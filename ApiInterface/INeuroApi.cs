using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiInterface
{
    public abstract class INeuroApi
    {
        protected float _temperature;
        protected float _repetitionPenalty;
        protected float _topP;
        protected string? _prePrompt;

        protected INeuroApi(float temperature, float repetitionPenalty, float topP, string? prePrompt = default)
        {
            _temperature = temperature;
            _repetitionPenalty = repetitionPenalty;
            _topP = topP;
            _prePrompt = prePrompt;
        }

        public abstract Task<string> GenerateBase(string promt, ushort maxTokens, string stopWord);

    }
}
