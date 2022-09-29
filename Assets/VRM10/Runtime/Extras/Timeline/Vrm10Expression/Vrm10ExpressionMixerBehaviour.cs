using System.Collections.Generic;
using UnityEngine.Playables;

namespace UniVRM10.Extras.Timeline
{
    public sealed class Vrm10ExpressionMixerBehaviour : PlayableBehaviour
    {
        private readonly Dictionary<ExpressionKey, float> _blendingWeights = new Dictionary<ExpressionKey, float>();
        private readonly Dictionary<ExpressionKey, float> _defaultWeights = new Dictionary<ExpressionKey, float>();

        private Vrm10Instance _bindingInstance;
        private bool _isFirstFrameHappened;
        private Vrm10RuntimeExpression _target;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _bindingInstance = playerData as Vrm10Instance;
            if (_bindingInstance == null) return;
            if (_bindingInstance.Runtime == null) return;
            _target = _bindingInstance.Runtime.Expression;
            if (_target == null) return;

            if (!_isFirstFrameHappened)
            {
                _isFirstFrameHappened = true;
                _defaultWeights.Clear();
                foreach (var key in _target.ExpressionKeys)
                {
                    _defaultWeights.Add(key, 0f);
                }
            }

            var inputCount = playable.GetInputCount();
            _blendingWeights.Clear();
            for (var idx = 0; idx < inputCount; ++idx)
            {
                var inputWeight = playable.GetInputWeight(idx);
                var inputPlayable = (ScriptPlayable<Vrm10ExpressionBehaviour>)playable.GetInput(idx);
                var input = inputPlayable.GetBehaviour();

                if (_blendingWeights.ContainsKey(input.Key))
                {
                    _blendingWeights[input.Key] += inputWeight;
                }
                else
                {
                    _blendingWeights.Add(input.Key, inputWeight);
                }
            }

            _target.SetWeights(_blendingWeights);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            _isFirstFrameHappened = false;

            if (_bindingInstance == null) return;
            if (_target == null) return;

            _target.SetWeights(_defaultWeights);
        }
    }
}