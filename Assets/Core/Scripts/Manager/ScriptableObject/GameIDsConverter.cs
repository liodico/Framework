using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameIDsConverter", menuName = "DevTools/Game IDs + Constants")]
public class GameIDsConverter : ScriptableObject
{
    [BoxGroup("Excel File")] [TextArea] public string excelPath;
    [BoxGroup("IDs file")] [TextArea] public string idsPath;
    [BoxGroup("Constants file")] [TextArea] public string constantsPath;
    
    [Button(ButtonSizes.Large)]
    public void SayHello()
    {
        
    }
}
