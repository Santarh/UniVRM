using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UniVRM10.Extras.Timeline
{
    public class Vrm10ExpressionBehaviour : PlayableBehaviour
    {
        public Vrm10RuntimeExpression Target { get; internal set; }
        public ExpressionKey Key { get; internal set; }
        public float Weight { get; internal set; }

        private float _defaultWeight;

        public override void OnGraphStart(Playable playable)
        {
            _defaultWeight = Target.GetWeight(Key);
        }

        public override void OnGraphStop(Playable playable)
        {
            Target.SetWeight(Key, _defaultWeight);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {

        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            Target.SetWeight(Key, 0f);
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            Target.SetWeight(Key, Weight * info.weight);
        }
    }
}
