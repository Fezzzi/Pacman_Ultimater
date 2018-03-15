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

        const int PacTimer = 100;
        const byte EntityCount = 5;
        const byte MaxLives = 6;
        const byte FieldSizeInRows = 31;
        const byte FieldSizeInColumns = 28;
        const byte TileSizeInPxs = 16;
        const byte EntitiesSizeInPxs = 28;
        const int OpeningThemeLength = 4000;
        const byte pacmanInitialX = 13;
        const byte pacmanInitialY = 23;

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
            if(color != Color.White)
                MapColor = color;
            for (int i = 0; i < FieldSizeInRows; i++)
                for (int j = 0; j < FieldSizeInColumns; j++)
                {
                    tiles[i][j].DrawTile(bg.Graphics, new Point(j * TileSizeInPxs, (i + 3) * TileSizeInPxs), color);
                }
        }

        /// <summary>
        /// Creates deep copy of map array so the game can modify actual map but does not lose
        /// information about original map.
        /// </summary>
        /// <param name="source">Original Map.</param>
        /// <param name="destination">New Map.</param>
        private void DeepCopy(Tile[][] source, ref Tile[][] destination)
        {
            
            destination = new Tile[source.Count()][];
            for (int i = 0; i < source.Count(); i++)
            {
                destination[i] = new Tile[source[i].Count()];
                for (int j = 0; j < source[i].Count(); j++)
                    destination[i][j] = new Tile(source[i][j].tile);
            }
        }

        /// <summary>
        /// Physicaly places picture's control in the form.
        /// </summary>
        /// <param name="pic">Picture's control.</param>
        /// <param name="image">Image to be assigned to picture's control.</param>
        /// <param name="point">Picture's location.</param>
        /// <param name="size">Picture's image size.</param>
        private void PlacePictureBox(PictureBox pic, Image image, Point point, Size size)
        {
            pic.Image = image;
            pic.Location = point;
            pic.Size = size;
            this.Controls.Add(pic);
        }

        /// <summary>
        /// Physically places label's control in the form.
        /// </summary>
        /// <param name="label">Label's control.</param>
        /// <param name="text">Text to be assigned to label's control.</param>
        /// <param name="color">Assigned text's color.</param>
        /// <param name="point">Label's location.</param>
        /// <param name="font">Text's font.</param>
        private void PlaceLabel(Label label, string text, Color color, Point point, Font font)
        {
            label.Text = text;
            label.ForeColor = color;
            label.Location = point;
            label.Font = font;
            label.AutoSize = true;
            this.Controls.Add(label);
        }

        /// <summary>
        /// Function that loads all the game entities and presets all their default settings such as position, direction, etc...
        /// </summary>
        private void LoadEntities()
        {
            //Entitie's Data strucute consists of:
            //  - Two numbers - x and y position on the map in Tiles.
            //  - Picturebox containing entity's image and its physical location.
            //  - Direction used later for entity's movement and selecting the right image.
            //  - Type of entity such as Player1, Player2, or all the other kinds of enemy's AI.
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

        /// <summary>
        /// Function that loads score labels and pacman lives.
        /// </summary>
        /// <param name="hp">Number of lives to be displayed.</param>
        private void LoadHud(int hp)
        {
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

        /// <summary>
        /// Procedure serving simply for initialization of variables at the map load up
        /// and displaying loading screen.
        /// </summary>
        /// <param name="loading">Control of loading label.</param>
        /// <param name="levelLabel">Control of level label.</param>
        /// <param name="color">From file loaded color.</param>
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
                for (int i = 0; i < SoundPlayersCount; i++)
                    SoundPlayers[i] = new WMPLib.WindowsMediaPlayer();

                CollectedDots = 0;
                Lives = 3;
                Level++;
            }

            this.Controls.Clear();
            loading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            loading.AutoSize = true;
            loading.Visible = true;
            PlaceLabel(loading, "Loading...", Color.Yellow, new Point(90, 159), new Font("Ravie", 30F, FontStyle.Bold));
            levelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            levelLabel.AutoSize = true;
            levelLabel.Visible = true;
            PlaceLabel(levelLabel, "- Level " + Level.ToString() + " -", Color.Red, new Point(120, 350), new Font("Ravie", 20F, FontStyle.Bold));
            Refresh();

            keyPressed1 = false; //has player 1 pressed a valid button?
            keyPressed2 = false; //has player 2 pressed a valid button?
            GhostsEaten = 0; //number of ghost eaten by pacman during pacman"s excitement
            keyCountdown1 = 0;
            keyCountdown2 = 0;
            killed = false;
            Ticks = 0; // Counts tick to enable power pellets flashing and ghost flashing at the end of pac's excitement.
            FreeGhost = 1; // Number of active ghosts moving through the map.
            GhostRelease = Player2 ? 130 / 3 : (260 - Level) / 3; // Timer for ghost releasing - decreasing with level.
            EatEmTimer = 0; //timer for pacman's excitement
            PacLives = new PictureBox[MaxLives];
            for (int i = 0; i < MaxLives; i++)
                PacLives[i] = new PictureBox();

            // Yet empty fields of the array would redraw over top right corner of the map.
            // This way it draw empty tile on pacman's initial position tile which is empty by definiton.
            for (int i = 0; i < RDPSize; i++)
                redrawPellets[i] = new Point(pacmanInitialY, pacmanInitialX);

            if (HighScore == -1)
            {
                HighScoreClass hscr = new HighScoreClass();
                HighScore = hscr.LoadHighScore();
            }
        }

        /// <summary>
        /// Returns random color by fixing one channel to max, another to min and the last one chooses randomly.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Plays loading animation.
        /// </summary>
        private async Task PlayAnimation()
        {
            PictureBox[] Elements = new PictureBox[5];
            Random rndm = new Random();
            int elemCount = rndm.Next(1, 5);
            int pacCount = 0;

            for (int i = 1; i <= elemCount; i++)
            {
                Elements[i-1] = new PictureBox();
                Elements[i-1].Image = Image.FromFile("../textures/Entity" + i.ToString() + "Left.png");
                Elements[i-1].Size = new Size(EntitiesSizeInPxs, EntitiesSizeInPxs);
                this.Controls.Add(Elements[i-1]);
            }

            for (int j = FieldSizeInColumns * TileSizeInPxs; j > -200; j -= 4)
            {
                ++pacCount;
                for (int i = 0; i < elemCount; i++)
                {
                    Elements[i].Location = new Point(j + (i == 0 ? 0 : (i + 4) * (2 * TileSizeInPxs)), (FieldSizeInRows / 2 + 3) * TileSizeInPxs);
                    if (i == 0 && pacCount % 4 == 0)
                        if (pacCount % 8 == 0)
                            Elements[i].Image = Image.FromFile("../Textures/PacStart.png");
                        else
                            Elements[i].Image = Image.FromFile("../textures/Entity1Left.png");
                }

                Refresh();
                await Task.Delay(10);
            }

            for (int i = 0; i < elemCount; i++)
                this.Controls.Remove(Elements[i]);           
        }

        /// <summary>
        /// Provides loading and general preparing of the game at the level start-up.
        /// </summary>
        /// <param name="restart">Whether triggered by lavel's restart or finish.</param>
        private async void PlayGame(bool restart)
        {
            Label loading = new Label();
            Label levelLabel = new Label();
            LoadingAndInit(loading, levelLabel, Map.Item4);

            if (!restart)
            {
                if (Music)
                {
                    MusicPlayer.SoundLocation = "../sounds/pacman_intermission.wav";
                    MusicPlayer.PlayLooping();
                }
                Task playAnim = PlayAnimation();
                await playAnim;
            }

            LoadHud(Lives - 2);
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
                if (Level <= 13)
                    GhostUpdater.Interval -= 5;
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
                PacUpdater.Start();
                GhostUpdater.Interval = Player2 ? (PacTimer + 40 - (Level > 13 ? 65 : Level * 5)) : PacUpdater.Interval + 10;
                GhostUpdater.Start();
            }
        }

        /// <summary>
        /// Function that provides bridge between menu and the game.
        /// Disables menu's functionality by assifning empty function to it.
        /// Switches input (KeyDown) to game mode by turning gameOn.
        /// Initializes Map and calls function that makes the game start.
        /// </summary>
        /// <param name="loadMap"></param>
        private void MakeItHappen(LoadMap loadMap)
        {
            Menu(() => { });    // Lambda expression - calls empty action <=> Does nothing
            Level = 0;
            gameOn = true;
            Map = loadMap.Map;
            PlayGame(false);
        }

        #endregion
    }
}
