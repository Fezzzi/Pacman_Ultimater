using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    /// <summary>
    /// Class providing methods for AI movement and enumeration for easy programmicaly recognition of entities.
    /// </summary>
    class DefaultAI
    {
        //Instruction for adding another AI algorithm:
        //  1) Create Class that inherits from DefaultAI
        //  2) Override methods for AI's behaviour:
        //      2.1) HostileAttack - entity is hostile and attacking pacman
        //      2.2) HostileRetreat - entity is hostile but is retreating from attack
        //      2.3) Eaten - entity has been eaten
        //      2.4) CanBeEaten - entity can be eaten by pacman

        public nType State;

        public DefaultAI(nType state)
        {
            State = state;
        }

        /// <summary>
        /// Enumeration that characterizes all the possible entity's states.
        /// </summary>
        public enum nType
        {
            PLAYER1, PLAYER2, HOSTILEATTACK, HOSTILERETREAT, CANBEEATEN, EATEN
        }

        /// <summary>
        /// Function that chooses entity's next position based on set AI algorithms and entity's current state.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="target">Target tile.</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        public Direction.nType NextStep
            (Tuple<int,int> position, Tuple<int,int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            //Calls function to return AI's next direction
            if (State == nType.HOSTILERETREAT)
                return HostileRetreat(position, target, direction, map);
            else if (State == nType.EATEN)
                return Eaten(position, target, direction, map);
            else if (State == nType.CANBEEATEN)
                return CanBeEaten(position, target, direction, map);
            else
                return HostileAttack(position, target, direction, map);
        }

        /// <summary>
        /// AI Algorithm choosing next position for hostille entities during their attack phase.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="target">Target tile (Usually pacman's location).</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        virtual public Direction.nType HostileAttack(Tuple<int, int> position, Tuple<int, int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            return RandomAI(position, direction, map);
        }

        /// <summary>
        /// AI Algorithm choosing next position for hostille entities during their retreat phase.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="target">Target tile (Usually some corner tile).</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        virtual public Direction.nType HostileRetreat(Tuple<int, int> position, Tuple<int, int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            return RandomAI(position, direction, map);
        }

        /// <summary>
        /// AI Algorithm choosing next position for eaten entities.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="target">Target tile (Usuallyghost house entrance tile).</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        virtual public Direction.nType Eaten(Tuple<int, int> position, Tuple<int, int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            return RandomAI(position, direction, map);
        }

        /// <summary>
        /// AI Algorithm choosing next position for vulnerable entities.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="target">Target tile.</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        virtual public Direction.nType CanBeEaten(Tuple<int, int> position, Tuple<int, int> target, Direction.nType direction, Tile.nType?[][] map)
        {
            return RandomAI(position, direction, map);
        }

        /// <summary>
        /// Random AI algorithm that decides new direction randomly at each crossroad.
        /// if there is no other way, AI chooses direction it came from or stops.
        /// </summary>
        /// <param name="position">The entity's position.</param>
        /// <param name="direction">The entity's curent direction.</param>
        /// <returns>Returns chosen direction for the entity.</returns>
        public Direction.nType RandomAI(Tuple<int, int> position, Direction.nType direction, Tile.nType?[][] map)
        {
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

        /// <summary>
        /// Tests if the examined tile is free to move on.
        /// </summary>
        /// <param name="tile">The examined tile.</param>
        /// <returns>Boolean indicating occupancy of the examined tile.</returns>
        public bool CanAdd(Tile.nType? tile)
        {
            //
            if (tile == Tile.nType.DOT || tile == Tile.nType.POWERDOT || tile == Tile.nType.FREE)
                return true;
            else
                return false;
        }
    }
}
