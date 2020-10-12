using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            var str = textToWrap.Split(' ').ToList();   // TODO Add some wrapping logic
            return str;
        }
    }
}
