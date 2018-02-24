using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Input;

namespace PacManUltimate
{
    partial class PacManUltimate : Form
    {
        #region - VARIABLES Block -

        int Lives, Level;
        Tuple<Tile.nType?[][], int, Tuple<int, int>> Map;
        Tuple<int, int, PictureBox, Direction.nType, DefaultAI>[] Entities;
        DefaultAI[] DefaultAIs;
        Tuple<int, int> TopGhostInTiles;
        PictureBox[] PacLives;

        const int FieldSizeInRows = 31;
        const int FieldSizeInColumns = 28;
        const int TileSizeInPxs = 16;
        const int EntitiesSizeInPxs = 28;
        const int OpeningThemeLength = 4000;
        const int pacmanInitialX = 13;
        const int pacmanInitialY = 23;

        #endregion

        #region - STARTGAME Block -

        private PictureBox[][] RenderMap(Tile.nType?[][] tiles)
        {
            //Function that handles loading, setting and placing of all the map tiles in game control
            PictureBox[][] pictureMap = new PictureBox[FieldSizeInRows][];
            for (int i = 0; i < FieldSizeInRows; i++)
            {
                pictureMap[i] = new PictureBox[FieldSizeInColumns];
                for (int j = 0; j < FieldSizeInColumns; j++)
                {
                    PictureBox pic = new PictureBox();
                    PlacePictureBox
                        (
                            pic,
                            Image.FromFile("../textures/" + tiles[i][j].ToString() + ".png"),
                            new Point((j * TileSizeInPxs), ((i + 3) * TileSizeInPxs)),
                            new Size(TileSizeInPxs, TileSizeInPxs)
                        );
                    pictureMap[i][j] = pic;
                }
            }
            PlacePictureBox(new PictureBox(), Image.FromFile("../textures/FREE.png"),
                            new Point(FieldSizeInColumns * TileSizeInPxs, 0),
                            new Size(TileSizeInPxs, TileSizeInPxs));
            return pictureMap;
        }

        private void DeepCopy(Tile.nType?[][] source, ref Tile.nType?[][] destination)
        {
            //Creates deep copy of map array so the game can modify actual map but does not lose
            //information about original map 
            destination = new Tile.nType?[source.Count()][];
            for (int i = 0; i < source.Count(); i++)
            {
                destination[i] = new Tile.nType?[source[i].Count()];
                for (int j = 0; j < source[i].Count(); j++)
                {
                    destination[i][j] = source[i][j];
                }
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
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK),
                new DefaultAI(DefaultAI.nType.HOSTILEATTACK)
            };

            Entities = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>[5]
                {
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (pacmanInitialX, pacmanInitialY, new PictureBox(),Direction.nType.LEFT, new DefaultAI(DefaultAI.nType.PLAYER1)),
                    new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                        (TopGhostInTiles.Item1, TopGhostInTiles.Item2, new PictureBox(),
                        Direction.nType.LEFT, Player2 ? new DefaultAI(DefaultAI.nType.PLAYER2) : DefaultAIs[0]),
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
            const int EntityCount = 6;
            for (int i = 1; i < EntityCount; i++)
                Entities[i - 1].Item3.Name = "Entity" + i.ToString();

            // Physical placing of the entities's images on the map and preseting  their starting images.
            // All those magic numbers are X and Y axis correction for entities' pictures to be correctly placed.
            PlacePictureBox(Entities[0].Item3,
                Image.FromFile("../Textures/PacStart.png"),
                new Point(Entities[0].Item1 * TileSizeInPxs + 4, Entities[0].Item2 * TileSizeInPxs + 42), new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));
            PlacePictureBox(Entities[1].Item3,
                Image.FromFile("../Textures/Entity2Left.png"),
                new Point(Entities[1].Item1 * TileSizeInPxs - 13, Entities[1].Item2 * TileSizeInPxs + 42), new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));

            for (int i = 2; i < EntityCount - 1; i++)
                PlacePictureBox(Entities[i].Item3,
                    Image.FromFile("../Textures/Entity" + (i + 1).ToString() + (i % 2 == 0 ? "Up.png" : "Down.png")),
                    new Point(Entities[i].Item1 * TileSizeInPxs - 13, Entities[i].Item2 * TileSizeInPxs + 42), new Size(EntitiesSizeInPxs, EntitiesSizeInPxs));
        }

        private void LoadHud(int hp)
        {
            //Function that loads score labels and pacman lives
            const int heartSizeInPx = 32;
            int lives = 0;

            PlaceLabel(new Label(), "1UP", Color.White,
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
                PlaceLabel(new Label(), "2UP", Color.White,
                    new Point(22 * TileSizeInPxs, 0), new Font("Arial", 13, FontStyle.Bold));
                PlaceLabel(Score2Box, Score2 > 0 ? Score2.ToString() : "00", Color.White,
                    new Point(23 * TileSizeInPxs, 20), new Font("Arial", 13, FontStyle.Bold));
            }
            //Places all three lives on their supposed place
            foreach (var item in PacLives)
            {
                item.Image = Image.FromFile("../textures/Life.png");
                item.Location = new Point((1 + lives) * heartSizeInPx, 34 * TileSizeInPxs);
                item.Size = new Size(heartSizeInPx, heartSizeInPx);
                this.Controls.Add(item);
                lives++;
            }
            //Sets visibility of lives depending on number of player's lives
            for (int i = 2; i > hp - 2 && i >= 0; i--)
                PacLives[i].Visible = false;
        }

        private void LoadingAndInit(Label loading, Label levelLabel)
        {
            if (Level == 0)
            {
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
            PacLives = new PictureBox[3]{
                    new PictureBox(),new PictureBox(), new PictureBox()};

            if (HighScore == -1)
            {
                HighScoreClass hscr = new HighScoreClass();
                HighScore = hscr.LoadHighScore();
            }
            LoadHud(Lives);
        }

        private void PlayGame(bool restart)
        {
            //provides loading and general preparing of the game at the level start-up
            Label loading = new Label();
            Label levelLabel = new Label();
            LoadingAndInit(loading, levelLabel);

            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_intermission.wav";
                MusicPlayer.PlayLooping();
            }
            //gets the position of the first ghost located on the top of a ghost house
            TopGhostInTiles = new Tuple<int, int>(Map.Item3.Item1, Map.Item3.Item2 - 1);
            LoadEntities();
            //places ready label displayed at the beginning of each game
            Label ready = new Label();
            PlaceLabel(ready, "READY!", Color.Yellow, new Point(11 * TileSizeInPxs - 8, 20 * TileSizeInPxs - 6), new Font("Ravie", 14, FontStyle.Bold));

            //nulls pellets and actual map in case of new level laod
            if (!restart)
            {
                CollectedDots = 0;
                DeepCopy(Map.Item1, ref MapFresh);
            }
            PictureMap = RenderMap(MapFresh);

            loading.Visible = false;
            levelLabel.Visible = false;
            Refresh();
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_beginning.wav";
                MusicPlayer.Play();
            }
            System.Threading.Thread.Sleep(OpeningThemeLength);
            ready.Dispose();
            Refresh();
            //corects start positions of pacman and first ghost as they were located between the tiles at first
            Entities[0].Item3.Location = new Point(Entities[0].Item3.Location.X - 10, Entities[0].Item3.Location.Y);
            Entities[1].Item3.Location = new Point(Entities[1].Item3.Location.X + 6, Entities[1].Item3.Location.Y);
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_siren.wav";
                MusicPlayer.PlayLooping();
            }
            //starts updater that provides effect of main game cycle
            Updater.Start();
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
