using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.UI
{
    public static class TextBank
    {
        public static Language CurrentLanguage = Language.French;

        public static Dictionary<int, string> FrenchTexts = new Dictionary<int, string>
        {
            {1, "Texte numéro 1"},
            {2, "Texte numéro 2"},
            {3, "FR : Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."}
        };

        public static Dictionary<int, string> EnglishTexts = new Dictionary<int, string>
        {
            {1, "Text Number 1"},
            {2, "Text Number 2"},
            {3, "EN : Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."}
        };

        public static void ChangeLanguage(Language language)
        {
            CurrentLanguage = language;
        }

        public static string GetText(int key)
        {
            return CurrentLanguage == Language.English ? EnglishTexts[key] : FrenchTexts[key];
        }
    }

    public enum Language
    {
        French,
        English
    }
}
