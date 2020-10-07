using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using Spine.Unity;
using Spine;
using System.Collections.Generic;
using System;
using FoodZombie;

public class EnemyExControl : EnemyControl
{
    //public float knockBack = 1f;

    private const int ANIM_RUN_STAGE = 2;
    private const int ANIM_ATTACK_STAGE = 3;

    public SkeletonAnimation skeletonAnimation;
    public EnemyMove autoMove;
    public EnemyAttack attack;
    public EnemyAutoTarget autoTarget;

    //info
    public float MeleeAtk
    {
        get
        {
            return enemyData.MeleeAtk;
        }
    }
    public float RangeAtk
    {
        get
        {
            return enemyData.RangeAtk;
        }
    }
    public float MeleeAtkSpeed
    {
        get
        {
            return enemyData.MeleeAtkSpeed * (1f + random / 5f);
        }
    }
    public float RangeAtkSpeed
    {
        get
        {
            return enemyData.RangeAtkSpeed * (1f + random / 5f);
        }
    }
    public float Speed
    {
        get
        {
            return enemyData.Speed * (1f + random / 5f);
        }
    }
    //

    public EnemyData enemyData;

    [HideInInspector]
    public HeroControl target;
    float random = 1f;

    [SerializeField] private string[] nameOfIdleAnimations;
    [SerializeField] private string[] nameOfRunAnimations;
    [SerializeField] private string[] nameOfDeadAnimations;

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

    public override void Init(EnemyData _enemyData)
    {
        base.Init(_enemyData);
        enemyData = _enemyData;
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

        GameplayController.Instance.AddRage(enemyData.baseData.rageGain);
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

    public void SetTarget(HeroControl _target)
    {
        target = _target;
    }
}