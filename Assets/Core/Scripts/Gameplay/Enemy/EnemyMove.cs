using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    protected EnemyExControl enemyExControl;
    //protected float MoveThreshold = 0.02f;
    protected float length = 0f;

    public void Init(EnemyExControl _enemyExControl)
    {
        enemyExControl = _enemyExControl;

        enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (length > 0f)
        {
            length -= Time.fixedDeltaTime;
        }
        else
        {
            if (enemyExControl.target != null) Move();
            length = 0f;
        }
    }

    public void StopMove(float _length)
    {
        length = _length;
        LookAtTarget();
    }

    //move to the target
    public void Move()
    {
        LookAtTarget();

        Vector3 targetPosition = enemyExControl.target.transform.position;

        //IDLE
        float totalX = (enemyExControl.size.x + enemyExControl.target.size.x) / 2;
        float totalY = (enemyExControl.size.y + enemyExControl.target.size.y) / 2;
        if (Mathf.Abs(transform.position.x - targetPosition.x) < totalX
            && Mathf.Abs(transform.position.y - targetPosition.y) < totalY)
        {
            return;
        }

        //RUN TO TARGET
        Vector2 delta = targetPosition - transform.position;
        transform.Translate(delta.normalized * enemyExControl.enemyData.Speed * Time.fixedDeltaTime / 10f);
        enemyExControl.AnimRun();
    }

    ////When the Primitive collides with the walls, it will reverse direction
    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if(col.CompareTag(Config.TAG_ENEMY))
    //    {
    //        if (transform.position.x >= col.transform.position.x && length <= 0f)
    //        {
    //            StopMove(0.5f);
    //        }
    //    }
    //}

    private void LookAtTarget()
    {
        if (enemyExControl.target != null)
        {
            float x = enemyExControl.target.transform.position.x;

            if (transform.position.x >= x)
            {
                Left();
            }
            else
            {
                Right();
            }
        }
    }

    private void Left()
    {
        if (enemyExControl.GetDirect() == DIRECTION.Right)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            scale = enemyExControl.hpBar.transform.localScale;
            enemyExControl.hpBar.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }

    private void Right()
    {
        if (enemyExControl.GetDirect() == DIRECTION.Left)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            scale = enemyExControl.hpBar.transform.localScale;
            enemyExControl.hpBar.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}
