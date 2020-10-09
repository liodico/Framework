using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Beemob.Localization.Scripts
{
    [CreateAssetMenu(fileName = "LanguageFontData", menuName = "Beemob/Language/LanguageFontData", order = 1)]
    public class LanguageFontData : SerializedScriptableObject
    {
        public LanguageFont[] LanguageFonts;
    }

    [Serializable]
    public class LanguageFont
    {
        public SystemLanguage[] language;
        public TMP_FontAsset fontAsset;
        public Material[] fontMaterials;
    
    }
}