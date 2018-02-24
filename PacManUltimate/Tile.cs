using System;
using System.Collections.Generic;
using System.Drawing;
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

        public void DrawTile(nType tile, Graphics g, Point location, Color color)
        {
            switch (tile)
            {
                case nType.DOT:
                    Dot(g, location); break;
                case nType.POWERDOT:
                    PowerDot(g, location);  break;
                case nType.GATE:
                    Gate(g, location); break;
                case nType.LWALLDOUBLE:
                    LWallDouble(g, location, color); break;
                case nType.RWALLDOUBLE:
                    RWallDouble(g, location, color); break;
                case nType.TWALLDOUBLE:
                    TWallDouble(g, location, color); break;
                case nType.BWALLDOUBLE:
                    BWallDouble(g, location, color); break;
                case nType.LWALLSINGLE:
                    LWallSingle(g, location, color); break;
                case nType.RWALLSINGLE:
                    RWallSingle(g, location, color); break;
                case nType.TWALLSINGLE:
                    TWallSingle(g, location, color); break;
                case nType.BWALLSINGLE:
                    BWallSingle(g, location, color); break;
                case nType.TLCURVEDOUBLE:
                    TLCurveDouble(g, location, color); break;
                case nType.TRCURVEDOUBLE:
                    TRCurveDouble(g, location, color); break;
                case nType.BRCURVEDOUBLE:
                    BRCurveDouble(g, location, color); break;
                case nType.BLCURVEDOUBLE:
                    BLCurveDouble(g, location, color); break;
                case nType.TLCURVESINGLE:
                    TLCurveSingle(g, location, color); break;
                case nType.TRCURVESINGLE:
                    TRCurveSingle(g, location, color); break;
                case nType.BRCURVESINGLE:
                    BRCurveSingle(g, location, color); break;
                case nType.BLCURVESINGLE:
                    BLCurveSingle(g, location, color); break;
                default:
                    break;
            }
        }

        public void FreeTile(Graphics g, Point location)
        {
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(location.X, location.Y, 16, 16));
        }

        private void Dot(Graphics g, Point location)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(246, 172, 152)), new Rectangle(location.X + 6, location.Y + 6, 4, 4));   
        }

        private void PowerDot(Graphics g, Point location)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(246, 172, 152)), new Rectangle(location.X, location.Y, 16, 16));
        }

        private void Gate(Graphics g, Point location)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 187, 229)), new Rectangle(location.X, location.Y + 10, 16, 5));
        }

        private void LWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y, 2, 16));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 6, location.Y, 2, 16));
        }

        private void LWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 6, location.Y, 2, 16));
        }

        private void RWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 14, location.Y, 2, 16));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 8, location.Y, 2, 16));
        }

        private void RWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 8, location.Y, 2, 16));
        }

        private void TWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y, 16, 2));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 6, 2, 16));
        }
        private void TWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 6, 2, 16));
        }

        private void BWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 14, 16, 2));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 8, 2, 16));
        }

        private void BWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 8, 2, 16));
        }

        private void TLCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y + 15),
                                                        new Point(location.X + 3,location.Y + 3),
                                                        new Point(location.X + 15, location.Y)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 6,location.Y + 15),
                                                        new Point(location.X + 8,location.Y + 8),
                                                        new Point(location.X + 15, location.Y + 6)});
        }

        private void TLCurveSingle(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 8,location.Y + 15),
                                                        new Point(location.X + 10,location.Y + 10),
                                                        new Point(location.X + 15, location.Y + 8)});
        }

        // Have no idea how I aligned this shit
        private void TRCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y),
                                                        new Point(location.X + 12,location.Y + 3),
                                                        new Point(location.X + 15, location.Y + 15)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y + 6),
                                                        new Point(location.X + 7,location.Y + 8),
                                                        new Point(location.X + 9, location.Y + 15)});
        }

        // Aligned to left
        private void TRCurveSingle(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y + 8),
                                                        new Point(location.X + 4,location.Y + 10),
                                                        new Point(location.X + 6, location.Y + 15)});
        }

        // Aligned to topleft border of the line
        private void BRCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 8,location.Y),
                                                        new Point(location.X + 6,location.Y + 6),
                                                        new Point(location.X, location.Y + 8)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 14,location.Y),
                                                        new Point(location.X + 11,location.Y + 11),
                                                        new Point(location.X, location.Y + 14)});
        }

        // Aligned to top left
        private void BRCurveSingle(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 6,location.Y),
                                                        new Point(location.X + 4,location.Y + 4),
                                                        new Point(location.X, location.Y + 6)});
        }

        private void BLCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y),
                                                        new Point(location.X + 3,location.Y + 12),
                                                        new Point(location.X + 15, location.Y + 15)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 6,location.Y),
                                                        new Point(location.X + 8,location.Y + 7),
                                                        new Point(location.X + 15, location.Y + 9)});
        }

        private void BLCurveSingle(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 8,location.Y),
                                                        new Point(location.X + 10,location.Y + 5),
                                                        new Point(location.X + 15, location.Y + 7)});
        }
    }
}
