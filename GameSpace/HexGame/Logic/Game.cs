using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    class GameState
    {
        private Board board;
        private int playerToMoveIndex;

        public GameState()
        {
            board = new Board();
            playerToMoveIndex = 1;
        }

        public GameState copy()
        {
            GameState newState = new GameState();
            newState.playerToMoveIndex = playerToMoveIndex;
            newState.board = board.copy();
            return newState;
        }

        public List<HexCoord> getLegalMoves()
        {
            return board.getOpenCoords();
        }

        public HexCoord getRandomLegalMove()
        {
            List<HexCoord> legal = getLegalMoves();
            if (legal.Count > 0)
            {
                return Util.getRandomElement(legal);
            }
            return new HexCoord(0, 0);
        }

        public void makeMove(HexCoord move)
        {
            board.markHex(move, toMoveColor);
            playerToMoveIndex = (playerToMoveIndex == 1) ? 2 : 1;
        }

        public PlayerColor winner()
        {
            return board.winner();
        }

        public PlayerColor toMoveColor
        {
            //assume by default that player 1 is red and player 2 is blue
            get { return (playerToMoveIndex == 1) ? PlayerColor.Red : PlayerColor.Blue; }
        }
    }

    enum PlayerColor { None, Red, Blue }
}
