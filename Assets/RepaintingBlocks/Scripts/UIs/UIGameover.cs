using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RepaintingBlocks
{
    public class UIGameover : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _menuBtn;
        [SerializeField] private Button _replayBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _gameoverText;
        [SerializeField] private TextMeshProUGUI _menuBtnText;
        [SerializeField] private TextMeshProUGUI _replayBtnText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _recordText;

        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;
            GameplayManager.OnGameOver += LoadScoreText;
            GameplayManager.OnGameOver += LoadRecordText;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;
            GameplayManager.OnGameOver -= LoadScoreText;
            GameplayManager.OnGameOver -= LoadRecordText;
        }

        private void Start()
        {
            LoadLanguague();
            LoadScoreText();
            LoadRecordText();

            _menuBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.MenuScene);
            });

            _replayBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.GameplayScene);
            });

            
        }

        private void OnDestroy()
        {
            _replayBtn.onClick.RemoveAllListeners();
            _menuBtn.onClick.RemoveAllListeners();
        }

        private void LoadLanguague()
        {
            _gameoverText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "Game\nover");
            _menuBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "menu");
            _replayBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "replay");
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
