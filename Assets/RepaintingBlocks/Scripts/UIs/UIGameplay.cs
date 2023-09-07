using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RepaintingBlocks
{
    public class UIGameplay : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _pauseBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _pauseBtnText;
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

            _pauseBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.PAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayPauseMenu(true);
            });


        }

        private void OnDestroy()
        {
            _pauseBtn.onClick.RemoveAllListeners();
        }



        private void LoadLanguague()
        {

            _pauseBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "pause");
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
