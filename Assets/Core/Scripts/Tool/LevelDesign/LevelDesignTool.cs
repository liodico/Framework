using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Common;
using Utilities.Components;

public class LevelDesignTool : MonoBehaviour
{
    [SerializeField, Tooltip("Buildin Pool")] private List<SimpleTMPButton> btnMapsPool;
    public Transform transformBtnMapsPool;

    // Start is called before the first frame update
    void Start()
    {
        btnMapsPool.Free();
        
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.dataPath + "\\Core\\Resources\\LevelDesign\\");
        int count = dir.GetDirectories().Length;
        for (int i = 0; i < count; i++)
        {
            int index = i;
            var btnMap = btnMapsPool.Obtain(transformBtnMapsPool);
            btnMap.onClick.AddListener(() =>
            {
                BtnMap_Pressed(index);
            });
            btnMap.labelTMP.text = "Map " + (index + 1);
            btnMap.SetActive(true);
        }
    }

    private void BtnMap_Pressed(int index)
    {
        
    }
}

/*[System.Serializable]
public class EnemyDefinition : 
{
    
}*/
