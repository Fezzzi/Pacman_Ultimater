using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    /// <summary>
    /// Class providing data structure for directions via enumerable nType.
    /// It also provides two usefull functions for conversions.
    /// </summary>
    class Direction
    {

        /// <summary>
        /// Enumarable representing possible directions.
        /// </summary>
        public enum nType
        {
            LEFT, RIGHT, UP, DOWN, DIRECTION
        }

        /// <summary>
        /// Function for converting from direction to delta tuple.
        /// </summary>
        /// <param name="direction">Input direction enumerable.</param>
        /// <returns>Returns input's representation as delta tuple of two integers.</returns>
        public Tuple<int,int> DirectionToTuple(nType direction)
        {
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

        /// <summary>
        /// Function for converting from delta tuple to nType direction.
        /// </summary>
        /// <param name="tuple">Input delta tuple of two integers.</param>
        /// <returns>Returns input's representation as member of nType enumerable of possible directions.</returns>
        public nType TupleToDirection(Tuple<int,int> tuple)
        {
            //
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
