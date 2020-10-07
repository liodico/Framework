using FoodZombie;
using HedgehogTeam.EasyTouch;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : MonoBehaviour
{
    public const int TYPE_DUMMY = 1;
    public const int TYPE_NORMAL = 2;
    public int type = TYPE_DUMMY;

    //public float knockBack;
    private float HP;
    private float HP_MAX;

    protected const int ANIM_IDLE_STAGE = 1;
    protected const int ANIM_DEAD_STAGE = 0;

    public SkeletonAnimation skeletonAnimation;
    public HpBar hpBar;
    public BoxCollider2D collider2D;

    public Vector2 size = new Vector2(0.4f, 0.1f);

    [SerializeField] protected string[] nameOfIdleAnimations;
    [SerializeField] protected string[] nameOfDeadAnimations;

    private int stage = -1;
    public virtual int STAGE
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
                }
            }
        }
    }

    public void Init()
    {
        //máu của barrier, cái này kế thừa ra 1 cái riêng sau
        HP = GameData.Instance.VehicleData.GetHp();
        HP_MAX = HP;
        hpBar.Init();
        hpBar.ShowHP(HP, HP_MAX);

        collider2D.enabled = true;

        type = TYPE_DUMMY;
        stage = ANIM_IDLE_STAGE;
        GameplayController.Instance.AddHero(this);
    }

    public virtual void Init(HeroData _heroData)
    {
        HP = _heroData.Hp;
        HP_MAX = HP;
        hpBar.Init();
        hpBar.ShowHP(HP, HP_MAX);

        collider2D.enabled = true;
        
        type = TYPE_NORMAL;
        stage = ANIM_IDLE_STAGE;
        GameplayController.Instance.AddHero(this);
    }

    public virtual void StopMove(float _length)
    {

    }

    public void AnimIdle()
    {
        STAGE = ANIM_IDLE_STAGE;
    }
    
    public void AnimDead()
    {
        STAGE = ANIM_DEAD_STAGE;
        OnDead();
    }

    public bool IsDead()
    {
        return STAGE.Equals(ANIM_DEAD_STAGE);
    }

    public void Release()
    {
        Destroy(gameObject);
    }

    public virtual void OnDead()
    {
        collider2D.enabled = false;

        GameplayController.Instance.RemoveHero(this);
    }

    public virtual void IEShowDead()
    {

    }

    public void GetHit(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            AnimDead();
        }

        var pos = hpBar.ShowHP(HP, HP_MAX);
        GameplayController.Instance.SpawnHp(pos + new Vector3(0f, 0f, -1f), damage, false, false, TextHp.TEXT_DAMAGE_HP);
    }

    public virtual void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, size);
    }
}