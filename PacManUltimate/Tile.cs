using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManUltimate
{
    /// <summary>
    /// Class provides resources for saving text symbols as tiles and easier further manipulation.
    /// Also contains usefull function for converting selected keywords to tiles.
    /// </summary>
    class Tile
    {
        public nType tile;

        /// <summary>
        /// Enumerable for possible tile states.
        /// HTBDTILE = Horizontal To Be Decided Tile (VTBDTILE <=> Vertical....).
        /// </summary>
        public enum nType { DOT, POWERDOT, FREE, GATE, LWALLDOUBLE, RWALLDOUBLE,
            TWALLDOUBLE, BWALLDOUBLE, LWALLSINGLE, RWALLSINGLE, TWALLSINGLE,
            BWALLSINGLE, TLCURVEDOUBLE, TRCURVEDOUBLE, BRCURVEDOUBLE, BLCURVEDOUBLE,
            TLCURVESINGLE, TRCURVESINGLE, BRCURVESINGLE, BLCURVESINGLE, HTBDTILE, VTBDTILE, TILE};

        /// <summary>
        /// Function for conversion between string representation of tile states and enumerable.
        /// </summary>
        /// <param name="tile">string representation of tile state.</param>
        /// <returns>Returns coresponding tile state.</returns>
        public Tile(string tile)
        {
            switch (tile)
            {
                case " ":
                    this.tile = nType.FREE;
                    break;
                case "X":
                    this.tile = nType.GATE;
                    break;
                case ".":
                    this.tile = nType.DOT;
                    break;
                case "o":
                    this.tile = nType.POWERDOT;
                    break;
                case "DWL":
                    this.tile = nType.LWALLDOUBLE;
                    break;
                case "DWR":
                    this.tile = nType.RWALLDOUBLE;
                    break;
                case "DWT":
                    this.tile = nType.TWALLDOUBLE;
                    break;
                case "DWB":
                    this.tile = nType.BWALLDOUBLE;
                    break;
                case "SWL":
                    this.tile = nType.LWALLSINGLE;
                    break;
                case "SWR":
                    this.tile = nType.RWALLSINGLE;
                    break;
                case "SWT":
                    this.tile = nType.TWALLSINGLE;
                    break;
                case "SWB":
                    this.tile = nType.BWALLSINGLE;
                    break;
                case "DCTL":
                    this.tile = nType.TLCURVEDOUBLE;
                    break;
                case "DCTR":
                    this.tile = nType.TRCURVEDOUBLE;
                    break;
                case "DCBR":
                    this.tile = nType.BRCURVEDOUBLE;
                    break;
                case "DCBL":
                    this.tile = nType.BLCURVEDOUBLE;
                    break;
                case "SCTL":
                    this.tile = nType.TLCURVESINGLE;
                    break;
                case "SCTR":
                    this.tile = nType.TRCURVESINGLE;
                    break;
                case "SCBR":
                    this.tile = nType.BRCURVESINGLE;
                    break;
                case "SCBL":
                    this.tile = nType.BLCURVESINGLE;
                    break;
                case "VTBDTILE":
                    this.tile = nType.VTBDTILE;
                    break;
                case "HTBDTILE":
                    this.tile = nType.HTBDTILE;
                    break;
                default:
                    this.tile = nType.TILE;
                    break;
            }
        }

        /// <summary>
        /// Constructor for tile map deep copying.
        /// </summary>
        /// <param name="tileType">Source tile type.</param>
        public Tile(nType tileType)
        {
            this.tile = tileType;
        }

        /// <summary>
        /// Function for drawing tile states as their curve representation.
        /// </summary>
        /// <param name="tile">Processed tile.</param>
        /// <param name="location">Tile's top left corner.</param>
        /// <param name="color">Desired color of tile's curve representation.</param>
        public void DrawTile(Graphics g, Point location, Color color)
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

        /// <summary>
        /// Visualy frees tile by drawing rectangle over it, with the color of background.
        /// </summary>
        public void FreeTile(Graphics g, Point location, Color bkgColor)
        {
            g.FillRectangle(new SolidBrush(bkgColor), new Rectangle(location.X, location.Y, 16, 16));
        }

        #region - TILE STATES Curves representations -

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
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y - 1, 2, 18));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 6, location.Y - 1, 2, 18));
        }

        private void LWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 6, location.Y, 2, 16));
        }

        private void RWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 14, location.Y - 1, 2, 18));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 8, location.Y - 1, 2, 18));
        }

        private void RWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X + 8, location.Y, 2, 16));
        }

        private void TWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X - 1, location.Y, 18, 2));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X - 1, location.Y + 6, 18, 2));
        }
        private void TWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 6, 16, 2));
        }

        private void BWallDouble(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X - 1, location.Y + 14, 18, 2));
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X - 1, location.Y + 8, 18, 2));
        }

        private void BWallSingle(Graphics g, Point location, Color color)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(location.X, location.Y + 8, 16, 2));
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

        private void TRCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y),
                                                        new Point(location.X + 12,location.Y + 3),
                                                        new Point(location.X + 15, location.Y + 15)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y + 6),
                                                        new Point(location.X + 7,location.Y + 8),
                                                        new Point(location.X + 9, location.Y + 15)});
        }

        private void TRCurveSingle(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X,location.Y + 8),
                                                        new Point(location.X + 4,location.Y + 10),
                                                        new Point(location.X + 6, location.Y + 15)});
        }

        private void BRCurveDouble(Graphics g, Point location, Color color)
        {
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 8,location.Y),
                                                        new Point(location.X + 6,location.Y + 6),
                                                        new Point(location.X, location.Y + 8)});
            g.DrawCurve(new Pen(color, 2), new Point[] {new Point(location.X + 14,location.Y),
                                                        new Point(location.X + 11,location.Y + 11),
                                                        new Point(location.X, location.Y + 14)});
        }

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

        #endregion
    }
}
