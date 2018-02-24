using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    /// <summary>
    /// Class that handles manipulation with text files containing map.
    /// </summary>
    class LoadMap
    {
        const int TileTypes = 5;
        const int GhostHouseSize = 38;
        const int MapWidthInTiles = 28;
        const int MapHeightInTiles = 31;
        const int PacManInitialY = 23;
        const int PacManInitialX = 13;

        public Tuple<Tile.nType?[][], int, Tuple<int,int>> Map;

        /// <summary>
        /// Calls loading function with default symbols.
        /// </summary>
        /// <param name="path">Path to the map file that is to be loaded.</param>
        public LoadMap(string path)
        {
            Map = LdMap(path,new char[TileTypes] {' ','.','o','x','X'});
        }

        /// <summary>
        /// Calls loading function with given symbols.
        /// </summary>
        /// <param name="path">Path to the map file that is to be loaded.</param>
        /// <param name="symbols">Set of 5 symbols representing tile types in to be loaded file.</param>
        public LoadMap(string path, char[] symbols)
        {
            Map = LdMap(path, symbols);
        }

        /// <summary>
        /// Handles function calling to ensure map's loading and testing of its playability.
        /// </summary>
        /// <param name="path">Path to the map file that is to be loaded.</param>
        /// <param name="symbols">Set of 5 symbols representing tile types in to be loaded file.</param>
        /// <returns>Return array of loaded tiles, translated to game's language (null in case of failure).</returns>
        private Tuple<Tile.nType?[][], int, Tuple<int, int>> LdMap(string path, char[] symbols)
        {
            Tuple<char[][],Tile.nType?[][],int> map = LoadIt(path,symbols);
            
            if (map != null)
            {
                // if LoadIt ended successfully preforms test of map's playability.
                Tuple<bool, Tuple<int, int>> playable = IsPlayable(map.Item1, symbols, map.Item3);
                if (playable.Item1)
                    return new Tuple<Tile.nType?[][],int,Tuple<int,int>>(map.Item2,map.Item3,playable.Item2);
            }
            // In case map is playable final Tuple is created and returned.
            // Null is returned to signalise something went wrong otherwise.
            return null;
        }

        /// <summary>
        /// Function that loads map from text file and creates Tile map if possible. Returns null otherwise.
        /// </summary>
        /// <param name="path">Path to the map file that is to be loaded.</param>
        /// <param name="symbols">Set of 5 symbols representing tile types in to be loaded file.</param>
        /// <returns>Returns Tile map if possible, null otherwise.</returns>
        private Tuple<char[][],Tile.nType?[][],int> LoadIt(string path,char[] symbols)
        {
            // Map is created bigger so it is possible to automaticaly call transform to tiles with indexes
            // out of range of map without causing IndexOutOfRange Exception.
            int numOfDots = 0;
            char[][] map = new char[MapHeightInTiles + 2][];
            Tile.nType?[][] tileMap = new Tile.nType?[MapHeightInTiles][];
            int lineNum = 1,column = 0;
            StreamReader sr = new StreamReader(path);
            map[0] = new char[MapWidthInTiles + 2];
            map[1] = new char[MapWidthInTiles + 2];
            tileMap[0] = new Tile.nType?[MapWidthInTiles];

            // Reads whole text file symbol by symbol and saves it in Map array.
            while (!sr.EndOfStream)
            {
                column++;
                // Tests whether text file does not contain more that 31 lines.
                if (lineNum > MapHeightInTiles)
                    return null;
                char symbol = (char)sr.Read();
                while (symbol == 13 || symbol == 10)
                    symbol = (char)sr.Read();

                // Counts number of pellets on the map.
                if (symbol == symbols[1] || symbol == symbols[2])
                    numOfDots++;
                map[lineNum][column] = symbol;

                //Transforms symbol in line above with use of already read surrounding symbols
                //The only one sybol not read yet is accessed via sr.peek function 
                if (lineNum > 1)
                {
                    tileMap[lineNum - 2][column - 1] = TransformToTile
                        (
                        map[lineNum - 1][column], map[lineNum - 1][column - 1],
                        map[lineNum - 1][column + 1], map[lineNum - 2][column],
                        symbol, map[lineNum][column - 1], map[lineNum - 2][column - 1],
                        map[lineNum - 2][column + 1], (char)sr.Peek(),
                        lineNum > 2 ? tileMap[lineNum - 3][column - 1] : null, 
                        column > 1 ? tileMap[lineNum - 2][column - 2] : null, symbols
                        );
                    //Quits whole reading in case the program was unable to identify tile
                    if (tileMap[lineNum - 2][column - 1] == null)
                        return null;
                }
                if (column == 28 && !sr.EndOfStream)
                {
                    //Moves to another line and initialize new array for this line
                    //Returns null in case the file has already reached 31 lines
                    column = 0;
                    lineNum++;
                    map[lineNum] = new char[MapWidthInTiles + 2];
                    if (lineNum < MapHeightInTiles + 1)
                        tileMap[lineNum - 1] = new Tile.nType?[MapWidthInTiles];
                    else return null;
                }           
            }
            if(lineNum == MapHeightInTiles && column == MapWidthInTiles)
            {
                //Separetly Transform to tile last line because reading loop has ended on 31st line
                lineNum++;
                for (int i = 1; i < column + 1; i++)
                {
                    tileMap[lineNum - 2][i - 1] = TransformToTile
                        (
                        map[lineNum - 1][i], map[lineNum - 1][i - 1],
                        map[lineNum - 1][i + 1], map[lineNum - 2][i],
                        '\0', '\0', map[lineNum - 2][i - 1],
                        map[lineNum - 2][i + 1], '\0', tileMap[lineNum - 3][i-1], 
                        i > 1 ? tileMap[lineNum - 2][i - 2] : null, symbols
                        );
                }
            }
            return new Tuple<char[][],Tile.nType?[][],int>(map,tileMap,numOfDots);
        }

        /// <summary>
        /// Tests whether the already loaded map is playable and finishable.
        /// That means: Contains Ghost House, Pacman initial position is free 
        /// and all the pellets are accessible for both pacman and ghosts.
        /// </summary>
        /// <param name="map">Loaded tile map.</param>
        /// <param name="symbols">Set of 5 symbols representing tile types in to be loaded file.</param>
        /// <param name="numOfDots">Number of pellets found on the loaded map.</param>
        /// <returns></returns>
        private Tuple<bool, Tuple<int, int>> IsPlayable(char[][] map, char[] symbols, int numOfDots)
        {
            int ghostHouse = 0, dotsFound = 0;
            Tuple<int, int> ghostPosition = null;
            Tuple<int, int> position = new Tuple<int, int>(PacManInitialY + 1, PacManInitialX + 1);
            bool[][] connectedTiles = new bool[MapHeightInTiles + 2][];
            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();

            if (map[position.Item1][position.Item2] == symbols[0] && map[position.Item1][position.Item2 + 1] == symbols[0])
            {
                // Performs classical BFS from pacman initial position.
                connectedTiles[position.Item1] = new bool[MapWidthInTiles + 2];
                connectedTiles[position.Item1][position.Item2] = true;
                stack.Push(position);
                while ((dotsFound != numOfDots || ghostHouse != GhostHouseSize) && stack.Count > 0)
                {
                    position = stack.Pop();
                    if (map[position.Item1][position.Item2] != symbols[3] && 
                        map[position.Item1][position.Item2] != symbols[4] && position.Item2 > 0 && position.Item2 < MapWidthInTiles + 1)
                    {
                        // Counts number of dots accessible from starting point for further comparison.
                        if (map[position.Item1][position.Item2] != symbols[0])
                            dotsFound++;
                        if (connectedTiles[position.Item1] == null)
                            connectedTiles[position.Item1] = new bool[MapWidthInTiles + 2];
                        if (connectedTiles[position.Item1 - 1] == null)
                            connectedTiles[position.Item1 - 1] = new bool[MapWidthInTiles + 2];
                        if (connectedTiles[position.Item1 + 1] == null)
                            connectedTiles[position.Item1 + 1] = new bool[MapWidthInTiles + 2];
                        if (connectedTiles[position.Item1][position.Item2 - 1] == false)
                        {
                            stack.Push(new Tuple<int, int>(position.Item1, position.Item2 - 1));
                            connectedTiles[position.Item1][position.Item2 - 1] = true;
                        }
                        if (connectedTiles[position.Item1][position.Item2 + 1] == false)
                        {
                            stack.Push(new Tuple<int, int>(position.Item1, position.Item2 + 1));
                            connectedTiles[position.Item1][position.Item2 + 1] = true;
                        }
                        if (connectedTiles[position.Item1 - 1][position.Item2] == false)
                        {
                            stack.Push(new Tuple<int, int>(position.Item1 - 1, position.Item2));
                            connectedTiles[position.Item1 - 1][position.Item2] = true;
                        }
                        if (connectedTiles[position.Item1 + 1][position.Item2] == false)
                        {
                            stack.Push(new Tuple<int, int>(position.Item1 + 1, position.Item2));
                            connectedTiles[position.Item1 + 1][position.Item2] = true;
                        }

                        if (map[position.Item1 + 1][position.Item2] == symbols[4] && map[position.Item1 + 1][position.Item2 + 1] == symbols[4])
                        {
                            //If the algorithm has reached gate Tiles searches tiles underneath to find out if the contain ghost House
                            //The algorithm knows how the ghost house should look and tests each tile if it contains what it should
                            //the algorithm increases ghostHouse for each successfull comparison
                            if (map[position.Item1][position.Item2] == symbols[0] && map[position.Item1][position.Item2 + 1] == symbols[0])
                            {
                                ghostPosition = new Tuple<int, int>(position.Item2, position.Item1);
                                // Saves location of tiles above the gate for further in-game use.
                                for (int k = 1; k < 6; k++)                               
                                    for (int l = -3; l < 5; l++)                                    
                                        switch (k)
                                        {
                                            case 1:
                                                if (map[position.Item1 + k][position.Item2 + l] == symbols[3] && (l != 0 || l != 1))
                                                    ghostHouse++;
                                                break;
                                            case 5:
                                                if (map[position.Item1 + k][position.Item2 + l] == symbols[3])
                                                    ghostHouse++;
                                                break;
                                            default:
                                                if (map[position.Item1 + k][position.Item2 + l] == symbols[3] && (l == -3 || l == 4))
                                                    ghostHouse++;
                                                else if (map[position.Item1 + k][position.Item2 + l] == symbols[0])
                                                    ghostHouse++;
                                                break;
                                        }
                            }
                        }
                    }
                }

                // Compares number of found pellets and ghostHouse and decides whether the map is playable or not.
                // The fact that ghost house was found means it is accessible from pacman's starting postion.
                if (dotsFound == numOfDots && ghostHouse == GhostHouseSize)
                    return new Tuple<bool, Tuple<int, int>>(true, ghostPosition);
                else return new Tuple<bool,Tuple<int,int>> (false,null);
            }
            else return new Tuple<bool, Tuple<int, int>>(false, null);
        }

        /// <summary>
        /// Function that transforms readed symbol to tile.
        /// </summary>
        /// <param name="tile">Currently processed symbol.</param>
        /// <param name="tileLeft">Symbol to the left.</param>
        /// <param name="tileRight">Symbol to the right.</param>
        /// <param name="tileUp">Symbol above.</param>
        /// <param name="tileDown">Symbol under.</param>
        /// <param name="BLCorner">Symbol in bottom-left corner.</param>
        /// <param name="TLCorner">Symbol in tom-left corner.</param>
        /// <param name="TRCorner">Symbol in top-right corner.</param>
        /// <param name="BRCorner">Symbol in bottom-right corner.</param>
        /// <param name="upperTile">Allready translated tile above.</param>
        /// <param name="leftTile">Allready translated tile to the left.</param>
        /// <param name="symbols">Set of 5 symbols representing tile types in to be loaded file.</param>
        /// <returns>Returns input symbol translated to the program's tile alphabet.</returns>
        private Tile.nType? TransformToTile
            (
            char tile, char tileLeft, char tileRight,
            char tileUp, char tileDown, char BLCorner,
            char TLCorner,char TRCorner, char BRCorner,
            Tile.nType? upperTile, Tile.nType? leftTile, 
            char[] symbols)
        {
            Tile tl = new Tile();
            if (tile == symbols[0])
                return tl.ReturnType(" ");
            else if (tile == symbols[1])
                return tl.ReturnType(".");
            else if (tile == symbols[2])
                return tl.ReturnType("o");
            else if (tile == symbols[3])
            {
                // Determines type of wall with use of adjacent symbols and tiles.
                if (tileLeft == symbols[3] && tileRight == symbols[3] && 
                    tileUp == symbols[3] && tileDown == symbols[3])
                {
                    if (TLCorner == symbols[0] || TLCorner == symbols[1] || TLCorner == symbols[2])
                        return tl.ReturnType("SCBR");
                    else if (TRCorner == symbols[0] || TRCorner == symbols[1] || TRCorner == symbols[2])
                        return tl.ReturnType("SCBL");
                    else if (BRCorner == symbols[0] || BRCorner == symbols[1] || BRCorner == symbols[2])
                        return tl.ReturnType("SCTL");
                    else if (BLCorner == symbols[0] || BLCorner == symbols[1] || BLCorner == symbols[2])
                        return tl.ReturnType("SCTR");
                    else return null;
                }
                else if ((tileLeft == symbols[3] || tileLeft == symbols[4]) &&
                    (tileRight == symbols[3] || tileRight == symbols[4]) && 
                    (tileUp != symbols[3] || tileDown != symbols[3]) || (tileRight == '\0' && tileUp != symbols[3] && tileDown != symbols[3]))
                {
                    if (tileUp == symbols[3])
                        return tl.ReturnType("SWB");
                    else if (tileDown == symbols[3])
                        return tl.ReturnType("SWT");
                    else if (tileUp == symbols[1] || tileUp == symbols[2] || leftTile == Tile.nType.BWALLDOUBLE || leftTile == Tile.nType.TLCURVESINGLE)
                        return tl.ReturnType("DWB");
                    else if (tileDown == symbols[1] || tileDown == symbols[2] || leftTile == Tile.nType.TWALLDOUBLE || leftTile == Tile.nType.BLCURVESINGLE)
                        return tl.ReturnType("DWT");
                    else if (tileUp == symbols[0])
                        return tl.ReturnType("DWB");
                    else if (tileDown == symbols[0])
                        return tl.ReturnType("DWT");
                    else return null;
                }
                else if (tileUp == symbols[3] && tileDown == symbols[3])
                {
                    if (tileRight == symbols[3])
                        return tl.ReturnType("SWL");
                    else if (tileLeft == symbols[3])
                        return tl.ReturnType("SWR");
                    else if (tileRight == symbols[1] || tileRight == symbols[2])
                        return tl.ReturnType("DWL");
                    else if (tileLeft == symbols[1] || tileLeft == symbols[2])
                        return tl.ReturnType("DWR");
                    else if (tileLeft == symbols[0] && (upperTile == Tile.nType.LWALLDOUBLE || upperTile == Tile.nType.TRCURVESINGLE))
                        return tl.ReturnType("DWL");
                    else if (tileRight == symbols[0] && (upperTile == Tile.nType.RWALLDOUBLE || upperTile == Tile.nType.TLCURVESINGLE))
                        return tl.ReturnType("DWR");
                    else return null;
                }
                else if (tileUp == symbols[3] && tileLeft == symbols[3])
                {
                    if (TLCorner == symbols[3] || TLCorner == symbols[0])
                        return tl.ReturnType("SCBR");
                    else if (TLCorner == symbols[1] || TLCorner == symbols[2])
                        return tl.ReturnType("DCBR");
                    else return null;
                }
                else if (tileRight == symbols[3] && tileDown == symbols[3])
                {
                    if (BRCorner == symbols[3] || BRCorner == symbols[0])
                        return tl.ReturnType("SCTL");
                    else if (BRCorner == symbols[1] || BRCorner == symbols[2])
                        return tl.ReturnType("DCTL");
                    else return null;
                }
                else if (tileUp == symbols[3] && tileRight == symbols[3])
                {
                    if (TRCorner == symbols[3] || TRCorner == symbols[0])
                        return tl.ReturnType("SCBL");
                    else if (TRCorner == symbols[1] || TRCorner == symbols[2])
                        return tl.ReturnType("DCBL");
                    else return null;
                }
                else if (tileLeft == symbols[3] && tileDown == symbols[3])
                {
                    if (BLCorner == symbols[3] || BLCorner == symbols[0])
                        return tl.ReturnType("SCTR");
                    else if (BLCorner == symbols[1] || BLCorner == symbols[2])
                        return tl.ReturnType("DCTR");
                    else return null;
                }
                else if (tileLeft == '\0' && tileRight == symbols[3])
                    return tl.ReturnType("DWT");
                else if ((tileUp == '\0' && tileDown == symbols[3]) ||
                        (tileDown == '\0' && tileUp == symbols[3]))
                    return tl.ReturnType("DWL");
                else return null;
            }
            else if (tile == symbols[4])
                return tl.ReturnType("X");
            else return null;
        }
    }
}
