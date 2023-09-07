using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace RepaintingBlocks
{
    public class UILanguage : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _leftBtn;
        [SerializeField] private Button _rightBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _headingText;
        [SerializeField] private TextMeshProUGUI _languageSelectionText;
        [SerializeField] private TextMeshProUGUI _backBtnText;

        [Header("Image")]
        [SerializeField] private Image _flag;

        [Header("Sprites")]
        [SerializeField] private Sprite _english;
        [SerializeField] private Sprite _russian;


        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;
        }


        private void Start()
        {
            LoadLanguague();
            LoadLanguageFlagUI();

            _backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplayMainMenu(true);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _leftBtn.onClick.AddListener(() =>
            {
                ChangeLanguage();
                LoadLanguageFlagUI();
            });

            _rightBtn.onClick.AddListener(() =>
            {
                ChangeLanguage();
                LoadLanguageFlagUI();
            });
        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();

            _leftBtn.onClick.RemoveAllListeners();
            _rightBtn.onClick.RemoveAllListeners();
        }


        public void ChangeLanguage()
        {
            if(LanguageManager.Instance.CurrentLanguague == LanguageManager.Languague.English)
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.Russian);           
            }          
            else
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.English);
            }

            
        }

        private void LoadLanguague()
        {
            _languageSelectionText.text = LanguageManager.Instance.GetLanguageString(LanguageManager.Instance.CurrentLanguague);

            _headingText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "LANGUAGE");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "back");           
        }

        private void LoadLanguageFlagUI()
        {
            if (LanguageManager.Instance.CurrentLanguague == LanguageManager.Languague.English)
                _flag.sprite = _english;
            else
                _flag.sprite = _russian;
        }

    }
}
