using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace RepaintingBlocks
{
    public class UIMainMenu : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _languageBtn;
        [SerializeField] private Button _soundBtn;
        [SerializeField] private Button _musicBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _playBtnText;
        [SerializeField] private TextMeshProUGUI _languageBtnText;
        [SerializeField] private TextMeshProUGUI _soundBtnText;
        [SerializeField] private TextMeshProUGUI _musicBtnText;


        [Header("Sprites")]
        [SerializeField] private Sprite _activeBtnSprite;
        [SerializeField] private Sprite _deactiveBtnSprite;
        [SerializeField] private Sprite _activeSoundSprite;
        [SerializeField] private Sprite _deactiveSoundSprite;
        [SerializeField] private Sprite _activeMusicSprite;
        [SerializeField] private Sprite _deactiveMusicSprite;

        [Header("Font Assets")]
        [SerializeField] private TMP_FontAsset _greyOutlineFont;
        [SerializeField] private TMP_FontAsset _purpleOutlineFont;

        [Header("Colors")]
        [SerializeField] private Color _activeTextColor;
        [SerializeField] private Color _deactiveTextColor;

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
            UpdateMusicUI();
            UpdateSoundFXUI();

            _playBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                Loader.Load(Loader.Scene.GameplayScene);
            });

            _languageBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplayLanguageMenu(true);
            });

            _musicBtn.onClick.AddListener(() =>
            {
                ToggleMusic();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _soundBtn.onClick.AddListener(() =>
            {
                ToggleSFX();
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

        }

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveAllListeners();
            _languageBtn.onClick.RemoveAllListeners();
            _musicBtn.onClick.RemoveAllListeners();
            _soundBtn.onClick.RemoveAllListeners();
        }



        private void LoadLanguague()
        {

            switch (LanguageManager.Instance.CurrentLanguague)
            {
                default:
                case LanguageManager.Languague.English:
                    _languageBtnText.fontSize = 30;
                    _playBtnText.fontSize = 45;
                    _soundBtnText.fontSize = 42;
                    _musicBtnText.fontSize = 42;

                    break;
                case LanguageManager.Languague.Russian:
                    _languageBtnText.fontSize = 35;
                    _playBtnText.fontSize = 40;
                    _soundBtnText.fontSize = 37;
                    _musicBtnText.fontSize = 37;
                    break;
            }



            _playBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "PLAY");
            _languageBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "LANGUAGE");
            _soundBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "sound");
            _musicBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "music");
        }

        private void ToggleSFX()
        {

            SoundManager.Instance.MuteSoundFX(SoundManager.Instance.isSoundFXActive);
            SoundManager.Instance.isSoundFXActive = !SoundManager.Instance.isSoundFXActive;

            UpdateSoundFXUI();
        }


        private void UpdateSoundFXUI()
        {
            if (SoundManager.Instance.isSoundFXActive)
            {
                _soundBtn.image.sprite = _activeBtnSprite;
                _soundBtn.transform.Find("Icon").GetComponent<Image>().sprite = _activeSoundSprite;
                _soundBtnText.font = _purpleOutlineFont;
                _soundBtnText.color = _activeTextColor;
            }
            else
            {
                _soundBtn.image.sprite = _deactiveBtnSprite;
                _soundBtn.transform.Find("Icon").GetComponent<Image>().sprite = _deactiveSoundSprite;
                _soundBtnText.font = _greyOutlineFont;
                _soundBtnText.color = _deactiveTextColor;

            }
        }

        private void ToggleMusic()
        {
            SoundManager.Instance.MuteBackground(SoundManager.Instance.isMusicActive);
            SoundManager.Instance.isMusicActive = !SoundManager.Instance.isMusicActive;

            UpdateMusicUI();
        }

        private void UpdateMusicUI()
        {
            if (SoundManager.Instance.isMusicActive)
            {
                _musicBtn.image.sprite = _activeBtnSprite;
                _musicBtn.transform.Find("Icon").GetComponent<Image>().sprite = _activeMusicSprite;
                _musicBtnText.font = _purpleOutlineFont;
                _musicBtnText.color = _activeTextColor;
            }
            else
            {
                _musicBtn.image.sprite = _deactiveBtnSprite;
                _musicBtn.transform.Find("Icon").GetComponent<Image>().sprite = _deactiveMusicSprite;
                _musicBtnText.font = _greyOutlineFont;
                _musicBtnText.color = _deactiveTextColor;
            }
        }
    }
}

