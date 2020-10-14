using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using Spine.Unity;
using Spine;
using System.Collections.Generic;
using System;
using FoodZombie;

public class EnemyControl : MonoBehaviour
{
    public const int TYPE_DUMMY = 1;
    public const int TYPE_NORMAL = 2;
    public int type = TYPE_DUMMY;

    //public float knockBack = 1f;
    protected float HP;
    protected float HP_MAX;

    protected const int ANIM_IDLE_STAGE = 1;
    protected const int ANIM_DEAD_STAGE = 0;

    public HpBar hpBar;
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
        HP = LogicAPI.GetDummyHP();
        HP_MAX = HP;
        type = TYPE_DUMMY;
        
        Refresh();
        GameplayController.Instance.AddEnemy(this);
    }

    public virtual void Init(EnemyData _enemyData)
    {
        HP = _enemyData.Hp;
        HP_MAX = HP;
        type = TYPE_NORMAL;
        
        Refresh();
        GameplayController.Instance.AddEnemy(this);
    }

    public virtual void Refresh()
    {
        hpBar.Init();
        hpBar.ShowHP(HP, HP_MAX);
        collider2D.enabled = true;

        stage = -1;
        STAGE = ANIM_IDLE_STAGE;
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

        GameplayController.Instance.RemoveEnemy(this);
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
        GameplayController.Instance.SpawnHp(pos + new Vector3(0f, 0f, -1f), damage, true, false, TextHp.TEXT_DAMAGE_HP);
    }

    public virtual void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, size);
    }
}