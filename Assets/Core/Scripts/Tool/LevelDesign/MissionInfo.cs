using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MissionInfo
{
    public int id;
    public int waveNumber;
    public int coinBonus;
    public int userEXPBonus;
    public int heroEXPBonus;
    public int gemBonus;
    public List<WaveInfo> waveInfos;
    public List<EnemyInfo> enemyInfos;
    
    public MissionInfo(int _id, 
                    int _waveNumber, 
                    int _coinBonus, 
                    int _userEXPBonus, 
                    int _heroEXPBonus,
                    int _gemBonus,
                    List<WaveInfo> _waveInfos,
                    List<EnemyInfo> _enemyInfos)
    {
        id = _id;
        waveNumber = _waveNumber;
        coinBonus = _coinBonus;
        userEXPBonus = _userEXPBonus; 
        heroEXPBonus = _heroEXPBonus;
        gemBonus = _gemBonus;
        waveInfos = _waveInfos;
        enemyInfos = _enemyInfos;
    }
}

[System.Serializable]
public class WaveInfo
{
    public int id;
    public float time;
    public EnemyInfo[] enemyInfos;

    public WaveInfo(int _id, float _time, EnemyInfo[] _enemyInfos)
    {
        id = _id;
        time = _time;
        enemyInfos = _enemyInfos;
    }
}

[System.Serializable]
public class EnemyInfo
{
    public int id;
    public int level;

    public EnemyInfo()
    {
        id = 0;
        level = 0;
    }
    
    public EnemyInfo(int _id, int _level)
    {
        id = _id;
        level = _level;
    }
}
