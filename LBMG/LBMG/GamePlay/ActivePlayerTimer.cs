using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace LBMG.GamePlay
{
    class ActivePlayerTimer
    {
        public event EventHandler ChangeActivePlayer;
        private Timer _timer;

        public ActivePlayerTimer()
        {
            _timer = new Timer();
        }

        public void Start()
        {
            _timer.AutoReset = true;
            _timer.Elapsed += (s, e) => ChangeActivePlayer?.Invoke(this, EventArgs.Empty);
            _timer.Start();
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
