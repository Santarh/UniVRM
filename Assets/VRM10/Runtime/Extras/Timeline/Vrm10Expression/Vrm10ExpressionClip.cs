using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniVRM10.Extras.Timeline
{
    [System.Serializable]
    public class Vrm10ExpressionClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] public ExpressionPreset Preset = ExpressionPreset.happy;
        [SerializeField] public string CustomName;

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject)
        {
            var playable = ScriptPlayable<Vrm10ExpressionBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();

            behaviour.Key = new ExpressionKey(Preset, CustomName);

            return playable;
        }
    }
}
