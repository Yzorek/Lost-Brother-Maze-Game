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

        private Stopwatch _sw = new Stopwatch();
        private double _millisecInterval;

        public TimeSpan ElapsedTime => _sw.Elapsed;

        public ActivePlayerTimer(double millisecInterval)
        {
            _millisecInterval = millisecInterval;
        }

        public void Start()
        {
            _sw.Start();
        }

        public void Update(GameTime gameTime)
        {
            if (_sw.Elapsed.TotalMilliseconds > _millisecInterval)
            {
                ChangeActivePlayer?.Invoke(this, EventArgs.Empty);
                _sw.Restart();
            }
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
