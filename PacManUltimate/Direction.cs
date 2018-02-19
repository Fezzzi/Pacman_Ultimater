using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    class Direction
    {
        //Class provides data structure for directions done vie enumerable nType
        //It also provides two usefull functions for conversions
        public enum nType
        {
            LEFT, RIGHT, UP, DOWN, DIRECTION
        }

        public Tuple<int,int> DirectionToTuple(nType direction)
        {
            //Function for converting from direction to delta tuple
            switch (direction)
            {
                case nType.DOWN:
                    return new Tuple<int, int>(1, 0);
                case nType.UP:
                    return new Tuple<int, int>(-1, 0);
                case nType.LEFT:
                    return new Tuple<int, int>(0, -1);
                case nType.RIGHT:
                    return new Tuple<int, int>(0, 1);
                default:
                    return new Tuple<int, int>(0, 0);
            }
        }

        public nType TupleToDirection(Tuple<int,int> tuple)
        {
            //Function for converting from delta tuple to nType direction
            if(tuple.Item1 == 0)
            {
                if (tuple.Item2 == 1)
                    return nType.RIGHT;
                else if (tuple.Item2 == -1)
                    return nType.LEFT;
                else return nType.DIRECTION;
            }
            else
            {
                if (tuple.Item1 == 1)
                    return nType.DOWN;
                else if (tuple.Item1 == -1)
                    return nType.UP;
                else return nType.DIRECTION;
            }
        }
    }
}
