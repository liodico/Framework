using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class EnemyLevelUpStatAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Core/Data/Excel/Enemy.xlsx";
    private static readonly string assetFilePath = "Assets/Core/Data/Excel/EnemyLevelUpStat.asset";
    private static readonly string sheetName = "EnemyLevelUpStat";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            EnemyLevelUpStat data = (EnemyLevelUpStat)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(EnemyLevelUpStat));
            if (data == null) {
                data = ScriptableObject.CreateInstance<EnemyLevelUpStat> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<EnemyLevelUpStatBase>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<EnemyLevelUpStatBase>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
