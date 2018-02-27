using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PacManUltimate
{
    partial class PacManUltimate : Form
    {
        #region - VARIABLES Block -

        int Lives, Level;
        Tuple<Tile[][], int, Tuple<int, int>, Color, List<Point>> Map;
        Tuple<int, int, PictureBox, Direction.nType, DefaultAI>[] Entities;
        DefaultAI[] DefaultAIs;
        Tuple<int, int> TopGhostInTiles;
        PictureBox[] PacLives;
        Graphics g;
        BufferedGraphics bg;
        Size defSize;
        Label up1, up2;
        Color MapColor;

        const int MaxLives = 6;
        const int FieldSizeInRows = 31;
        const int FieldSizeInColumns = 28;
        const int TileSizeInPxs = 16;
        const int EntitiesSizeInPxs = 28;
        const int OpeningThemeLength = 4000;
        const int pacmanInitialX = 13;
        const int pacmanInitialY = 23;

        #endregion

        #region - STARTGAME Block -

        /// <summary>
        /// Function that handles loading, setting and placing of all the map tiles in game control.
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        private void RenderMap(Tile[][] tiles, Color color)
        {
            g.Clear(this.BackColor);
            BufferedGraphicsContext bgc = BufferedGraphicsManager.Current;
            bg = bgc.Allocate(g, this.DisplayRectangle);

            if (color == Color.Transparent)
                color = ChooseRandomColor();

            MapColor = color;
            for (int i = 0; i < FieldSizeInRows; i++)
                for (int j = 0; j < FieldSizeInColumns; j++)
                {
                    tiles[i][j].DrawTile(bg.Graphics, new Point(j * TileSizeInPxs, (i + 3) * TileSizeInPxs), color);
                }
        }

        private void DeepCopy(Tile[][] source, ref Tile[][] destination)
        {
            //Creates deep copy of map array so the game can modify actual map but does not lose
            //information about original map 
            destination = new Tile[source.Count()][];
            for (int i = 0; i < source.Count(); i++)
            {
                destination[i] = new Tile[source[i].Count()];
                for (int j = 0; j < source[i].Count(); j++)
                    destination[i][j] = new Tile(source[i][j].tile);
            }
        }

        private void PlacePictureBox(PictureBox pic, Image image, Point point, Size size)
        {
            //Physicaly places picture in the control
            pic.Image = image;
            pic.Location = point;
            pic.Size = size;
            this.Controls.Add(pic);
        }

        private void PlaceLabel(Label label, string text, Color color, Point point, Font font)
        {
            //Physically places label in the control
            label.Text = text;
            label.ForeColor = color;
            label.Location = point;
            label.Font = font;
            label.AutoSize = true;
            this.Controls.Add(label);
        }

        private void LoadEntities()
        {
            //Function that loads all the game entities and presets all their default settings such as position, direction, etc...
            //This Data strucute consists of:
            //  - Two numbers - x and y position on the map in Tiles
            //  - Picturebox containing entity's image and its physical location
            //  - Direction used later for entity's movement and selecting the right image
            //  - Type of entity such as Player1, Player2, or all the kinds of enemy AI
            DefaultAIs = new DefaultAI[4]
            {
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK, FieldSizeInColumns, FieldSizeInRows),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK, FieldSizeInColumns, FieldSizeInRows),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK, FieldSizeInColumns, FieldSizeInRows),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK, FieldSizeInColumns, FieldSizeInRows)
            };

            Entities = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>[5]
                {
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (pacmanInitialX, pacmanInitialY, new PictureBox(),Direction.nType.LEFT, new DefaultAI(DefaultAI.nType.PLAYER1, FieldSizeInColumns, FieldSizeInRows)),
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (TopGhostInTiles.Item1, TopGhostInTiles.Item2, new PictureBox(),
                        Direction.nType.LEFT, Player2 ? new DefaultAI(DefaultAI.nType.PLAYER2, FieldSizeInColumns, FieldSizeInRows) : DefaultAIs[0]),
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (TopGhostInTiles.Item1 - 2, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[1]),
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (TopGhostInTiles.Item1, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[2]),
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (TopGhostInTiles.Item1 + 2, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[3]),
                };

            //Setting entities names for easy later manipulation and automatic image selection
            const int EntityCount = 5;
            for (int i = 1; i <= EntityCount; i++)
                Entities[i - 1].Item3.Name = "Entity" + i.ToString();

            // Physical placing of the entities's images on the map and preseting  their starting images.
            // All those magic numbers are X and Y axis correction for entities' pictures to be correctly placed.
            PlacePictureBox(Entities[0].Item3,
                            Image.FromFile("../Textures/PacStart.png"),
                            new Point(Entities[0].Item1 * TileSizeInPxs + 3, Entities[0].Item2 * TileSizeInPxs + 42),
                            new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));

            PlacePictureBox(Entities[1].Item3,
                            Image.FromFile("../Textures/Entity2Left.png"),
                            new Point(Entities[1].Item1 * TileSizeInPxs + 3, Entities[1].Item2 * TileSizeInPxs + 42), 
                            new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));

            for (int i = 2; i < EntityCount; i++)
                PlacePictureBox(Entities[i].Item3,
                                Image.FromFile("../Textures/Entity" + (i + 1).ToString() + (i % 2 == 0 ? "Up.png" : "Down.png")),
                                new Point(Entities[i].Item1 * TileSizeInPxs + 3, Entities[i].Item2 * TileSizeInPxs + 42), 
                                new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));
        }

        private void LoadHud(int hp)
        {
            //Function that loads score labels and pacman lives
            const int heartSizeInPx = 32;
            int lives = 0;

            up1 = new Label();
            PlaceLabel(up1, "1UP", Color.White,
                new Point(3 * TileSizeInPxs, 0), new Font("Arial", 13, FontStyle.Bold));
            PlaceLabel(ScoreBox, Score > 0 ? Score.ToString() : "00", Color.White,
                new Point(4 * TileSizeInPxs, 20), new Font("Arial", 13, FontStyle.Bold));
            //Selects labels depeding on game mode
            if (!Player2)
            {
                PlaceLabel(HighScoreBox, HighScore > 0 ? HighScore.ToString() : "00", Color.White,
                    new Point(14 * TileSizeInPxs, 20), new Font("Arial", 13, FontStyle.Bold));
                PlaceLabel(new Label(), "HIGH SCORE", Color.White,
                    new Point(10 * TileSizeInPxs, 0), new Font("Arial", 13, FontStyle.Bold));
            }
            else
            {
                up2 = new Label();
                PlaceLabel(up2, "2UP", Color.White,
                    new Point(22 * TileSizeInPxs, 0), new Font("Arial", 13, FontStyle.Bold));
                PlaceLabel(Score2Box, Score2 > 0 ? Score2.ToString() : "00", Color.White,
                    new Point(23 * TileSizeInPxs, 20), new Font("Arial", 13, FontStyle.Bold));
            }
            // Places all lives on their supposed place.
            foreach (var item in PacLives)
            {
                item.Image = Image.FromFile("../textures/Life.png");
                item.Location = new Point(lives * heartSizeInPx + TileSizeInPxs, ((FieldSizeInRows + 3) * TileSizeInPxs) + 4);
                item.Size = new Size(heartSizeInPx, heartSizeInPx);
                this.Controls.Add(item);
                lives++;
            }
            // Sets visibility of lives depending on number of player's lives.
            for (int i = MaxLives - 1; i > hp && i >= 0; i--)
                PacLives[i].Visible = false;
        }

        private void LoadingAndInit(Label loading, Label levelLabel, Color color)
        {
            if (Level == 0)
            {
                defSize = this.Size;
                this.AutoSize = false;
                this.Size = new Size((FieldSizeInColumns + 1) * TileSizeInPxs, (FieldSizeInRows + 8) * TileSizeInPxs);
                g = this.CreateGraphics();
                extraLifeGiven = false; //has the player received extra life at 10000pts?
                Score = 0; //p1 score
                Score2 = 0; //p2 score
                SoundTick = 0; //used for sound players to take turns
                munch = 0; //used for pacman to alternate between cloased and open mouth images
                CollectedDots = 0;
                Lives = 3;
                Level++;
            }

            //Procedure serving simply for initialization of variables at the map load up
            //and displaying loading screen
            this.Controls.Clear();
            loading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            loading.AutoSize = true;
            loading.Visible = true;
            PlaceLabel(loading, "Loading...", Color.Yellow, new Point(33, 199), new Font("Ravie", 30F, FontStyle.Bold));
            levelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            levelLabel.AutoSize = true;
            levelLabel.Visible = true;
            PlaceLabel(levelLabel, "- Level " + Level.ToString() + " -", Color.Red, new Point(75, 280), new Font("Ravie", 20F, FontStyle.Bold));
            Refresh();

            keyPressed1 = false; //has player 1 pressed a valid button?
            keyPressed2 = false; //has player 2 pressed a valid button?
            GhostsEaten = 0; //number of ghost eaten by pacman during pacman"s excitement
            keyCountdown1 = 0;
            keyCountdown2 = 0;
            killed = false;
            ticks = 0; //counts tick to enable pacman's slowdown throught the levels
            FreeGhost = 1; //number of active ghosts moving through the map
            GhostRelease = Player2 ? 130 / 3 : (260 - Level) / 3; //timer for ghost releasing - decreasing with level
            EatEmTimer = 0; //timer for pacman's excitement
            PacLives = new PictureBox[MaxLives];
            for (int i = 0; i < MaxLives; i++)
                PacLives[i] = new PictureBox();

            if (HighScore == -1)
            {
                HighScoreClass hscr = new HighScoreClass();
                HighScore = hscr.LoadHighScore();
            }

            LoadHud(Lives - 2);
        }

        private Color ChooseRandomColor()
        {
            Random rndm = new Random();
            switch (rndm.Next(1, 6))
            {
                case 1:
                    return Color.FromArgb(0, 255, rndm.Next(0, 255));
                case 2:
                    return Color.FromArgb(0, rndm.Next(0, 255), 0);
                case 3:
                    return Color.FromArgb(255, 0, rndm.Next(0, 255));
                case 4:
                    return Color.FromArgb(255, rndm.Next(0, 255), 0);
                case 5:
                    return Color.FromArgb(rndm.Next(0, 255), 0, 255);
                default:
                    return Color.FromArgb(rndm.Next(0, 255), 255, 0);
            }
        }

        private async void PlayGame(bool restart)
        {
            //provides loading and general preparing of the game at the level start-up
            Label loading = new Label();
            Label levelLabel = new Label();
            LoadingAndInit(loading, levelLabel, Map.Item4);

            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_intermission.wav";
                MusicPlayer.PlayLooping();
            }
            // Gets the position of the first ghost located on the top of a ghost house.
            TopGhostInTiles = new Tuple<int, int>(Map.Item3.Item1 - 1, Map.Item3.Item2 - 1);
            LoadEntities();
            // Places ready label displayed at the beginning of each game.
            Label ready = new Label();
            PlaceLabel(ready, "READY!", Color.Yellow, new Point(11 * TileSizeInPxs - 8, 20 * TileSizeInPxs - 6), new Font("Ravie", 14, FontStyle.Bold));

            // Nulls pellets and actual map in case of new level laod.
            if (!restart)
            {
                CollectedDots = 0;
                DeepCopy(Map.Item1, ref MapFresh);
            }
            RenderMap(MapFresh, Map.Item4);

            loading.Visible = false;
            levelLabel.Visible = false;
            Refresh();
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_beginning.wav";
                MusicPlayer.Play();
            }
            bg.Render(g);

            await Task.Delay(OpeningThemeLength);

            // It's possible that the player has pressed escape during the opening theme.
            if (gameOn)
            {
                ready.Dispose();
                Update();
                // Corects start positions of pacman and first ghost as they were located between the tiles at first.
                Entities[0].Item3.Location = new Point(Entities[0].Item3.Location.X - 9, Entities[0].Item3.Location.Y);
                Entities[1].Item3.Location = new Point(Entities[1].Item3.Location.X - 9, Entities[1].Item3.Location.Y);
                if (Music)
                {
                    MusicPlayer.SoundLocation = "../sounds/pacman_siren.wav";
                    MusicPlayer.PlayLooping();
                }
                // Starts updater that provides effect of main game cycle.
                Updater.Start();
            }
        }

        private void MakeItHappen(LoadMap loadMap)
        {
            //Function that provides bridge between menu and the game
            //disables menu functionality by setting it to menu
            //switches input to game mode by turning gameOn
            //Initializes Map and calls function that makes the game start
            Menu(() => { });    // Lambda expression - calls empty action <=> Does nothing
            Level = 0;
            gameOn = true;
            Map = loadMap.Map;
            PlayGame(false);
        }

        #endregion
    }
}
