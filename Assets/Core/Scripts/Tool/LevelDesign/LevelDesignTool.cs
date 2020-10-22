using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utilities.Common;
using Utilities.Components;
using System.IO;
using FoodZombie;
using UnityEngine.EventSystems;
using Utilities.Inspector;

public class LevelDesignTool : MonoBehaviour
{
    [Separator("List Maps")]
    public SimpleTMPButton btnMapPrefab;
    public Transform transformBtnMapsPool;
    private MapInfo currentMapInfo;

    [Separator("Map Settings")]
    public TMP_InputField inputMapName;
    public TMP_InputField inputMissionNumber;
    public TMP_InputField inputCoinStartBonus;
    public TMP_InputField inputCoinNextMissionAdd;
    public TMP_InputField inputUserEXPStartBonus;
    public TMP_InputField inputUserEXPNextMissionAdd;
    public TMP_InputField inputHeroEXPStartBonus;
    public TMP_InputField inputHeroEXPNextMissionAdd;
    public TMP_InputField inputGemBonus;

    public SimpleTMPButton btnBuildMap;

    [Separator("Enemies List")]
    public Enemy enemy;
    public SimpleTMPButton btnEnemyPrefab;
    public GameObject btnEnemyHighLightPrefab;
    public Transform transformBtnEnemiesPool;
    public List<GameObject> listBtnEnemyHighLight;
    public List<SimpleTMPButton> listBtnEnemy;
    private int currentEnemyIndex = 0;
    public SimpleTMPButton btnAddEnemy;
    public SimpleTMPButton btnRemoveEnemy;

    [Separator("Mission List")]
    public SimpleTMPButton btnMissionPrefab;
    public GameObject btnMissionHighLightPrefab;
    public TextMeshProUGUI txtMissionPowerPrefab;
    public Transform transformBtnMissionsPool;
    public List<GameObject> listBtnMissionHighLight;
    public List<SimpleTMPButton> listBtnMission;
    private int currentMissionIndex = 0;
    private MissionInfo currentMissionInfo;
    
    [Separator("Mission setting")]
    public TMP_InputField inputMissionWaveNumber;
    public TMP_InputField inputMissionCoinBonus;
    public TMP_InputField inputMissionGemBonus;
    public TMP_InputField inputMissionUserEXPBonus;
    public TMP_InputField inputMissionHeroEXPBonus;
    
    [Separator("Wave List")]
    public SimpleTMPButton btnWavePrefab;
    public GameObject btnWaveHighLightPrefab;
    public Transform transformBtnWavesPool;
    public List<GameObject> listBtnWaveHighLight;
    public List<SimpleTMPButton> listBtnWave;
    public TMP_InputField inputWaveTime;
    private int currentWaveIndex = 0;
    private WaveInfo currentWaveInfo;
    
    [Separator("Block List")]
    public BlockEnemy btnBlockPrefab;
    public Transform transformBtnBlocksPool;
    public List<BlockEnemy> listBtnBlock;
    
    [Separator("Mission Enemies List")]
    public EnemyInfoTool btnMissionEnemyPrefab;
    public Transform transformBtnMissionEnemiesPool;
    public List<EnemyInfoTool> listBtnMissionEnemy;
    private int currentMissionEnemyIndex = 0;
    private EnemyInfo currentMissionEnemyInfo;

    [Separator("Wave Statistic")]
    public EnemyStatisticTool enemyStatisticToolPrefab;
    public Transform transformEnemyStatisticToolsPool;
    public List<EnemyStatisticTool> listEnemyStatisticTool;
    public TextMeshProUGUI txtTotalPower;
    public TextMeshProUGUI txtTotalCoinDrop;

    
    public SimpleTMPButton btnSave;

    private int currentMapIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        listBtnEnemy = new List<SimpleTMPButton>();
        listBtnEnemyHighLight = new List<GameObject>();
        listBtnMission = new List<SimpleTMPButton>();
        listBtnMissionHighLight = new List<GameObject>();
        
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "/Core/Resources/LevelDesign/");
        int count = dir.GetDirectories().Length;
        for (int i = 0; i < count; i++)
        {
            int index = i;
            var btnMap = GameObject.Instantiate(btnMapPrefab, transformBtnMapsPool);
            btnMap.onClick.AddListener(() =>
            {
                BtnMap_Pressed(index);
            });
            btnMap.labelTMP.text = "Map " + (index + 1);
            btnMap.SetActive(true);
        }

        btnBuildMap.onClick.AddListener(BtnBuildMap_Pressed);
        btnSave.onClick.AddListener(BtnSave_Pressed);
        inputMissionWaveNumber.onValueChanged.AddListener(InputMissionWaveNumber_Changed);
        inputWaveTime.onValueChanged.AddListener(InputWaveTime_Changed);
        
        btnAddEnemy.onClick.AddListener(BtnAddEnemy_Pressed);
        btnRemoveEnemy.onClick.AddListener(BtnRemoveEnemy_Pressed);

        LoadMap();
        LoadAllEnemy();
        LoadAllMission();
    }

    private void Clear()
    {
        inputMapName.text = "";
        inputMissionNumber.text = "";
        inputCoinStartBonus.text = "";
        inputCoinNextMissionAdd.text = "";
        inputUserEXPStartBonus.text = "";
        inputUserEXPNextMissionAdd.text = "";
        inputHeroEXPStartBonus.text = "";
        inputHeroEXPNextMissionAdd.text = "";
        inputGemBonus.text = "";
        // for (int i = listBtnEnemy.Count - 1; i >= 0; i--)
        // {
        //     Destroy(listBtnEnemy[i].gameObject);
        // }
        // listBtnEnemy = new List<SimpleTMPButton>();
        // listBtnEnemyHighLight = new List<GameObject>();
        
        inputMissionWaveNumber.text = "";
        inputMissionCoinBonus.text = "";
        inputMissionGemBonus.text = "";
        inputMissionUserEXPBonus.text = "";
        inputMissionHeroEXPBonus.text = "";
       
        for (int i = listBtnMission.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnMission[i].gameObject);
        }
        listBtnMission = new List<SimpleTMPButton>();
        listBtnMissionHighLight = new List<GameObject>();
        
        for (int i = listBtnWave.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnWave[i].gameObject);
        }
        listBtnWave = new List<SimpleTMPButton>();
        listBtnWaveHighLight = new List<GameObject>();
        
        for (int i = listBtnBlock.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnBlock[i].gameObject);
        }
        listBtnBlock = new List<BlockEnemy>();
        
        for (int i = listBtnMissionEnemy.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnMissionEnemy[i].gameObject);
        }
        listBtnMissionEnemy = new List<EnemyInfoTool>();
        inputWaveTime.text = "";
    }

    private string GetFolderMap()
    {
        return Application.dataPath + "/Core/Resources/LevelDesign/Map" + (currentMapIndex + 1);
    }

    private void BtnMap_Pressed(int index)
    {
        currentMapIndex = index;
        if (File.Exists(GetFolderMap() + "/mapInfo.json"))
        {
            LoadMap();
            LoadAllMission();
        }
        else
        {
            Clear();
        }
    }
    
    private void BtnBuildMap_Pressed()
    {
        currentMapInfo = new MapInfo(currentMapIndex, 
                                        inputMapName.text, 
                                        int.Parse(inputMissionNumber.text), 
                                        int.Parse(inputCoinStartBonus.text), 
                                        int.Parse(inputCoinNextMissionAdd.text), 
                                        int.Parse(inputUserEXPStartBonus.text), 
                                        int.Parse(inputUserEXPNextMissionAdd.text), 
                                        int.Parse(inputHeroEXPStartBonus.text), 
                                        int.Parse(inputHeroEXPNextMissionAdd.text), 
                                        int.Parse(inputGemBonus.text)
        );
        var json = JsonUtility.ToJson(currentMapInfo);
        
        WriteString(GetFolderMap() + "/mapInfo.json", json);

        var misionNumber = currentMapInfo.missionNumber;
        for (int i = 0; i < misionNumber; i++)
        {
            WriteString(GetFolderMap() + "/mission_" + (i + 1) + ".json", "");
        }

        LoadAllMission();
    }

    private void LoadMap()
    {
        if(File.Exists(GetFolderMap() + "/mapInfo.json"))
        {
            currentMapInfo = JsonUtility.FromJson<MapInfo>(ReadString(GetFolderMap() + "/mapInfo.json"));
            
            inputMapName.text = currentMapInfo.mapName;
            inputMissionNumber.text = currentMapInfo.missionNumber + "";
            inputCoinStartBonus.text = currentMapInfo.coinStartBonus + "";
            inputCoinNextMissionAdd.text = currentMapInfo.coinNextMissionAdd + "";
            inputUserEXPStartBonus.text = currentMapInfo.userEXPStartBonus + "";
            inputUserEXPNextMissionAdd.text = currentMapInfo.userEXPNextMissionAdd + "";
            inputHeroEXPStartBonus.text = currentMapInfo.heroEXPStartBonus + "";
            inputHeroEXPNextMissionAdd.text = currentMapInfo.heroEXPNextMissionAdd + "";
            inputGemBonus.text = currentMapInfo.gemBonus + "";
        }
    }

    private void LoadAllEnemy()
    {
        for (int i = listBtnEnemy.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnEnemy[i].gameObject);
        }
        listBtnEnemy = new List<SimpleTMPButton>();
        listBtnEnemyHighLight = new List<GameObject>();

        var lenght = enemy.dataArray.Length;
        for (int i = 0; i < lenght; i++)
        {
            int index = i;
            var btnEnemy = GameObject.Instantiate(btnEnemyPrefab, transformBtnEnemiesPool);
            btnEnemy.onClick.AddListener(() =>
            {
                ChoiceEnemy(index);
            });
            btnEnemy.img.sprite = AssetsCollection.instance.enemyIcon.GetAsset((int)enemy.dataArray[i].ENEMYID - 1);
            btnEnemy.SetActive(true);
            var btnEnemyHighLight = GameObject.Instantiate(btnEnemyHighLightPrefab, btnEnemy.transform);
            btnEnemyHighLight.transform.localPosition = Vector3.zero;
            btnEnemyHighLight.SetActive(false);
            listBtnEnemyHighLight.Add(btnEnemyHighLight);
            listBtnEnemy.Add(btnEnemy);
        }

        currentEnemyIndex = 0;
        ChoiceEnemy();
    }

    private void ChoiceEnemy()
    {
        for (int i = 0; i < listBtnEnemyHighLight.Count; i++)
        {
            listBtnEnemyHighLight[i].SetActive(false);
        } 
        listBtnEnemyHighLight[currentEnemyIndex].SetActive(true);
    }
    
    private void ChoiceEnemy(int index)
    {
        currentEnemyIndex = index;
        ChoiceEnemy();
    }

    private void LoadAllMission()
    {
        if (currentMapInfo != null)
        {
            for (int i = listBtnMission.Count - 1; i >= 0; i--)
            {
                Destroy(listBtnMission[i].gameObject);
            }
            listBtnMission = new List<SimpleTMPButton>();
            listBtnMissionHighLight = new List<GameObject>();

            var lenght = currentMapInfo.missionNumber;
            for (int i = 0; i < lenght; i++)
            {
                int index = i;
                var btnMission = GameObject.Instantiate(btnMissionPrefab, transformBtnMissionsPool);
                btnMission.onClick.AddListener(() =>
                {
                    ChoiceMission(index);
                });
                btnMission.SetActive(true);
                btnMission.labelTMP.text = "Mission " + (index + 1);
                var btnMissionHighLight = GameObject.Instantiate(btnMissionHighLightPrefab, btnMission.transform);
                btnMissionHighLight.transform.localPosition = Vector3.zero;
                btnMissionHighLight.SetActive(false);
                var txtMissionPower = GameObject.Instantiate(txtMissionPowerPrefab, btnMission.transform);
                txtMissionPower.transform.localPosition = new Vector3(80f, 0f, 0f);
                txtMissionPower.SetActive(true);
                txtMissionPower.text = "P 4000";
                listBtnMissionHighLight.Add(btnMissionHighLight);
                listBtnMission.Add(btnMission);
            }

            currentMissionIndex = 0;
            ChoiceMission();
        }
    }

    private void ChoiceMission()
    {
        for (int i = 0; i < listBtnMissionHighLight.Count; i++)
        {
            listBtnMissionHighLight[i].SetActive(false);
        } 
        listBtnMissionHighLight[currentMissionIndex].SetActive(true);

        if (File.Exists(GetFolderMap() + "/mission_" + (currentMissionIndex + 1) + ".json"))
        {
            var missionInfo = JsonUtility.FromJson<MissionInfo>(ReadString(GetFolderMap() + "/mission_" + (currentMissionIndex + 1) + ".json"));
            if (missionInfo != null)
            {
                currentMissionInfo = missionInfo;
            }
            else
            {
                var wayInfos = new List<WaveInfo>();
                for (int i = 0; i < 10; i++)
                {
                    var enemiesInfos = new EnemyInfo[11 * 13];
                    for (int j = 0; j < enemiesInfos.Length; j++)
                    {
                        enemiesInfos[j] = new EnemyInfo();
                    }
                    wayInfos.Add(new WaveInfo(i, 10f, enemiesInfos));
                }
                
                currentMissionInfo = new MissionInfo(currentMissionIndex, 10, 
                    currentMapInfo.coinStartBonus + currentMapInfo.coinNextMissionAdd * currentMissionIndex,
                    currentMapInfo.userEXPStartBonus + currentMapInfo.userEXPNextMissionAdd * currentMissionIndex,
                    currentMapInfo.heroEXPStartBonus + currentMapInfo.heroEXPNextMissionAdd * currentMissionIndex,
                    currentMapInfo.gemBonus,
                    wayInfos,
                    new List<EnemyInfo>()
                    );
            }
            inputMissionWaveNumber.text = currentMissionInfo.waveNumber + "";
            inputMissionCoinBonus.text = currentMissionInfo.coinBonus + "";
            inputMissionGemBonus.text = currentMissionInfo.gemBonus + "";
            inputMissionUserEXPBonus.text = currentMissionInfo.userEXPBonus + "";
            inputMissionHeroEXPBonus.text = currentMissionInfo.heroEXPBonus + "";

            LoadAllWave();
            LoadAllMissionEnemy();
        }
    }
    
    private void ChoiceMission(int index)
    {
        currentMissionIndex = index;
        ChoiceMission();
    }

    private void BtnSave_Pressed()
    {
        // currentMissionInfo.id = currentMissionIndex;
        currentMissionInfo.waveNumber = int.Parse(inputMissionWaveNumber.text);
        currentMissionInfo.coinBonus = int.Parse(inputMissionCoinBonus.text);
        currentMissionInfo.userEXPBonus = int.Parse(inputMissionUserEXPBonus.text);
        currentMissionInfo.heroEXPBonus = int.Parse(inputMissionHeroEXPBonus.text);
        currentMissionInfo.gemBonus = int.Parse(inputGemBonus.text);
        
            
        var json = JsonUtility.ToJson(currentMissionInfo);
        
        WriteString(GetFolderMap()  + "/mission_" + (currentMissionIndex + 1) + ".json", json);
    }

    private void InputMissionWaveNumber_Changed(string s)
    {
        if (!s.Equals(""))
        {
            currentMissionInfo.waveNumber = int.Parse(inputMissionWaveNumber.text);
            LoadAllWave();
        }
    }

    private void LoadAllWave()
    {
        for (int i = listBtnWave.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnWave[i].gameObject);
        }
        listBtnWave = new List<SimpleTMPButton>();
        listBtnWaveHighLight = new List<GameObject>();

        var lenght = currentMissionInfo.waveNumber;
        for (int i = 0; i < lenght; i++)
        {
            int index = i;
            var btnWave = GameObject.Instantiate(btnWavePrefab, transformBtnWavesPool);
            btnWave.onClick.AddListener(() =>
            {
                ChoiceWave(index);
            });
            btnWave.SetActive(true);
            btnWave.labelTMP.text = "Wave " + (index + 1);
            var btnWaveHighLight = GameObject.Instantiate(btnWaveHighLightPrefab, btnWave.transform);
            btnWaveHighLight.transform.localPosition = Vector3.zero;
            btnWaveHighLight.SetActive(false);
            listBtnWaveHighLight.Add(btnWaveHighLight);
            listBtnWave.Add(btnWave);
        }

        currentWaveIndex = 0;
        ChoiceWave();
    }
    
    private void ChoiceWave()
    {
        currentWaveInfo = currentMissionInfo.waveInfos[currentWaveIndex];
        
        for (int i = 0; i < listBtnWaveHighLight.Count; i++)
        {
            listBtnWaveHighLight[i].SetActive(false);
        } 
        listBtnWaveHighLight[currentWaveIndex].SetActive(true);
        
        LoadAllBlock();
        
        inputWaveTime.text = currentWaveInfo.time + "";
    }
    
    private void ChoiceWave(int index)
    {
        currentWaveIndex = index;
        ChoiceWave();
    }
    
    private void LoadAllBlock()
    {
        for (int i = listBtnBlock.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnBlock[i].gameObject);
        }
        listBtnBlock = new List<BlockEnemy>();

        var length = currentWaveInfo.enemyInfos.Length;
        for (int i = 0; i < length; i++)
        {
            var enemyInfo = currentWaveInfo.enemyInfos[i];
            var btnBlock = GameObject.Instantiate(btnBlockPrefab, transformBtnBlocksPool);
            btnBlock.Init(i, enemyInfo, AddEnemy, RemoveEnemy);
            btnBlock.SetActive(true);
            listBtnBlock.Add(btnBlock);
        }

        ShowWaveStatistic();
    }
    
    private void AddEnemy(int index)
    {
        currentWaveInfo.enemyInfos[index] = currentMissionEnemyInfo;

        LoadAllBlock();
    }

    private void RemoveEnemy(int index)
    {
        currentWaveInfo.enemyInfos[index] = new EnemyInfo(0, 0);

        LoadAllBlock();
    }

    private void LoadAllMissionEnemy()
    {
        for (int i = listBtnMissionEnemy.Count - 1; i >= 0; i--)
        {
            Destroy(listBtnMissionEnemy[i].gameObject);
        }
        listBtnMissionEnemy = new List<EnemyInfoTool>();

        var length = currentMissionInfo.enemyInfos.Count;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            var enemyInfo = currentMissionInfo.enemyInfos[index];
            var btnMissionEnemy = GameObject.Instantiate(btnMissionEnemyPrefab, transformBtnMissionEnemiesPool);
            btnMissionEnemy.Init(index, enemyInfo, ChoiceMissionEnemy, ChangeMissionEnemy);
            btnMissionEnemy.SetActive(true);
            listBtnMissionEnemy.Add(btnMissionEnemy);
        }

        currentMissionEnemyIndex = 0;
        ChoiceMissionEnemy();
    }
    
    private void ChoiceMissionEnemy()
    {
        if (currentMissionInfo.enemyInfos.Count > 0)
        {
            currentMissionEnemyInfo = currentMissionInfo.enemyInfos[currentMissionEnemyIndex];
        }

        for (int i = 0; i < listBtnMissionEnemy.Count; i++)
        {
            listBtnMissionEnemy[i].UnChoice();
        }
        listBtnMissionEnemy[currentMissionEnemyIndex].Choice();
        
        LoadAllBlock();
    }
    
    private void ChoiceMissionEnemy(int index)
    {
        currentMissionEnemyIndex = index;
        ChoiceMissionEnemy();
    }

    private void ChangeMissionEnemy(int index, int level)
    {
        var idOld = currentMissionInfo.enemyInfos[index].id;
        var levelOld = currentMissionInfo.enemyInfos[index].level;
        currentMissionInfo.enemyInfos[index].level = level;
        for (int i = 0; i < currentMissionInfo.waveNumber; i++)
        {
            var enemyInfos = currentMissionInfo.waveInfos[i].enemyInfos;
            for (int j = 0; j < enemyInfos.Length; j++)
            {
                if (enemyInfos[j].id == idOld && enemyInfos[j].level == levelOld)
                {
                    enemyInfos[j].level = level;
                }
            }
        }
        LoadAllBlock();
    }
    
    private void BtnAddEnemy_Pressed()
    {
        var enemyId = (int) enemy.dataArray[currentEnemyIndex].ENEMYID;
        currentMissionInfo.enemyInfos.Add(new EnemyInfo(enemyId, 1));
        
        LoadAllMissionEnemy();
    }

    private void BtnRemoveEnemy_Pressed()
    {
        // var enemyId = (int) enemy.dataArray[currentEnemyIndex].ENEMYID;
        // if (currentMissionInfo.enemyInfos.Contains(enemyId))
        // {
        //     currentMissionInfo.enemyInfos.Remove(enemyId);
        // }

        LoadAllMissionEnemy();
    }
    
    private void InputWaveTime_Changed(string s)
    {
        if (!s.Equals(""))
        {
            currentWaveInfo.time = int.Parse(inputWaveTime.text);
        }
    }

    private void ShowWaveStatistic()
    {
        for (int i = listEnemyStatisticTool.Count - 1; i >= 0; i--)
        {
            Destroy(listEnemyStatisticTool[i].gameObject);
        }
        listEnemyStatisticTool = new List<EnemyStatisticTool>();

        var length = currentMissionInfo.enemyInfos.Count;
        int totalCoin = 0;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            int count = 0;
            var enemyInfo = currentMissionInfo.enemyInfos[index];
            for (int j = 0; j < currentWaveInfo.enemyInfos.Length; j++)
            {
                var enemyInfoInWave = currentWaveInfo.enemyInfos[j];
                if (enemyInfoInWave.id == enemyInfo.id && enemyInfoInWave.level == enemyInfo.level)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                var enemyStatisticTool = GameObject.Instantiate(enemyStatisticToolPrefab, transformEnemyStatisticToolsPool);
                var coinDrop = enemy.dataArray[enemyInfo.id].Coindrop;
                enemyStatisticTool.Init(enemyInfo, count, 0, enemy.dataArray[enemyInfo.id].Coindrop);
                totalCoin += count * coinDrop;
                enemyStatisticTool.SetActive(true);
                listEnemyStatisticTool.Add(enemyStatisticTool);
            }
        }

        txtTotalPower.text = "Power 0";
        txtTotalCoinDrop.text = "Coin " + totalCoin;
    }
    
    //===========================================
    private void WriteString(string path, string text)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(text);
        writer.Close();
    }
    
    private string ReadString(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        var text = reader.ReadToEnd();
        reader.Close();

        return text;
    }
}
