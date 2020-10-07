using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour {
    protected HeroExControl heroExControl;
    //protected float MoveThreshold = 0.02f;
    protected float length = 0f;

    public void Init(HeroExControl _heroExControl)
    {
        heroExControl = _heroExControl;

        enabled = true;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		if (length > 0f)
		{
			length -= Time.fixedDeltaTime;
            heroExControl.AnimIdle();
        }
        else
		{
            if (heroExControl.target != null) Move();
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

        Vector3 targetPosition = heroExControl.target.transform.position;

        //IDLE
        float totalX = (heroExControl.size.x + heroExControl.target.size.x) / 2;
        float totalY = (heroExControl.size.y + heroExControl.target.size.y) / 2;
        if (Mathf.Abs(transform.position.x - targetPosition.x) < totalX
            && Mathf.Abs(transform.position.y - targetPosition.y) < totalY)
        {
            return;
        }

        //RUN TO TARGET
        Vector2 delta = targetPosition - transform.position;
        transform.Translate(delta.normalized * heroExControl.Speed * Time.fixedDeltaTime / 10f);
        heroExControl.AnimRun();
    }

    public void LookAtTarget()
    {
        if (heroExControl.target != null)
        {
            float x = heroExControl.target.transform.position.x;

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
        if (heroExControl.GetDirect() == DIRECTION.Right)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            scale = heroExControl.hpBar.transform.localScale;
            heroExControl.hpBar.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }

    private void Right()
    {
        if (heroExControl.GetDirect() == DIRECTION.Left)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            scale = heroExControl.hpBar.transform.localScale;
            heroExControl.hpBar.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}
