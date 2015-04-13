using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    class Board
    {
        public static int boardSize = 13;
        private Dictionary<int, Hexagon> board;
        public Board()
        {
            board = new Dictionary<int, Hexagon>();
            for (int q = 0; q < boardSize; q++)
            {
                for (int r = 0; r < boardSize; r++)
                {
                    Hexagon hex = new Hexagon(q, r);
                    int hash = hex.GetHashCode();
                    board.Add(hex.GetHashCode(), hex);
                }
            }
        }

        public Board(Dictionary<int, Hexagon> board)
        {
            this.board = board;
        }

        public List<HexCoord> getOpenCoords()
        {
            List<HexCoord> list = new List<HexCoord>();

            for (int q = 0; q < boardSize; q++)
            {
                for (int r = 0; r < boardSize; r++)
                {
                    HexCoord coord = new HexCoord(q, r);
                    if (!isMarked(coord, PlayerColor.None))
                    {
                        list.Add(coord);
                    }
                }
            }

            return list;
        }

        public PlayerColor winner()
        {
            Dictionary<HexCoord, bool> marked = new Dictionary<HexCoord, bool>();
            foreach (KeyValuePair<int, Hexagon> pair in board)
            {
                marked.Add(pair.Value.coord, false);
            }

            foreach (KeyValuePair<int, Hexagon> pair in board)
            {
                HexCoord coord = pair.Value.coord;
                bool isMarked = marked[coord];
                PlayerColor hexColor = lookupHex(coord).color;

                if (!isMarked && hexColor != PlayerColor.None)
                {
                    if (floodFill(coord, marked, hexColor, Side.None))
                    {
                        return hexColor;
                    }
                }
            }

            return PlayerColor.None;;
        }

        private bool floodFill(HexCoord cur, Dictionary<HexCoord, bool> marked, PlayerColor targetColor, Side targetEdge)
        {
            List<HexCoord> neighbors = getNeighbors(cur);
            marked[cur] = true;


            Side edge = getEdge(cur);
            if (targetEdge == Side.None && edge != Side.None)
            {
                switch (edge)
                {
                    case Side.Bottom:
                        targetEdge = Side.Top;
                        break;
                    case Side.Top:
                        targetEdge = Side.Bottom;
                        break;
                    case Side.Left:
                        targetEdge = Side.Right;
                        break;
                    case Side.Right:
                        targetEdge = Side.Left;
                        break;
                    default:
                        targetEdge = Side.None;
                        break;
                }
            }
            else if (targetEdge != Side.None && edge == targetEdge)
            {
                return true;
            }

            bool won = false;
            foreach (HexCoord neighbor in neighbors)
            {
                if (marked[neighbor] || lookupHex(neighbor).color != targetColor)
                    continue;
                won = won || floodFill(neighbor, marked, targetColor, targetEdge);
            }
            return won;
        }

        public Hexagon lookupHex(HexCoord coord)
        {
            int lookupCode = coord.GetHashCode();
            return board[lookupCode];
        }

        public void markHex(HexCoord coord, PlayerColor color)
        {
            Hexagon hex = lookupHex(coord);
            hex.color = color;
        }

        public bool isMarked(HexCoord coord, PlayerColor color)
        {
            Hexagon hex = lookupHex(coord);
            if (color == PlayerColor.None)
            {
                //No preference
                return hex.color != PlayerColor.None;
            }
            return hex.color == color;
        }

        public List<HexCoord> getNeighbors(HexCoord coord)
        {
            HexCoord[] neigbors = coord.getNeighbors();
            List<HexCoord> result = new List<HexCoord>();

            foreach (HexCoord neighbor in neigbors)
            {
                if (inBounds(neighbor))
                {
                    result.Add(neighbor);
                }
            }
            return result;
        }

        public Board copy()
        {
            Dictionary<int, Hexagon> newBoard = new Dictionary<int, Hexagon>();
            {
                for (int q = 0; q < boardSize; q++)
                {
                    for (int r = 0; r < boardSize; r++)
                    {
                        Hexagon hex = new Hexagon(q, r);
                        hex.color = lookupHex(hex.coord).color;
                        newBoard.Add(hex.GetHashCode(), hex);
                    }
                }
            }
            return new Board(newBoard);
        }

        private bool inBounds(HexCoord coord)
        {
            return
                coord.q >= 0 &&
                coord.r >= 0 &&
                coord.q < boardSize &&
                coord.r < boardSize;
        }

        private Side getEdge(HexCoord coord)
        {
            if (coord.r == 0)
            {
                return Side.Top;
            }
            if (coord.r == boardSize - 1)
            {
                return Side.Bottom;
            }
            if (coord.q == 0)
            {
                return Side.Left;
            }
            if (coord.q == boardSize - 1)
            {
                return Side.Right;
            }
            return Side.None;
        }
    }

    enum Side
    {
        None, Top, Left, Bottom, Right
    }
}
