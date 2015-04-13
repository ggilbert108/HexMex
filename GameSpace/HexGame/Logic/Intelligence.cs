using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace HexGame.Logic
{
    static class Intelligence
    {
        public static PlayerColor aiColor;
        public static int totalSims = 1;
        public static HexCoord getBestMove(GameState initialState)
        {
            GameState state = initialState.copy();
            aiColor = state.toMoveColor;

            Node root = new Node(null, new HexCoord());

            for (int i = 0; i < 100; i++)
            {
                root.visit(state);
            }

            Console.WriteLine("done");
            return new HexCoord();
        }
    }

    class Node
    {
        public Node parent;
        public List<Node> children;
        public HexCoord move;
        public int simulationCount;
        public int winCount;
        public int depth;

        public Node(Node parent, HexCoord move)
        {
            this.parent = parent;
            this.move = move;
            children = new List<Node>();


            simulationCount = 1;
            winCount = 0;

            if (parent == null)
            {
                depth = 0;
            }
            else
            {
                depth = parent.depth + 1;
            }
        }

        public void visit(GameState state)
        {
            GameState afterState = state.copy();
            if (parent != null)
            {
                afterState.makeMove(move);
            }
            int minIndex = -1;
            double minUcb = double.MaxValue;
            
            for (int i = 0; i < children.Count; i++)
            {
                Node child = children[i];
                double ucb = child.getUcb();
                if (ucb < minUcb)
                {
                    minUcb = ucb;
                    minIndex = i;
                }
            }


            double exploreUcb = getUcbExplore();

            //Console.WriteLine("explore: " + exploreUcb + " exploit: " + minUcb);
            if(minIndex == -1 || exploreUcb < minUcb)
            {
                HexCoord m = afterState.getRandomLegalMove();
                Node child = new Node(this, m);
                child.simulate(afterState);
                children.Add(child);
            }
            else
            {
                Node bestNode = children[minIndex];
                bestNode.visit(afterState);
            }
        }

        public void simulate(GameState fromState)
        {
            GameState newState = fromState.copy();
            newState.makeMove(move);

            int sims = 10;
            int wins = 0;
            Simulator.testMove(newState, Intelligence.aiColor, sims, ref wins);
            backpropagate(wins, sims);
        }

        public void backpropagate(int wins, int sims)
        {
            if (parent == null)
            {
                return;
            }
            winCount += wins;
            simulationCount += sims;
            parent.backpropagate(wins, sims);
        }

        private double getUcb()
        {
            double first;
            if (simulationCount == 0)
            {
                first = 1;
            }
            else
            {
                first = (double)winCount / simulationCount;
            }
            double second = Intelligence.totalSims;
            second = Math.Log(second);
            second /= simulationCount;
            second = Math.Sqrt(second);
            second *= 0.7;
            return first + second;
        }

        private double getUcbExplore()
        {
            double second = Intelligence.totalSims;
            second = Math.Log(second);
            second = Math.Sqrt(second);
            second *= 0.7;
            return second - 1;
        }
    }
}
