using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class EnemyCloseAttack : EnemyAttack
{
    //public Vector2 sizeAtt;
    //public Vector2 rangeAttack = new Vector2(0.2f, 0.05f);

    //public override bool CanAttack()
    //{
    //    if (enemyExControl.target == null) return false;

    //    Vector3 targetPosition = enemyExControl.target.transform.position;
    //    if (transform.position.x < targetPosition.x && enemyExControl.GetDirect() == DIRECTION.Left)
    //        return false;
    //    if (transform.position.x > targetPosition.x && enemyExControl.GetDirect() == DIRECTION.Right)
    //        return false;

    //    return (Mathf.Abs(transform.position.x - targetPosition.x) < rangeAttack.x + enemyExControl.target.size.x / 2
    //            && Mathf.Abs(transform.position.y - targetPosition.y) < rangeAttack.y + enemyExControl.target.size.y / 2);
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
    //                var heroControl = item.collider.GetComponent<HeroControl>();
    //                if(heroControl == enemyExControl.target) heroControl.GetHit(enemyExControl.enemyData.GetAtk());
    //            }
    //        }
    //    }
    //}

    //void OnDrawGizmosSelected()
    //{
    //    float x = transform.localScale.x;

    //    // Draw a semitransparent blue cube at the transforms position
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), sizeAtt);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(transform.position + new Vector3(rangeAttack.x * x / 2, rangeAttack.y / 2, 0f), rangeAttack);
    //}
}
