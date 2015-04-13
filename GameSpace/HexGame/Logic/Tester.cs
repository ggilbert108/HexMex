using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    class Tester
    {
        static void Main(string[] args)
        {
            GameState state = new GameState();

            //for (int i = 0; i < 25; i++)
            //{
            //    HexCoord move = state.getRandomLegalMove();
            //    state.makeMove(move);
            //}

            Intelligence.getBestMove(state);

            Console.Read();
        }
    }
}
