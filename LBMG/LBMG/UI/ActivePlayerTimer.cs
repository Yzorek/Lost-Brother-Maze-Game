using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace LBMG.UI
{
    class ActivePlayerTimer
    {
        public event EventHandler ChangeActivePlayer;

        private readonly TimeSpan _intervalTimeSpan;
        private readonly Stopwatch _sw = new Stopwatch();

        public TimeSpan RemainingTime => _intervalTimeSpan - _sw.Elapsed;

        public ActivePlayerTimer(TimeSpan intervalTs)
        {
            _intervalTimeSpan = intervalTs;
        }

        public void Start()
        {
            _sw.Start();
        }

        public void Stop()
        {
            _sw.Stop();
            _sw.Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (RemainingTime.TotalSeconds <= 1)
            {
                ChangeActivePlayer?.Invoke(this, EventArgs.Empty);
                _sw.Restart();
            }
        }
    }
}
