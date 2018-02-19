using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    class EntitiesClass
    {
        //Class providing methods for AI movement and easy program recognition of entities
        //Instruction for adding another AI algorithm:
        //  1) Name your algorithm and add it to nType
        //  2) To NextStep function add case nType.YourName record returning YourAlgorithm
        //  3) Add YourAlgorithm function
        //  4) In Form1.cs LoadEntities function change DefaultUIs to YourAlgorithm
        public enum nType
        {
            PLAYER1, PLAYER2, HOSTILERNDM, EATEN
        }

        public Direction.nType NextStep
            (nType entity, Tuple<int,int> position, Tuple<int,int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            //Calls function to return AI's next direction
            switch (entity)
            {
                case nType.HOSTILERNDM:
                    return RandomAI(position, direction, map);
                case nType.EATEN:
                    return RandomAI(position, direction, map);
                default:
                    return Direction.nType.DIRECTION;
            }
        }

        private Direction.nType RandomAI(Tuple<int, int> position, Direction.nType direction, Tile.nType?[][] map)
        {
            //Tests all four direction and randomly chooses one of them
            //if there is no other way chooses direction UI came from or stops it
            Random rnmd = new Random();
            List<Tuple<int, int>> possibilities = new List<Tuple<int, int>>();
            Direction dir = new Direction();
            Tuple<int, int> back = dir.DirectionToTuple(direction);
            back = new Tuple<int, int>(back.Item1 * -1, back.Item2 * -1);

            for (int j = 0; j < 2; j++)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    if (CanAdd(map[position.Item2 + (j == 0 ? i : 0)][position.Item1 + (j == 1 ? i : 0)])
                        && ((j == 0 ? i : 0) != back.Item1 || (j == 1 ? i : 0) != back.Item2))
                        possibilities.Add(new Tuple<int, int>((j == 0 ? i : 0), (j == 1 ? i : 0)));
                }
            }

            if (possibilities.Count() > 0)
                return dir.TupleToDirection(possibilities[rnmd.Next(possibilities.Count())]);
            else if (direction != Direction.nType.DIRECTION)
                return dir.TupleToDirection(back);
            else
                return Direction.nType.DIRECTION;
        }

        private bool CanAdd(Tile.nType? tile)
        {
            //Tests if the examined tile is free to move on
            if (tile == Tile.nType.DOT || tile == Tile.nType.POWERDOT || tile == Tile.nType.FREE)
                return true;
            else
                return false;
        }
    }
}
