using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RepaintingBlocks
{
    public class UIPause : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _menuBtn;
        [SerializeField] private Button _backBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _pauseText;
        [SerializeField] private TextMeshProUGUI _menuBtnText;
        [SerializeField] private TextMeshProUGUI _backBtnText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _recordText;

        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;
            GameManager.OnScoreUp += LoadScoreText;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;
            GameManager.OnScoreUp -= LoadScoreText;
        }

        private void Start()
        {
            LoadLanguague();
            LoadScoreText();
            LoadRecordText();

            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.UNPAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayGameplayMenu(true);
            });

            _menuBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.MenuScene);
            });
        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();
            _menuBtn.onClick.RemoveAllListeners();
        }

        private void LoadLanguague()
        {

            //switch (LanguageManager.Instance.CurrentLanguague)
            //{
            //    default:
            //    case LanguageManager.Languague.English:
            //        _playBtnText.fontSize = 70;
            //        _settingsBtnText.fontSize = 70;
            //        _languageBtnText.fontSize = 70;
            //        break;
            //    case LanguageManager.Languague.Norwegian:
            //    case LanguageManager.Languague.Italian:
            //    case LanguageManager.Languague.German:
            //        _playBtnText.fontSize = 55;
            //        _settingsBtnText.fontSize = 55;
            //        _languageBtnText.fontSize = 55;
            //        break;
            //}


            _pauseText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "pause");
            _menuBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "menu");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "back");
        }

        private void LoadScoreText()
        {
            string scoreString = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "score");
            _scoreText.text = $"{scoreString} {GameManager.Instance.Score}";
        }

        private void LoadRecordText()
        {
            string recordString = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "best");
            _recordText.text = $"{recordString} {GameManager.Instance.Record}";
        }
    }
}
