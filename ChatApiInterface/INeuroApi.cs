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

        protected INeuroApi(float temperature, float repetitionPenalty, float topP)
        {
            _temperature = temperature;
            _repetitionPenalty = repetitionPenalty;
            _topP = topP;
        }

        public abstract Task GenerateBase(string promt, ushort maxTokens, string stopWord);

    }
}
