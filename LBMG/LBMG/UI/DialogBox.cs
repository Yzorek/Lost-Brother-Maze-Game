using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LBMG.Tools;
using Microsoft.Xna.Framework;

namespace LBMG.UI
{
    public class DialogBox
    {
        public bool Visible { get; set; }

        public List<string> TextWritten { get; set; }

        public int CurrentTextIndex = 0;

        public Point Size { get; set; }

        public string FontPath { get; set; }

        public int CharWidth { get; set; }

        public int DisplaySpeed { get; set; }

        public DialogBox(string font, Point size)
        {
            Visible = false;
            FontPath = font;
            Size = size;
            CharWidth = 10;
        }

        public void Write(int keyOfTextToDisplay, string[] args)
        {
            if (Visible)
                return;

            TextWritten = new List<string>();
            Visible = true;

            List<string> texts = WrapText(string.Format(TextBank.GetText(keyOfTextToDisplay), args));

            foreach (var text in texts)
            {
                TextWritten.Add(text);
            }
            Debug.WriteLine(TextWritten[CurrentTextIndex]);
        }

        public void Write(List<int> keysOfTextToDisplay)
        {
            if (Visible)
                return;

            TextWritten = new List<string>();
            Visible = true;

            foreach (var key in keysOfTextToDisplay)
            {
                List<string> texts = WrapText(TextBank.GetText(key));

                foreach (var text in texts)
                {
                    TextWritten.Add(text);
                }
            }
            Debug.WriteLine(TextWritten[CurrentTextIndex]);
        }

        public void Write(int keyOfTextToDisplay)
        {
            if (Visible)
                return;

            TextWritten = new List<string>();
            Visible = true;

            List<string> texts = WrapText(TextBank.GetText(keyOfTextToDisplay));

            foreach (var text in texts)
            {
                TextWritten.Add(text);
            }
            Debug.WriteLine(TextWritten[CurrentTextIndex]);
        }

        public void NextDialog()
        {
            if (Visible == false)
                return;

            if (CurrentTextIndex == TextWritten.Count - 1)
            {
                EndDialog();
                return;
            }

            CurrentTextIndex++;
            Debug.WriteLine(TextWritten[CurrentTextIndex]);
        }

        public void EndDialog()
        {
            CurrentTextIndex = 0;
            Visible = false;
            TextWritten.Clear();
            Debug.WriteLine("Dialog end");
        }

        private List<string> WrapText(string textToWrap)
        {
            var splittedText = textToWrap.Split(' ');
            List<string> textWrapped = new List<string>();
            int lineLength = Size.X - Constants.DBoxPaddingLeft - Constants.DBoxPaddingRight;
            int maxNbOfChar = lineLength / CharWidth;
            int counter = 0;
            string block = "";
            string line = "";

            foreach (var word in splittedText)
            {
                if (word.Length > maxNbOfChar)
                    throw new TooBigWordException("A word delimited by spaces exceed the maximum amount of char available in a line.");
                if (line.Length + word.Length >= maxNbOfChar)
                {
                    line += '\n';
                    block += line;
                    line = "";
                    line += word;
                    counter++;
                }
                else
                {
                    string adjustedWord = word + ' ';
                    line += adjustedWord;
                }

                if (counter == 3)
                {
                    textWrapped.Add(block);
                    block = "";
                    counter = 0;
                }
            }

            line += '\n';
            block += line;
            textWrapped.Add(block);

            return textWrapped;
        }
    }

    public class TooBigWordException : SystemException
    {
        public TooBigWordException() : base() { }
        public TooBigWordException(string message) : base(message) { }
        public TooBigWordException(string message, SystemException inner) : base(message, inner) { }
    }
}
