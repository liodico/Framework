using System;
using System.Collections.Generic;
using Beemob.Utilities;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Beemob.Localization.Scripts
{
    public class LanguageManager: MonoBehaviour
    {
        [SerializeField,OnValueChanged(nameof(ChangeLanguage))]
        private SystemLanguage currentLanguage;

        [SerializeField] private bool useMultiFont;
        [SerializeField,ShowIf(nameof(useMultiFont))] private LanguageFontData languageFontData;
        private LanguageData _currentLanguagePack;
        private List<LocalizeText> _languageDisplay;
        private const string LANGUAGE_PREF = "language";
        private const string FILE_PATH = "Languages/{0}";
        private const SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.English;
        
        #region Public function

        private void Awake()
        {
            ServiceLocator.Register(this);
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            _languageDisplay = new List<LocalizeText>();
            var saveLanguageString = PlayerPrefs.GetString(LANGUAGE_PREF);
            if (string.IsNullOrEmpty(saveLanguageString))
            {
                var systemLanguage = Application.systemLanguage;
                for (var i = 0; i < languageFontData.LanguageFonts.Length; i++)
                {
                    for (var j = 0; j < languageFontData.LanguageFonts[i].language.Length; j++)
                    {
                        if (languageFontData.LanguageFonts[i].language[j] != systemLanguage) continue;
                        Initialize(systemLanguage);
                        return;
                    }
                }
            }
            var saveLanguage = Enum.TryParse<SystemLanguage>(saveLanguageString, true, out var language)
                ? language
                : DEFAULT_LANGUAGE;
            Initialize(saveLanguage);
        }

        

        public bool IsLoaded { get; private set; }

        public bool UseMultiFont => useMultiFont;

        public string GetTextByKey(string key)
        {
            return _currentLanguagePack.GetString(key);
        }

        public void AddToCollection(LocalizeText text)
        {
            if (!_languageDisplay.Contains(text))
                _languageDisplay.Add(text);
        }

        public void RemoveFromCollection(LocalizeText text)
        {
            if (_languageDisplay.Contains(text))
                _languageDisplay.Remove(text);
        }
        public TMP_FontAsset GetLanguageFont(TMP_FontAsset fontAsset)
        {
            if (!UseMultiFont) return fontAsset;
            for (var i = 0; i < languageFontData.LanguageFonts.Length; i++)
            {
                for (var j = 0; j < languageFontData.LanguageFonts[i].language.Length; j++)
                {
                    if (languageFontData.LanguageFonts[i].language[j] != currentLanguage) continue;
                    return languageFontData.LanguageFonts[i].fontAsset;
                }
            }
            return languageFontData.LanguageFonts[0].fontAsset;
        }

        public Material GetFontMaterial(TMP_FontAsset baseFont, Material baseMaterial)
        {
            if (!UseMultiFont) return baseMaterial;
            var font = GetLanguageFont(baseFont);
            var materialName = baseMaterial.name.Replace(baseFont.name, font.name).Trim();
            for (var i = 0; i < languageFontData.LanguageFonts.Length; i++)
            {
                if (languageFontData.LanguageFonts[i].fontAsset != font) continue;
                for (var j = 0; j < languageFontData.LanguageFonts[i].fontMaterials.Length; j++)
                {
                    if (languageFontData.LanguageFonts[i].fontMaterials[j].name != materialName) continue;
                    return languageFontData.LanguageFonts[i].fontMaterials[j];
                }
            }
            return languageFontData.LanguageFonts[0].fontMaterials[0];
        }
        #endregion

        #region Private function
        private void Initialize(SystemLanguage language)
        {
            currentLanguage = language;
            PlayerPrefs.SetString(LANGUAGE_PREF, currentLanguage.ToString());
            PlayerPrefs.Save();
            LoadLanguageFile(currentLanguage).Forget();
        }

        private void ChangeLanguage()
        {
            PlayerPrefs.SetString(LANGUAGE_PREF, currentLanguage.ToString());
            PlayerPrefs.Save();
            LoadLanguageFile(currentLanguage).Forget();
        }

        private void ReloadTextInScene()
        {
            for (var i = _languageDisplay.Count - 1; i >= 0; i--)
            {
                if (_languageDisplay[i] == null || _languageDisplay[i].gameObject == null)
                {
                    _languageDisplay.RemoveAt(i);
                    continue;
                }

                _languageDisplay[i].ReloadText();
            }
        }
        private async UniTaskVoid LoadLanguageFile(SystemLanguage language)
        {
            IsLoaded = false;
            var loadName = Enum.GetName(typeof(SystemLanguage), language);
            var filePath = string.Format(FILE_PATH, loadName);
            var resource = await Resources.LoadAsync<LanguageData>(filePath);
            _currentLanguagePack = resource as LanguageData;
            IsLoaded = true;
            ReloadTextInScene();
            Resources.UnloadAsset(resource);
        }

        #endregion
    }
}