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

    private const int ANIM_ATTACK_STAGE = 3;

    public SkeletonAnimation skeletonAnimation;
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

    [SerializeField] protected string[] nameOfIdleAnimations;
    [SerializeField] protected string[] nameOfDeadAnimations;
    
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
                        if (stage != value)
                        {
                            trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, nameOfIdleAnimations[UnityEngine.Random.Range(0, nameOfIdleAnimations.Length)], true);
                            trackEntry.TimeScale = 1f;
                            stage = value;
                        }
                        break;
                    case ANIM_ATTACK_STAGE:
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

        if (attack != null) attack.Init(this);
        autoTarget.Init(this);
    }
    
    public override void Refresh()
    {
        skeletonAnimation.Initialize(false);
        skeletonAnimation.skeleton.A = 1f;

        hpBar.Init();
        hpBar.ShowHP(HP, HP_MAX);
        collider2D.enabled = true;

        stage = -1;
        STAGE = ANIM_IDLE_STAGE;
    }
    
    public override void LinkHubInfoHero(HubInfoHero _hubInfoHero)
    {
        base.LinkHubInfoHero(_hubInfoHero);
        hubInfoHero.Init(heroData.GetIcon());
    }
    
    public void AnimAttack()
    {
        STAGE = ANIM_ATTACK_STAGE;
    }

    public override void OnDead()
    {
        base.OnDead();

        if (attack != null) attack.End();
        if (autoTarget != null) autoTarget.enabled = false;
    }

    public void SetTarget(EnemyControl _target)
    {
        target = _target;
    }

    public override void LookAt(Transform target)
    {
        base.LookAt(target);
        
        skeletonAnimation.transform.rotation = Quaternion.Euler(0f, 0f, Util.GetAngleFromTwoPosition(transform, target, Util.AXIS.X_AND_Y));
    }
}