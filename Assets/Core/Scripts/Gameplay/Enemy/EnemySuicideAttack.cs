using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class EnemySuicideAttack : EnemyAttack
{
    //[SerializeField] private GameObject explosionPrefab;

    //public Vector2 sizeAtt;

    //public override void OnEnable()
    //{
        
    //}

    //public override void End()
    //{
    //    enabled = false;
    //}

    //public override void CheckEvent(Spine.Event e)
    //{
    //    base.CheckEvent(e);

    //    bool eventMatch = (eventDataAttack == e.Data); // Performance recommendation: Match cached reference instead of string.
    //    if (eventMatch)
    //    {
    //        float x = transform.localScale.x;
    //        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), sizeAtt, 0f, Vector2.zero);

    //        foreach (var item in hits)
    //        {
    //            if (item.collider.CompareTag(Config.TAG_HERO))
    //            {
    //                var heroGame = item.collider.GetComponent<HeroControl>();
    //                heroGame.GetHit(enemyExControl.enemyData.GetAtk());
    //            }
    //        }

    //        Instantiate<GameObject>(explosionPrefab, transform.position - 10f * Vector3.forward, Quaternion.identity);
    //    }
    //}

    //void OnDrawGizmosSelected()
    //{
    //    float x = transform.localScale.x;

    //    // Draw a semitransparent blue cube at the transforms position
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), sizeAtt);
    //}
}
