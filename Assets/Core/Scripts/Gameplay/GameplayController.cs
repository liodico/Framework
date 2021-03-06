﻿using FoodZombie;
using FoodZombie.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Common;

[System.Serializable]
public class HeroControlsPool
{
    public List<HeroControl> list;

    public HeroControlsPool()
    {

    }

    public HeroControlsPool(List<HeroControl> _list)
    {
        list = _list;
    }
}

[System.Serializable]
public class EnemyControlsPool
{
    public List<EnemyControl> list;

    public EnemyControlsPool()
    {

    }

    public EnemyControlsPool(List<EnemyControl> _list)
    {
        list = _list;
    }
}

[System.Serializable]
public class BuiltInPoolingBullet
{
    public Bullet prefab;
    public List<Bullet> builtIn;
}

public class GameplayController : MonoBehaviour
{
    private static GameplayController instance;
    public static GameplayController Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
    }

    private HubPanel hubPanel;

    public Camera camera;
    
    //pool
    public Transform[] heroSpawnPoses;
    public Transform transformPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<HeroControlsPool> heroControlsPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<EnemyControlsPool> enemyControlsPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<HeroControl> barriersPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<TextHp> textHpsPool;

    private PoolsContainer<Bullet> bulletPools;
    [SerializeField] private List<BuiltInPoolingBullet> builtInBullet;
    
    private List<EnemyData> listEnemyDatas;

    private List<HeroControl> heroes;
    [SerializeField] private HeroControl baseHome;
    private List<EnemyControl> enemies;

    private SystemKoTrungUnit systemKoTrung = new SystemKoTrungUnit();
    private List<EnemyControl> koTrungEnemies;
    private Dictionary<int, int> totalKills;

    public bool autoPlay = false;
    public TapToTarget tapToTarget;    
    void Start()
    {
        StartCoroutine(IEAutoSpawnEnemy());
        StartCoroutine(IECheckKoTrung());
    }

    public void Init(HubPanel _hubPanel)
    {
        foreach (var item in heroControlsPool)
        {
            item.list.Free();
        }
        foreach (var item in enemyControlsPool)
        {
            item.list.Free();
        }

        barriersPool.Free();
        textHpsPool.Free();
        
        bulletPools = new PoolsContainer<Bullet>("PoolsBullet", 20, transformPool);
        for (int i = 0; i < builtInBullet.Count; i++)
        {
            var prefab = builtInBullet[i].prefab;
            var pool = bulletPools.Get(prefab);
            pool.AddOutsiders(builtInBullet[i].builtIn);
        }
        
        hubPanel = _hubPanel;
        
        listEnemyDatas = GameData.Instance.EnemiesGroup.GetAllEnemyDatas();
        heroes = new List<HeroControl>();
        baseHome.Init(GameData.Instance.VehicleData.GetHp());
        enemies = new List<EnemyControl>();
        koTrungEnemies = new List<EnemyControl>();

        totalKills = new Dictionary<int, int>();
        
        //load team
        var heroDatas = GameData.Instance.HeroesGroup.GetEquippedHeroes();
        var lenght = heroDatas.Count;
        if (lenght > heroSpawnPoses.Length) lenght = heroSpawnPoses.Length;
        for (int i = 0; i < lenght; i++)
        {
            SpawnHero(heroDatas[i], heroSpawnPoses[i].position);
        }

        LoadAutoPlay();
    }

    public void SpawnHero(HeroData heroData, Vector3 heroSpawnPos)
    {
        var idHero = heroData.Id;
        var heroControl = heroControlsPool[idHero - 1].list.Obtain(transformPool);
        var pos = heroSpawnPos;
        heroControl.transform.position = pos;
        heroControl.Init(heroData);
        heroControl.SetActive(true);
        hubPanel.LinkHubInfoHero((HeroExControl) heroControl);
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        var idEnemy = enemyData.Id;
        var enemyControl = enemyControlsPool[idEnemy - 1].list.Obtain(transformPool);
        var pos = new Vector3(Config.EasyRandom(-2.3f, 2.3f), Config.EasyRandom(9f, 11f), OderLayerZ.PIVOT_POINT);
        enemyControl.transform.position = pos;
        enemyControl.Init(enemyData);
        enemyControl.SetActive(true);
        //mMaxHpWave += enemyControl.MaxHp;
    }

    private IEnumerator IEAutoSpawnEnemy()
    {
        while(true)
        {
            SpawnEnemy(listEnemyDatas[Config.EasyRandom(listEnemyDatas.Count)]);
            SpawnEnemy(listEnemyDatas[Config.EasyRandom(listEnemyDatas.Count)]);
            SpawnEnemy(listEnemyDatas[Config.EasyRandom(listEnemyDatas.Count)]);
            SpawnEnemy(listEnemyDatas[Config.EasyRandom(listEnemyDatas.Count)]);
            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator IECheckKoTrung()
    {
        while (true)
        {
            systemKoTrung.UpdateCheckKoTrung(koTrungEnemies);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SpawnHp(Vector3 pos, float hp, bool isRight = true, bool crit = false, int typeAttack = TextHp.TEXT_DAMAGE_HP)
    {
        var textHp = textHpsPool.Obtain(transformPool);
        textHp.transform.position = pos;
        textHp.SetInfo(hp, isRight, crit, typeAttack);
        textHp.SetActive(true);
    }
    
    public List<HeroControl> GetHeroes()
    {
        return heroes;
    }

    public void AddHero(HeroControl hero)
    {
        heroes.Add(hero);
    }

    public void RemoveHero(HeroControl hero)
    {
        heroes.Remove(hero);
        if (hero == baseHome)
        {
            var rewards = GameData.Instance.MissionsGroup.Lose(Config.TYPE_MODE_NORMAL);
            MainGamePanel.instance.ShowLosePanel(rewards);
        }
    }

    public List<EnemyControl> GetEnemies()
    {
        return enemies;
    }

    public void AddEnemy(EnemyControl enemy)
    {
        enemies.Add(enemy);
        if (enemy.type != EnemyType.TYPE_DUMMY) koTrungEnemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyControl enemy)
    {
        enemies.Remove(enemy);
        if (enemy.type != EnemyType.TYPE_DUMMY) {
            var enemyEx = (EnemyExControl)enemy;
            var id = enemyEx.enemyData.Id;
            if (!totalKills.ContainsKey(id))
            {
                var count = 1;
                totalKills.Add(id, count);
            }
            else
            {
                var count = totalKills[id]++;
                totalKills[id] = count;
            }

            koTrungEnemies.Remove(enemy);
        }
        else
        {
            var rewards = GameData.Instance.MissionsGroup.Cleared(Config.TYPE_MODE_NORMAL, totalKills);
            MainGamePanel.instance.ShowWinPanel(rewards, 0);
        }
    }
    
    public Bullet SpawnBullet(float atk, Bullet bullet, Vector3 pos, Quaternion rot)
    {
        var pool = bulletPools.Get(bullet);
        var obj = pool.Spawn(pos, true);
        obj.transform.rotation = rot;
        obj.Init(atk);
        
        BulletManager.instance.AddBullet(obj);
        return obj;
    }

    public void ReleaseBullet(Bullet bullet)
    {
        BulletManager.instance.RemoveBullet(bullet);
        bulletPools.Release(bullet);
    }

    public void SetHeroesLookAt(Transform target)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].LookAt(target);
        }
    }

    public void PauseGame()
    {
        //Config.stageGamePlay = Config.STAGE_GAMEPLAY_PAUSE;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        //Config.stageGamePlay = Config.STAGE_GAMEPLAY_PLAYING;
        Time.timeScale = 1f;
    }

    public void ChangeAutoPlay()
    {
        autoPlay = !autoPlay;
        LoadAutoPlay();
    }

    public void LoadAutoPlay()
    {
        tapToTarget.SetActive(!autoPlay);
    }

    public void AddBarrier(Vector2 pos)
    {
        var barrier = barriersPool.Obtain(transformPool);
        barrier.Init(2000f);
        barrier.transform.position = new Vector3(pos.x, pos.y, OderLayerZ.PIVOT_POINT);
        barrier.SetActive(true);
    }
}
