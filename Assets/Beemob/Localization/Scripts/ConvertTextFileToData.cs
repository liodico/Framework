#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Beemob.Localization.Scripts
{
    [Serializable]
    public class ConvertTextFileToData
    {
        [SerializeField,FilePath(RequireExistingPath = true)] public string languageCsvFile;
        private  readonly string[] _splitSeparator = {"	"};
        private const string FILE_FORMAT = "Assets/Beemob/Localization/Resources/Languages/{0}.asset";
        private const string PATH_KEY_WORDS = "Assets/Beemob/Localization/TextKeyword.cs";
        [Button(ButtonSizes.Large)]
        public void ConvertToLanguageData()
        {
            var dataDic = ReadAllFile(languageCsvFile);
            WriteScriptableObject(dataDic);

            SaveKeyWord(dataDic);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Localization", "Finished!", "OK");
        }
        private Dictionary<string, Dictionary<string, string>> ReadAllFile(string fileName)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var lines = File.ReadAllLines(fileName, Encoding.UTF8);
            if (lines == null) return null;
            var languages = lines[0].Split(_splitSeparator,  StringSplitOptions.None);
            languages = languages.Skip(1).ToArray();
            
            foreach (var t in languages)
            {
                result.Add(t, new Dictionary<string, string>());
            }
            for (var i = 1; i < lines.Length; i++)
            {
                var translateData = lines[i].Split(_splitSeparator,  StringSplitOptions.None);
                //Debug.Log("translateData.length = "+translateData.Length);
                for (var j = 0; j < translateData.Length-1; j++)
                {
                    if (!result[languages[j]].ContainsKey(translateData[0]))
                    {
                        //Debug.Log("key = "+translateData[0]+"----- value = "+translateData[j+1]);
                        result[languages[j]].Add(translateData[0], translateData[j + 1]);
                    }
                }
            }
            return result;
        }
        private void WriteScriptableObject(Dictionary<string, Dictionary<string, string>> dataDic)
        {
            
            foreach (var keyValue in dataDic)
            {
                LanguageData data;
                var filePath = string.Format(FILE_FORMAT, keyValue.Key);
                //Debug.Log("filePath = "+filePath);
                if (File.Exists(filePath))
                {
                    data = AssetDatabase.LoadAssetAtPath<LanguageData>(filePath);
                }
                else
                {
                    data = ScriptableObject.CreateInstance<LanguageData>();
                    AssetDatabase.CreateAsset(data, filePath);
                    //Debug.Log("go here + "+filePath);
                }

                data.languageData = keyValue.Value;
                EditorUtility.SetDirty(data);
                
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private void SaveKeyWord(Dictionary<string, Dictionary<string, string>> dataDic)
        {

            var result = dataDic.Values.ToArray()[0];
            var totalKeyWord = result.Keys.ToArray();
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("public static class TextKeyword").AppendLine();
            strBuilder.AppendLine("{");
            for (var i = 1; i < totalKeyWord.Length; i++)
            {
                strBuilder.Append("\t").AppendFormat($"public const string {totalKeyWord[i].ToUpper().Replace(' ','_')} = \"{totalKeyWord[i]}\";")
                    .AppendLine();
            }
            strBuilder.AppendLine("\tpublic const string[] totalKey = {");
            for (var i = 1; i < totalKeyWord.Length; i++)
            {
                strBuilder.AppendLine($"\t\t\"{totalKeyWord[i]}\",");
            }
            strBuilder.AppendLine($"\t\t\"EmptyString\",");
            strBuilder.AppendLine("\t};");
            strBuilder.AppendLine("}");
            var directoryName = Path.GetDirectoryName(PATH_KEY_WORDS);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName ?? throw new InvalidOperationException());
            }

            File.WriteAllText(PATH_KEY_WORDS, strBuilder.ToString(), Encoding.UTF8);
        }
    }
}

#endif