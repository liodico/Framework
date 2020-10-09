using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Beemob.Localization.Scripts
{
    [CreateAssetMenu(fileName = "LanguageData", menuName = "Beemob/Language/LanguageData", order = 0)]
    public class LanguageData : SerializedScriptableObject
    {
        [SerializeField] public Dictionary<string,string> languageData;
        private const string NEW_LINE = "\\n";
        private const string ERROR_KEY = "BAD_KEY";
        public string GetString(string key)
        {
            var result =  languageData.TryGetValue(key, out var value) ;
            return result ? value.Replace(NEW_LINE, Environment.NewLine) : ERROR_KEY;
        }
    }
}
