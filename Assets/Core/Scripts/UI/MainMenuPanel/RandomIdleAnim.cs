using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class RandomIdleAnim : MonoBehaviour
{
    public SkeletonAnimation model;
    public string[] anims;

    // Start is called before the first frame update
    void Start()
    {
        var trackEntry = model.AnimationState.SetAnimation(0, anims[UnityEngine.Random.RandomRange(0, anims.Length)], false);
        trackEntry.Complete += OnComplete;
    }

    private void OnComplete(TrackEntry track)
    {
        var trackEntry = model.AnimationState.SetAnimation(0, anims[UnityEngine.Random.RandomRange(0, anims.Length)], false);
        trackEntry.Complete += OnComplete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
