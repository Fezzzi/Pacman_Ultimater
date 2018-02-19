using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    class Tile
    {
        //Class provides resources for saving text symbols as tiles and easier further manipulation
        //also contains usefull function for converting selected keywords to tiles
        public enum nType { DOT, POWERDOT, FREE, GATE, LWALLDOUBLE, RWALLDOUBLE,
            TWALLDOUBLE, BWALLDOUBLE, LWALLSINGLE, RWALLSINGLE, TWALLSINGLE,
            BWALLSINGLE, TLCURVEDOUBLE, TRCURVEDOUBLE, BRCURVEDOUBLE, BLCURVEDOUBLE,
            TLCURVESINGLE, TRCURVESINGLE, BRCURVESINGLE, BLCURVESINGLE, TILE};

        public nType ReturnType(string tile)
        {
            nType ret;
            switch (tile)
            {
                case " ":
                    ret = nType.FREE;
                    break;
                case "X":
                    ret = nType.GATE;
                    break;
                case ".":
                    ret = nType.DOT;
                    break;
                case "o":
                    ret = nType.POWERDOT;
                    break;
                case "DWL":
                    ret = nType.LWALLDOUBLE;
                    break;
                case "DWR":
                    ret = nType.RWALLDOUBLE;
                    break;
                case "DWT":
                    ret = nType.TWALLDOUBLE;
                    break;
                case "DWB":
                    ret = nType.BWALLDOUBLE;
                    break;
                case "SWL":
                    ret = nType.LWALLSINGLE;
                    break;
                case "SWR":
                    ret = nType.RWALLSINGLE;
                    break;
                case "SWT":
                    ret = nType.TWALLSINGLE;
                    break;
                case "SWB":
                    ret = nType.BWALLSINGLE;
                    break;
                case "DCTL":
                    ret = nType.TLCURVEDOUBLE;
                    break;
                case "DCTR":
                    ret = nType.TRCURVEDOUBLE;
                    break;
                case "DCBR":
                    ret = nType.BRCURVEDOUBLE;
                    break;
                case "DCBL":
                    ret = nType.BLCURVEDOUBLE;
                    break;
                case "SCTL":
                    ret = nType.TLCURVESINGLE;
                    break;
                case "SCTR":
                    ret = nType.TRCURVESINGLE;
                    break;
                case "SCBR":
                    ret = nType.BRCURVESINGLE;
                    break;
                case "SCBL":
                    ret = nType.BLCURVESINGLE;
                    break;
                default:
                    ret = nType.TILE;
                    break;
            }
            return ret;
        }
    }
}
