using FoodZombie;
using FoodZombie.UI;
using System.Collections;
using System.Collections.Generic;
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

    //stats
    public Transform heroSpawnPos;
    public Transform transformPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<HeroControlsPool> heroControlsPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<EnemyControlsPool> enemyControlsPool;
    [SerializeField, Tooltip("Buildin Pool")] private List<TextHp> textHpsPool;

    private List<EnemyData> listEnemyDatas;

    private List<HeroControl> heroes;
    private List<EnemyControl> enemies;
    [SerializeField] private EnemyControl barrier;

    private SystemKoTrungUnit systemKoTrung = new SystemKoTrungUnit();
    private List<EnemyControl> koTrungEnemies;
    private Dictionary<int, int> totalKills;
    // Start is called before the first frame update
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
        textHpsPool.Free();

        hubPanel = _hubPanel;
        var heroDatas = GameData.Instance.HeroesGroup.GetEquippedHeroes();
        hubPanel.ShowHubHeroButtons(heroDatas);

        listEnemyDatas = GameData.Instance.EnemiesGroup.GetAllEnemyDatas();
        heroes = new List<HeroControl>();
        enemies = new List<EnemyControl>();
        koTrungEnemies = new List<EnemyControl>();
        barrier.Init();

        totalKills = new Dictionary<int, int>();
    }

    public void SpawnHero(HeroData heroData)
    {
        var idHero = heroData.Id;
        var heroControl = heroControlsPool[idHero - 1].list.Obtain(transformPool);
        var pos = heroSpawnPos.position;
        heroControl.transform.position = pos;
        heroControl.Init(heroData);
        heroControl.SetActive(true);
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        var idEnemy = enemyData.Id;
        var enemyControl = enemyControlsPool[idEnemy - 1].list.Obtain(transformPool);
        var pos = new Vector3(Config.EasyRandom(11.3f, 17f), Config.EasyRandom(-5.11f, 1.07f), OderLayerZ.PIVOT_POINT);
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
        if (hero.type == HeroControl.TYPE_DUMMY)
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
        if (enemy.type != EnemyControl.TYPE_DUMMY) koTrungEnemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyControl enemy)
    {
        enemies.Remove(enemy);
        if (enemy.type != EnemyControl.TYPE_DUMMY) {
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
}
