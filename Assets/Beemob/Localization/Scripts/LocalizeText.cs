using Beemob.Utilities;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Beemob.Localization.Scripts
{
    public class LocalizeText : MonoBehaviour
    {
        [SerializeField,ValueDropdown(nameof(TotalKeyWord))] private string languageId;
        private object[] _param;
        private TMP_Text _txTmpText;
        private LanguageManager _languageManager;
        private Material _baseMaterial;
        private TMP_FontAsset _baseFont;
        private bool _isInit;
        private void Awake()
        {
            _txTmpText = GetComponent<TMP_Text>();
            _baseFont = _txTmpText.font;
            _baseMaterial = _txTmpText.fontSharedMaterial;
            _isInit = true;
        }

        private string[] TotalKeyWord()
        {
            return TextKeyword.totalKey;
        }
        private void Start()
        {
            Initialize().Forget();
        }

        private void OnEnable()
        {
            if(!_isInit) return;
            _languageManager.AddToCollection(this);
            ReloadText();
        }

        private void OnDisable()
        {
            _languageManager.RemoveFromCollection(this);
        }

        private void OnDestroy()
        {
            _languageManager.RemoveFromCollection(this);
        }

        public void ReloadText()
        {
            if (string.IsNullOrEmpty(languageId)) return;
            SetText();
        }

        private async UniTaskVoid Initialize()
        {
            await UniTask.WaitUntil(ServiceLocator.Contain<LanguageManager>);
            _languageManager = ServiceLocator.GetService<LanguageManager>();
            _languageManager.AddToCollection(this);
            await UniTask.WaitUntil(() => _languageManager.IsLoaded);
            SetText();
        }
        private void SetText()
        {
            if (string.IsNullOrEmpty(languageId)) return;
            var languageText = _languageManager.GetTextByKey(languageId);
            _txTmpText.text = _param == null ? languageText : string.Format(languageText, _param);
            if(!_languageManager.UseMultiFont) return;
            var font = _languageManager.GetLanguageFont(_txTmpText.font);
            var material = _languageManager.GetFontMaterial(_baseFont, _baseMaterial);
            _txTmpText.font = font;
            _txTmpText.fontSharedMaterial = material;
        }
        public void SetLanguageId(string key, object[] param = null)
        {
            languageId = key;
            _param = param;
            SetText();
        }

        [Button]
        void Log()
        {
            Debug.Log(languageId==null);
        }
    }
}