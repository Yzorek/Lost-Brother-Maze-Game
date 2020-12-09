using System;
using System.Collections.Generic;
using System.Text;

namespace LBMG.Main
{

    class SettingsChangedEventArgs : EventArgs
    {
        public bool FullScreenEnabled { get; set; }

        public SettingsChangedEventArgs()
        {
        }

        public SettingsChangedEventArgs(bool fullScreenEnabled)
        {
            FullScreenEnabled = fullScreenEnabled;
        }
    }

}
