using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace LBMG.UI
{
    public class UI
    {
        public DialogBox DialogBox { get; set; }

        public UI()
        {
            DialogBox = new DialogBox("Fonts/myFont", new Point(640, 112));
        }
    }
}
