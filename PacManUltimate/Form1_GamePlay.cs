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

        int keyTicks = 5, Score, CollectedDots, munch, GhostsEaten, ticks;
        int SoundTick, Score2, FreeGhost, GhostRelease, EatEmTimer, keyCountdown1, keyCountdown2;
        bool keyPressed1, keyPressed2;
        bool extraLifeGiven, killed;
        string SoundPath = System.IO.Path.GetFullPath("../sounds/");
        WMPLib.WindowsMediaPlayer SoundPlayer = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer SoundPlayer2 = new WMPLib.WindowsMediaPlayer();
        System.Media.SoundPlayer MusicPlayer = new System.Media.SoundPlayer();
        Tile[][] MapFresh;
        Direction.nType NewDirection1 = Direction.nType.DIRECTION;
        Direction.nType NewDirection2 = Direction.nType.DIRECTION;
        Point[] redrawPellets = new Point[RDPSize];
        byte RDPIndex = 0;

        const byte RDPSize = 12;
        const int MaxLevel = 256;
        const int BonusLifeScore = 10000;
        const int PelletScore = 10;
        const int PowerPelletScore = 50;
        const int BaseEatEmTimer = 100;
        const int BaseGhostReleaseTimer = 260;
        const byte GhostAccLimLevel = 30;
        const byte ghostMaxSpeed = 3;    // The smaller the number the faster the ghosts.
        const int ghostBaseSpeed = (GhostAccLimLevel / 2) + ghostMaxSpeed + 1;

        #endregion

        #region - GAMEPLAY Block -

        /// <summary>
        /// Function handling key pressing during gameplay.
        /// </summary>
        /// <param name="e">Identifies pressed key.</param>
        private void GameKeyDownHandler(KeyEventArgs e)
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

        /// <summary>
        /// Ends game by stoping game loop and enabling menu functionality.
        /// Saves new highscore in case of beating the previous one by the player.
        /// Generally destroys all of the forms's controls and loads them again with their default settings.
        /// </summary>
        private void EndGame()
        {
            Updater.Stop();
            gameOn = false;
            MapFresh = null;
            if (Score >= HighScore)
            {
                HighScoreClass hscr = new HighScoreClass();
                hscr.SaveHighScore(Score);
            }
            MusicPlayer.Stop();
            this.AutoSize = true;
            this.Size = defSize;
            this.Controls.Clear();
            components.Dispose();
            InitializeComponent(false);
            HighlightSelected(menuSelected.Item2, HighScr);     // To unhighlight previous selection and enable highscore load.
            menuSelected = new Tuple<mn, Label>(mn.highscore, HighScr);
            menuLayer = mn.submenu;
            Menu(Menu_HighScore);
        }

        /// <summary>
        /// Handles the event raised by unexcited pacman's contact with one of the ghosts.
        /// </summary>
        private void KillPacman()
        {
            const int PauseBeforeDeath = 500;
            const int ExplodingTime = 600;
            const int P2ScoreForKill = 1500;

            Updater.Stop();
            if (Music)
            {
                MusicPlayer.SoundLocation = "../sounds/pacman_death.wav";
                MusicPlayer.Play();
            }
            if (Player2)
                Score2 += P2ScoreForKill;
            Entities[0].Item3.Image = Image.FromFile("../textures/PacStart.png");
            Refresh();
            Thread.Sleep(PauseBeforeDeath);
            Entities[0].Item3.Image = Image.FromFile("../textures/PacExplode.png");
            Refresh();
            Thread.Sleep(ExplodingTime);
            Lives--;
            Controls.Clear();

            if (Lives > 0)
                PlayGame(true);
            else
                EndGame();
        }

        /// <summary>
        /// Updates desired score box (highscore/player).
        /// </summary>
        /// <param name="score">New Value to be set.</param>
        /// <param name="box">Box whose value is to be set.</param>
        private void UpdateHud(int? score, Label box)
        {          
            box.Text = score.ToString();
        }

        /// <summary>
        /// Checks whether the direction the entity is aiming in is free.
        /// </summary>
        /// <param name="y">Y-axis delta.</param>
        /// <param name="x">X-axis delta.</param>
        /// <param name="entity">The observed entity.</param>
        /// <returns>Returns boolean value indicating emptiness of the observed tile.</returns>
        private bool IsDirectionFree(int y, int x, Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
                int indexX = entity.Item1 + x;
                int indexY = entity.Item2 + y;

                if (indexX < 0 || indexX >= FieldSizeInColumns || indexY < 0 || indexY >= FieldSizeInRows
                    || MapFresh[indexY][indexX].tile == Tile.nType.FREE
                    || MapFresh[indexY][indexX].tile == Tile.nType.DOT
                    || MapFresh[indexY][indexX].tile == Tile.nType.POWERDOT)
                    return true;
                else
                    return false;
        }

        /// <summary>
        /// Tests whether the direction entity wants to move in is free and if so,
        /// sests its direction and sets variable for saving direction to default value.
        /// </summary>
        /// <param name="newDirection">Variable with stored target direction (set to default in case of success).</param>
        /// <param name="entity">The observed entity.</param>
        private void SetToMove
            (ref Direction.nType newDirection, ref Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
            //
            Direction dir = new Direction();
            Tuple<int, int> delta = dir.DirectionToTuple(newDirection);
            if (IsDirectionFree(delta.Item1, delta.Item2, entity))
            {
                entity = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                            (entity.Item1, entity.Item2, entity.Item3, newDirection, entity.Item5);
                newDirection = Direction.nType.DIRECTION;
            }
        }

        /// <summary>
        /// Checks whether the entity can continue in the direction it goes otherwise stops it.
        /// </summary>
        /// <param name="entity">The observed Entity.</param>
        private void CanMove(ref Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
            Direction dir = new Direction();
            Tuple<int, int> delta = dir.DirectionToTuple(entity.Item4);
            if (!IsDirectionFree(delta.Item1, delta.Item2, entity))
                entity = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                    (entity.Item1, entity.Item2, entity.Item3, Direction.nType.DIRECTION, entity.Item5);
        }

        /// <summary>
        /// Physically moves the entity and loads right image depending on the game situation and direction.
        /// </summary>
        /// <param name="change">Boolean indicating whether the entity's direction has changed.</param>
        /// <param name="dx">X-axis change.</param>
        /// <param name="dy">Y-axis change.</param>
        /// <param name="entity">The observed entity.</param>
        private void MovePicture
            (bool change, int dx, int dy, ref Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
            const int ghostFlashingStart = 30;

            if (entity.Item5.State != DefaultAI.nType.PLAYER1 && MapFresh[entity.Item2][entity.Item1].tile != Tile.nType.FREE)
            {
                redrawPellets[RDPIndex] = new Point(entity.Item2, entity.Item1);
                ++RDPIndex;
                RDPIndex %= RDPSize;
            }

            entity.Item3.Location = new Point((entity.Item3.Location.X + dx), (entity.Item3.Location.Y + dy));
            if (change)
            {
                // Normal situation.
                // Last Line of if statement ensures ghost flashing at the end of pacman excited mode
                if (EatEmTimer <= 0 || entity.Item5.State == DefaultAI.nType.PLAYER1 ||
                   (entity.Item5.State != DefaultAI.nType.EATEN && (munch % 3 == 0) && EatEmTimer < ghostFlashingStart))    
                        entity.Item3.Image = Image.FromFile("../Textures/" + entity.Item3.Name + entity.Item4.ToString() + ".png");
                // Entity has been eaten.
                else if (entity.Item5.State == DefaultAI.nType.EATEN)
                    entity.Item3.Image = Image.FromFile("../Textures/Eyes" + entity.Item4.ToString() + ".png");
                // Pacman is in the excited mode - ghosts are vulnerable.
                else
                    entity.Item3.Image = Image.FromFile("../textures/CanBeEaten.png");
            }
        }

        /// <summary>
        /// Combines entity's in-tiles movement with procedure handling its physicall movement and 
        /// right texture loading.
        /// </summary>
        /// <param name="change">Boolean indicating whether the entity's direction has changed.</param>
        /// <param name="entX">New entity's in-tiles X-axis position.</param>
        /// <param name="entY">New entity's in-tiles Y-axis position.</param>
        /// <param name="dX">Physicall X-axis change.</param>
        /// <param name="dY">Physicall Y-axis change.</param>
        /// <param name="entity">The observed entity.</param>
        private void MoveEntity(bool change, int entX, int entY, int dX, int dY,
            ref Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
            entity = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                            (entX, entY, entity.Item3, entity.Item4, entity.Item5);
            MovePicture(change, dX, dY, ref entity);
        }

        /// <summary>
        /// Moves entity in its saved direction and tells called function true or false in order to stop it from overwriting entity's image
        /// in case of teleporting (is reverse move from the program's point of view).
        /// </summary>
        /// <param name="entity">The observed entity.</param>
        private void MoveIt(ref Tuple<int, int, PictureBox, Direction.nType, DefaultAI> entity)
        {
            switch (entity.Item4)
            {
                case Direction.nType.LEFT:
                    if (entity.Item1 > 0)
                        MoveEntity(true, entity.Item1 - 1, entity.Item2, -TileSizeInPxs, 0, ref entity);
                    else
                        MoveEntity(false, FieldSizeInColumns - 1, entity.Item2, (FieldSizeInColumns - 1) * TileSizeInPxs, 0, ref entity);
                    break;
                case Direction.nType.RIGHT:
                    if (entity.Item1 < FieldSizeInColumns - 1)
                        MoveEntity(true, entity.Item1 + 1, entity.Item2, TileSizeInPxs, 0, ref entity);
                    else
                        MoveEntity(false, 0, entity.Item2, -(FieldSizeInColumns - 1) * TileSizeInPxs, 0, ref entity);
                    break;
                case Direction.nType.UP:
                    if (entity.Item2 > 0)
                        MoveEntity(true, entity.Item1, entity.Item2 - 1, 0, -TileSizeInPxs, ref entity);
                    else
                        MoveEntity(false, entity.Item1, FieldSizeInRows - 1, 0, (FieldSizeInRows - 1) * TileSizeInPxs, ref entity);
                    break;
                case Direction.nType.DOWN:
                    if (entity.Item2 < FieldSizeInRows - 1)
                        MoveEntity(true, entity.Item1, entity.Item2 + 1, 0, TileSizeInPxs, ref entity);
                    else
                        MoveEntity(false, entity.Item1, 0, 0, -(FieldSizeInRows - 1) * TileSizeInPxs, ref entity);
                    break;
                default:
                    break;
            }

            //Provides switching between pacman's direction image and closed mouth image
            if (entity.Item5.State == DefaultAI.nType.PLAYER1)
            {
                const byte munchLimit = 4;
                munch++;
                if (entity.Item4 != Direction.nType.DIRECTION && munch >= munchLimit)
                {
                    entity.Item3.Image = Image.FromFile("../textures/PacStart.png");
                    munch = 0;
                }
            }
        }

        /// <summary>
        /// Checks whether the position is a crossroad.
        /// </summary>
        /// <param name="x">X-axis postion.</param>
        /// <param name="y">Y-axis position.</param>
        /// <returns>Boolean indicating crossroad.</returns>
        private bool IsAtCrossroad(int x, int y)
        {
            int turns = 0;
            for (int j = 0; j < 2; j++)
                for (int i = -1; i < 2; i += 2)
                {
                    int indexY = y + (j == 0 ? i : 0);
                    int indexX = x + (j == 1 ? i : 0);
                    if (indexY < 0 || indexY >= FieldSizeInRows || indexX < 0 || indexX >= FieldSizeInColumns)
                        ++turns;
                    else if (Map.Item1[indexY][indexX].tile == Tile.nType.FREE ||
                        Map.Item1[indexY][indexX].tile == Tile.nType.DOT ||
                        Map.Item1[indexY][indexX].tile == Tile.nType.POWERDOT)
                        turns++;
                }
            if (turns >= 3)
                return true;
            else return false;
        }

        /// <summary>
        /// Function that moves all of the entities and checks whether the pacman and a ghost have met.
        /// </summary>
        private void UpdateMove()
        { 
            // Direction of entities controlled by players are updated via newDirection variables.
            // Direction of UI entities is set through AI algorithms.

            const int ghostEatBaseScore = 200;
            if (NewDirection1 != Direction.nType.DIRECTION)
                SetToMove(ref NewDirection1, ref Entities[0]);
            if (NewDirection2 != Direction.nType.DIRECTION)
                SetToMove(ref NewDirection2, ref Entities[1]);

            for (int i = 0; i <= FreeGhost; i++)
            {
                // if statemnt causes each odd turn to be skipped during pacman's excitemnt
                // causing ghosts to slow down to half of the pacman's speed.
                if ((i == 0 || (EatEmTimer <= 0 || (EatEmTimer > 0 && munch % 2 == 1))) && !(i == 0 && ticks == 0))
                {                   
                    if ((i > 0 && !Player2) || i > 1)
                    {
                        // if entity is AI, creates new instance with direction selected by AI algorithm.
                        if (Entities[i].Item4 == Direction.nType.DIRECTION || IsAtCrossroad(Entities[i].Item1, Entities[i].Item2))
                            Entities[i] = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                                (
                                Entities[i].Item1,
                                Entities[i].Item2,
                                Entities[i].Item3,
                                Entities[i].Item5.NextStep
                                    (
                                    new Tuple<int, int>(Entities[i].Item1, Entities[i].Item2),
                                    Entities[i].Item5.State == DefaultAI.nType.EATEN ?
                                        TopGhostInTiles 
                                        : new Tuple<int, int>(Entities[0].Item1, Entities[0].Item2), 
                                    Entities[i].Item4, Map.Item1), Entities[i].Item5);
                    }
                    CanMove(ref Entities[i]);
                    MoveIt(ref Entities[i]);
                }

                // Checks if distance in tiles between pacman and entity is smaller or equal 1.
                if (i > 0 &&
                    (Math.Abs(Entities[i].Item1 - Entities[0].Item1) + Math.Abs(Entities[i].Item2 - Entities[0].Item2)) <= 1)
                {
                    if (EatEmTimer <= 0)
                    {
                        KillPacman();
                        killed = true;
                        return;
                    }
                    // In case of pacman's excitemnet and if the ghost is not already eaten
                    // changes the ghost's state to eaten and increases player's score.
                    else if (Entities[i].Item5.State != DefaultAI.nType.EATEN)
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
                        Score += ghostEatBaseScore * GhostsEaten;
                        UpdateHud(Score, ScoreBox);
                        if(GhostsEaten == 4 && Lives < MaxLives - 1)
                        {
                            PacLives[Lives - 1].Visible = true;
                            ++Lives;
                            if (Sound)
                            {
                                SoundPlayer.URL = SoundPath + "pacman_extrapac.wav";
                                SoundPlayer.controls.play();
                            }
                        }
                        Entities[i].Item5.State = DefaultAI.nType.EATEN;
                    }
                }
            }
        }

        /// <summary>
        /// Provides timer for pacman's excitement and changes all of the ghost back to normal at the end.
        /// </summary>
        private void UpdateEatEmTimer()
        {
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
                        Entities[i].Item5.State = (Player2 && i == 1 ? DefaultAI.nType.PLAYER2 : DefaultAIs[i - 1].State);
                    GhostsEaten = 0;
                }
                EatEmTimer = 0;
            }
        }

        /// <summary>
        /// Checks whether pacman is not on a tile with pellet.
        /// if so, plays the sound and increases number of collected pellets and score.
        /// Also enables pacman's excitemnet via EatEmTimer.
        /// </summary>
        private void UpdateEatPellet()
        {
            if (MapFresh[Entities[0].Item2][Entities[0].Item1].tile == Tile.nType.DOT ||
                MapFresh[Entities[0].Item2][Entities[0].Item1].tile == Tile.nType.POWERDOT)
            {
                if (Sound)
                {
                    SoundTick++;
                    if (SoundTick >= 4 || SoundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                        SoundTick = 0;
                    if (SoundTick == 0 && (Score < BonusLifeScore || Score > BonusLifeScore + 100))
                    {
                        SoundPlayer.URL = SoundPath + "pacman_chomp.wav";
                        SoundPlayer.controls.play();
                    }
                    else if (SoundTick == 2 && (Score < BonusLifeScore || Score > BonusLifeScore + 100))
                    {
                        SoundPlayer2.URL = SoundPath + "pacman_chomp.wav";
                        SoundPlayer2.controls.play();
                    }
                }
                CollectedDots++;
                if (MapFresh[Entities[0].Item2][Entities[0].Item1].tile == Tile.nType.DOT)
                    Score += PelletScore;
                else
                {
                    Score += PowerPelletScore;
                    if (Music)
                    {
                        MusicPlayer.SoundLocation = "../sounds/pacman_powersiren.wav";
                        MusicPlayer.PlayLooping();
                    }
                    //Pacman's excitemnt lasts shorter each level
                    EatEmTimer = Player2 ? (3 * BaseEatEmTimer) / 4 : BaseEatEmTimer - Level;
                    GhostsEaten = 0;
                    //Return all of the ghost to normal state to be able to be eaten again later
                    for (int i = 1; i < 5; i++)
                        Entities[i].Item5.State = DefaultAI.nType.CANBEEATEN;
                }

                //Delets pellet from the tile
                MapFresh[Entities[0].Item2][Entities[0].Item1].tile = Tile.nType.FREE;
                MapFresh[Entities[0].Item2][Entities[0].Item1].FreeTile(bg.Graphics, new Point(Entities[0].Item1 * TileSizeInPxs, 
                                                                                              (Entities[0].Item2 + 3) * TileSizeInPxs), this.BackColor);

                if (Score > HighScore)
                {
                    HighScore = Score;
                    UpdateHud(HighScore, HighScoreBox);
                }
                UpdateHud(Score, ScoreBox);
            }

            RedrawPellets();
            Blink();
        }

        /// <summary>
        /// Places ghost specified by number out of ghost house and sets its start direction. 
        /// Also resets timer for ghost releasing.
        /// </summary>
        /// <param name="ghostNum"></param>
        private void SetGhostFree(int ghostNum)
        {
            Entities[ghostNum] = new Tuple<int, int, PictureBox, Direction.nType, DefaultAI>
                (TopGhostInTiles.Item1, TopGhostInTiles.Item2, Entities[ghostNum].Item3,
                Direction.nType.LEFT, Entities[ghostNum].Item5);
            Entities[ghostNum].Item3.Location = new Point(Entities[ghostNum].Item1 * TileSizeInPxs - 7, Entities[ghostNum].Item2 * TileSizeInPxs + 42);
            GhostRelease = Player2 ? (BaseGhostReleaseTimer / 2) / 3 : (BaseGhostReleaseTimer - Level) / 3;
            FreeGhost++;
        }

        /// <summary>
        /// Body of the update mechanism. Selectivly updates entities and map.
        /// </summary>
        private void UpdateGame()
        {
            ticks++;
            if ((Level < GhostAccLimLevel && ticks > ghostBaseSpeed - (Level / 2)) || (Level >= GhostAccLimLevel && ticks > ghostMaxSpeed))
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
            if (Score >= BonusLifeScore && !extraLifeGiven)
            {
                extraLifeGiven = true;
                if (Sound)
                {
                    SoundPlayer.URL = SoundPath + "pacman_extrapac.wav";
                    SoundPlayer.controls.play();
                }
                PacLives[Lives - 1].Visible = true;
                ++Lives;
            }

            //Handles successive ghost releasing
            if (GhostRelease <= 0 && FreeGhost < 4)
                SetGhostFree(FreeGhost + 1);
            if (GhostRelease > 0 && FreeGhost < 4)
                GhostRelease --;
            bg.Render(g);
        }

        /// <summary>
        /// Handles periodical blinking of 1up, 2up and Power Pellets.
        /// </summary>
        private void Blink()
        {
            if (ticks % 3 == 0)
            {
                this.up1.Visible = false;
                if (Player2)
                    this.up2.Visible = false;
            }
            else
            {
                this.up1.Visible = true;
                if (Player2)
                    this.up2.Visible = true;
            }

            for (int i = 0; i < Map.Item5.Count; ++i)
                if (MapFresh[Map.Item5[i].X][Map.Item5[i].Y].tile == Tile.nType.POWERDOT)
                {
                    if (ticks % 2 == 0)
                        MapFresh[Map.Item5[i].X][Map.Item5[i].Y].FreeTile(bg.Graphics, new Point(Map.Item5[i].Y * TileSizeInPxs, 
                                                                                                (Map.Item5[i].X + 3) * TileSizeInPxs), this.BackColor);
                    else
                        MapFresh[Map.Item5[i].X][Map.Item5[i].Y].DrawTile(bg.Graphics, new Point(Map.Item5[i].Y * TileSizeInPxs,
                                                                                                (Map.Item5[i].X + 3) * TileSizeInPxs), this.BackColor);
                }
        }

        /// <summary>
        /// Handles necessary redrawing of pellets cover by ghosts.
        /// </summary>
        private void RedrawPellets()
        {
            for (int i = 0; i < RDPSize; i++)
            {
                MapFresh[redrawPellets[i].X][redrawPellets[i].Y].DrawTile(
                    bg.Graphics, new Point(redrawPellets[i].Y * TileSizeInPxs, (redrawPellets[i].X + 3) * TileSizeInPxs), this.BackColor);
            }
        }

        /// <summary>
        /// Checks whether the direction player intends to move in is free and nulls assosiated timer
        /// in such case, continues in countdown otherwise.
        /// </summary>
        /// <param name="direction">Direction player intends to move in.</param>
        /// <param name="keyCountdown">Number of tries left.</param>
        private void KeyCountAndDir(ref Direction.nType direction, ref int keyCountdown)
        {
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

        /// <summary>
        /// Topmost layer of instructions executed each frame.
        /// </summary>
        private void GameLoop()
        {
            // In case that one of the players have pushed a valid key, countdown, which represents 
            // the number tiles remaining until the information about the pushed button is lost, is started.
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

            // Calls main Update function.
            UpdateGame();

            // Function for key countdown.
            KeyCountAndDir(ref NewDirection1, ref keyCountdown1);
            if (Player2)
                KeyCountAndDir(ref NewDirection2, ref keyCountdown2);

            // Checks if the player has already collected all the pellets.
            // In such case in relation to level and game mode, plays another level or ends the game.
            if (CollectedDots >= Map.Item2)
                EndLevel();
        }

        /// <summary>
        /// Handles end game freezing, blinking and calls new level or end game function.
        /// </summary>
        private async void EndLevel()
        {
            Updater.Stop();
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    RenderMap(MapFresh, Color.White);
                else
                    RenderMap(MapFresh, MapColor);

                bg.Render(g);
                await Task.Delay(500);
            }

            Controls.Clear();
            Level++;
            if (Level < MaxLevel && !Player2)
                PlayGame(false);
            else
                EndGame();
        }

        /// <summary>
        /// Creates ilusion of game loop.
        /// Handles event raised by timer's periodical ticks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Updater_Tick(object sender, EventArgs e)
        {
            GameLoop();
        }
    
        #endregion
    }
}
