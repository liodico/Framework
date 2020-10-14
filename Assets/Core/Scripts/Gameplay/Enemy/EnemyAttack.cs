using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyAttackType
{
    ATK_TYPE_MELEE,
    ATK_TYPE_RANGE,
    ATK_TYPE_MULTI_MELEE,
    ATK_TYPE_MULTI_RANGE
}

[System.Serializable]
public class EnemyDetailAttack
{
    public string[] nameOfAttackAnimations;
    public EnemyAttackType attackType;

    //vùng đánh của enemy khi animation ra, mỗi enemy có nhiều vùng đánh tương ứng vỡi nhiều anim attack
    public Vector2 originAttack;
    public Vector2 sizeAttack;
    public Color attackColor;
}

public class EnemyAttack : MonoBehaviour {
    protected EnemyExControl enemyExControl;
    private float atkSpeed = 0.75f;

    public string eventNameAttack;
    public Spine.EventData eventDataAttack;

    public EnemyDetailAttack[] detailAttacks;
    //tầm đánh của enemy, mỗi enemy chỉ có một tầm đánh,
    //nhưng trong phạm vi tầm đánh, gần thì có thể đánh melee, xa thì đánh range
    public Vector2 rangeAttack = new Vector2(0.2f, 0.05f);

    private int indexAttack = -1;
    public float currentPlayAttackTime;
    
    public void Init(EnemyExControl _enemyExControl)
    {
        enemyExControl = _enemyExControl;
        currentPlayAttackTime = 0f;

        eventDataAttack = enemyExControl.skeletonAnimation.Skeleton.Data.FindEvent(eventNameAttack);
        enemyExControl.skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
        
        enabled = true;
    }

    private void Update()
    {
        if (currentPlayAttackTime <= 0f)
        {
            if (CanAttack())
            {
                Attack();
            }
        }

        currentPlayAttackTime -= Time.deltaTime;
        if (currentPlayAttackTime < 0f) currentPlayAttackTime = 0f;
    }

    public virtual void Attack()
    {
        enemyExControl.StopMove(atkSpeed);
        enemyExControl.AnimAttack();
    }

    public TrackEntry OnAttack()
    {
        if (currentPlayAttackTime <= 0f)
        {
            currentPlayAttackTime = atkSpeed;

            var animNames = detailAttacks[indexAttack].nameOfAttackAnimations;
            var animName = animNames[UnityEngine.Random.Range(0, animNames.Length)];
            var trackEntry = enemyExControl.skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
            trackEntry.Complete += AnimAttack_Complete;
            var duration = trackEntry.Animation.Duration;
            if(atkSpeed < duration) trackEntry.TimeScale = duration / atkSpeed;
            return trackEntry;
        }
        return null;
    }

    private void AnimAttack_Complete(TrackEntry trackEntry)
    {
        enemyExControl.AnimIdle();
    }

    public void End()
    {
        enabled = false;
    }

    public bool CanAttack()
    {
        if (enemyExControl.target == null) return false;

        Vector3 transformPos = transform.position;

        //nếu hero quay lưng vào target
        Vector3 targetPosition = enemyExControl.target.transform.position;
        if (transformPos.x < targetPosition.x && enemyExControl.GetDirect() == DIRECTION.Left)
        {
            indexAttack = -1;
            return false;
        }
        if (transformPos.x > targetPosition.x && enemyExControl.GetDirect() == DIRECTION.Right)
        {
            indexAttack = -1;
            return false;
        }

        //nếu khoảng cách giữa hero và target nhỏ hơn tầm đánh + một nửa size target
        var canAttack = (Mathf.Abs(transformPos.x - targetPosition.x) < rangeAttack.x / 2f + enemyExControl.target.size.x / 2
                && Mathf.Abs(transformPos.y - targetPosition.y) < rangeAttack.y + enemyExControl.target.size.y / 2);

        if (canAttack)
        {
            for (int i = 0; i < detailAttacks.Length; i++)
            {
                var detailAttack = detailAttacks[i];
                var originAttack = detailAttack.originAttack;
                var sizeAttack = detailAttack.sizeAttack;

                //ở trong vùng đánh nào được xếp trước thì đánh kiểu đấy
                //pos là vị trí khởi đầu vùng đánh
                Vector2 pos = (Vector2)transformPos + transform.localScale.x * (originAttack - sizeAttack / 2f);
                if (Mathf.Abs(pos.x - targetPosition.x) < sizeAttack.x + enemyExControl.target.size.x / 2
                        && Mathf.Abs(pos.y - targetPosition.y) < sizeAttack.y + enemyExControl.target.size.y / 2)
                {
                    indexAttack = i;

                    //tính atkSpeed của atk hiện tại
                    if (detailAttack.attackType == EnemyAttackType.ATK_TYPE_MELEE
                        || detailAttack.attackType == EnemyAttackType.ATK_TYPE_MULTI_MELEE)
                    {
                        atkSpeed = enemyExControl.MeleeAtkSpeed;
                    }
                    else if (detailAttack.attackType == EnemyAttackType.ATK_TYPE_RANGE
                        || detailAttack.attackType == EnemyAttackType.ATK_TYPE_MULTI_RANGE)
                    {
                        atkSpeed = enemyExControl.RangeAtkSpeed;
                    }

                    break;
                }
            }
        }
        else
        {
            indexAttack = -1;
        }

        if (indexAttack == -1) return false;
        return canAttack;
    }

    //-------------active event-----
    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        CheckEvent(e);
    }

    public void CheckEvent(Spine.Event e)
    {
        bool eventMatch = (eventDataAttack == e.Data); // Performance recommendation: Match cached reference instead of string.
        if (indexAttack != -1 && eventMatch)
        {
            var originAtt = detailAttacks[indexAttack].originAttack;
            var sizeAttack = detailAttacks[indexAttack].sizeAttack;
            float x = transform.localScale.x;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + new Vector3(originAtt.x * x, originAtt.y, 0f), sizeAttack, 0f, Vector2.zero);

            var type = detailAttacks[indexAttack].attackType;
            switch (type)
            {
                case EnemyAttackType.ATK_TYPE_MELEE:
                    foreach (var item in hits)
                    {
                        if (item.collider.CompareTag(Config.TAG_HERO))
                        {
                            var heroControl = item.collider.GetComponent<HeroControl>();
                            if (heroControl == enemyExControl.target) heroControl.GetHit(enemyExControl.MeleeAtk);
                        }
                    }
                    break;
                case EnemyAttackType.ATK_TYPE_RANGE:
                    foreach (var item in hits)
                    {
                        if (item.collider.CompareTag(Config.TAG_HERO))
                        {
                            var heroControl = item.collider.GetComponent<HeroControl>();
                            if (heroControl == enemyExControl.target) heroControl.GetHit(enemyExControl.RangeAtk);
                        }
                    }
                    break;
                case EnemyAttackType.ATK_TYPE_MULTI_MELEE:
                    foreach (var item in hits)
                    {
                        if (item.collider.CompareTag(Config.TAG_HERO))
                        {
                            var heroControl = item.collider.GetComponent<HeroControl>();
                            heroControl.GetHit(enemyExControl.MeleeAtk);
                        }
                    }
                    break;
                case EnemyAttackType.ATK_TYPE_MULTI_RANGE:
                    foreach (var item in hits)
                    {
                        if (item.collider.CompareTag(Config.TAG_HERO))
                        {
                            var heroControl = item.collider.GetComponent<HeroControl>();
                            heroControl.GetHit(enemyExControl.RangeAtk);
                        }
                    }
                    break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float x = transform.localScale.x;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -rangeAttack.y / 2f, 0f), rangeAttack);

        //vẽ các vùng đánh
        for (int i = 0; i < detailAttacks.Length; i++)
        {
            var item = detailAttacks[i];
            Gizmos.color = item.attackColor;
            var originAttack = item.originAttack;
            var sizeAttack = item.sizeAttack;
            Gizmos.DrawWireCube(transform.position + new Vector3(originAttack.x * x, originAttack.y, 0f), sizeAttack);
        }
    }
}
