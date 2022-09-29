using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UniVRM10.Extras.Timeline
{
    [System.Serializable]
    public class Vrm10ExpressionAsset : PlayableAsset
    {
        [SerializeField] public ExpressionPreset Preset = ExpressionPreset.happy;
        [SerializeField] public string CustomName;
        [SerializeField] public float Weight = 1f;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject)
        {
            var playable = ScriptPlayable<Vrm10ExpressionBehaviour>.Create(graph);

            var behaviour = playable.GetBehaviour();
            behaviour.Key = new ExpressionKey(Preset, CustomName);
            behaviour.Weight = Weight;

            return playable;
        }
    }
}
