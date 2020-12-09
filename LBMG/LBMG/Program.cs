using LBMG.Main;
using System;
using LBMG.Map;
using MonoGame.Extended;

namespace LBMG
{
    class Program
    {
        static void Main(string[] args)
        {
            using var LostBrotherMazeGame = new LBMGGame();
            LostBrotherMazeGame.Run();
        }
    }
}
