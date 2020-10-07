using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoTarget : MonoBehaviour
{
    private EnemyExControl enemyExControl;

    private IEnumerator findTarget = null;

    public void Init(EnemyExControl _enemyExControl)
    {
        enemyExControl = _enemyExControl;

        if (findTarget != null)
        {
            StopCoroutine(findTarget);
            findTarget = null;
        }

        enabled = true;
    }

    private void OnEnable()
    {
        if (enemyExControl != null)
        {
            findTarget = IEFindTarget();
            StartCoroutine(findTarget);
        }
    }

    private void Start()
    {
        if (findTarget == null)
        {
            findTarget = IEFindTarget();
            StartCoroutine(findTarget);
        }
    }

    // Start is called before the first frame update
    IEnumerator IEFindTarget()
    {
        var heroes = GameplayController.Instance.GetHeroes();
        while (true)
        {
            enemyExControl.SetTarget(GetNearestHero(heroes));

            yield return new WaitForSeconds(1f);
        }
    }

    private HeroControl GetNearestHero(List<HeroControl> heroes)
    {
        if (heroes == null || heroes.Count == 0) return null;

        float min = -1f;
        HeroControl iMin = null;
        foreach (var item in heroes)
        {
            if (item.IsDead()) continue;

            var distance = Vector2.Distance(transform.position, item.transform.position);
            if (min == -1f || distance < min)
            {
                min = distance;
                iMin = item;
            }
        }

        return iMin;
    }
}
