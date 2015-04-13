using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    class Hexagon
    {
        public readonly HexCoord coord;
        public PlayerColor color;

        public Hexagon(int q, int r)
        {
            coord = new HexCoord(q, r);
            color = PlayerColor.None;
        }

        public override bool Equals(object obj)
        {
            Hexagon other = (Hexagon) obj;
            return coord.Equals(other.coord);
        }

        public override int GetHashCode()
        {
            return coord.GetHashCode();
        }

        public int q
        {
            get { return coord.q; }
        }

        public int r
        {
            get { return coord.r; }
        }
    }

    struct HexCoord
    {
        public readonly int q;
        public readonly int r;

        public HexCoord(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public HexCoord[] getNeighbors()
        {
            HexCoord[] result =
            {
                new HexCoord(q, r - 1),
                new HexCoord(q + 1, r -1 ),
                new HexCoord(q + 1, r),
                new HexCoord(q, r + 1),
                new HexCoord(q - 1, r + 1),
                new HexCoord(q - 1, r)
            };
            return result;
        }

        public override bool Equals(object obj)
        {
            HexCoord other = (HexCoord) obj;
            return q == other.q && r == other.r;
        }

        public override int GetHashCode()
        {
            return (51*q) + (37*r);
        }

        public override string ToString()
        {
            return q + " " + r;
        }
    }
}
