namespace PacManUltimate
{
    partial class PacManUltimate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(bool firstTime)
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacManUltimate));
            this.Pacman = new System.Windows.Forms.Label();
            this.ultimate = new System.Windows.Forms.Label();
            this.copyright = new System.Windows.Forms.Label();
            this.PressEnter = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.selectMap = new System.Windows.Forms.Label();
            this.OrgGame = new System.Windows.Forms.Label();
            this.Settings = new System.Windows.Forms.Label();
            this.Updater = new System.Windows.Forms.Timer(this.components);
            this.EscLabel = new System.Windows.Forms.Label();
            this.HighScr = new System.Windows.Forms.Label();
            this.VS = new System.Windows.Forms.Label();
            this.HighScoreLabel = new System.Windows.Forms.Label();
            this.ScoreLabel = new System.Windows.Forms.Label();
            this.HighScoreNum = new System.Windows.Forms.Label();
            this.ScoreNum = new System.Windows.Forms.Label();
            this.MusicButton = new System.Windows.Forms.Label();
            this.MusicBtnSelector = new System.Windows.Forms.Label();
            this.SoundsButton = new System.Windows.Forms.Label();
            this.SoundsBtnSelector = new System.Windows.Forms.Label();
            this.GameOverLabel = new System.Windows.Forms.Label();
            this.ErrorLdMap = new System.Windows.Forms.Label();
            this.ErrorInfo = new System.Windows.Forms.Label();
            this.TryAgainBut = new System.Windows.Forms.Label();
            this.AdvancedLdBut = new System.Windows.Forms.Label();
            this.TypeSymbols = new System.Windows.Forms.Label();
            this.TypedSymbols = new System.Windows.Forms.Label();
            this.TypeHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Pacman
            // 
            this.Pacman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Pacman.AutoSize = true;
            this.Pacman.Font = new System.Drawing.Font("Ravie", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pacman.ForeColor = System.Drawing.Color.Yellow;
            this.Pacman.Location = new System.Drawing.Point(64, 19);
            this.Pacman.Name = "Pacman";
            this.Pacman.Size = new System.Drawing.Size(337, 66);
            this.Pacman.TabIndex = 3;
            this.Pacman.Text = "PAC-MAN";
            this.Pacman.Visible = false;
            // 
            // ultimate
            //
            this.ultimate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ultimate.AutoSize = true;
            this.ultimate.Font = new System.Drawing.Font("Ravie", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultimate.ForeColor = System.Drawing.Color.Red;
            this.ultimate.Location = new System.Drawing.Point(80, 85);
            this.ultimate.Name = "ultimate";
            this.ultimate.Size = new System.Drawing.Size(267, 36);
            this.ultimate.TabIndex = 4;
            this.ultimate.Text = "- ULTIMATER -";
            this.ultimate.Visible = false;
            // 
            // copyright
            // 
            this.copyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyright.AutoSize = true;
            this.copyright.ForeColor = System.Drawing.Color.Yellow;
            this.copyright.Location = new System.Drawing.Point(280, 452);
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(179, 17);
            this.copyright.TabIndex = 5;
            this.copyright.Text = "CopyRight Filip Horký 2018";
            this.copyright.Visible = false;
            // 
            // PressEnter
            // 
            this.PressEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.PressEnter.AutoSize = true;
            this.PressEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PressEnter.ForeColor = System.Drawing.Color.White;
            this.PressEnter.Location = new System.Drawing.Point(80, 189);
            this.PressEnter.Name = "PressEnter";
            this.PressEnter.Size = new System.Drawing.Size(256, 126);
            this.PressEnter.TabIndex = 6;
            this.PressEnter.Text = "  INSERT COIN\r\n\r\n- press any key -\r\n";
            this.PressEnter.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Choose_Map";
            this.openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            // 
            // selectMap
            // 
            this.selectMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.selectMap.AutoSize = true;
            this.selectMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectMap.ForeColor = System.Drawing.Color.White;
            this.selectMap.Location = new System.Drawing.Point(123, 118);
            this.selectMap.Name = "selectMap";
            this.selectMap.Size = new System.Drawing.Size(190, 39);
            this.selectMap.TabIndex = 8;
            this.selectMap.Text = "Select Map";
            this.selectMap.Click += new System.EventHandler(this.selectMap_Click);
            this.selectMap.MouseEnter += new System.EventHandler(this.Hover);
            this.selectMap.Visible = false;
            // 
            // OrgGame
            // 
            this.OrgGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.OrgGame.AutoSize = true;
            this.OrgGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OrgGame.ForeColor = System.Drawing.Color.White;
            this.OrgGame.Location = new System.Drawing.Point(94, 51);
            this.OrgGame.Name = "OrgGame";
            this.OrgGame.Size = new System.Drawing.Size(241, 41);
            this.OrgGame.TabIndex = 9;
            this.OrgGame.Text = "Original Game";
            this.OrgGame.Click += new System.EventHandler(this.OrgGame_Click);
            this.OrgGame.MouseEnter += new System.EventHandler(this.Hover);
            this.OrgGame.Visible = false;
            // 
            // Settings
            // 
            this.Settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Settings.AutoSize = true;
            this.Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Settings.ForeColor = System.Drawing.Color.White;
            this.Settings.Location = new System.Drawing.Point(148, 186);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(143, 39);
            this.Settings.TabIndex = 10;
            this.Settings.Text = "Settings";
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            this.Settings.MouseEnter += new System.EventHandler(this.Hover);
            this.Settings.Visible = false;
            // 
            // Updater
            // 
            this.Updater.Tick += new System.EventHandler(this.Updater_Tick);
            // 
            // EscLabel
            // 
            this.EscLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EscLabel.AutoSize = true;
            this.EscLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.EscLabel.ForeColor = System.Drawing.Color.Yellow;
            this.EscLabel.Location = new System.Drawing.Point(135, 427);
            this.EscLabel.Name = "EscLabel";
            this.EscLabel.Size = new System.Drawing.Size(185, 22);
            this.EscLabel.TabIndex = 11;
            this.EscLabel.Text = "Press ESC to return";
            this.EscLabel.Visible = false;
            // 
            // HighScr
            // 
            this.HighScr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.HighScr.AutoSize = true;
            this.HighScr.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HighScr.ForeColor = System.Drawing.Color.White;
            this.HighScr.Location = new System.Drawing.Point(104, 328);
            this.HighScr.Name = "HighScr";
            this.HighScr.Size = new System.Drawing.Size(235, 38);
            this.HighScr.TabIndex = 12;
            this.HighScr.Text = "Highest Score";
            this.HighScr.Click += new System.EventHandler(this.HighScr_Click);
            this.HighScr.MouseEnter += new System.EventHandler(this.Hover);
            this.HighScr.Visible = false;
            // 
            // VS
            // 
            this.VS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.VS.AutoSize = true;
            this.VS.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VS.ForeColor = System.Drawing.Color.White;
            this.VS.Location = new System.Drawing.Point(191, 255);
            this.VS.Name = "VS";
            this.VS.Size = new System.Drawing.Size(63, 39);
            this.VS.TabIndex = 13;
            this.VS.Text = "VS";
            this.VS.Click += new System.EventHandler(this.VS_Click);
            this.VS.MouseEnter += new System.EventHandler(this.Hover);
            this.VS.Visible = false;
            // 
            // HighScoreLabel
            // 
            this.HighScoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.HighScoreLabel.AutoSize = true;
            this.HighScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HighScoreLabel.ForeColor = System.Drawing.Color.Yellow;
            this.HighScoreLabel.Location = new System.Drawing.Point(61, 113);
            this.HighScoreLabel.Name = "HighScoreLabel";
            this.HighScoreLabel.Size = new System.Drawing.Size(231, 39);
            this.HighScoreLabel.TabIndex = 14;
            this.HighScoreLabel.Text = "Highest Score";
            this.HighScoreLabel.Visible = false;
            // 
            // ScoreLabel
            // 
            this.ScoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ScoreLabel.AutoSize = true;
            this.ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ScoreLabel.ForeColor = System.Drawing.Color.White;
            this.ScoreLabel.Location = new System.Drawing.Point(62, 253);
            this.ScoreLabel.Name = "ScoreLabel";
            this.ScoreLabel.Size = new System.Drawing.Size(165, 36);
            this.ScoreLabel.TabIndex = 15;
            this.ScoreLabel.Text = "Your Score";
            this.ScoreLabel.Visible = false;
            // 
            // HighScoreNum
            // 
            this.HighScoreNum.AutoSize = true;
            this.HighScoreNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HighScoreNum.ForeColor = System.Drawing.Color.Yellow;
            this.HighScoreNum.Location = new System.Drawing.Point(148, 171);
            this.HighScoreNum.Name = "HighScoreNum";
            this.HighScoreNum.Size = new System.Drawing.Size(0, 58);
            this.HighScoreNum.TabIndex = 16;
            this.HighScoreNum.Visible = false;
            // 
            // ScoreNum
            // 
            this.ScoreNum.AutoSize = true;
            this.ScoreNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ScoreNum.ForeColor = System.Drawing.Color.White;
            this.ScoreNum.Location = new System.Drawing.Point(151, 320);
            this.ScoreNum.Name = "ScoreNum";
            this.ScoreNum.Size = new System.Drawing.Size(0, 54);
            this.ScoreNum.TabIndex = 17;
            this.ScoreNum.Visible = false;
            // 
            // MusicButton
            // 
            this.MusicButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.MusicButton.AutoSize = true;
            this.MusicButton.BackColor = System.Drawing.SystemColors.Window;
            this.MusicButton.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MusicButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MusicButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MusicButton.ForeColor = System.Drawing.Color.White;
            this.MusicButton.Location = new System.Drawing.Point(140, 120);
            this.MusicButton.Name = "MusicButton";
            this.MusicButton.Size = new System.Drawing.Size(155, 48);
            this.MusicButton.TabIndex = 18;
            this.MusicButton.Text = "MUSIC";
            this.MusicButton.Click += new System.EventHandler(this.MusicButton_Click);
            this.MusicButton.MouseEnter += new System.EventHandler(this.MusicButton_MouseEnter);
            this.MusicButton.Visible = false;
            //
            // MusicBtnSelector
            //
            this.MusicBtnSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.MusicBtnSelector.Location = new System.Drawing.Point(130, 120);
            this.MusicBtnSelector.Name = "MusicBtnSelector";
            this.MusicBtnSelector.Size = new System.Drawing.Size(48, 48);
            this.MusicBtnSelector.TabIndex = 29;
            this.MusicBtnSelector.BackColor = System.Drawing.Color.Yellow;
            this.MusicBtnSelector.Visible = false;
            // 
            // SoundsButton
            // 
            this.SoundsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SoundsButton.AutoSize = true;
            this.SoundsButton.BackColor = System.Drawing.SystemColors.Window;
            this.SoundsButton.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SoundsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SoundsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SoundsButton.ForeColor = System.Drawing.Color.White;
            this.SoundsButton.Location = new System.Drawing.Point(140, 233);
            this.SoundsButton.Name = "SoundsButton";
            this.SoundsButton.Size = new System.Drawing.Size(172, 48);
            this.SoundsButton.TabIndex = 19;
            this.SoundsButton.Text = "SOUND";
            this.SoundsButton.Click += new System.EventHandler(this.SoundsButton_Click);
            this.SoundsButton.MouseEnter += new System.EventHandler(this.SoundsButton_MouseEnter);
            this.SoundsButton.Visible = false;
            //
            // SoundsBtnSelector
            //
            this.SoundsBtnSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SoundsBtnSelector.Location = new System.Drawing.Point(130, 233);
            this.SoundsBtnSelector.Name = "SoundsBtnSelector";
            this.SoundsBtnSelector.Size = new System.Drawing.Size(48, 48);
            this.SoundsBtnSelector.TabIndex = 28;
            this.SoundsBtnSelector.BackColor = System.Drawing.Color.Black;
            this.SoundsBtnSelector.Visible = false;

            // 
            // GameOverLabel
            // 
            this.GameOverLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.GameOverLabel.AutoSize = true;
            this.GameOverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GameOverLabel.ForeColor = System.Drawing.Color.Red;
            this.GameOverLabel.Location = new System.Drawing.Point(79, 33);
            this.GameOverLabel.Name = "GameOverLabel";
            this.GameOverLabel.Size = new System.Drawing.Size(304, 52);
            this.GameOverLabel.TabIndex = 20;
            this.GameOverLabel.Text = "GAME OVER";
            this.GameOverLabel.Visible = false;
            // 
            // ErrorLdMap
            // 
            this.ErrorLdMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ErrorLdMap.AutoSize = true;
            this.ErrorLdMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ErrorLdMap.ForeColor = System.Drawing.Color.White;
            this.ErrorLdMap.Location = new System.Drawing.Point(81, 22);
            this.ErrorLdMap.Name = "ErrorLdMap";
            this.ErrorLdMap.Size = new System.Drawing.Size(302, 39);
            this.ErrorLdMap.TabIndex = 21;
            this.ErrorLdMap.Text = "Error Loading Map";
            this.ErrorLdMap.Visible = false;
            // 
            // ErrorInfo
            // 
            this.ErrorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ErrorInfo.AutoSize = true;
            this.ErrorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ErrorInfo.ForeColor = System.Drawing.Color.White;
            this.ErrorInfo.Location = new System.Drawing.Point(56, 69);
            this.ErrorInfo.Name = "ErrorInfo";
            this.ErrorInfo.Size = new System.Drawing.Size(349, 300);
            this.ErrorInfo.TabIndex = 22;
            this.ErrorInfo.Text = resources.GetString("ErrorInfo.Text");
            this.ErrorInfo.Visible = false;
            // 
            // TryAgainBut
            // 
            this.TryAgainBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TryAgainBut.AutoSize = true;
            this.TryAgainBut.BackColor = System.Drawing.Color.White;
            this.TryAgainBut.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TryAgainBut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TryAgainBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TryAgainBut.ForeColor = System.Drawing.Color.Black;
            this.TryAgainBut.Location = new System.Drawing.Point(34, 384);
            this.TryAgainBut.Name = "TryAgainBut";
            this.TryAgainBut.Size = new System.Drawing.Size(148, 31);
            this.TryAgainBut.TabIndex = 23;
            this.TryAgainBut.Text = "TRY AGAIN";
            this.TryAgainBut.Click += new System.EventHandler(this.selectMap_Click);
            this.TryAgainBut.Visible = false;
            // 
            // AdvancedLdBut
            // 
            this.AdvancedLdBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.AdvancedLdBut.AutoSize = true;
            this.AdvancedLdBut.BackColor = System.Drawing.Color.White;
            this.AdvancedLdBut.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AdvancedLdBut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AdvancedLdBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.AdvancedLdBut.ForeColor = System.Drawing.Color.Black;
            this.AdvancedLdBut.Location = new System.Drawing.Point(208, 384);
            this.AdvancedLdBut.Name = "AdvancedLdBut";
            this.AdvancedLdBut.Size = new System.Drawing.Size(230, 31);
            this.AdvancedLdBut.TabIndex = 24;
            this.AdvancedLdBut.Text = "ADVANCED LOAD";
            this.AdvancedLdBut.Click += new System.EventHandler(this.AdvancedLdBut_Click);
            this.AdvancedLdBut.Visible = false;
            // 
            // TypeSymbols
            // 
            this.TypeSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TypeSymbols.AutoSize = true;
            this.TypeSymbols.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TypeSymbols.ForeColor = System.Drawing.Color.White;
            this.TypeSymbols.Location = new System.Drawing.Point(44, 163);
            this.TypeSymbols.Name = "TypeSymbols";
            this.TypeSymbols.Size = new System.Drawing.Size(379, 39);
            this.TypeSymbols.TabIndex = 25;
            this.TypeSymbols.Text = "      Type 5 symbols:      ";
            this.TypeSymbols.Visible = false;
            // 
            // TypedSymbols
            // 
            this.TypedSymbols.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TypedSymbols.AutoSize = true;
            this.TypedSymbols.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TypedSymbols.ForeColor = System.Drawing.Color.Yellow;
            this.TypedSymbols.Location = new System.Drawing.Point(97, 241);
            this.TypedSymbols.Name = "TypedSymbols";
            this.TypedSymbols.Size = new System.Drawing.Size(0, 48);
            this.TypedSymbols.TabIndex = 26;
            this.TypedSymbols.Visible = false;
            // 
            // TypeHint
            // 
            this.TypeHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TypeHint.AutoSize = true;
            this.TypeHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TypeHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TypeHint.ForeColor = System.Drawing.Color.White;
            this.TypeHint.Location = new System.Drawing.Point(42, 151);
            this.TypeHint.Name = "TypeHint";
            this.TypeHint.Size = new System.Drawing.Size(386, 147);
            this.TypeHint.TabIndex = 27;
            this.TypeHint.Text = "\r\n\r\nFree ; Pellet ; P. pellet ; Wall ; Gate\r\n\r\n\r\n";
            this.TypeHint.Visible = false;
            // 
            // PacManUltimate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(472, 482);
            this.Controls.Add(this.TypedSymbols);
            this.Controls.Add(this.TypeSymbols);
            this.Controls.Add(this.TypeHint);
            this.Controls.Add(this.AdvancedLdBut);
            this.Controls.Add(this.TryAgainBut);
            this.Controls.Add(this.ErrorLdMap);
            this.Controls.Add(this.GameOverLabel);
            this.Controls.Add(this.SoundsButton);
            this.Controls.Add(this.SoundsBtnSelector);
            this.Controls.Add(this.MusicBtnSelector);
            this.Controls.Add(this.MusicButton);
            this.Controls.Add(this.ScoreNum);
            this.Controls.Add(this.HighScoreNum);
            this.Controls.Add(this.ScoreLabel);
            this.Controls.Add(this.HighScoreLabel);
            this.Controls.Add(this.VS);
            this.Controls.Add(this.HighScr);
            this.Controls.Add(this.EscLabel);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.OrgGame);
            this.Controls.Add(this.selectMap);
            this.Controls.Add(this.PressEnter);
            this.Controls.Add(this.copyright);
            this.Controls.Add(this.ultimate);
            this.Controls.Add(this.Pacman);
            this.Controls.Add(this.ErrorInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(490, 529);
            this.Name = "PacManUltimate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pac-Man Ultimate";

            //This part should be executed only once at the beggining of the game as components.clear does not erase
            //form's EventHandler and keyHandler which will cause existance of multiple handlers at the same time
            //which will result in misfunctions in menu and throughout the game during pressing keys and teleporting
            if (firstTime)
            {
                this.Load += new System.EventHandler(this.PacManUltimate_Load);
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PacManUltimate_KeyDown);
            }
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label Pacman;
        private System.Windows.Forms.Label ultimate;
        private System.Windows.Forms.Label copyright;
        private System.Windows.Forms.Label PressEnter;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label selectMap;
        private System.Windows.Forms.Label OrgGame;
        private System.Windows.Forms.Label Settings;
        private System.Windows.Forms.Timer Updater;
        private System.Windows.Forms.Label EscLabel;
        private System.Windows.Forms.Label HighScr;
        private System.Windows.Forms.Label VS;
        private System.Windows.Forms.Label HighScoreLabel;
        private System.Windows.Forms.Label ScoreLabel;
        private System.Windows.Forms.Label HighScoreNum;
        private System.Windows.Forms.Label ScoreNum;
        private System.Windows.Forms.Label MusicButton;
        private System.Windows.Forms.Label SoundsButton;
        private System.Windows.Forms.Label MusicBtnSelector;
        private System.Windows.Forms.Label SoundsBtnSelector;
        private System.Windows.Forms.Label GameOverLabel;
        private System.Windows.Forms.Label ErrorLdMap;
        private System.Windows.Forms.Label ErrorInfo;
        private System.Windows.Forms.Label TryAgainBut;
        private System.Windows.Forms.Label AdvancedLdBut;
        private System.Windows.Forms.Label TypeSymbols;
        private System.Windows.Forms.Label TypedSymbols;
        private System.Windows.Forms.Label TypeHint;
    }
}

