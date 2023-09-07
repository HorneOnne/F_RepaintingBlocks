using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace RepaintingBlocks
{

    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager Instance { get; private set; }
        public static System.Action OnLanguageChanged;

        public TMP_FontAsset NormalFont;
        public TMP_FontAsset RusFont;

        private Dictionary<string, WordDict> dict = new Dictionary<string, WordDict>()
        {
            {"PLAY", new WordDict("PLAY", "играть")},
            {"LANGUAGE", new WordDict("LANGUAGE", "язык")},
            {"sound", new WordDict("sound", "звук")},
            {"music", new WordDict("music", "музыка")},
            {"back", new WordDict("back", "назад")},
            {"best", new WordDict("best", "лучший")},
            {"score", new WordDict("score", "счет")},
            {"pause", new WordDict("pause", "пауза")},
            {"menu", new WordDict("menu", "меню")},
            {"Game\nover", new WordDict("Game\nover", "конец\nигры")},
            {"replay", new WordDict("replay", "повтор")},
 
        };


        public enum Languague
        {
            English,
            Russian,
        }

        public Languague CurrentLanguague;


        private void Awake()
        {
            // Check if an instance already exists, and destroy the duplicate
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            // Make the GameObject persist across scenes
            DontDestroyOnLoad(this.gameObject);
        }


        public void ChangeLanguague(Languague languague)
        {
            this.CurrentLanguague = languague;
            OnLanguageChanged?.Invoke();
        }

        public string GetWord(Languague type, string word)
        {
            if (dict.ContainsKey(word))
            {
                return dict[word].GetWord(type);
            }
            return "";
        }

        public string GetLanguageString(Languague language)
        {
            switch (language)
            {
                default:
                case LanguageManager.Languague.English:
                    return "English";
                case LanguageManager.Languague.Russian:
                    return "русский";
            }
        }
    }

    public class WordDict
    {
        public string English;
        public string Russian;


        public WordDict(string english, string russian)
        {
            English = english;
            Russian = russian;
        }

        public string GetWord(LanguageManager.Languague language)
        {
            switch (language)
            {
                default:
                case LanguageManager.Languague.English:
                    return English;
                case LanguageManager.Languague.Russian:
                    return Russian;
            }
        }
    }
}
