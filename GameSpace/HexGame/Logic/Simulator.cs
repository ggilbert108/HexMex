using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    static class Simulator
    {
        public static void testMove(GameState fromState, PlayerColor aiColor,
            int simCount, ref int winCount)
        {
            for (int i = 0; i < simCount; i++)
            {
                Intelligence.totalSims++;
                GameState stateCopy = fromState.copy();
                winCount += playout(stateCopy, aiColor);
            }
        }

        private static int playout(GameState state, PlayerColor aiColor)
        {
            while (state.winner() == PlayerColor.None)
            {
                if (state.getLegalMoves().Count == 0)
                {
                    break;
                }
                HexCoord randomMove = state.getRandomLegalMove();
                state.makeMove(randomMove);
            }
            if (state.winner() == aiColor)
            {
                return 1;
            }
            return 0;
        }
    }
}
