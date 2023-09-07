using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RepaintingBlocks
{
    public class UIPause : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _backBtn;


        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _pauseText;
        [SerializeField] private TextMeshProUGUI _homeBtnText;
        [SerializeField] private TextMeshProUGUI _backBtnText;

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

            _backBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.UNPAUSE);

                UIGameplayManager.Instance.CloseAll();
                UIGameplayManager.Instance.DisplayGameplayMenu(true);
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


            _pauseText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "PAUSE");
            _homeBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "HOME");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "BACK");
        }
    }
}
