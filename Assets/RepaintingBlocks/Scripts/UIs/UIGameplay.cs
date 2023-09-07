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



        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;
            GameplayManager.OnStartNextRound += LoadScoreText;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;
            GameplayManager.OnStartNextRound -= LoadScoreText;
        }


        private void Start()
        {
            LoadLanguague();
            LoadScoreText();

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
            _pauseBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "PAUSE");
        }

        private void LoadScoreText()
        {
       
        }
    }
}
