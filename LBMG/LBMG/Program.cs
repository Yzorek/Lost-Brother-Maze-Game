using LBMG.Main;
using System;

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
