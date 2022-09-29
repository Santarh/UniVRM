using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniVRM10.Extras.Timeline
{
    [TrackClipType(typeof(Vrm10ExpressionClip))]
    [TrackBindingType(typeof(Vrm10Instance))]
    public class Vrm10ExpressionTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<Vrm10ExpressionMixerBehaviour>.Create(graph, inputCount);
            var behaviour = playable.GetBehaviour();

            return playable;
        }
    }
}
