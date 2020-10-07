using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    public SpriteRenderer imgHpBarBG;
    public SpriteRenderer imgHpBar;

    private float length;
    private float sizeY;

    float timeToHidden = 2f;

    private void OnDrawGizmosSelected()
    {
        length = imgHpBar.sprite.rect.width / 100f;
        //imgHpBar.sprite.textureRect.width

        imgHpBar.transform.localPosition = new Vector3(-length / 2f, 0f, 0.01f);
    }

    public void Init()
    {
        length = imgHpBar.sprite.rect.width / 100f;
        sizeY = imgHpBar.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToHidden -= Time.fixedDeltaTime;

        if(timeToHidden <= 0f)
        {
            imgHpBar.color = new Color(1f, 1f, 1f, 0f);
            imgHpBarBG.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public Vector3 ShowHP(float hp, float hpMax)
    {
        if (hp <= 0f)
        {
            timeToHidden = 0f;

            imgHpBar.color = new Color(1f, 1f, 1f, 0f);
            imgHpBarBG.color = new Color(1f, 1f, 1f, 0f);

            return imgHpBar.transform.position;
        }
        else
        {
            imgHpBar.size = new Vector2(length * (hp / hpMax), sizeY);

            timeToHidden = 2f;
            imgHpBar.color = new Color(1f, 1f, 1f, 1f);
            imgHpBarBG.color = new Color(1f, 1f, 1f, 1f);

            return imgHpBar.transform.position + new Vector3(length * (hp / hpMax), 0f, 0f);
        }
    }
}
