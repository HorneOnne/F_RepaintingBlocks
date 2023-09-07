using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RepaintingBlocks
{
    public class UIGameover : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _backBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _gameoverText;
        [SerializeField] private TextMeshProUGUI _homeBtnText;
        [SerializeField] private TextMeshProUGUI _backBtnText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private TextMeshProUGUI _scoreValueText;
        [SerializeField] private TextMeshProUGUI _recordValueText;

        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;

            GameplayManager.OnGameOver += LoadScore;
            GameplayManager.OnGameOver += LoadRecord;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;

            GameplayManager.OnGameOver -= LoadScore;
            GameplayManager.OnGameOver -= LoadRecord;
        }

        private void Start()
        {
            LoadLanguague();
            LoadScore();
            LoadRecord();

            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.GameplayScene);
            });

            _homeBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.EXIT);
                Loader.Load(Loader.Scene.MenuScene);
            });
        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();
            _homeBtn.onClick.RemoveAllListeners();
        }

        private void LoadLanguague()
        {

            switch (LanguageManager.Instance.CurrentLanguague)
            {
                default:
                case LanguageManager.Languague.English:
                    _scoreText.fontSize = 50;
                    _recordText.fontSize = 50;
                    break;
                case LanguageManager.Languague.Russian:
                    _scoreText.fontSize = 50;
                    _recordText.fontSize = 50;
                    break;
            }


            _gameoverText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "GAME\nOVER");
            _homeBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "HOME");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "BACK");
            _scoreText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "SCORE");
            _recordText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "RECORD");
        }

        private void LoadScore()
        {
           
        }

        private void LoadRecord()
        {
            _recordValueText.text = $"{GameManager.Instance.Record}";
        }
    }
}
