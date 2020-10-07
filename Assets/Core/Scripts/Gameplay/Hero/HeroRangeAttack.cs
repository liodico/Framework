using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class HeroRangeAttack : HeroAttack
{
    //public float radius;
    //public float rangeAttack = 1f;

    //public override bool CanAttack()
    //{
    //    if (heroControl.target == null) return false;

    //    Vector3 targetPosition = heroControl.target.transform.position;
        
    //    return (Vector2.Distance(transform.position, targetPosition) < rangeAttack);
    //}

    //public override void CheckEvent(Spine.Event e)
    //{
    //    base.CheckEvent(e);

    //    bool eventMatch = (eventDataAttack == e.Data); // Performance recommendation: Match cached reference instead of string.
    //    if (eventMatch)
    //    {
    //        float x = transform.localScale.x;
    //        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), radius, Vector2.zero);

    //        foreach (var item in hits)
    //        {
    //            if (item.collider.CompareTag(Config.TAG_ENEMY))
    //            {
    //                var enemyGame = item.collider.GetComponent<EnemyControl>();
    //                enemyGame.GetHit(heroControl.heroData.GetAtk());
    //            }
    //        }
    //    }
    //}

    //void OnDrawGizmosSelected()
    //{
    //    // Draw a semitransparent blue cube at the transforms position
    //    Gizmos.color = Color.red;
    //    float x = transform.localScale.x;
    //    Gizmos.DrawWireSphere(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), radius);
    //}
}
