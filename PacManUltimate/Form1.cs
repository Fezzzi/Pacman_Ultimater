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
    public partial class PacManUltimate : Form
    {
        #region - VARIABLES Block -

        enum mn { game = 0, selectmap, settings, vs, highscore, start, submenu = 6 };

        int HighScore = -1, keyTicks = 5, Score, CollectedDots, Lives, munch, GhostsEaten, ticks;
        int Level, SoundTick, Score2, FreeGhost, GhostRelease, EatEmTimer, keyCountdown1, keyCountdown2;
        bool gameOn = false, Player2 = false, keyPressed1, keyPressed2;
        bool Sound = true, Music = true, extraLifeGiven, killed;
        string SoundPath = System.IO.Path.GetFullPath("../sounds/");
        Tuple<mn,Label> menuSelected;
        mn menuLayer;
        List<char> symbols = new List<char>();
        WMPLib.WindowsMediaPlayer SoundPlayer = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer SoundPlayer2 = new WMPLib.WindowsMediaPlayer();
        System.Media.SoundPlayer MusicPlayer = new System.Media.SoundPlayer();
        Tuple<Tile.nType?[][], int, Tuple<int, int>> Map;
        Tile.nType?[][] MapFresh;
        Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>[] Entities;
        Direction.nType NewDirection1 = Direction.nType.DIRECTION;
        Direction.nType NewDirection2 = Direction.nType.DIRECTION;
        PictureBox[][] PictureMap;
        PictureBox[] PacLives;
        Tuple<int, int> TopGhostInTiles;
        Label ScoreBox = new Label(), HighScoreBox = new Label(), Score2Box = new Label();
        EntitiesClass.nType[] DefaultAIs;
        List<Label> activeElements = new List<Label>();

        #endregion

        #region - MENU Block -

        public PacManUltimate()
        {
            InitializeComponent(true);
        }

        private void PacManUltimate_Load(object sender, EventArgs e)
        {
            menuLayer = mn.start;
            menuSelected = new Tuple<mn, Label>(mn.game, OrgGame);
            Menu(Menu_Start);
        }

        private new void Menu(Action Menu_Func)     // Use of built-in delegate for void functions that take no parameters 
        {
            // Function that makes Menu work by simple enabling and disabling visibility of selected controls.
            // Depends on the part of menu the player is in.

            for (int i = 0; i < activeElements.Count; i++)
                activeElements[i].Visible = false;

            activeElements = new List<Label>();
            Menu_Func();

            for (int i = 0; i < activeElements.Count; i++)
                activeElements[i].Visible = true;
        }

        private void Menu_Start()
        {
            activeElements.Add(PressEnter);
            activeElements.Add(Pacman);
            activeElements.Add(ultimate);
            activeElements.Add(copyright);
        }

        private void Menu_MainMenu()
        {
            activeElements.Add(selectMap);
            activeElements.Add(OrgGame);
            activeElements.Add(Settings);
            activeElements.Add(EscLabel);
            activeElements.Add(HighScr);
            activeElements.Add(VS);
        }

        private void Menu_SelectMap()
        {
            activeElements.Add(ErrorLdMap);
            activeElements.Add(ErrorInfo);
            activeElements.Add(AdvancedLdBut);
            activeElements.Add(TryAgainBut);
            activeElements.Add(EscLabel);
        }

        private void Menu_HighScore()
        {
            activeElements.Add(GameOverLabel);
            activeElements.Add(ScoreLabel);
            activeElements.Add(ScoreNum);
            activeElements.Add(HighScoreLabel);
            activeElements.Add(HighScoreNum);
            activeElements.Add(EscLabel);

            //Two branches depending on the mode player has chosen - normal x VS
            if (!Player2)
            {
                if (Score != 0)
                {
                    GameOverLabel.Text = "GAME OVER";
                    GameOverLabel.ForeColor = Color.Red;
                    GameOverLabel.Location = new Point(52, 33);
                    ScoreLabel.Text = "Your Score";
                    ScoreNum.Text = Score.ToString();
                }
                else
                {
                    ScoreLabel.Text = "";
                    ScoreNum.Text = "";
                    GameOverLabel.Text = "";
                }
                if (HighScore == -1)
                {
                    //In case the HighScore is not loaded yet (value is -1) do so
                    HighScoreClass hscr = new HighScoreClass();
                    HighScore = hscr.LoadHighScore();
                }
                HighScoreLabel.Text = "Highest Score";
                HighScoreLabel.ForeColor = Color.Yellow;
                HighScoreNum.ForeColor = Color.Yellow;
                HighScoreNum.Text = HighScore.ToString();
            }
            else
            {
                //Game selects the winner as the pleyer with highest score
                //In case of tie chooses the winner by remaing pacman lives
                if (Score < Score2 || (Score == Score2 && Lives <= 0))
                {
                    GameOverLabel.Text = "GHOSTS WIN";
                    GameOverLabel.ForeColor = Color.Red;
                    GameOverLabel.Location = new Point(36, 33);
                    HighScoreLabel.Text = "2UP";
                    HighScoreLabel.ForeColor = Color.Red;
                    HighScoreNum.Text = Score2.ToString();
                    HighScoreNum.ForeColor = Color.Red;
                    ScoreLabel.Text = "1UP";
                    ScoreNum.Text = Score.ToString();
                }
                else
                {
                    GameOverLabel.Text = "PACMAN WINS";
                    GameOverLabel.ForeColor = Color.Yellow;
                    GameOverLabel.Location = new Point(34, 33);
                    HighScoreLabel.Text = "1UP";
                    HighScoreNum.Text = Score.ToString();
                    ScoreLabel.Text = "2UP";
                    ScoreNum.Text = Score2.ToString();
                }
            }
            //It is necessary to set scre and player boolean in order to be able to access default Highscore
            //page later on from menu
            Score = 0;
            Player2 = false;
        }

        private void Menu_Settings()
        {
            activeElements.Add(MusicButton);
            activeElements.Add(SoundsButton);
            activeElements.Add(EscLabel);

            //Buttons load with color depending on associated booleans
            if (Music)
                MusicButton.BackColor = Color.Yellow;
            else
                MusicButton.BackColor = Color.Gray;

            if (Sound)
                SoundsButton.BackColor = Color.Yellow;
            else
                SoundsButton.BackColor = Color.Gray;
        }

        private void OrgGame_Click(object sender, EventArgs e)
        {
            //loads predefined original map and calls MakeItHappen to procced to game
            LoadMap loadMap = new LoadMap("../OriginalMap.txt");
            if (loadMap.Map != null)
                MakeItHappen(loadMap);
        }

        private void AdvancedLdBut_Click(object sender, EventArgs e)
        {
            menuLayer = mn.submenu;
            activeElements.Add(TypeSymbols);
            activeElements.Add(TypedSymbols);
            TypedSymbols.Text = "";
            activeElements.Add(TypeHint);
            for (int i = 0; i < activeElements.Count; i++)
                activeElements[i].Visible = true;
        }

        private void selectMap_Click(object sender, EventArgs e)
        {
            //Opens file dialog after clinking on Select Map in menu
            //Afterwards tries to open a load a map from the file
            //In case of success calls procedure MakeItHappen
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                menuLayer = mn.submenu;
                Menu(Menu_SelectMap);
                string path = openFileDialog1.FileName;
                LoadMap loadMap;
                if (symbols.Count == 0)
                    loadMap = new LoadMap(path);
                else
                {
                    loadMap = new LoadMap(path, symbols.ToArray());
                    symbols = new List<char>();
                }
                if (loadMap.Map != null)
                    MakeItHappen(loadMap);
            }
        }

        private void MusicButton_Click(object sender, EventArgs e)
        {
            Music = !Music;
            if (Music)
                MusicButton.BackColor = Color.Yellow;
            else
                MusicButton.BackColor = Color.Gray;
        }

        private void SoundsButton_Click(object sender, EventArgs e)
        {
            Sound = !Sound;
            if (Sound)
                SoundsButton.BackColor = Color.Yellow;
            else
                SoundsButton.BackColor = Color.Gray;
        }

        private void VS_Click(object sender, EventArgs e)
        {
            selectMap_Click(new object(), EventArgs.Empty);
            if(gameOn)
                Player2 = true;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            menuLayer = mn.submenu;
            Menu(Menu_Settings);
        }

        private void HighScr_Click(object sender, EventArgs e)
        {
            menuLayer = mn.submenu;
            Menu(Menu_HighScore);
        }

        private string CharListToString(List<char> source)
        {
            //Function to fill string  with typed characters separated by ';'
            string output = "";
            if(source.Count() > 0)
                output = source[0].ToString();
            char[] array = source.ToArray();
            for(int i = 1; i < source.Count(); i++)
            {
                output += " ; " + source[i];
            }
            return output;
        }

        private void MoveInMenu(int delta)
        {
            const byte menuSize = 5;
            Label newLabel = EnumToLabel((mn)(((int)menuSelected.Item1 + delta + menuSize) % menuSize));
            HighlightSelected(menuSelected.Item2, newLabel);
            menuSelected = new Tuple<mn, Label>((mn)(((int)menuSelected.Item1 + delta + menuSize) % menuSize), newLabel);
        }

        private void PacManUltimate_KeyDown(object sender, KeyEventArgs e)
        {
            // Function that handles player's input.
            // Seperate branch for menu input and game input.
            const byte symbolsLimit = 5;
            if (!gameOn)
            {
                if (menuLayer == mn.start)
                {
                    Menu(Menu_MainMenu);
                    HighlightSelected(menuSelected.Item2, OrgGame);
                    menuSelected = new Tuple<mn, Label>(mn.game, OrgGame);
                    menuLayer = mn.game;
                }
                else
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        // Escape returns you to menu form everywhere except from menu itself.
                        if (menuLayer == mn.submenu)
                        {
                            Menu(Menu_MainMenu);
                            HighlightSelected(menuSelected.Item2, OrgGame);
                            menuSelected = new Tuple<mn, Label>(mn.game, OrgGame);
                            menuLayer = mn.game;
                        }
                        else
                        {
                            menuLayer = mn.start;
                            Menu(Menu_Start);
                        }
                    }
                    else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                        MoveInMenu(-1);
                    else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                        MoveInMenu(+1);
                    else if (e.KeyCode == Keys.Enter)
                    {
                        (EnumToAction(menuSelected.Item1))(new object(), new EventArgs());
                    }

                    else if (TypedSymbols.Visible == true)
                    {
                        // Branch accessible during typing of symbols used on Map to load.
                        if (e.KeyCode == Keys.Back && symbols.Count() > 0)
                        {
                            symbols.RemoveAt(symbols.Count() - 1);
                            TypedSymbols.Text = CharListToString(symbols);
                        }
                        if (!symbols.Contains((char)e.KeyValue) && e.KeyCode != Keys.Back)
                        {
                            symbols.Add((char)e.KeyValue);
                            TypedSymbols.Text = CharListToString(symbols);
                        }
                        Refresh();
                        if (symbols.Count() == symbolsLimit)
                            selectMap_Click(new object(), EventArgs.Empty);
                    }
                }
            }
            else
            {
                //Two booleans keyPressed1 and 2 to notice which of the players during VS play has pushed the key
                if (Player2)
                {
                    if (e.KeyCode == Keys.A || e.KeyCode == Keys.W || e.KeyCode == Keys.D || e.KeyCode == Keys.S)
                        keyPressed1 = true;
                    else
                        keyPressed2 = true;
                }
                else
                    keyPressed1 = true;
                //NewDirection1 and 2 to save desired direction of both players
                if (e.KeyCode == Keys.A || !Player2 && e.KeyCode == Keys.Left)
                    NewDirection1 = Direction.nType.LEFT;
                else if (e.KeyCode == Keys.W || !Player2 && e.KeyCode == Keys.Up)
                    NewDirection1 = Direction.nType.UP;
                else if (e.KeyCode == Keys.D || !Player2 && e.KeyCode == Keys.Right)
                    NewDirection1 = Direction.nType.RIGHT;
                else if (e.KeyCode == Keys.S || !Player2 && e.KeyCode == Keys.Down)
                    NewDirection1 = Direction.nType.DOWN;
                else if (Player2 && e.KeyCode == Keys.Left)
                    NewDirection2 = Direction.nType.LEFT;
                else if (Player2 && e.KeyCode == Keys.Up)
                    NewDirection2 = Direction.nType.UP;
                else if (Player2 && e.KeyCode == Keys.Right)
                    NewDirection2 = Direction.nType.RIGHT;
                else if (Player2 && e.KeyCode == Keys.Down)
                    NewDirection2 = Direction.nType.DOWN;
                else if (e.KeyCode == Keys.Escape)
                    EndGame();
                //In case the statment has reached this part the pushed key is invalid so disable the booleans
                else if (Player2)
                {
                    keyPressed1 = false;
                    keyPressed2 = false;
                }
                else
                    keyPressed1 = false;
            }
        }

        private void Hover(object sender, EventArgs e)
        {
            //Function that provides color change of labels in menu          
            Label label = (Label)sender;
            HighlightSelected(menuSelected.Item2, label);
            menuSelected = new Tuple<mn, Label>(LabelToEnum(label), label);
        }

        private mn LabelToEnum(Label label)
        {
            if (label == OrgGame)
                return mn.game;
            else if (label == selectMap)
                return mn.selectmap;
            else if (label == VS)
                return mn.vs;
            else if (label == HighScr)
                return mn.highscore;
            else if (label == Settings)
                return mn.settings;
            else return mn.start;
        }

        private Label EnumToLabel(mn selected)
        {
            if (selected == mn.game)
                return OrgGame;
            else if (selected == mn.selectmap)
                return selectMap;
            else if (selected == mn.vs)
                return VS;
            else if (selected == mn.highscore)
                return HighScr;
            else if (selected == mn.settings)
                return Settings;
            else return OrgGame;
        }

        private Action<object,EventArgs> EnumToAction(mn selected)
        {
            if (selected == mn.game)
                return OrgGame_Click;
            else if (selected == mn.selectmap)
                return selectMap_Click;
            else if (selected == mn.vs)
                return VS_Click;
            else if (selected == mn.highscore)
                return HighScr_Click;
            else if (selected == mn.settings)
                return Settings_Click;
            else return OrgGame_Click;
        }

        private void HighlightSelected(Label prevLabel, Label newLabel)
        {
            prevLabel.ForeColor = Color.White;
            prevLabel.Font = new Font(prevLabel.Font.FontFamily, 21);
            newLabel.ForeColor = Color.Yellow;
            newLabel.Font = new Font(newLabel.Font.FontFamily, 23);
        }

        #endregion

        //----------START LEVEL BLOCK-----------------------------------------------------------

        private PictureBox[][] RenderMap(Tile.nType?[][] tiles)
        {
            //Function that handles loading, setting and placing of all the map tiles in game control
            PictureBox[][] pictureMap = new PictureBox[31][];
            for (int i = 0; i < 31; i++)
            {
                pictureMap[i] = new PictureBox[28];
                for (int j = 0; j < 28; j++)
                {
                    PictureBox pic = new PictureBox();
                    placePictureBox
                        (
                            pic,
                            Image.FromFile("../textures/" + tiles[i][j].ToString() + ".png"),
                            new Point((j * 16), ((i + 3) * 16)),
                            new Size(16, 16)
                        );
                    pictureMap[i][j] = pic;
                }
            }
            placePictureBox
                (new PictureBox(), Image.FromFile("../textures/FREE.png"), new Point(28 * 16, 0), new Size(16, 16));
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

        private void placePictureBox(PictureBox pic, Image image, Point point, Size size)
        {
            //Physicaly places picture in the control
            pic.Image = image;
            pic.Location = point;
            pic.Size = size;
            this.Controls.Add(pic);
        }

        private void placeLabel(Label label, string text, Color color, Point point, Font font)
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
            DefaultAIs = new EntitiesClass.nType[4]
            {
                EntitiesClass.nType.HOSTILERNDM, EntitiesClass.nType.HOSTILERNDM,
                EntitiesClass.nType.HOSTILERNDM, EntitiesClass.nType.HOSTILERNDM
            };

            Entities = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>[5]
                {
                    new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                        (13, 23, new PictureBox(),Direction.nType.LEFT,EntitiesClass.nType.PLAYER1),
                    new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                        (TopGhostInTiles.Item1, TopGhostInTiles.Item2, new PictureBox(),
                        Direction.nType.LEFT, Player2 ? EntitiesClass.nType.PLAYER2 : DefaultAIs[0]),
                    new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                        (TopGhostInTiles.Item1 - 2, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[1]),
                    new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                        (TopGhostInTiles.Item1, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[2]),
                    new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                        (TopGhostInTiles.Item1 + 2, TopGhostInTiles.Item2 + 3,
                        new PictureBox(),Direction.nType.DIRECTION, DefaultAIs[3]),
                };

            //Setting entities names for easy later manipulation and automatic image selection
            for (int i = 1; i < 6; i++)
                Entities[i - 1].Item3.Name = "Entity" + i.ToString();

            //Physical placing of the entities's images on the map and preseting  their starting images
            placePictureBox(Entities[0].Item3,
                Image.FromFile("../Textures/PacStart.png"),
                new Point(Entities[0].Item1 * 16 + 4, Entities[0].Item2 * 16 + 42), new Size(28, 28));
            placePictureBox(Entities[1].Item3,
                Image.FromFile("../Textures/Entity2Left.png"),
                new Point(Entities[1].Item1 * 16 - 13, Entities[1].Item2 * 16 + 42), new Size(28, 28));

            for (int i = 2; i < 5; i++)
                placePictureBox(Entities[i].Item3,
                    Image.FromFile("../Textures/Entity" + (i + 1).ToString() + (i % 2 == 0 ? "Up.png" : "Down.png")),
                    new Point(Entities[i].Item1 * 16 - 13, Entities[i].Item2 * 16 + 42), new Size(28, 28));
        }

        private void LoadHud(int hp)
        {
            //Function that loads score labels and pacma lives
            int lives = 0;
            placeLabel(new Label(), "1UP", Color.White,
                new Point(3 * 16, 0), new Font("Arial", 13, FontStyle.Bold));
            placeLabel(ScoreBox, Score > 0 ? Score.ToString() : "00", Color.White,
                new Point(4 * 16, 20), new Font("Arial", 13, FontStyle.Bold));
            //Selects labels depeding on game mode
            if (!Player2)
            {
                placeLabel(HighScoreBox, HighScore > 0 ? HighScore.ToString() : "00", Color.White,
                    new Point(14 * 16, 20), new Font("Arial", 13, FontStyle.Bold));
                placeLabel(new Label(), "HIGH SCORE", Color.White,
                    new Point(10 * 16, 0), new Font("Arial", 13, FontStyle.Bold));
            }
            else
            {
                placeLabel(new Label(), "2UP", Color.White,
                    new Point(22 * 16, 0), new Font("Arial", 13, FontStyle.Bold));
                placeLabel(Score2Box, Score2 > 0 ? Score2.ToString() : "00", Color.White,
                    new Point(23 * 16, 20), new Font("Arial", 13, FontStyle.Bold));
            }
            //Places all three lives on their supposed place
            foreach (var item in PacLives)
            {
                item.Image = Image.FromFile("../textures/Life.png");
                item.Location = new Point((1 + lives) * 32, 17 * 32);
                item.Size = new Size(32, 32);
                this.Controls.Add(item);
                lives++;
            }
            //Sets visibility of lives depending on number of player's lives
            for (int i = 2; i > hp - 2 && i >= 0; i--)
                PacLives[i].Visible = false;
        }

        private void LoadingAndInit(Label loading)
        {
            //Procedure serving simply for initialization of variables at the map load up
            //and displaying loading screen
            this.Controls.Clear();
            loading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            loading.AutoSize = true;
            loading.Size = new System.Drawing.Size(361, 66);
            loading.Visible = true;
            placeLabel(loading, "Loading...", Color.Yellow, new Point(33, 199), new Font("Ravie", 30F, System.Drawing.FontStyle.Bold));
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

            if (Level == 0)
            {
                extraLifeGiven = false; //has the player received extra life at 10000pts?
                Score = 0; //p1 score
                Score2 = 0; //p2 score
                SoundTick = 0; //used for sound players to take turns
                munch = 0; //used for pacman to alternate between cloased and open mouth images
                CollectedDots = 0;
                Lives = 3;
                Level = 101;
                //Level++;
            }

            if (HighScore == -1)
            {
                HighScoreClass hscr = new HighScoreClass();
                HighScore = hscr.LoadHighScore();
            }
            LoadHud(Lives);
        }

        private void PlayGame(bool restart)
        {
            //provides loading and general preparing of the game at the level start up
            Label loading = new Label();
            LoadingAndInit(loading);

            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_intermission.wav";
                MusicPlayer.PlayLooping();
            }
            //gets the position of the first ghost located on teh top of a ghost house
            TopGhostInTiles = new Tuple<int, int>(Map.Item3.Item1, Map.Item3.Item2 - 1);
            LoadEntities();
            //places ready label displayed at the beginning of each game
            Label ready = new Label();
            placeLabel(ready, "READY!", Color.Yellow, new Point(11 * 16 - 8, 20 * 16 - 6), new Font("Ravie", 14, FontStyle.Bold));

            //nulls pellets and actual map in case of new level laod
            if (!restart)
            {
                CollectedDots = 0;
                DeepCopy(Map.Item1, ref MapFresh);
            }
            PictureMap = RenderMap(MapFresh);

            loading.Visible = false;
            Refresh();
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_beginning.wav";
                MusicPlayer.Play();
            }
            System.Threading.Thread.Sleep(4000);
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

        //----------PLAY LEVEL BLOCK------------------------------------------------------------

        private void EndGame()
        {
            //ends game by stoping game loop and enabling menu functionality
            //saves highscore in case of beating previous one by player
            //generally destroys all of the forms's controls and loading them again
            //with their default settings
            Updater.Stop();
            gameOn = false;
            MapFresh = null;
            if (Score >= HighScore)
            {
                HighScoreClass hscr = new HighScoreClass();
                hscr.SaveHighScore(Score);
            }
            MusicPlayer.Stop();
            this.Controls.Clear();
            components.Dispose();
            InitializeComponent(false);
            HighlightSelected(menuSelected.Item2, HighScr);     // To unhighlight previous selection and enable highscore load.
            menuSelected = new Tuple<mn, Label>(mn.highscore, HighScr);
            menuLayer = mn.submenu;
            Menu(Menu_HighScore);
        }

        private void KillPacman()
        {
            //Handles the event raised by unexcited pacman's contact with one of the ghosts
            Updater.Stop();
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_death.wav";
                MusicPlayer.Play();
            }
            if (Player2)
                Score2 += 1500;
            Entities[0].Item3.Image = Image.FromFile("../textures/PacStart.png");
            Refresh();
            Thread.Sleep(500);
            Entities[0].Item3.Image = Image.FromFile("../textures/PacExplode.png");
            Refresh();
            Thread.Sleep(600);
            Lives--;
            Controls.Clear();

            if (Lives > 0)
                PlayGame(true);
            else
                EndGame();
        }

        private void UpdateHud(int? score, Label box)
        {
            //update desired score box (highscore/player)
            box.Text = score.ToString();
        }

        private bool IsDirectionFree(int y, int x, Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType> entity)
        {
            //Checks whether the direction the entity is aiming in is free
            //function catches outofrange exception in order to provide possibility for
            //entities to teleport from left hand side to right and the opossite
            try
            {
                if (MapFresh[entity.Item2 + y][entity.Item1 + x] == Tile.nType.FREE ||
                MapFresh[entity.Item2 + y][entity.Item1 + x] == Tile.nType.DOT ||
                MapFresh[entity.Item2 + y][entity.Item1 + x] == Tile.nType.POWERDOT)
                    return true;
                else
                    return false;
            }
            catch (IndexOutOfRangeException e)
            {
                return true;
            }
        }

        private void SetToMove
            (ref Direction.nType newDirection, ref Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType> entity)
        {
            //Asks whether the direction entity wants to move is free and if so,
            //sests its direction and nulls variable for saving direction
            Direction dir = new Direction();
            Tuple<int, int> delta = dir.DirectionToTuple(newDirection);
            if (IsDirectionFree(delta.Item1, delta.Item2, entity))
            {
                entity = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                            (entity.Item1, entity.Item2, entity.Item3, newDirection, entity.Item5);
                newDirection = Direction.nType.DIRECTION;
            }
        }

        private void CanMove(ref Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType> entity)
        {
            //Checks whether the entity can continue in the direction it goes otherwise stops it
            Direction dir = new Direction();
            Tuple<int, int> delta = dir.DirectionToTuple(entity.Item4);
            if (!IsDirectionFree(delta.Item1, delta.Item2, entity))
                entity = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                    (entity.Item1, entity.Item2, entity.Item3, Direction.nType.DIRECTION, entity.Item5);
        }

        private void MovePicture
            (bool change, int dx, int dy, ref Tuple<int, int, PictureBox,
                Direction.nType, EntitiesClass.nType> entity)
        {
            //Phisically moves the entity and loads right image depending on the game situation and direction
            entity.Item3.Location = new Point((entity.Item3.Location.X + dx), (entity.Item3.Location.Y + dy));
            if (change)
            {
                //normal situation
                if (EatEmTimer <= 0 || entity.Item5 == EntitiesClass.nType.PLAYER1 ||
                    (entity.Item5 != EntitiesClass.nType.EATEN && (munch % 3 == 0) && EatEmTimer < 30))
                    entity.Item3.Image = Image.FromFile("../Textures/" + entity.Item3.Name + entity.Item4.ToString() + ".png");
                //entity has been eaten
                else if (entity.Item5 == EntitiesClass.nType.EATEN)
                    entity.Item3.Image = Image.FromFile("../Textures/Eyes" + entity.Item4.ToString() + ".png");
                //pacman is in the excited mode - ghost are vulnareble
                else
                    entity.Item3.Image = Image.FromFile("../textures/CanBeEaten.png");
            }
        }

        private void MoveEntity
            (bool change, int entX, int entY, int invX, int invY,
            ref Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType> entity)
        {
            //Moves entity's location in tiles 
            entity = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                            (entX, entY, entity.Item3, entity.Item4, entity.Item5);
            //Moves entity's physicall location and loads right image
            MovePicture(change, invX, invY, ref entity);
        }

        private void MoveIt(ref Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType> entity)
        {
            //moves entity in its saved direction
            //tells called function true or false in order to stop it from overwriting entity's image
            //in case of teleporting
            //e.g.: from program's point of view teleporting from left to right is moving to right as the
            //x axis has increased but in fact the entity is moving to left
            switch (entity.Item4)
            {
                case Direction.nType.LEFT:
                    if (entity.Item1 - 1 > -1)
                        MoveEntity(true, entity.Item1 - 1, entity.Item2, -16, 0, ref entity);
                    else
                        MoveEntity(false, 27, entity.Item2, 27 * 16, 0, ref entity);
                    break;
                case Direction.nType.RIGHT:
                    if (entity.Item1 + 1 < 28)
                        MoveEntity(true, entity.Item1 + 1, entity.Item2, 16, 0, ref entity);
                    else
                        MoveEntity(false, 0, entity.Item2, -27 * 16, 0, ref entity);
                    break;
                case Direction.nType.UP:
                    if (entity.Item2 - 1 > -1)
                        MoveEntity(true, entity.Item1, entity.Item2 - 1, 0, -16, ref entity);
                    else
                        MoveEntity(false, entity.Item1, 31, 0, 31 * 16, ref entity);
                    break;
                case Direction.nType.DOWN:
                    if (entity.Item2 + 1 < 31)
                        MoveEntity(true, entity.Item1, entity.Item2 + 1, 0, 16, ref entity);
                    else
                        MoveEntity(false, entity.Item1, 0, 0, -31 * 16, ref entity);
                    break;
                default:
                    break;
            }
            if (entity.Item5 == EntitiesClass.nType.PLAYER1)
            {
                //Provides switching between pacman's direction image and closed mouth image
                munch++;
                if (entity.Item4 != Direction.nType.DIRECTION && munch >= 4)
                {
                    entity.Item3.Image = Image.FromFile("../textures/PacStart.png");
                    munch = 0;
                }
            }
        }

        private bool IsAtCrossroad(int x, int y)
        {
            //checks whether the entity has reached a crossroad
            //catches outofrange exception in case the entity is on verge of teleporting
            int turns = 0;
            for (int j = 0; j < 2; j++)
            {
                for (int i = -1; i < 2; i += 2)
                {
                    try
                    {
                        if (Map.Item1[y + (j == 0 ? i : 0)][x + (j == 1 ? i : 0)] == Tile.nType.FREE ||
                            Map.Item1[y + (j == 0 ? i : 0)][x + (j == 1 ? i : 0)] == Tile.nType.DOT ||
                            Map.Item1[y + (j == 0 ? i : 0)][x + (j == 1 ? i : 0)] == Tile.nType.POWERDOT)
                            turns++;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        turns++;
                    }
                }
            }
            if (turns >= 3)
                return true;
            else return false;
        }

        private void UpdateMove()
        {
            //Function that moves all of the entities and checks whether pacman and ghost has met
            //direction of entities controlled by players are updated via newDirection variables
            //direction of UI entities is set through entitiesclass UI algorithms
            if (NewDirection1 != Direction.nType.DIRECTION)
                SetToMove(ref NewDirection1, ref Entities[0]);
            if (NewDirection2 != Direction.nType.DIRECTION)
                SetToMove(ref NewDirection2, ref Entities[1]);

            EntitiesClass entClass = new EntitiesClass();
            for (int i = 0; i <= FreeGhost; i++)
            {
                if ((i == 0 || (EatEmTimer <= 0 || (EatEmTimer > 0 && munch % 2 == 1))) && !(i == 0 && ticks == 0))
                {
                    //if statemnt causes each odd turn to be skipped during pacman's excitemnt
                    //causing ghosts to slow down to half of the pacman's speed
                    if ((i > 0 && !Player2) || i > 1)
                    {
                        //if entity is UI, creates new instance with direction selected by entitiesclass UI algorithm
                        if (Entities[i].Item4 == Direction.nType.DIRECTION || IsAtCrossroad(Entities[i].Item1, Entities[i].Item2))
                            Entities[i] = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                                (
                                Entities[i].Item1,
                                Entities[i].Item2,
                                Entities[i].Item3,
                                entClass.NextStep
                                    (
                                    Entities[i].Item5,
                                    new Tuple<int, int>(Entities[i].Item1, Entities[i].Item2),
                                    Entities[i].Item5 == EntitiesClass.nType.EATEN ?
                                        TopGhostInTiles : new Tuple<int, int>(Entities[0].Item1, Entities[0].Item2),
                                    Entities[i].Item4, Map.Item1
                                    ),
                                Entities[i].Item5
                                );
                    }
                    CanMove(ref Entities[i]);
                    MoveIt(ref Entities[i]);
                }

                //checks if distance in tiles between pacman and entity is smaller or equal 1
                if (i > 0 &&
                    (Math.Abs(Entities[i].Item1 - Entities[0].Item1) + Math.Abs(Entities[i].Item2 - Entities[0].Item2)) <= 1)
                {
                    if (EatEmTimer <= 0)
                    {
                        KillPacman();
                        killed = true; //tady to nehraje-------------------------------------------------------------------------------------
                        return;
                    }
                    //In case of pacman's excitemnet and if the ghost is not already  eaten
                    //changes the ghost's state to eaten and increases player's score
                    else if (Entities[i].Item5 != EntitiesClass.nType.EATEN)
                    {
                        if (Sound)
                        {
                            SoundPlayer2.URL = SoundPath + "pacman_eatghost.wav";
                            SoundPlayer2.controls.play();
                        }
                        if (Music)
                        {
                            MusicPlayer.SoundLocation = "../sounds/pacman_eatensiren.wav";
                            MusicPlayer.PlayLooping();
                        }
                        SoundTick = 2;
                        GhostsEaten++;
                        Score += 200 * GhostsEaten;
                        UpdateHud(Score, ScoreBox);
                        Entities[i] = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                            (Entities[i].Item1, Entities[i].Item2, Entities[i].Item3, Entities[i].Item4, EntitiesClass.nType.EATEN);
                    }
                }
            }
        }

        private void UpdateEatEmTimer()
        {
            //Provides timer for pacman's excitement and changes all of the ghost back to normal at the end
            if (EatEmTimer > 1)
                EatEmTimer--;
            else if (EatEmTimer == 1)
            {
                if (Music)
                {
                    MusicPlayer.SoundLocation = "../sounds/pacman_siren.wav";
                    MusicPlayer.PlayLooping();
                }
                if (GhostsEaten != 0)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        Entities[i] = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                                (Entities[i].Item1, Entities[i].Item2, Entities[i].Item3, Entities[i].Item4,
                                (Player2 && i == 1 ? EntitiesClass.nType.PLAYER2 : DefaultAIs[i - 1]));
                    }
                    GhostsEaten = 0;
                }
                EatEmTimer = 0;
            }
        }

        private void UpdateEatPellet()
        {
            //Controls whether pacman is not on a tile with pellet
            //if so plays the sound and increases number of collected pellets and score
            //also enables pacman's excitemnet via EatEmTimer 
            if (MapFresh[Entities[0].Item2][Entities[0].Item1] == Tile.nType.DOT ||
                MapFresh[Entities[0].Item2][Entities[0].Item1] == Tile.nType.POWERDOT)
            {
                if (Sound)
                {
                    SoundTick++;
                    if (SoundTick >= 4 || SoundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                        SoundTick = 0;
                    if (SoundTick == 0 && (Score < 10000 || Score > 10100))
                    {
                        SoundPlayer.URL = SoundPath + "pacman_chomp.wav";
                        SoundPlayer.controls.play();
                    }
                    else if (SoundTick == 2)
                    {
                        SoundPlayer2.URL = SoundPath + "pacman_chomp.wav";
                        SoundPlayer2.controls.play();
                    }
                }
                CollectedDots++;
                if (MapFresh[Entities[0].Item2][Entities[0].Item1] == Tile.nType.DOT)
                    Score += 10;
                else
                {
                    Score += 50;
                    if (Music)
                    {
                        MusicPlayer.SoundLocation = "../sounds/pacman_powersiren.wav";
                        MusicPlayer.PlayLooping();
                    }
                    //Pacman's excitemnt lasts shorter each level
                    EatEmTimer = Player2 ? 75 : 100 - Level;
                    GhostsEaten = 0;
                    //Return all of the ghost to normal state to be able to be eaten again later
                    for (int i = 1; i < 5; i++)
                    {
                        Entities[i] = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                                (Entities[i].Item1, Entities[i].Item2, Entities[i].Item3, Entities[i].Item4,
                                (Player2 && i == 1 ? EntitiesClass.nType.PLAYER2 : EntitiesClass.nType.HOSTILERNDM));
                    }
                }

                //Delets pellet from the tile by direct replacing of the image
                //Sometimes the image is still in use when the program needed it to be used again
                //in such case the assosiated exception is ignored and the action is tried again
                MapFresh[Entities[0].Item2][Entities[0].Item1] = Tile.nType.FREE;
                try
                {
                    PictureMap[Entities[0].Item2][Entities[0].Item1].Image = Image.FromFile("../Textures/Free.png");
                }
                catch (InvalidOperationException e)
                {
                    PictureMap[Entities[0].Item2][Entities[0].Item1].Image = Image.FromFile("../Textures/Free.png");
                }
                if (Score > HighScore)
                {
                    HighScore = Score;
                    UpdateHud(HighScore, HighScoreBox);
                }
                UpdateHud(Score, ScoreBox);
            }
        }

        private void SetGhostFree(int ghostNum)
        {
            //Places ghost specified by number out of ghost house and sets his start direction 
            //in order to make him move around.
            //Also resets timer for ghost releasing
            Entities[ghostNum] = new Tuple<int, int, PictureBox, Direction.nType, EntitiesClass.nType>
                (TopGhostInTiles.Item1, TopGhostInTiles.Item2, Entities[ghostNum].Item3,
                Direction.nType.LEFT, Entities[ghostNum].Item5);
            Entities[ghostNum].Item3.Location = new Point(Entities[ghostNum].Item1 * 16 - 7, Entities[ghostNum].Item2 * 16 + 42);
            GhostRelease = Player2 ? 130 / 3 : (260 - Level) / 3;
            FreeGhost++;
        }

        private void Update()
        {
            //Body of the update mechanism
            //selectivly updates entities and map
            ticks++;
            if ((Level < 15 && ticks > 19 - Level) || (Level >= 15 && ticks > 3))
                ticks = 0;
            UpdateMove();
            if (killed)
            {
                killed = false;
                return;
            }
            UpdateEatEmTimer();
            UpdateEatPellet();            
            //Gives player one extra life at 10000pts
            if (Score >= 10000 && !extraLifeGiven)
            {
                extraLifeGiven = true;
                if (Sound)
                {
                    SoundPlayer.URL = SoundPath + "pacman_extrapac.wav";
                    SoundPlayer.controls.play();
                }
                Lives++;
                PacLives[Lives - 2].Visible = true;
            }
            //Handles successive ghost releasing
            if (GhostRelease <= 0 && FreeGhost < 4)
                SetGhostFree(FreeGhost + 1);
            if (GhostRelease > 0 && FreeGhost < 4)
                GhostRelease --;
        }

        private void KeyCountAndDir(ref Direction.nType direction, ref int keyCountdown)
        {
            //Checks whether the direction player intends to move is free in such case nulls assosiated timer
            //otherwise continues in countdown
            if (direction == Direction.nType.DIRECTION && keyCountdown != 0)
                keyCountdown = 0;
            else if (direction != Direction.nType.DIRECTION && keyCountdown > 1)
                keyCountdown--;
            else if (direction != Direction.nType.DIRECTION && keyCountdown == 1)
            {
                direction = Direction.nType.DIRECTION;
                keyCountdown = 0;
            }
        }

        private void GameLoop()
        {
            //Topmost layer of instructions to execute each frame

            //In case one of the players have pushed a valid key starts countdown
            //which represents time remaining until the information about the pushed button is lost
            if (keyPressed1)
            {
                keyCountdown1 = keyTicks;
                keyPressed1 = false;
            }
            if (keyPressed2)
            {
                keyCountdown2 = keyTicks;
                keyPressed2 = false;
            }
            //Calls main Update function
            Update();

            //Function for countdown
            KeyCountAndDir(ref NewDirection1, ref keyCountdown1);
            if (Player2)
                KeyCountAndDir(ref NewDirection2, ref keyCountdown2);

            //Checks if the player has not already collected all the pellets
            //in such case depending on level and game mode plays another level or ends game
            if (CollectedDots >= Map.Item2)
            {
                Updater.Stop();
                Controls.Clear();
                Level++;
                if (Level < 256 && !Player2)
                    PlayGame(false);
                else
                    EndGame();
            }
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            //Creates ilusion of game loop
            GameLoop();
        }
    }
}
