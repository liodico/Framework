using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControl : HeroControl
{
    public SpriteRenderer imgBody;
    
    private IEnumerator disapear = null;
    
    public override void Refresh()
    {
        base.Refresh();
        
        if (disapear != null)
        {
            StopCoroutine(disapear);
            disapear = null;
        }
        var color = imgBody.color;
        color.a = 1f;
        imgBody.color = color;
    }

    public override void OnDead()
    {
        base.OnDead();
        
        disapear = IEDisapear();
        StartCoroutine(disapear);
    }
    
    private IEnumerator IEDisapear()
    {
        float timePlay = 1.0f;
        while (timePlay >= 0f)
        {
            yield return null;
            timePlay -= Time.deltaTime;
            var color = imgBody.color;
            color.a = timePlay;
            imgBody.color = color;
        }
        
        gameObject.SetActive(false);
    }
}
