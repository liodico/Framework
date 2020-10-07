using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAutoTarget : MonoBehaviour
{
    private HeroExControl heroExControl;

    private IEnumerator findTarget = null;

    public void Init(HeroExControl _heroExControl)
    {
        heroExControl = _heroExControl;

        if (findTarget != null)
        {
            StopCoroutine(findTarget);
            findTarget = null;
        }

        enabled = true;
    }

    private void OnEnable()
    {
        if (heroExControl != null)
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
        var enemies = GameplayController.Instance.GetEnemies();
        while (true)
        {
            heroExControl.SetTarget(GetNearestEnemy(enemies));

            yield return new WaitForSeconds(0.25f);
        }
    }

    private EnemyControl GetNearestEnemy(List<EnemyControl> enemies)
    {
        if (enemies == null || enemies.Count == 0) return null;

        float min = -1f;
        EnemyControl iMin = null;
        foreach (var item in enemies)
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
