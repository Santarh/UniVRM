using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniVRM10.Extras.Timeline
{
    [TrackClipType(typeof(Vrm10ExpressionAsset))]
    [TrackBindingType(typeof(Vrm10Instance))]
    public class Vrm10ExpressionTrack : TrackAsset
    {
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            var director = gameObject.GetComponent<PlayableDirector>();
            var vrmInstance = director.GetGenericBinding(this) as Vrm10Instance;

            var playable = base.CreatePlayable(graph, gameObject, clip);
            var behaviour = (ScriptPlayable<Vrm10ExpressionBehaviour>)playable;
            behaviour.GetBehaviour().Target = behaviour.GetBehaviour().Target = vrmInstance.Runtime.Expression;

            return playable;
        }
    }
}
