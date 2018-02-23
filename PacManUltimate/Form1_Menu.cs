﻿using System;
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

        bool gameOn = false, Player2 = false;
        bool Sound = true, Music = true;
        List<Label> activeElements = new List<Label>();
        Tuple<mn, Label> menuSelected;
        mn menuLayer;
        Label ScoreBox = new Label(), HighScoreBox = new Label(), Score2Box = new Label();

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

        private void Menu_HighScore1P()
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

        private void Menu_HighScore2P()
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
                Menu_HighScore1P();
            else
                Menu_HighScore2P();

            //It is necessary to set scre and player boolean in order to be able to access default Highscore
            //page later on from menu
            Score = 0;
            Player2 = false;
        }

        private void Menu_Settings()
        {
            activeElements.Add(MusicButton);
            activeElements.Add(SoundsButton);
            activeElements.Add(SoundsBtnSelector);
            activeElements.Add(MusicBtnSelector);
            activeElements.Add(EscLabel);

            // Buttons load with color depending on associated booleans.
            // Music button is defaulty selected by arrows.
            if (Music)
                MusicButton.BackColor = Color.Black;
            else
                MusicButton.BackColor = Color.Gray;

            if (Sound)
                SoundsButton.BackColor = Color.Black;
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
            {
                MusicButton.BackColor = Color.Black;
                MusicButton.ForeColor = Color.White;
            }
            else
            {
                MusicButton.BackColor = Color.Gray;
                MusicButton.ForeColor = Color.Black;
            }
        }

        private void SoundsButton_Click(object sender, EventArgs e)
        {
            Sound = !Sound;
            if (Sound)
            {
                SoundsButton.BackColor = Color.Black;
                SoundsButton.ForeColor = Color.White;
            }
            else
            {
                SoundsButton.BackColor = Color.Gray;
                SoundsButton.ForeColor = Color.Black;
            }
        }

        private void VS_Click(object sender, EventArgs e)
        {
            selectMap_Click(new object(), EventArgs.Empty);
            if (gameOn)
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
            if (source.Count() > 0)
                output = source[0].ToString();
            char[] array = source.ToArray();
            for (int i = 1; i < source.Count(); i++)
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

        private void MoveInSettings()
        {
            if (SoundsBtnSelector.BackColor == Color.Yellow)
                MusicButton_MouseEnter(new object(), new EventArgs());
            else
                SoundsButton_MouseEnter(new object(), new EventArgs());
        }

        private void SoundsButton_MouseEnter(object sender, EventArgs e)
        {
            SoundsBtnSelector.BackColor = Color.Yellow;
            MusicBtnSelector.BackColor = Color.Black;
        }

        private void MusicButton_MouseEnter(object sender, EventArgs e)
        {
            SoundsBtnSelector.BackColor = Color.Black;
            MusicBtnSelector.BackColor = Color.Yellow;
        }

        private void MenuKeyDownHandler(KeyEventArgs e)
        {
            if (menuLayer == mn.start)
            {
                if (e.KeyCode == Keys.Escape)
                    this.Dispose();
                else
                {
                    Menu(Menu_MainMenu);
                    HighlightSelected(menuSelected.Item2, OrgGame);
                    menuSelected = new Tuple<mn, Label>(mn.game, OrgGame);
                    menuLayer = mn.game;
                }
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
                {
                    if (menuLayer == mn.submenu && menuSelected.Item1 == mn.settings)
                        MoveInSettings();
                    else
                        MoveInMenu(-1);
                }
                else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                {
                    if (menuLayer == mn.submenu && menuSelected.Item1 == mn.settings)
                        MoveInSettings();
                    else
                        MoveInMenu(+1);
                }
                else if (e.KeyCode == Keys.Enter)
                    if (menuLayer == mn.game)
                        (EnumToAction(menuSelected.Item1))(new object(), new EventArgs());
                    else if (menuSelected.Item1 == mn.settings)
                    {
                        if (MusicBtnSelector.BackColor == Color.Yellow)
                            MusicButton_Click(new object(), new EventArgs());
                        else
                            SoundsButton_Click(new object(), new EventArgs());
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
                        const byte symbolsLimit = 5;
                        if (symbols.Count() == symbolsLimit)
                            selectMap_Click(new object(), EventArgs.Empty);
                    }
            }
        }

        private void PacManUltimate_KeyDown(object sender, KeyEventArgs e)
        {
            // Function that handles player's input.
            // Seperate branch for menu input and game input.
            if (!gameOn)
                MenuKeyDownHandler(e);
            else
                GameKeyDownHandler(e);
        }

        private void Hover(object sender, EventArgs e)
        {
            //Function that provides color change of labels in menu          
            Label label = (Label)sender;
            HighlightSelected(menuSelected.Item2, label);
            menuSelected = new Tuple<mn, Label>(LabelToEnum(label), label);
        }

        private void HighlightSelected(Label prevLabel, Label newLabel)
        {
            prevLabel.ForeColor = Color.White;
            prevLabel.Font = new Font(prevLabel.Font.FontFamily, 21);
            newLabel.ForeColor = Color.Yellow;
            newLabel.Font = new Font(newLabel.Font.FontFamily, 23);
        }

        #endregion

        #region - MN enum -

        enum mn { game = 0, selectmap, settings, vs, highscore, start, submenu = 6 };

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

        private Action<object, EventArgs> EnumToAction(mn selected)
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

        #endregion
    }
}
