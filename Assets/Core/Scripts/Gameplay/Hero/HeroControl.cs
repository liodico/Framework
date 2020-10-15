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
    protected float HP;
    protected float HP_MAX;

    protected const int ANIM_IDLE_STAGE = 1;
    protected const int ANIM_DEAD_STAGE = 0;

    public HpBar hpBar;
    protected HubInfoHero hubInfoHero;
    public BoxCollider2D collider2D;

    public Vector2 size = new Vector2(0.4f, 0.1f);

    private int stage = -1;
    public virtual int STAGE
    {
        get
        {
            return stage;
        }
        set
        {
            stage = value;
        }
    }

    public void Init()
    {
        //máu của barrier, cái này kế thừa ra 1 cái riêng sau
        HP = GameData.Instance.VehicleData.GetHp();
        HP_MAX = HP;
        type = TYPE_DUMMY;
        
        Refresh();
        GameplayController.Instance.AddHero(this);
    }

    public virtual void Init(HeroData _heroData)
    {
        HP = _heroData.Hp;
        HP_MAX = HP;
        type = TYPE_NORMAL;
        
        Refresh();
        GameplayController.Instance.AddHero(this);
    }

    public virtual void Refresh()
    {
        hpBar.Init();
        hpBar.ShowHP(HP, HP_MAX);
        collider2D.enabled = true;

        stage = -1;
        STAGE = ANIM_IDLE_STAGE;
    }

    public virtual void LinkHubInfoHero(HubInfoHero _hubInfoHero)
    {
        hubInfoHero = _hubInfoHero;
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
    
    public void GetHit(float damage)
    {
        if(IsDead()) return;

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            AnimDead();
        }

        var pos = hpBar.ShowHP(HP, HP_MAX);
        GameplayController.Instance.SpawnHp(pos + new Vector3(0f, 0f, -1f), damage, false, false, TextHp.TEXT_DAMAGE_HP);
        hubInfoHero.ShowHP(HP, HP_MAX);
    }

    public virtual void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, size);
    }

    public virtual void LookAt(Transform target)
    {
        
    }
}