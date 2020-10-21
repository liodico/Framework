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
    
    public MissionInfo(int _id, 
                    int _waveNumber, 
                    int _coinBonus, 
                    int _userEXPBonus, 
                    int _heroEXPBonus,
                    int _gemBonus,
                    List<WaveInfo> _waveInfos)
    {
        id = _id;
        waveNumber = _waveNumber;
        coinBonus = _coinBonus;
        userEXPBonus = _userEXPBonus; 
        heroEXPBonus = _heroEXPBonus;
        gemBonus = _gemBonus;
        waveInfos = _waveInfos;
    }
}

[System.Serializable]
public class WaveInfo
{
    public int id;
    public int[] enemiesIds;

    public WaveInfo(int _id, int[] _enemiesIds)
    {
        id = _id;
        enemiesIds = _enemiesIds;
    }
}
