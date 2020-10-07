using FoodZombie;
using HedgehogTeam.EasyTouch;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroExControl : HeroControl
{
    //public float knockBack;

    private const int ANIM_RUN_STAGE = 2;
    private const int ANIM_ATTACK_STAGE = 3;

    public HeroMove autoMove;
    public HeroAttack attack;
    public HeroAutoTarget autoTarget;

    //info
    public float MeleeAtk
    {
        get
        {
            return heroData.MeleeAtk;
        }
    }
    public float RangeAtk
    {
        get
        {
            return heroData.RangeAtk;
        }
    }
    public float MeleeAtkSpeed
    {
        get
        {
            return heroData.MeleeAtkSpeed * (1f + random / 5f);
        }
    }
    public float RangeAtkSpeed
    {
        get
        {
            return heroData.RangeAtkSpeed * (1f + random / 5f);
        }
    }
    public float Speed
    {
        get
        {
            return heroData.Speed * (1f + random / 5f);
        }
    }
    //

    private HeroData heroData;

    [HideInInspector]
    public EnemyControl target;
    float random = 1f;

    [SerializeField] private string[] nameOfRunAnimations;

    private int stage = -1;
    public override int STAGE
    {
        get
        {
            return stage;
        }
        set
        {
            TrackEntry trackEntry;
            if (stage != ANIM_DEAD_STAGE)
            {
                switch (value)
                {
                    case ANIM_DEAD_STAGE:
                        if (stage != value)
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0, nameOfDeadAnimations[UnityEngine.Random.Range(0, nameOfDeadAnimations.Length)], false);
                            stage = value;
                        }
                        break;
                    case ANIM_IDLE_STAGE:
                        if (stage != value && attack.currentPlayAttackTime <= 0f)
                        {
                            trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, nameOfIdleAnimations[UnityEngine.Random.Range(0, nameOfIdleAnimations.Length)], true);
                            trackEntry.TimeScale = 1f;
                            stage = value;
                        }
                        break;
                    case ANIM_RUN_STAGE:
                        if (stage != value && attack.currentPlayAttackTime <= 0f)
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0, nameOfRunAnimations[UnityEngine.Random.Range(0, nameOfRunAnimations.Length)], true);
                            stage = value;
                        }
                        break;
                    case ANIM_ATTACK_STAGE:
                        //trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, nameOfAttackAnimations[UnityEngine.Random.Range(0, nameOfAttackAnimations.Length)], false);
                        //trackEntry.Complete += AnimAtk_Complete;
                        //OnAtk(trackEntry);
                        trackEntry = attack.OnAttack();
                        if (trackEntry != null)
                        {
                            stage = value;
                        }
                        break;
                }
            }
        }
    }

    public override void Init(HeroData _heroData)
    {
        base.Init(_heroData);
        heroData = _heroData;
        random = UnityEngine.Random.Range(-1f, 1f);

        autoMove.Init(this);
        if (attack != null) attack.Init(this);
        autoTarget.Init(this);
    }

    public override void StopMove(float _length)
    {
        if (autoMove != null) autoMove.StopMove(_length);
    }
    
    public void AnimAttack()
    {
        STAGE = ANIM_ATTACK_STAGE;
    }

    public void AnimRun()
    {
        STAGE = ANIM_RUN_STAGE;
    }

    public override void OnDead()
    {
        base.OnDead();

        if (autoMove != null) autoMove.enabled = false;
        if (attack != null) attack.End();
        if (autoTarget != null) autoTarget.enabled = false;

        GameplayController.Instance.AddRage(heroData.RageGain);
    }
    
    public DIRECTION GetDirect()
    {
        if (transform.localScale.x >= 0)
        {
            return DIRECTION.Right;
        }
        else
        {
            return DIRECTION.Left;
        }
    }

    public void SetTarget(EnemyControl _target)
    {
        target = _target;
    }
}