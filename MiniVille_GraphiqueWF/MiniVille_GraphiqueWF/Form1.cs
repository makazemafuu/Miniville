using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MiniVille_GraphiqueWF
{
    public partial class Form1 : Form
    {
        #region Variables
        // Taille
        public int BoardSizeWidth, BoardSizeHeight;
        int CardHeight = 90, CardWidth = 72;
        // SFX
        SoundPlayer activeMusicBackGround;
        Label MoneyJoueur1, MoneyJoueur2, MoneyJoueur3, MoneyJoueur4, AvertissementJoueur;
        NAudio.Wave.WaveOut waveOutCards = new NAudio.Wave.WaveOut(), waveOutButtonHover = new NAudio.Wave.WaveOut(), waveOutButtonClick = new NAudio.Wave.WaveOut(), waveOutLanceDe = new NAudio.Wave.WaveOut();
        NAudio.Wave.Mp3FileReader readerButtonClick, readerButtonHover, readerLanceDe, readerCards;
        // Core Game
        Timer Time = new Timer();
        Random random = new Random();
        Game game;
        public int nbJoueur = 2;
        public int EndingChoice = 1;
        bool buyingPhase = false;
        bool RelanceDe;
        int diceScore1, diceScore2 = 0;
        Dictionary<int, int> MonuCost = new Dictionary<int, int>()
        {
            {1,4 },
            {2,10 },
            {3,22 },
            {4,16 },
        };
        public int TargetTelevision;
        public int TargetCentreAffaire, CarteRecue, CarteEnvoyee;
        // Affichage
        List<PictureBox> CarteBoardList;
        List<PictureBox> CarteJoueur1, CarteJoueur2, CarteJoueur3, CarteJoueur4;
        List<PictureBox> MonuJoueur1, MonuJoueur2, MonuJoueur3, MonuJoueur4;
        Label TourJoueur1, TourJoueur2, TourJoueur3, TourJoueur4;
        #endregion
        public Form1()
        {
            InitializeComponent();
            //Event Resize pour update la taille de du board en cas de modif du joueur
            this.Resize += new EventHandler((sender, e) =>
            {
                BoardSizeHeight = this.ClientSize.Height;
                BoardSizeWidth = this.ClientSize.Width - 250; //250 = la taille du menu à droite
            });
            StartMenu();
        }
        #region Start Menu
        void StartMenu() // Menu Initial du jeu
        {
            //Background Music
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Start.wav");
            activeMusicBackGround.PlayLooping();

            //PictureBox qui va contenir toutes les images 
            PictureBox StartMenu = new PictureBox
            {
                Anchor = AnchorStyles.None,
                Size = new Size(this.Width, this.Height), //Le cadre sera de la taille du Form
                Name = "StartMenu",
                AutoSize = true,
                BackColor = Color.Transparent, //Même couleur que le fonds du Form (ce n'est pas un vrai transparent)
            };
            this.Controls.Add(StartMenu);
            this.BackColor = Color.SkyBlue;

            //Gif du Soleil qui bouge
            PictureBox SunGif = new PictureBox
            {
                Anchor = AnchorStyles.None,
                ImageLocation = "Images/Sun.gif",
                Name = "Sun",
                SizeMode = PictureBoxSizeMode.CenterImage, //Centre le Soleil 
                Size = new Size(this.Width, this.Height / 2),  //Hauteur de cette picture box  / 2 afin de placer le soleil à hauteur du titre (possible de changer juste Location si besoin)
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                WaitOnLoad = true,
            };
            //Image du titre 
            PictureBox Title = new PictureBox
            {
                Anchor = AnchorStyles.None,
                ImageLocation = "Images/MV_Title.png",
                Name = "Title",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(this.Width, this.Height / 2),
                BackColor = Color.Transparent,
                WaitOnLoad = true,
            };

            SunGif.Controls.Add(Title); //Ajout du titre dans le soleil (Pas d'anchor + superposition)
            StartMenu.Controls.Add(SunGif); //Ajout du soleil dans notre Start Menu

            //Gif de gauche
            PictureBox CityGif = new PictureBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ImageLocation = "Images/city-unscreen.gif",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(500, 400),
                BackColor = Color.Transparent,
                Location = new Point(this.Width / 8, this.Height / 2),
            };
            //Gif de droite
            StartMenu.Controls.Add(CityGif);
            PictureBox MinesGif = new PictureBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ImageLocation = "Images/mines-unscreen.gif",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(500, 400),
                BackColor = Color.Transparent,
                Location = new Point(this.Width / 2, this.Height / 2),
            };
            StartMenu.Controls.Add(MinesGif);

            //Ajout des boutons Start/Quit
            Button StartGame_Button = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2),
                Text = "Start Game",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            StartGame_Button.Click += new EventHandler(buttonStartGame_Button_Click) + new EventHandler(MenuStart_Button_Click_SFX);
            StartGame_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            StartGame_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            StartGame_Button.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Button Leave_Button = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2 + StartGame_Button.Height + 10),
                Text = "Quit Game",
                Name = "LGButton",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            Leave_Button.Click += new EventHandler(buttonLeave_Click) + new EventHandler(MenuStart_Button_Click_SFX);
            Leave_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Leave_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Leave_Button.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            this.Controls.Add(StartGame_Button);
            this.Controls.Add(Leave_Button);
            StartMenu.SendToBack();
            Button_Enter_Anim(StartGame_Button);
            Button_Enter_Anim(Leave_Button);
        }
        async void buttonStartGame_Button_Click(object sender, EventArgs e)
        {
            Button StartGameButton = (Button)sender;
            var LeaveGameButton = this.Controls.Find("LGButton", true).FirstOrDefault();
            Button_Leave_Anim(StartGameButton);
            Button_Leave_Anim((Button)LeaveGameButton);
            var Title = this.Controls.Find("Title", true).FirstOrDefault();
            var Sun = this.Controls.Find("Sun", true).FirstOrDefault();
            Sun.Visible = false;
            Title.Visible = false;
            StartGameButton.Enabled = false;
            await Task.Delay(800);
            nbPlayerChoice();
        }
        private void buttonLeave_Click(object sender, EventArgs e)
        {
            var NewFonds = new PictureBox()
            {
                BackColor = this.BackColor,
                Size = new Size(this.Width, this.Height),
                Anchor = AnchorStyles.None,
            };
            var AreYouSure = new PictureBox()
            {
                ImageLocation = "Images/LeaveLogo.png",
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(300, 200),
                Location = new Point(this.ClientSize.Width / 2 - 150, this.ClientSize.Height / 2 - 300),
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent,
                WaitOnLoad = true,
            };

            var buttonYes = new Button()
            {
                Text = "Yes",
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2),
                Size = new Size(200, 60),
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            buttonYes.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            buttonYes.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            buttonYes.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);

            var buttonNo = new Button()
            {
                Text = "No",
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2 + buttonYes.Height + 10),
                Size = new Size(200, 60),
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            buttonNo.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            buttonNo.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            buttonNo.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);


            buttonYes.Click += new EventHandler((sender, e) => {
                EndGameCredit();
                buttonYes.Enabled = false;
                buttonNo.Enabled = false;
            }) + MenuStart_Button_Click_SFX;
            buttonNo.Click += new EventHandler((sender, e) => {
                this.Controls.Remove(buttonNo.Parent);
                buttonYes.Enabled = false;
                buttonNo.Enabled = false;
            }) + MenuStart_Button_Click_SFX;
            NewFonds.Controls.Add(AreYouSure);
            NewFonds.Controls.Add(buttonYes);
            NewFonds.Controls.Add(buttonNo);

            this.Controls.Add(NewFonds);
            NewFonds.BringToFront();
            AreYouSure.BringToFront();
            Button_Enter_Anim(buttonYes);
            Button_Enter_Anim(buttonNo);
        }
        void MenuStart_Button_OnEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = Color.White;
            button.BackColor = Color.DarkBlue;
            button.Size = new Size(button.Width + 14, button.Height + 14);
            button.Location = new Point(button.Location.X - 7, button.Location.Y - 7);
        }
        void MenuStart_Button_OnHover_SFX(object sender, EventArgs e)
        {
            readerButtonHover = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonHover.mp3");
            waveOutButtonHover.Init(readerButtonHover);
            waveOutButtonHover.Play();
        }
        void MenuStart_Button_Click_SFX(object sender, EventArgs e)
        {
            readerButtonClick = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonClick.mp3");
            waveOutButtonClick.Init(readerButtonClick);
            waveOutButtonClick.Play();
        }
        void MenuStart_Button_OnLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = Color.DarkBlue;
            button.BackColor = Color.White;
            button.Size = new Size(button.Width - 14, button.Height - 14);
            button.Location = new Point(button.Location.X + 7, button.Location.Y + 7);
        }
        async void endingTypeChoice()
        {
            await Task.Delay(800);
            Button Ending1 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 4),
                Text = "20 pièces pour gagner",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            Ending1.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending1.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending1.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button Ending2 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending1.Location.X, Ending1.Location.Y + Ending1.Height + 20),
                Text = "30 pièces pour gagner",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            Ending2.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending2.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending2.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button Ending3 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending2.Location.X, Ending2.Location.Y + Ending2.Height + 20),
                Text = "20 pièces + toutes les cartes (1 fois)",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            Ending3.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending3.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending3.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button TrueEnding = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending3.Location.X, Ending3.Location.Y + Ending3.Height + 20),
                Text = "Vraies règles du jeux (4 Monuments)",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            TrueEnding.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            TrueEnding.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            TrueEnding.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            Ending1.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 1;
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending2.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 2;
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending3.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 3;
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TrueEnding.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 4;
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            this.Controls.Add(Ending1);
            this.Controls.Add(Ending2);
            this.Controls.Add(Ending3);
            this.Controls.Add(TrueEnding);
            Ending1.BringToFront();
            Ending2.BringToFront();
            Ending3.BringToFront();
            TrueEnding.BringToFront();
            Button_Enter_Anim(Ending1);
            Button_Enter_Anim(Ending2);
            Button_Enter_Anim(Ending3);
            Button_Enter_Anim(TrueEnding);
        }
        void nbPlayerChoice()
        {
            Button TwoPlayers = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 3),
                Text = "Deux joueurs",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            TwoPlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            TwoPlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            TwoPlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button ThreePlayers = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(TwoPlayers.Location.X, TwoPlayers.Location.Y + TwoPlayers.Height + 20),
                Text = "Trois joueurs",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            ThreePlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            ThreePlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            ThreePlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button FourPlayers = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(ThreePlayers.Location.X, ThreePlayers.Location.Y + ThreePlayers.Height + 20),
                Text = "Quatre joueurs",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            FourPlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            FourPlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            FourPlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            TwoPlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 2; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                endingTypeChoice();
                TwoPlayers.Enabled = false; ThreePlayers.Enabled = false; FourPlayers.Enabled = false;
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            ThreePlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 3; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                endingTypeChoice();
                TwoPlayers.Enabled = false; ThreePlayers.Enabled = false; FourPlayers.Enabled = false;
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            FourPlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                endingTypeChoice();
                TwoPlayers.Enabled = false; ThreePlayers.Enabled = false; FourPlayers.Enabled = false;
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            this.Controls.Add(TwoPlayers);
            this.Controls.Add(ThreePlayers);
            this.Controls.Add(FourPlayers);
            TwoPlayers.BringToFront();
            ThreePlayers.BringToFront();
            FourPlayers.BringToFront();
            Button_Enter_Anim(TwoPlayers);
            Button_Enter_Anim(ThreePlayers);
            Button_Enter_Anim(FourPlayers);
        }
        #endregion
        #region Core Game

        #region Menu à droite
        private void AddMenu()
        {
            PictureBox MenuBackGround = new PictureBox
            {
                Size = new Size(this.ClientSize.Width - BoardSizeWidth, BoardSizeHeight),
                BackColor = Color.CornflowerBlue,
                Location = new Point(BoardSizeWidth, 0),
                Name = "MenuDroite",
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right)
            };

            Button NewGame = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, 25),
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Text = "NOUVELLE PARTIE",
                Font = new Font("COMIC SANS MS", 10),
            };
            NewGame.Click += new EventHandler(buttonNewGame_Click);
            NewGame.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            NewGame.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            Button ChangeMode = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Location = new Point(25, NewGame.Location.Y + 60),
                Text = "Retour choix nb \nJoueurs",
                Font = new Font("COMIC SANS MS", 10),
            };
            ChangeMode.Click += new EventHandler(buttonChangeMode_Click);
            ChangeMode.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            ChangeMode.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            Button LeaveButton = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Location = new Point(25, ChangeMode.Location.Y + 60),
                Text = "QUITTER",
                Font = new Font("COMIC SANS MS", 10),
            };
            LeaveButton.Click += new EventHandler(buttonLeave_Click);
            LeaveButton.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LeaveButton.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            Button LancerDe = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, MenuBackGround.Height - 195),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = "LANCER LE DE",
                Name = "BoutonLancer",
                Font = new Font("COMIC SANS MS", 10),
            };
            LancerDe.Click += new EventHandler(buttonLancer_Click);
            LancerDe.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LancerDe.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            LancerDe.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);
            CheckBox CheckBoxDe = new CheckBox
            {
                AutoCheck = true,
                Size = new Size(200, 50),
                Location = new Point(25, LancerDe.Location.Y + 60),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = "Cocher pour lancer deux dés",
                Name = "CheckBoxDe",
                Font = new Font("COMIC SANS MS", 10),
                Enabled = false,
            };
            CheckBoxDe.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            Button FinDuTour = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, CheckBoxDe.Location.Y + 60),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = "FIN DU TOUR",
                Name = "BoutonPasserTour",
                Enabled = false,
                Font = new Font("COMIC SANS MS", 10),
            };
            FinDuTour.Click += new EventHandler(buttonFinTour_Click);
            FinDuTour.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            FinDuTour.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            FinDuTour.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);
            MenuBackGround.Controls.Add(CheckBoxDe);
            MenuBackGround.Controls.Add(LancerDe);
            MenuBackGround.Controls.Add(FinDuTour);
            MenuBackGround.Controls.Add(NewGame);
            MenuBackGround.Controls.Add(ChangeMode);
            MenuBackGround.Controls.Add(LeaveButton);
            this.Controls.Add(MenuBackGround);
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkDe = (CheckBox)sender;
            if (checkDe.Checked)
            {
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LES DES ";
                game.NombreDe = 2;
            }
            else
            {
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LE DE ";
                game.NombreDe = 1;
            }
        }
        private void buttonChangeMode_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Changer de mode recommencera une nouvelle partie es-tu sur?", "Change MODE", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                activeMusicBackGround.Stop();
                activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Start.wav");
                activeMusicBackGround.PlayLooping(); Controls.Clear();
                nbPlayerChoice();
            }
        }
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Veux-tu lancer une nouvelle partie ?", "Nouvelle partie", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                List<Control> temp = Controls.OfType<Control>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].Name != "MenuDroite") Controls.Remove(temp[i]);
                }
                BoardSizeHeight = this.ClientSize.Height;
                BoardSizeWidth = this.ClientSize.Width - 250; // La taille du menu à gauche = 250
                game = new Game(nbJoueur);
                ZoomCardStart();
                DistributionCard_StartGame();
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = true;
            }
        }
        private async void buttonLancer_Click(object sender, EventArgs e)
        {
            diceScore2 = 0;
            Button thisButton = (Button)sender;
            thisButton.Enabled = false;
            if (game.NombreDe == 1)
            {
                game.scoreDes = game.die.Lancer();
                diceScore1 = game.scoreDes;
                DiceAnim(game.scoreDes, 0);
            }
            else
            {
                diceScore1 = game.die.Lancer();
                diceScore2 = game.die.Lancer();
                game.scoreDes = diceScore1 + diceScore2;
                DiceAnim(diceScore1, diceScore2);
            }
            game.DieThrowed = true;
            readerLanceDe = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Dice Roll.mp3");
            waveOutLanceDe.Init(readerLanceDe);
            waveOutLanceDe.Play();

            await Task.Delay(2000);

            if (game.playerList[0].hasTour)
            {
                await TourRadioActivation();
                diceScore2 = 0;
                if (RelanceDe)
                {
                    if (game.NombreDe == 1)
                    {
                        game.scoreDes = game.die.Lancer();
                        diceScore1 = game.scoreDes;
                        DiceAnim(game.scoreDes, 0);
                    }
                    else
                    {
                        diceScore1 = game.die.Lancer();
                        diceScore2 = game.die.Lancer();
                        game.scoreDes = diceScore1 + diceScore2;
                        DiceAnim(diceScore1, diceScore2);
                    }
                    game.DieThrowed = true;
                    readerLanceDe = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Dice Roll.mp3");
                    waveOutLanceDe.Init(readerLanceDe);
                    waveOutLanceDe.Play();
                }
            }
            await UpdateGame(game.scoreDes, 1);
            buyingPhase = true;
            UpdateLabels();
            if (isEnded().Count != 0)
            {
                List<int> vainqueurs = isEnded();
                if (vainqueurs.Count == 1) MessageBox.Show("Le gagnant est le Joueur " + vainqueurs[0], "FIN DU JEU");
                else
                {
                    string temp = "Les gagnants sont : ";
                    for (int i = 0; i < vainqueurs.Count; i++)
                    {
                        temp += "Joueur " + vainqueurs[i] + "  ";
                    }
                    MessageBox.Show(temp, "FIN DU JEU");
                }
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                return;
            }
            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = true;
        }
        private void buttonFinTour_Click(object sender, EventArgs e)
        {
            game.DieThrowed = false;
            //MessageBox.Show("Fin tour");
            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
            TourNext();
        }
        private void Menu_Game_Button_OnEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = Color.DarkBlue;
            button.ForeColor = Color.White;
            button.Size = new Size(button.Width + 14, button.Height + 14);
            button.Location = new Point(button.Location.X - 7, button.Location.Y - 7);
        }
        private void Menu_Game_Button_OnLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = Color.LightSteelBlue;
            button.ForeColor = Color.Black;
            button.Size = new Size(button.Width - 14, button.Height - 14);
            button.Location = new Point(button.Location.X + 7, button.Location.Y + 7);
        }
        private void Menu_Game_Button_EnabledChanged(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = button.Enabled == false ? Color.LightSlateGray : Color.LightSteelBlue;
        }
        #endregion
        private async void StartGame()
        {
            await Task.Delay(1000);
            Controls.Clear();
            BoardSizeHeight = this.ClientSize.Height;
            BoardSizeWidth = this.ClientSize.Width - 250;
            if (activeMusicBackGround != null) activeMusicBackGround.Stop();
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/InGame.wav");
            activeMusicBackGround.PlayLooping();
            game = new Game(nbJoueur);

            ZoomCardStart(); //Les cartes en gros quand on Hover
            AddMenu(); //Le menu à droite
            Time.Interval = 3;
            DistributionCard_StartGame();
            //TesteurFonction(); // Fonction pour le debugging et play test des scénarios

        }
        private async Task UpdateGame(int scoreDes, int tourJoueur)
        {
            for (int i = 0; i < game.playerList.Count; i++)
            {
                Player player = game.playerList[i];
                for (int j = 0; j < player.CarteAcquises.Count; j++) // on itère dans la liste des cartes de chaque joueur
                {
                    if (player.CarteAcquises[j].ActivationValue[0] == scoreDes || (player.CarteAcquises[j].ActivationValue.Length > 1 && player.CarteAcquises[j].ActivationValue[1] == scoreDes)) //si l'activation value corresponds aux dés
                    {
                        if (player.CarteAcquises[j].Color == CouleurCarte.Bleu)
                        {
                            player.Pieces += player.CarteAcquises[j].Gain;
                        }
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Vert && tourJoueur == (i + 1))
                        {
                            if (player.CarteAcquises[j].Name == NomCarte.Fromagerie)
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++)
                                {
                                    if (player.CarteAcquises[p].Type == 2) count++;
                                }
                                player.Pieces += player.CarteAcquises[j].Gain * count;
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Fabrique)
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++)
                                {
                                    if (player.CarteAcquises[p].Type == 5) count++;
                                }
                                player.Pieces += player.CarteAcquises[j].Gain * count;
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Marche)
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++)
                                {
                                    if (player.CarteAcquises[p].Type == 1) count++;
                                }
                                player.Pieces += player.CarteAcquises[j].Gain * count;
                            }
                            else
                            {
                                if (player.hasCentre && player.CarteAcquises[j].Type == 3) player.Pieces += player.CarteAcquises[j].Gain + 1; //Cas du Monument Centre qui buff les carte verte et rouge (de certains types)
                                else player.Pieces += player.CarteAcquises[j].Gain;
                            }
                        }
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Rouge && tourJoueur != 1)
                        {
                            if (player.hasCentre && player.CarteAcquises[j].Type == 4) //Cas du monement Centre + Carte de type restau
                            {
                                player.Pieces += Math.Min(game.playerList[tourJoueur - 1].Pieces, player.CarteAcquises[j].Gain + 1);
                                game.playerList[tourJoueur - 1].Pieces = Math.Max(0, game.playerList[tourJoueur - 1].Pieces - player.CarteAcquises[j].Gain - 1);
                            }
                            else
                            {
                                player.Pieces += Math.Min(game.playerList[tourJoueur - 1].Pieces, player.CarteAcquises[j].Gain);
                                game.playerList[tourJoueur - 1].Pieces = Math.Max(0, game.playerList[tourJoueur - 1].Pieces - player.CarteAcquises[j].Gain);
                            }

                        }
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Violet && tourJoueur == (i + 1))
                        {
                            if (player.CarteAcquises[j].Name == NomCarte.Stade)
                            {
                                for (int p = 0; p < game.playerList.Count; p++)
                                {
                                    if (p != i)
                                    {
                                        player.Pieces += Math.Min(game.playerList[p].Pieces, player.CarteAcquises[j].Gain);
                                        game.playerList[p].Pieces = Math.Max(0, game.playerList[p].Pieces - player.CarteAcquises[j].Gain);
                                    }
                                }
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Television)
                            {
                                if (tourJoueur == 1)
                                {
                                    await TelevisionActivation();
                                    player.Pieces += Math.Min(game.playerList[TargetTelevision - 1].Pieces, player.CarteAcquises[j].Gain);
                                    game.playerList[TargetTelevision - 1].Pieces = Math.Max(0, game.playerList[TargetTelevision - 1].Pieces - player.CarteAcquises[j].Gain);
                                    UpdateLabels();
                                }
                                else  //Ici l'IA choisit sa cible aléatoirement (à modif si IA plus développée)
                                {
                                    int Target = random.Next(0, game.playerList.Count);
                                    while (Target == i)
                                    {
                                        Target = random.Next(0, game.playerList.Count);
                                    }
                                    player.Pieces += Math.Min(game.playerList[Target].Pieces, player.CarteAcquises[j].Gain);
                                    game.playerList[Target].Pieces = Math.Max(0, game.playerList[Target].Pieces - player.CarteAcquises[j].Gain);
                                }
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Affaire)
                            {
                                if (tourJoueur == 1)
                                {
                                    await CentreAffaireActivation();
                                    bool[] changePlayer = game.playerList[i].TradingChange(CarteRecue, CarteEnvoyee);
                                    bool[] changeTarget = game.playerList[TargetCentreAffaire - 1].TradingChange(CarteEnvoyee, CarteRecue);
                                    ChangeDisplayAfterExchange(0, changePlayer[0], changePlayer[1], CarteEnvoyee, CarteRecue);
                                    ChangeDisplayAfterExchange(TargetCentreAffaire - 1, changeTarget[0], changeTarget[1], CarteRecue, CarteEnvoyee);
                                }
                                else  //A dev (franchement la flemme pour le moment on fait comme si l'IA voulait pas échanger)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
        private void TesteurFonction() //Fonction pour faire des test / debugging
        {
            Button Test = new Button
            {
                Location = new Point(0, 0),
                Size = new Size(80, 50),
                Text = "CHEAT"
            };
            Test.Click += new EventHandler((sender, e) =>
            {
                for (int i = 0; i < game.playerList.Count; i++)
                {
                    game.playerList[i].Pieces += 100;
                }

            });

            Controls.Add(Test);
            Test.BringToFront();
        }
        private async void TourNext()
        {
            if (!(game.playerList[game.tourJoueur - 1].hasParc && diceScore1 == diceScore2)) game.tourJoueur = game.tourJoueur % nbJoueur + 1;
            diceScore2 = 0;
            if (game.tourJoueur == 1)
            {
                if (nbJoueur == 4) TourJoueur4.ForeColor = Color.Black;
                if (nbJoueur == 3) TourJoueur3.ForeColor = Color.Black;
                if (nbJoueur == 2) TourJoueur2.ForeColor = Color.Black;
                TourJoueur1.ForeColor = Color.Yellow;
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = true;
                return;
            }
            else
            {
                if (game.tourJoueur == 2)
                {
                    TourJoueur1.ForeColor = Color.Black;
                    TourJoueur2.ForeColor = Color.Yellow;
                }
                if (game.tourJoueur == 3)
                {
                    TourJoueur2.ForeColor = Color.Black;
                    TourJoueur3.ForeColor = Color.Yellow;
                }
                if (game.tourJoueur == 4)
                {
                    TourJoueur3.ForeColor = Color.Black;
                    TourJoueur4.ForeColor = Color.Yellow;
                }
                bool hasBuy = false;
                #region Tour de l'IA

                game.NombreDeIA = 1;
                if (game.playerList[game.tourJoueur - 1].hasGare) game.NombreDeIA = random.Next(1, 3);
                if (game.NombreDeIA == 1)
                {
                    game.scoreDes = game.die.Lancer();
                    diceScore1 = game.scoreDes;
                    DiceAnim(game.scoreDes);
                }
                else
                {
                    diceScore1 = game.die.Lancer();
                    diceScore2 = game.die.Lancer();
                    game.scoreDes = diceScore1 + diceScore2;
                    DiceAnim(diceScore1, diceScore2);
                }

                await Task.Delay(2000);
                await UpdateGame(game.scoreDes, game.tourJoueur);
                UpdateLabels();
                if (isEnded().Count != 0)
                {
                    List<int> vainqueurs = isEnded();
                    if (vainqueurs.Count == 1) MessageBox.Show("Le gagnant est le Joueur " + vainqueurs[0], "FIN DU JEU");
                    else
                    {
                        string temp = "Les gagnants sont : ";
                        for (int i = 0; i < vainqueurs.Count; i++)
                        {
                            temp += "Joueur " + vainqueurs[i] + "  ";
                        }
                        MessageBox.Show(temp, "FIN DU JEU");
                    }
                    this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                    this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                    return;
                }
                #endregion

                #region Cas Ending 4 (4 monuments)
                if (EndingChoice == 4)
                {
                    if (!game.playerList[game.tourJoueur - 1].hasGare && game.playerList[game.tourJoueur - 1].Pieces >= MonuCost[1])
                    {
                        hasBuy = true;
                        game.playerList[game.tourJoueur - 1].hasGare = true;
                        PictureBox MonuToChange;
                        if (game.tourJoueur == 2) MonuToChange = MonuJoueur2[0];
                        else if (game.tourJoueur == 3) MonuToChange = MonuJoueur3[0];
                        else MonuToChange = MonuJoueur4[0];

                        var ZoomPictureOld = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        MonuToChange.Name = MonuToChange.Name.Remove(5);
                        MonuToChange.ImageLocation = "Images/" + MonuToChange.Name + ".png";
                        var ZoomPictureNew = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        if (ZoomPictureNew != null)
                        {
                            if (MonuToChange.Location.X > this.Width - MonuToChange.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(MonuToChange.Location.X - (ZoomPictureNew.Width + 30),
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);
                            else ZoomPictureNew.Location = new Point(MonuToChange.Location.X + MonuToChange.Width + 10,
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);

                            if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                        }
                        if (ZoomPictureOld != null)
                        {
                            ZoomPictureOld.Visible = false;
                        }

                        game.playerList[game.tourJoueur - 1].Pieces -= MonuCost[1];
                        UpdateLabels();
                    }

                    if (!hasBuy && game.playerList[game.tourJoueur - 1].hasGare && !game.playerList[game.tourJoueur - 1].hasCentre && game.playerList[game.tourJoueur - 1].Pieces >= MonuCost[2])
                    {
                        hasBuy = true;
                        game.playerList[game.tourJoueur - 1].hasCentre = true;
                        PictureBox MonuToChange;
                        if (game.tourJoueur == 2) MonuToChange = MonuJoueur2[1];
                        else if (game.tourJoueur == 3) MonuToChange = MonuJoueur3[1];
                        else MonuToChange = MonuJoueur4[1];

                        var ZoomPictureOld = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        MonuToChange.Name = MonuToChange.Name.Remove(5);
                        MonuToChange.ImageLocation = "Images/" + MonuToChange.Name + ".png";
                        var ZoomPictureNew = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        if (ZoomPictureNew != null)
                        {
                            if (MonuToChange.Location.X > this.Width - MonuToChange.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(MonuToChange.Location.X - (ZoomPictureNew.Width + 30),
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);
                            else ZoomPictureNew.Location = new Point(MonuToChange.Location.X + MonuToChange.Width + 10,
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);

                            if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                        }
                        if (ZoomPictureOld != null)
                        {
                            ZoomPictureOld.Visible = false;
                        }

                        game.playerList[game.tourJoueur - 1].Pieces -= MonuCost[2];
                        UpdateLabels();
                    }

                    if (!hasBuy && game.playerList[game.tourJoueur - 1].hasGare && game.playerList[game.tourJoueur - 1].hasCentre && !game.playerList[game.tourJoueur - 1].hasTour && game.playerList[game.tourJoueur - 1].Pieces >= MonuCost[3])
                    {
                        hasBuy = true;
                        game.playerList[game.tourJoueur - 1].hasTour = true;
                        PictureBox MonuToChange;
                        if (game.tourJoueur == 2) MonuToChange = MonuJoueur2[2];
                        else if (game.tourJoueur == 3) MonuToChange = MonuJoueur3[2];
                        else MonuToChange = MonuJoueur4[2];

                        var ZoomPictureOld = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        MonuToChange.Name = MonuToChange.Name.Remove(5);
                        MonuToChange.ImageLocation = "Images/" + MonuToChange.Name + ".png";
                        var ZoomPictureNew = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        if (ZoomPictureNew != null)
                        {
                            if (MonuToChange.Location.X > this.Width - MonuToChange.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(MonuToChange.Location.X - (ZoomPictureNew.Width + 30),
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);
                            else ZoomPictureNew.Location = new Point(MonuToChange.Location.X + MonuToChange.Width + 10,
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);

                            if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                        }
                        if (ZoomPictureOld != null)
                        {
                            ZoomPictureOld.Visible = false;
                        }

                        game.playerList[game.tourJoueur - 1].Pieces -= MonuCost[3];
                        UpdateLabels();
                    }

                    if (!hasBuy && game.playerList[game.tourJoueur - 1].hasGare && game.playerList[game.tourJoueur - 1].hasCentre && game.playerList[game.tourJoueur - 1].hasTour && !game.playerList[game.tourJoueur - 1].hasParc && game.playerList[game.tourJoueur - 1].Pieces >= MonuCost[3])
                    {
                        hasBuy = true;
                        game.playerList[game.tourJoueur - 1].hasParc = true;
                        PictureBox MonuToChange;
                        if (game.tourJoueur == 2) MonuToChange = MonuJoueur2[3];
                        else if (game.tourJoueur == 3) MonuToChange = MonuJoueur3[3];
                        else MonuToChange = MonuJoueur4[3];

                        var ZoomPictureOld = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        MonuToChange.Name = MonuToChange.Name.Remove(5);
                        MonuToChange.ImageLocation = "Images/" + MonuToChange.Name + ".png";
                        var ZoomPictureNew = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        if (ZoomPictureNew != null)
                        {
                            if (MonuToChange.Location.X > this.Width - MonuToChange.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(MonuToChange.Location.X - (ZoomPictureNew.Width + 30),
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);
                            else ZoomPictureNew.Location = new Point(MonuToChange.Location.X + MonuToChange.Width + 10,
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);

                            if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                        }
                        if (ZoomPictureOld != null)
                        {
                            ZoomPictureOld.Visible = false;
                        }

                        game.playerList[game.tourJoueur - 1].Pieces -= MonuCost[4];
                        UpdateLabels();

                        if (isEnded().Count != 0)
                        {
                            List<int> vainqueurs = isEnded();
                            if (vainqueurs.Count == 1) MessageBox.Show("Le gagnant est le Joueur " + vainqueurs[0], "FIN DU JEU");
                            else
                            {
                                string temp = "Les gagnants sont : ";
                                for (int i = 0; i < vainqueurs.Count; i++)
                                {
                                    temp += "Joueur " + vainqueurs[i] + "  ";
                                }
                                MessageBox.Show(temp, "FIN DU JEU");
                            }
                            this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                            return;
                        }
                    }
                }
                #endregion

                #region Cas Ending 3 (20 pièces + toutes les cartes différentes
                if (EndingChoice == 3)
                {
                    int[] CheckCarteAcquises = new int[15];
                    for (int i = 0; i < game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count; i++)
                    {
                        CheckCarteAcquises[(int)game.playerList[game.tourJoueur - 1].CarteAcquisesUniques[i].Name - 1] = 1;
                    }

                    for (int i = 0; i < 15; i++)
                    {
                        Cards temp; List<PictureBox> tempList = new List<PictureBox>();
                        if (CheckCarteAcquises[i] == 0 && game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces >= temp.Cost)
                        {
                            if (game.tourJoueur == 2)
                            {
                                tempList = CarteJoueur2;
                                if (nbJoueur == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 2));
                                else tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count, i + 1, 2));
                            }
                            else if (game.tourJoueur == 3)
                            {
                                tempList = CarteJoueur3;
                                tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 3));
                            }
                            else if (game.tourJoueur == 4)
                            {
                                tempList = CarteJoueur4;
                                tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 4));
                            }
                            hasBuy = true;
                            game.playerList[game.tourJoueur - 1].BuyCard(game.CartesDisponibles[i]);
                            if (game.CartesDisponibles[i].PileCartes.Count == 0)
                            {
                                foreach (Control c in this.Controls)
                                {
                                    if ((string)c.Tag == "Pile" + (i + 1))
                                    {
                                        c.Visible = false;
                                        c.Enabled = false;
                                    }
                                }
                            };
                            break;
                        }
                        else if (CheckCarteAcquises[i] == 0 && game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces < temp.Cost)
                        {
                            break;
                        }

                    }

                    UpdateLabels();
                    if (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count == 15 && game.playerList[game.tourJoueur - 1].Pieces >= 20)
                    {
                        List<int> vainqueurs = isEnded();
                        if (vainqueurs.Count == 1) MessageBox.Show("Le gagnant est le Joueur " + vainqueurs[0], "FIN DU JEU");
                        else
                        {
                            string temp = "Les gagnants sont : ";
                            for (int i = 0; i < vainqueurs.Count; i++)
                            {
                                temp += "Joueur " + vainqueurs[i] + "  ";
                            }
                            MessageBox.Show(temp, "FIN DU JEU");
                        }
                        this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                        this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                        return;
                    }
                }

                #endregion
                #region Achat IA
                if (!hasBuy)
                {
                    for (int i = 0; i < game.CartesDisponibles.Count; i++)
                    {
                        Cards temp; List<PictureBox> tempList = new List<PictureBox>();
                        if (game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces >= temp.Cost && game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count < 5)
                        {
                            if (!game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name))
                            {
                                if (game.tourJoueur == 2)
                                {
                                    tempList = CarteJoueur2;
                                    if (nbJoueur == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 2));
                                    else tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count, i + 1, 2));
                                }
                                else if (game.tourJoueur == 3)
                                {
                                    tempList = CarteJoueur3;
                                    tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 3));
                                }
                                else if (game.tourJoueur == 4)
                                {
                                    tempList = CarteJoueur4;
                                    tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 4));
                                }
                            }
                            else //faire +1 au label
                            {
                                if (game.tourJoueur == 2) tempList = CarteJoueur2;
                                if (game.tourJoueur == 3) tempList = CarteJoueur3;
                                if (game.tourJoueur == 4) tempList = CarteJoueur4;
                                for (int j = 0; j < tempList.Count; j++)
                                {
                                    if ((tempList[j].Name.Length == 6 && tempList[j].Name[5] - '0' == (i + 1)) || (tempList[j].Name.Length == 7 && (tempList[j].Name[5] - '0') * 10 + (tempList[j].Name[6] - '0') == (i + 1)))
                                    {
                                        Label tempLabel = tempList[j].Controls.OfType<Label>().First();
                                        tempLabel.Text = "x" + (tempLabel.Text[1] - '0' + 1);
                                        tempLabel.Visible = true;
                                    }
                                }

                            }
                            game.playerList[game.tourJoueur - 1].BuyCard(game.CartesDisponibles[i]);
                            if (game.CartesDisponibles[i].PileCartes.Count == 0)
                            {
                                foreach (Control c in this.Controls)
                                {
                                    if ((string)c.Tag == "Pile" + (i + 1))
                                    {
                                        c.Visible = false;
                                        c.Enabled = false;
                                    }
                                }
                            };
                            UpdateLabels();
                            break;
                        }
                    }
                }
                #endregion

                await Task.Delay(1500);
            }
            TourNext();
        }
        private void ZoomCardStart()
        {
            for (int i = 1; i < 16; i++)
            {
                PictureBox picturebox = new PictureBox
                {
                    ImageLocation = "Images/Carte" + i + ".png",
                    Visible = false,
                    Size = new Size(400, 600),
                    Name = "ZoomCarte" + i,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                this.Controls.Add(picturebox);
            }
            for (int i = 1; i < 5; i++)
            {
                PictureBox pictureMonument = new PictureBox
                {
                    ImageLocation = "Images/Monu" + i + ".png",
                    Visible = false,
                    Name = "ZoomMonu" + i,
                    Size = new Size(400, 600),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                PictureBox pictureMonumentLocked = new PictureBox
                {
                    ImageLocation = "Images/Monu" + i + "Locked.png",
                    Visible = false,
                    Name = "ZoomMonu" + i + "Locked",
                    Size = new Size(400, 600),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                this.Controls.Add(pictureMonument);
                this.Controls.Add(pictureMonumentLocked);
            }
        }
        private void Carte_OnHover(object sender, EventArgs e)
        {
            PictureBox thisPicture = (PictureBox)sender;

            thisPicture.Size = new Size(thisPicture.Width + 20, thisPicture.Height + 20);
            thisPicture.Location = new Point(thisPicture.Location.X - 10, thisPicture.Location.Y - 10); //déplacement en diagonale donc la moitié de l'aggrandissement
            if (thisPicture.Name[0] != 'M') thisPicture.BringToFront();
            var ZoomPicture = this.Controls.Find("Zoom" + thisPicture.Name, true).FirstOrDefault();
            if (ZoomPicture != null)
            {
                ZoomPicture.Visible = true;
                if (thisPicture.Location.X > this.Width - thisPicture.Width - (ZoomPicture.Width + 30)) ZoomPicture.Location = new Point(thisPicture.Location.X - (ZoomPicture.Width + 30),
                    this.Height - thisPicture.Location.Y > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);
                else ZoomPicture.Location = new Point(thisPicture.Location.X + thisPicture.Width + 10,
                    this.Height - thisPicture.Location.Y > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);

                if (ZoomPicture.Location.Y < -5) ZoomPicture.Location = new Point(ZoomPicture.Location.X, (this.Height - ZoomPicture.Height) / 2);
                ZoomPicture.BringToFront();
            }
        }
        private void Carte_OnLeave(object sender, EventArgs e)
        {
            PictureBox thisPicture = (PictureBox)sender;
            thisPicture.Size = new Size(thisPicture.Width - 20, thisPicture.Height - 20);
            thisPicture.Location = new Point(thisPicture.Location.X + 10, thisPicture.Location.Y + 10);
            var ZoomPicture = this.Controls.Find("Zoom" + thisPicture.Name, true).FirstOrDefault();
            if (ZoomPicture != null)
            {
                ZoomPicture.Visible = false;
            }
        }
        private void Card_OnClick(object sender, EventArgs e)
        {
            if (!buyingPhase)
            {
                MessageBox.Show("attends ton tour saligaud");
                return;
            }

            PictureBox picture = (PictureBox)sender;
            int numberCard = Int32.Parse(picture.Name.Remove(0, 5));

            if (game.CartesDisponibles[numberCard - 1].PileCartes.Peek().Cost > game.playerList[0].Pieces)
            {
                MessageBox.Show("Tu es trop pauvre pour acheter cette carte !", "Achat Etablissement");
                return;
            }

            if (!game.playerList[0].CarteAcquisesUniques.Any(Cards => Cards.Name == (NomCarte)numberCard))
            {
                CarteJoueur1.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * game.playerList[0].CarteAcquisesUniques.Count, numberCard));
            }
            else //faire +1 au label pour voir l'effet de stacking
            {
                for (int i = 0; i < CarteJoueur1.Count; i++)
                {
                    if ((CarteJoueur1[i].Name.Length == 6 && CarteJoueur1[i].Name[5] - '0' == numberCard) || (CarteJoueur1[i].Name.Length == 7 && (CarteJoueur1[i].Name[5] - '0') * 10 + (CarteJoueur1[i].Name[6] - '0') == numberCard))
                    {
                        Label temp = CarteJoueur1[i].Controls.OfType<Label>().First();
                        temp.Text = "x" + (temp.Text[1] - '0' + 1);
                        temp.Visible = true;
                    }
                }
            }
            game.playerList[0].BuyCard(game.CartesDisponibles[numberCard - 1]);
            buyingPhase = false;
            if (game.CartesDisponibles[numberCard - 1].PileCartes.Count == 0)
            {
                picture.Visible = false;
                picture.Enabled = false;
            };
            UpdateLabels();
        }
        private void MonumentBuy(object sender, EventArgs e)
        {
            if (!buyingPhase)
            {
                MessageBox.Show("Attends ton tour saligaud");
                return;
            }
            var picturebox = (PictureBox)sender;
            int numberMonu = (picturebox.Name)[4] - '0';
            if (MonuCost[numberMonu] > game.playerList[0].Pieces)
            {
                MessageBox.Show("Tu es trop pauvre pour acheter cette carte !", "Achat Etablissement");
                return;
            }
            var ZoomPictureOld = this.Controls.Find("Zoom" + picturebox.Name, true).FirstOrDefault();
            picturebox.Name = picturebox.Name.Remove(5);
            picturebox.ImageLocation = "Images/" + picturebox.Name + ".png";
            var ZoomPictureNew = this.Controls.Find("Zoom" + picturebox.Name, true).FirstOrDefault();
            if (ZoomPictureNew != null)
            {
                ZoomPictureNew.Visible = true;
                if (picturebox.Location.X > this.Width - picturebox.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(picturebox.Location.X - (ZoomPictureNew.Width + 30),
                    this.Height - picturebox.Location.Y > ZoomPictureNew.Height ? picturebox.Location.Y : picturebox.Location.Y + picturebox.Height - ZoomPictureNew.Height);
                else ZoomPictureNew.Location = new Point(picturebox.Location.X + picturebox.Width + 10,
                    this.Height - picturebox.Location.Y > ZoomPictureNew.Height ? picturebox.Location.Y : picturebox.Location.Y + picturebox.Height - ZoomPictureNew.Height);

                if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                ZoomPictureNew.BringToFront();
            }
            if (ZoomPictureOld != null)
            {
                ZoomPictureOld.Visible = false;
                ZoomPictureOld.SendToBack();
            }
            picturebox.Click -= new EventHandler(MonumentBuy);
            game.playerList[0].Pieces -= MonuCost[numberMonu];
            buyingPhase = false;
            if (numberMonu == 1)
            {
                game.playerList[0].hasGare = true;
                this.Controls.Find("CheckBoxDe", true).FirstOrDefault().Enabled = true;
            }
            if (numberMonu == 2) game.playerList[0].hasCentre = true;
            if (numberMonu == 3) game.playerList[0].hasTour = true;
            if (numberMonu == 4) game.playerList[0].hasParc = true;
            UpdateLabels();
            if (isEnded().Count != 0)
            {
                List<int> vainqueurs = isEnded();
                if (vainqueurs.Count == 1) MessageBox.Show("Le gagnant est le Joueur " + vainqueurs[0], "FIN DU JEU");
                else
                {
                    string temp = "Les gagnants sont : ";
                    for (int i = 0; i < vainqueurs.Count; i++)
                    {
                        temp += "Joueur " + vainqueurs[i] + "  ";
                    }
                    MessageBox.Show(temp, "FIN DU JEU");
                }
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                return;
            }
        }
        private void UpdateLabels()
        {
            MoneyJoueur1.Text = game.playerList[0].Pieces.ToString();
            MoneyJoueur2.Text = game.playerList[1].Pieces.ToString();
            if (nbJoueur >= 3) MoneyJoueur3.Text = game.playerList[2].Pieces.ToString();
            if (nbJoueur >= 4) MoneyJoueur4.Text = game.playerList[3].Pieces.ToString();
        }
        private List<int> isEnded()
        {
            List<int> Vainqueurs = new List<int>();
            if (EndingChoice == 1)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].Pieces >= 20) Vainqueurs.Add(i + 1);
                }
            }
            else if (EndingChoice == 2)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].Pieces >= 30) Vainqueurs.Add(i + 1);
                }
            }
            else if (EndingChoice == 3)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].Pieces >= 20 && game.playerList[i].CarteAcquisesUniques.Count == 15) Vainqueurs.Add(i + 1);
                }
            }
            else if (EndingChoice == 4)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].hasCentre && game.playerList[i].hasGare && game.playerList[i].hasParc && game.playerList[i].hasTour) Vainqueurs.Add(i + 1);
                }
            }
            return Vainqueurs;
        }
        private async Task TelevisionActivation()
        {
            bool choosed = false;

            PictureBox TargetToSteal = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(TargetToSteal);


            Button Joueur2 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(TargetToSteal.Width / 3, TargetToSteal.Height / 4),
                Text = "Joueur 2",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            Label TextSteal = new Label
            {
                Size = new Size(400, 30),
                Text = "Sélectionner le joueur à dépouiller (5 pièces)",
                Location = new Point(TargetToSteal.Width / 4, Joueur2.Location.Y - 80),
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };
            TargetToSteal.Controls.Add(TextSteal);
            Joueur2.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Joueur2.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Joueur2.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Joueur2.Click += new EventHandler((sender, e) =>
            {
                TargetTelevision = 2;
                choosed = true;
                Controls.Remove(TargetToSteal);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TargetToSteal.Controls.Add(Joueur2);


            if (nbJoueur >= 3)
            {
                Button Joueur3 = new Button
                {
                    Size = new Size(200, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + Joueur2.Height + 20),
                    Text = "Joueur 3",
                    Anchor = AnchorStyles.None,
                    BackColor = Color.White,
                    Font = new Font("COMIC SANS MS", 12f),
                    ForeColor = Color.DarkBlue,
                };
                Joueur3.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
                Joueur3.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
                Joueur3.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
                Joueur3.Click += new EventHandler((sender, e) =>
                {
                    TargetTelevision = 3;
                    choosed = true;
                    Controls.Remove(TargetToSteal);
                }) + new EventHandler(MenuStart_Button_Click_SFX);
                TargetToSteal.Controls.Add(Joueur3);
            }
            if (nbJoueur == 4)
            {
                Button Joueur4 = new Button
                {
                    Size = new Size(200, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + 2 * (Joueur2.Height + 20)),
                    Text = "Joueur 4",
                    Anchor = AnchorStyles.None,
                    BackColor = Color.White,
                    Font = new Font("COMIC SANS MS", 12f),
                    ForeColor = Color.DarkBlue,
                };
                Joueur4.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
                Joueur4.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
                Joueur4.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
                Joueur4.Click += new EventHandler((sender, e) =>
                {
                    TargetTelevision = 4;
                    choosed = true;
                    Controls.Remove(TargetToSteal);
                }) + new EventHandler(MenuStart_Button_Click_SFX);
                TargetToSteal.Controls.Add(Joueur4);
            }

            TargetToSteal.BringToFront();
            while (!choosed)
            {
                await Task.Delay(1000);
            }
        }
        private async Task CentreAffaireActivation()
        {
            #region Choix Target
            bool TargetChosen = false;

            PictureBox TargetToSteal = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 60),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(TargetToSteal);


            Button Joueur2 = new Button
            {
                Size = new Size(170, 60),
                Location = new Point(TargetToSteal.Width / 3, TargetToSteal.Height / 4),
                Text = "Joueur 2",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            Label TextSteal = new Label
            {
                Size = new Size(400, 30),
                Text = "Sélectionner le joueur avec qui échanger",
                Location = new Point(TargetToSteal.Width / 4, Joueur2.Location.Y - 80),
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };
            TargetToSteal.Controls.Add(TextSteal);
            Joueur2.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Joueur2.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Joueur2.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Joueur2.Click += new EventHandler((sender, e) =>
            {
                TargetCentreAffaire = 2;
                TargetChosen = true;
                Controls.Remove(TargetToSteal);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TargetToSteal.Controls.Add(Joueur2);

            Button NoExchangeWanted = new Button
            {
                Size = new Size(170, 60),
                Text = "Pas d'échange",
                Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + Joueur2.Height + 20),
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,

            };
            NoExchangeWanted.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            NoExchangeWanted.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            NoExchangeWanted.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            NoExchangeWanted.Click += new EventHandler((sender, e) =>
            {
                TargetChosen = true;
                TargetCentreAffaire = -1;
                Controls.Remove(TargetToSteal);
            });
            TargetToSteal.Controls.Add(NoExchangeWanted);

            if (nbJoueur >= 3)
            {
                Button Joueur3 = new Button
                {
                    Size = new Size(170, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + Joueur2.Height + 20),
                    Text = "Joueur 3",
                    Anchor = AnchorStyles.None,
                    BackColor = Color.White,
                    Font = new Font("COMIC SANS MS", 12f),
                    ForeColor = Color.DarkBlue,
                };
                Joueur3.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
                Joueur3.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
                Joueur3.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
                Joueur3.Click += new EventHandler((sender, e) =>
                {
                    TargetCentreAffaire = 3;
                    TargetChosen = true;
                    Controls.Remove(TargetToSteal);
                }) + new EventHandler(MenuStart_Button_Click_SFX);
                TargetToSteal.Controls.Add(Joueur3);
                NoExchangeWanted.Location = new Point(Joueur2.Location.X, Joueur3.Location.Y + Joueur2.Height + 20);
            }
            if (nbJoueur == 4)
            {
                Button Joueur4 = new Button
                {
                    Size = new Size(170, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + 2 * (Joueur2.Height + 20)),
                    Text = "Joueur 4",
                    Anchor = AnchorStyles.None,
                    BackColor = Color.White,
                    Font = new Font("COMIC SANS MS", 12f),
                    ForeColor = Color.DarkBlue,
                };
                Joueur4.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
                Joueur4.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
                Joueur4.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
                Joueur4.Click += new EventHandler((sender, e) =>
                {
                    TargetCentreAffaire = 4;
                    TargetChosen = true;
                    Controls.Remove(TargetToSteal);
                }) + new EventHandler(MenuStart_Button_Click_SFX);
                TargetToSteal.Controls.Add(Joueur4);
                NoExchangeWanted.Location = new Point(Joueur2.Location.X, Joueur4.Location.Y + Joueur2.Height + 20);
            }



            TargetToSteal.BringToFront();
            while (!TargetChosen)
            {
                await Task.Delay(1000);
            }

            #endregion

            if (TargetCentreAffaire == -1)
            {
                TargetCentreAffaire = 2;
                CarteRecue = 0;
                CarteEnvoyee = 0;
                return;
            }
            #region Choix notre Carte
            bool CardSelfChosen = false;
            PictureBox CardToGive = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(CardToGive);
            Label GiveCardText = new Label
            {
                Size = new Size(400, 30),
                Text = "Sélectionner la carte à donner (venant de vous)",
                Location = new Point(TargetToSteal.Width / 4, TargetToSteal.Height / 4 - 80),
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };
            CardToGive.Controls.Add(GiveCardText);

            int index = 0;
            for (int i = 0; i < game.playerList[0].CarteAcquisesUniques.Count; i++)
            {
                int numberCard = (int)game.playerList[0].CarteAcquisesUniques[i].Name;
                if (numberCard == 7 || numberCard == 8 || numberCard == 9) continue;
                PictureBox CarteToExchange = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    ImageLocation = "Images/Carte" + numberCard + ".png",
                    Location = new Point((index % 7 + 1) * CardWidth, GiveCardText.Location.Y + 40 + CardHeight * (index / 7)),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte" + numberCard,
                    Anchor = AnchorStyles.None,
                    WaitOnLoad = true,
                };
                index++;
                CarteToExchange.MouseEnter += new EventHandler((sender, e) =>
                {
                    PictureBox picture = (PictureBox)sender;
                    picture.Size = new Size(picture.Width + 10, picture.Height + 10);
                    picture.Location = new Point(picture.Location.X - 5, picture.Location.Y - 5);
                    picture.BringToFront();
                });
                CarteToExchange.MouseLeave += new EventHandler((sender, e) =>
                {
                    PictureBox picture = (PictureBox)sender;
                    picture.Size = new Size(picture.Width - 10, picture.Height - 10);
                    picture.Location = new Point(picture.Location.X + 5, picture.Location.Y + 5);
                });
                CarteToExchange.Click += new EventHandler((sender, e) =>
                {
                    CardSelfChosen = true;
                    if (CarteToExchange.Name.Length == 6) CarteEnvoyee = CarteToExchange.Name[5] - '0';
                    else CarteEnvoyee = (CarteToExchange.Name[5] - '0') * 10 + (CarteToExchange.Name[6] - '0');
                    Controls.Remove(CardToGive);
                });

                CardToGive.Controls.Add(CarteToExchange);
            }

            CardToGive.BringToFront();

            while (!CardSelfChosen)
            {
                await Task.Delay(1000);
            }
            #endregion

            #region Choix Carte adversaire
            bool CardChosen = false;
            PictureBox CardToReceive = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(CardToReceive);
            Label ReceiveCardText = new Label
            {
                Size = new Size(CardToReceive.Width, 30),
                Text = "Sélectionner la carte à recevoir (venant du Joueur " + TargetCentreAffaire + " )",
                Location = new Point(TargetToSteal.Width / 4 - 10, TargetToSteal.Height / 4 - 80),
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };
            CardToReceive.Controls.Add(ReceiveCardText);
            index = 0;
            for (int i = 0; i < game.playerList[TargetCentreAffaire - 1].CarteAcquisesUniques.Count; i++)
            {
                int numberCard = (int)game.playerList[TargetCentreAffaire - 1].CarteAcquisesUniques[i].Name;
                if (numberCard == 7 || numberCard == 8 || numberCard == 9) continue;
                PictureBox CarteToExchange = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    ImageLocation = "Images/Carte" + numberCard + ".png",
                    Location = new Point((index % 7 + 1) * CardWidth, GiveCardText.Location.Y + 40 + CardHeight * (index / 7)),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte" + numberCard,
                    Anchor = AnchorStyles.None,
                    WaitOnLoad = true,
                };
                index++;
                CarteToExchange.MouseEnter += new EventHandler((sender, e) =>
                {
                    PictureBox picture = (PictureBox)sender;
                    picture.Size = new Size(picture.Width + 10, picture.Height + 10);
                    picture.Location = new Point(picture.Location.X - 5, picture.Location.Y - 5);
                    picture.BringToFront();
                });
                CarteToExchange.MouseLeave += new EventHandler((sender, e) =>
                {
                    PictureBox picture = (PictureBox)sender;
                    picture.Size = new Size(picture.Width - 10, picture.Height - 10);
                    picture.Location = new Point(picture.Location.X + 5, picture.Location.Y + 5);
                });
                CarteToExchange.Click += new EventHandler((sender, e) =>
                {
                    CardChosen = true;
                    if (CarteToExchange.Name.Length == 6) CarteRecue = CarteToExchange.Name[5] - '0';
                    else CarteRecue = (CarteToExchange.Name[5] - '0') * 10 + (CarteToExchange.Name[6] - '0');
                    Controls.Remove(CardToReceive);
                });

                CardToReceive.Controls.Add(CarteToExchange);
            }

            CardToReceive.BringToFront();

            while (!CardChosen)
            {
                await Task.Delay(1000);
            }

            #endregion


        }
        private async Task TourRadioActivation()
        {
            bool choosed = false;

            PictureBox YesNo = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(YesNo);

            PictureBox OldScore1 = new PictureBox
            {
                Size = new Size(100, 100),
                ImageLocation = @"Images\Dice" + diceScore1 + ".png",
                Location = new Point(5, YesNo.Height / 2 - 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.None,
            };
            YesNo.Controls.Add(OldScore1);

            if (game.NombreDe == 2)
            {
                PictureBox OldScore2 = new PictureBox
                {
                    Size = new Size(100, 100),
                    ImageLocation = @"Images\Dice" + diceScore2 + ".png",
                    Location = new Point(5, OldScore1.Location.Y + OldScore1.Height + 10),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Anchor = AnchorStyles.None,
                };
                YesNo.Controls.Add(OldScore2);
            }

            Button YesButton = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(YesNo.Width / 3, YesNo.Height / 4),
                Text = "Oui",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            Label TextRelance = new Label
            {
                Size = new Size(400, 30),
                Text = "Veux-tu relancer le(s) dé(s) ?",
                Location = new Point(YesNo.Width / 3, YesButton.Location.Y - 80),
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };
            YesNo.Controls.Add(TextRelance);
            YesButton.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            YesButton.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            YesButton.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            YesButton.Click += new EventHandler((sender, e) =>
            {
                RelanceDe = true;
                choosed = true;
                Controls.Remove(YesNo);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            YesNo.Controls.Add(YesButton);

            Button NoButton = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(YesButton.Location.X, YesButton.Location.Y + YesButton.Height + 20),
                Text = "Non",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            NoButton.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            NoButton.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            NoButton.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            NoButton.Click += new EventHandler((sender, e) =>
            {
                RelanceDe = false;
                choosed = true;
                Controls.Remove(YesNo);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            YesNo.Controls.Add(NoButton);

            YesNo.BringToFront();
            while (!choosed)
            {
                await Task.Delay(1000);
            }

        }
        #endregion
        #region Affichage / Animation / VFX
        private void DiceAnim(int DiceScore1, int DiceScore2 = 0)
        {
            Timer Time = new Timer();
            Time.Interval = 50;
            float totalTime = 1000, elapsedTime = 0, finalDisplayTime = 1200;

            var pictureDice = new PictureBox
            {
                Size = new Size(100, 100),
                ImageLocation = @"Images\Dice1.png",
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(BoardSizeWidth / 2 - 75, BoardSizeHeight / 2 - 3 * (70 + 5) / 2 - 125),
                Anchor = AnchorStyles.None,
            };
            this.Controls.Add(pictureDice);
            pictureDice.BringToFront();
            if (DiceScore2 != 0)
            {
                pictureDice.Location = new Point(BoardSizeWidth / 2 - 100, BoardSizeHeight / 2 - 3 * (70 + 5) / 2 - 125);
                var pictureDice2 = new PictureBox
                {
                    Size = new Size(100, 100),
                    ImageLocation = @"Images\Dice6.png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(BoardSizeWidth / 2 + 25, BoardSizeHeight / 2 - 3 * (70 + 5) / 2 - 125),
                    Anchor = AnchorStyles.None,
                };
                this.Controls.Add(pictureDice2);
                pictureDice2.BringToFront();
                Time.Tick += new EventHandler((sender, e) =>
                {
                    if (elapsedTime > totalTime + finalDisplayTime)
                    {
                        pictureDice2.Visible = false;
                    }
                    else if (elapsedTime > totalTime)
                    {
                        pictureDice2.ImageLocation = @"Images\Dice" + DiceScore2 + ".png";
                    }
                    else
                    {
                        pictureDice2.ImageLocation = @"Images\Dice" + random.Next(1, 7) + ".png";
                    }
                });
            }
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime + finalDisplayTime)
                {
                    pictureDice.Visible = false;
                    Time.Stop();
                    return;
                }
                else if (elapsedTime > totalTime)
                {
                    pictureDice.ImageLocation = @"Images\Dice" + DiceScore1 + ".png";
                }
                else
                {
                    pictureDice.ImageLocation = @"Images\Dice" + random.Next(1, 7) + ".png";
                }
                elapsedTime += Time.Interval;
            });
            Time.Start();
        }
        private void Button_Enter_Anim(Button button)
        {
            int originalX = button.Size.Width, originalY = button.Size.Height;
            button.Location = new Point(button.Location.X + originalX / 2 - 1, button.Location.Y + originalY / 2 - 1);
            button.Size = new Size(1, 1);
            Timer Time = new Timer();
            Time.Interval = 5;
            float totalTime = 50, elapsedTime = 0;
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime)
                {
                    Time.Stop();
                }
                else
                {
                    button.Location = new Point(button.Location.X - originalX / 22, button.Location.Y - originalY / 22);
                    button.Size = new Size(button.Size.Width + originalX / 11, button.Size.Height + originalY / 11);
                }
                elapsedTime += Time.Interval;
            });
            Time.Start();
        }
        private void Button_Leave_Anim(Button button)
        {
            int originalX = button.Size.Width, originalY = button.Size.Height;
            Timer Time = new Timer();
            Time.Interval = 5;
            float totalTime = 50, elapsedTime = 0;
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime)
                {
                    Time.Stop();
                }
                else
                {
                    button.Location = new Point(button.Location.X + originalX / 22, button.Location.Y + originalY / 22);
                    button.Size = new Size(button.Size.Width - originalX / 11, button.Size.Height - originalY / 11);
                }
                elapsedTime += Time.Interval;
            });
            Time.Start();
        }
        private PictureBox Spawn_Card(int height, int width, int numberCard, int NumeroJoueur = 1)
        {
            PictureBox picture = new PictureBox
            {
                Size = new Size(CardWidth, CardHeight),
                BackColor = Color.Transparent,
                Location = new Point(width, height), // width/2 et height * 1
                ImageLocation = "Images/Carte" + numberCard + ".png",
                SizeMode = PictureBoxSizeMode.StretchImage,
                Name = "Carte" + numberCard,
                WaitOnLoad = true,
            };
            Label NumberCard = new Label
            {
                Text = "x1",
                Location = new Point(picture.Width - 23, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Font = new Font(Label.DefaultFont, FontStyle.Bold),
                Visible = false,
                ForeColor = Color.Yellow,
            };
            picture.Controls.Add(NumberCard);
            picture.MouseEnter += new EventHandler(Carte_OnHover);
            picture.MouseLeave += new EventHandler(Carte_OnLeave);
            if (nbJoueur == 2)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 3)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 4)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
                else if (NumeroJoueur == 4)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            this.Controls.Add(picture);
            return picture;
        }
        private void Move_Cards(int height, int width, List<PictureBox> CarteList, int index, int NumeroJoueur = 0)
        {
            PictureBox picture = CarteList[index];
            if (nbJoueur == 2)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 3)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 4)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3)
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
                else if (NumeroJoueur == 4)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            int directionY = height - picture.Location.Y, directionX = width - picture.Location.X;
            float totalTime = 15, elapsedTime = 0;
            EventHandler handler = null;
            handler = (sender, e) =>
            {
                if (elapsedTime > totalTime)
                {
                    Time.Tick -= handler;
                }
                else
                {
                    picture.Location = new Point(picture.Location.X + directionX / 6, picture.Location.Y + directionY / 6);
                }
                elapsedTime += Time.Interval;
            };
            Time.Tick += handler;
            Time.Start();
        }
        private async void DistributionCard_StartGame()
        {
            //Spawn des cartes sur le terrain (les 15 cartes différentes)
            CarteBoardList = new List<PictureBox>();
            for (int CarteNom = 1; CarteNom < 16; CarteNom++)
            {
                PictureBox picture = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    Location = new Point(BoardSizeWidth / 2 - CardWidth / 2, BoardSizeHeight - CardHeight),
                    ImageLocation = "Images/Carte" + CarteNom + ".png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte" + CarteNom,
                    Tag = "Pile" + CarteNom,
                    Anchor = AnchorStyles.None,
                };
                picture.Click += new EventHandler(Card_OnClick);
                picture.MouseEnter += new EventHandler(Carte_OnHover);
                picture.MouseLeave += new EventHandler(Carte_OnLeave);
                CarteBoardList.Add(picture);
                this.Controls.Add(picture);
            }
            //Animation distribution des cartes 
            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Move_Cards(BoardSizeHeight / 2 - (CardHeight / 2 * 3) + CardHeight * (i % 3), BoardSizeWidth / 2 - (CardWidth / 2 * 5) + CardWidth * (j % 5), CarteBoardList, k++);
                    await Task.Delay(50); ;
                }
            }
            //Pour chaque joueur, 4 monuments, champs de blé et boulangerie + image des golds
            for (int i = 1; i <= nbJoueur; i++)
            {
                List<PictureBox> JoueurMonumentList = new List<PictureBox>();
                List<PictureBox> JoueurStartCardList = new List<PictureBox>();
                #region Monument
                for (int j = 1; j < 5; j++)
                {
                    PictureBox picture = new PictureBox
                    {
                        Size = new Size(CardWidth, CardHeight),
                        BackColor = Color.Transparent,
                        Location = new Point(BoardSizeWidth / 2 - CardWidth / 2, BoardSizeHeight - CardHeight),
                        ImageLocation = "Images/Monu" + j + "Locked.png",
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Name = "Monu" + j + "Locked",
                    };
                    if (i == 1) picture.Click += new EventHandler(MonumentBuy); // le joueur ne pourra pas cliquer sur ceux des adversaires sinon ça n'aurait pas de sens
                    picture.MouseEnter += new EventHandler(Carte_OnHover);
                    picture.MouseLeave += new EventHandler(Carte_OnLeave);
                    JoueurMonumentList.Add(picture);
                    this.Controls.Add(picture);
                    picture.BringToFront();
                }
                #endregion
                #region Champs de Blé et Boulangerie
                PictureBox pictureChamps = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    Location = new Point(BoardSizeWidth / 2 - CardWidth / 2, BoardSizeHeight - CardHeight),
                    ImageLocation = "Images/Carte1.png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte1",
                };
                Label NumberChamp = new Label
                {
                    Text = "x1",
                    Location = new Point(pictureChamps.Width - 23, 0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Font = new Font(Label.DefaultFont, FontStyle.Bold),
                    Visible = false,
                    ForeColor = Color.Yellow,
                };
                pictureChamps.Controls.Add(NumberChamp);
                PictureBox pictureBoulang = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    Location = new Point(BoardSizeWidth / 2 - CardWidth / 2, BoardSizeHeight - CardHeight),
                    ImageLocation = "Images/Carte3.png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte3",
                };
                Label NumberBoulang = new Label
                {
                    Text = "x1",
                    Location = new Point(pictureChamps.Width - 23, 0),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Font = new Font(Label.DefaultFont, FontStyle.Bold),
                    Visible = false,
                    ForeColor = Color.Yellow,
                };
                pictureBoulang.Controls.Add(NumberBoulang);
                pictureChamps.MouseEnter += new EventHandler(Carte_OnHover);
                pictureChamps.MouseLeave += new EventHandler(Carte_OnLeave);
                pictureBoulang.MouseEnter += new EventHandler(Carte_OnHover);
                pictureBoulang.MouseLeave += new EventHandler(Carte_OnLeave);
                JoueurStartCardList.Add(pictureChamps);
                this.Controls.Add(pictureChamps);
                JoueurStartCardList.Add(pictureBoulang);
                this.Controls.Add(pictureBoulang);

                if (i == 1) { CarteJoueur1 = JoueurStartCardList; MonuJoueur1 = JoueurMonumentList; }
                else if (i == 2) { CarteJoueur2 = JoueurStartCardList; MonuJoueur2 = JoueurMonumentList; }
                else if (i == 3) { CarteJoueur3 = JoueurStartCardList; MonuJoueur3 = JoueurMonumentList; }
                else if (i == 4) { CarteJoueur4 = JoueurStartCardList; MonuJoueur4 = JoueurMonumentList; }
                #endregion
                #region Animation Distribution
                for (int j = 0; j < 4; j++) //Pas mis directement dans la boucle précédente car pour des soucis d'affichage des images on préfère d'abord les afficher, puis les déplacer (au lieu de faire en même temps)
                {
                    Move_Cards(BoardSizeHeight - 220, 5 + j * 30, JoueurMonumentList, j, i);
                    await Task.Delay(50);
                }
                Move_Cards(BoardSizeHeight - CardHeight - 10, 5, JoueurStartCardList, 0, i);
                await Task.Delay(50);
                Move_Cards(BoardSizeHeight - CardHeight - 10, 5 + 5 + CardWidth, JoueurStartCardList, 1, i);

                #endregion
                //Positionnement des Sacoches à gold  + Nom joueurs
                #region Image GOLD 
                PictureBox Gold = new PictureBox
                {
                    Size = new Size(CardWidth + 15, CardHeight + 15),
                    ImageLocation = "Images/Gold.png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                if (i == 1)
                {
                    Gold.Location = new Point(5, BoardSizeHeight - (CardHeight * 3) - 50);
                    Gold.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                    MoneyJoueur1 = new Label
                    {
                        Text = "3",
                        BackColor = Color.Transparent,
                        Location = new Point(Gold.Width / 2 - 15, Gold.Height / 2),
                        Anchor = AnchorStyles.None,
                        Font = new Font("COMIC SANS MS", 15),
                    };
                    Gold.Controls.Add(MoneyJoueur1);
                }
                else if (i == 2)
                {
                    MoneyJoueur2 = new Label
                    {
                        Text = "3",
                        BackColor = Color.Transparent,
                        Location = new Point(Gold.Width / 2 - 15, Gold.Height / 2),
                        Anchor = AnchorStyles.None,
                        Font = new Font("COMIC SANS MS", 15),
                    };
                    if (this.nbJoueur == 2)
                    {
                        Gold.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        Gold.Location = new Point(BoardSizeWidth - (CardWidth + 20), 2 * CardHeight + 50);
                    }
                    else if (this.nbJoueur == 3 || this.nbJoueur == 4)
                    {
                        Gold.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        Gold.Location = new Point(5, 2 * CardHeight + 50);
                    }
                    Gold.Controls.Add(MoneyJoueur2);
                }
                else if (i == 3)
                {
                    MoneyJoueur3 = new Label
                    {
                        Text = "3",
                        BackColor = Color.Transparent,
                        Location = new Point(Gold.Width / 2 - 15, Gold.Height / 2),
                        Anchor = AnchorStyles.None,
                        Font = new Font("COMIC SANS MS", 15),
                    };
                    Gold.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    Gold.Location = new Point(BoardSizeWidth - (CardWidth + 20), 2 * CardHeight + 50);
                    Gold.Controls.Add(MoneyJoueur3);
                }
                else if (i == 4)
                {
                    MoneyJoueur4 = new Label
                    {
                        Text = "3",
                        BackColor = Color.Transparent,
                        Location = new Point(Gold.Width / 2 - 15, Gold.Height / 2),
                        Anchor = AnchorStyles.None,
                        Font = new Font("COMIC SANS MS", 15),
                    };
                    Gold.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    Gold.Location = new Point(BoardSizeWidth - (CardWidth + 20), BoardSizeHeight - (CardHeight * 3) - 50);
                    Gold.Controls.Add(MoneyJoueur4);
                }
                this.Controls.Add(Gold);
                Label NomJoueur = new Label
                {
                    Size = new Size(100, 30),
                    Text = "Joueur " + i,
                    Font = new Font("COMIC SANS MS", 12f),
                };
                if (i == 1)
                {
                    NomJoueur.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                    NomJoueur.Location = new Point(Gold.Location.X + Gold.Width + 10, Gold.Location.Y + Gold.Height / 2 - 6);
                    TourJoueur1 = NomJoueur;
                    TourJoueur1.ForeColor = Color.Yellow;
                }
                else if (i == 2)
                {
                    if (nbJoueur == 2)
                    {
                        NomJoueur.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        NomJoueur.Location = new Point(Gold.Location.X - 80, Gold.Location.Y + Gold.Height / 2 - 6);
                    }
                    else
                    {
                        NomJoueur.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        NomJoueur.Location = new Point(Gold.Location.X + Gold.Width + 10, Gold.Location.Y + Gold.Height / 2 - 6);
                    }
                    TourJoueur2 = NomJoueur;
                }
                else if (i == 3)
                {
                    NomJoueur.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    NomJoueur.Location = new Point(Gold.Location.X - 80, Gold.Location.Y + Gold.Height / 2 - 6);
                    TourJoueur3 = NomJoueur;
                }
                else if (i == 4)
                {
                    NomJoueur.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    NomJoueur.Location = new Point(Gold.Location.X - 80, Gold.Location.Y + Gold.Height / 2 - 6);
                    TourJoueur4 = NomJoueur;
                }
                this.Controls.Add(NomJoueur);
                #endregion

            }


            AvertissementJoueur = new Label
            {
                Text = "AVERTISSEMENT TEST",
                Size = new Size(400, 100),
                BackColor = Color.Transparent,
                Location = new Point(BoardSizeWidth / 2 - 200, BoardSizeHeight / 2 - 200),
                Anchor = AnchorStyles.None,
                Font = new Font("COMIC SANS MS", 15),
                Visible = false,
            };
            Controls.Add(AvertissementJoueur);

        }
        private async void EndGameCredit()
        {
            Controls.Clear();
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Carefree.wav");
            activeMusicBackGround.PlayLooping();
            Font FontName = new Font(new FontFamily("Verdana"), 10);
            PictureBox thanks = new PictureBox
            {
                ImageLocation = "Images/Thanks.png",
                Size = new Size(400, 150),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.Top,
                Location = new Point(this.Width / 2 - 200, this.Height / 2 - 300),
                WaitOnLoad = true,
            };
            this.Controls.Add(thanks);
            #region Les danses Fornites anim
            PictureBox Fornite1 = new PictureBox
            {
                ImageLocation = "Images/Fornite1.gif",
                Size = new Size(200, 350),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.None,
                BackColor = Color.Transparent,
                Location = new Point(-200, this.Height / 2 - 350 / 2),
                WaitOnLoad = true,
            };
            Fornite1.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString("OCEANE", FontName, Brushes.Black, 40, 40);
            });
            Controls.Add(Fornite1);
            Timer Time1 = new Timer();
            Time1.Interval = 20;
            Time1.Tick += new EventHandler((sender, e) =>
            {
                if (Fornite1.Location.X > this.Width)
                {
                    Time1.Stop();
                }
                else
                {
                    Fornite1.Location = new Point(Fornite1.Location.X + 20, Fornite1.Location.Y);
                }
            });
            Time1.Start();
            while (Time1.Enabled == true) await Task.Delay(1000);
            PictureBox Fornite2 = new PictureBox
            {
                ImageLocation = "Images/Fornite2.gif",
                Size = new Size(200, 350),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None,
                Location = new Point(this.Width + 200, this.Height / 2 - 350 / 2),
                WaitOnLoad = true,
            };
            Fornite2.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString("DIANE", FontName, Brushes.Black, 40, 40);
            });
            Controls.Add(Fornite2);
            Timer Time2 = new Timer();
            Time2.Interval = 20;
            Time2.Tick += new EventHandler((sender, e) =>
            {
                if (Fornite2.Location.X < -Fornite2.Width)
                {
                    Time2.Stop();
                }
                else
                {
                    Fornite2.Location = new Point(Fornite2.Location.X - 20, Fornite2.Location.Y);
                }
            });
            Time2.Start();
            while (Time2.Enabled == true) await Task.Delay(1000);
            PictureBox Fornite3 = new PictureBox
            {
                ImageLocation = "Images/Fornite3.gif",
                Size = new Size(200, 350),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None,
                Location = new Point(-200, this.Height / 2 - 350 / 2),
                WaitOnLoad = true,
            };
            Fornite3.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString("BAO", FontName, Brushes.Black, 40, 40);
            });
            Controls.Add(Fornite3);
            Timer Time3 = new Timer();
            Time3.Interval = 20;
            Time3.Tick += new EventHandler((sender, e) =>
            {
                if (Fornite3.Location.X > this.Width)
                {
                    Time3.Stop();
                }
                else
                {
                    Fornite3.Location = new Point(Fornite3.Location.X + 20, Fornite3.Location.Y);
                }
            });
            Time3.Start();
            while (Time3.Enabled == true) await Task.Delay(1000);


            PictureBox Fornite4 = new PictureBox
            {
                ImageLocation = "Images/Fornite4.gif",
                Size = new Size(200, 350),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None,
                Location = new Point(this.Width + 200, this.Height / 2 - 350 / 2),
                WaitOnLoad = true,
            };
            Fornite4.Paint += new PaintEventHandler((sender, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString("GUILLAUME", FontName, Brushes.Black, 40, 40);
            });
            Controls.Add(Fornite4);
            Timer Time4 = new Timer();
            Time4.Interval = 20;
            Time4.Tick += new EventHandler((sender, e) =>
            {
                if (Fornite4.Location.X < -Fornite4.Width)
                {
                    Time4.Stop();
                }
                else
                {
                    Fornite4.Location = new Point(Fornite4.Location.X - 20, Fornite4.Location.Y);
                }
            });
            Time4.Start();
            while (Time4.Enabled == true) await Task.Delay(1000);
            #endregion
            //On remet leur pos ensemble à 4 pour la fin
            Fornite1.Location = new Point(this.Width / 2 - (Fornite1.Width * 2), Fornite1.Location.Y);
            Fornite2.Location = new Point(this.Width / 2 - Fornite2.Width, Fornite2.Location.Y);
            Fornite3.Location = new Point(this.Width / 2, Fornite3.Location.Y);
            Fornite4.Location = new Point(this.Width / 2 + Fornite4.Width, Fornite4.Location.Y);
            await Task.Delay(8000);
            Application.Exit();
        }
        private void ChangeDisplayAfterExchange(int indexPlayer, bool toAdd, bool toRemove, int CarteTobeRemoved, int CarteToBeReceived)
        {
            List<PictureBox> tempList = new List<PictureBox>();
            if (indexPlayer == 0) tempList = CarteJoueur1;
            if (indexPlayer == 1) tempList = CarteJoueur2;
            if (indexPlayer == 2) tempList = CarteJoueur3;
            if (indexPlayer == 3) tempList = CarteJoueur4;

            if (toAdd && toRemove)
            {
                Point LocationToRemove = new Point();
                int Maxwidth = Int32.MinValue, indexToMove = -1, indexToRemove = -1;
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Maxwidth < tempList[i].Location.X)
                    {
                        indexToMove = i;
                        Maxwidth = tempList[i].Location.X;
                    }
                    int CarteNumber;
                    if (tempList[i].Name.Length == 6) CarteNumber = tempList[i].Name[5] - '0';
                    else CarteNumber = (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0');
                    if (CarteNumber == CarteTobeRemoved)
                    {
                        LocationToRemove = new Point(tempList[i].Location.X, tempList[i].Location.Y);
                        Controls.Remove(tempList[i]);
                        indexToRemove = i;
                    }
                }
                tempList[indexToMove].Location = new Point(LocationToRemove.X, LocationToRemove.Y);
                tempList.RemoveAt(indexToRemove);

                if (indexPlayer == 0) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived));
                else if (indexPlayer == 1 && nbJoueur == 2) tempList.Add(Spawn_Card(9, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 1) tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 3));
                else if (indexPlayer == 3) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 4));
            }
            else if (toAdd)
            {
                if (indexPlayer == 0) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived));
                else if (indexPlayer == 1 && nbJoueur == 2) tempList.Add(Spawn_Card(9, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 1) tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 3));
                else if (indexPlayer == 3) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 4));
            }
            else if (toRemove)
            {
                Point LocationToRemove = new Point();
                int Maxwidth = tempList[0].Width, indexToMove = 0, indexToRemove = 0;
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (indexPlayer == 0 || (indexPlayer == 1 && nbJoueur == 2))
                    {
                        if (Maxwidth < tempList[i].Location.X)
                        {
                            indexToMove = i;
                            Maxwidth = tempList[i].Location.X;
                        }
                    }
                    else
                    {
                        if (Maxwidth > tempList[i].Location.X)
                        {
                            indexToMove = i;
                            Maxwidth = tempList[i].Location.X;
                        }
                    }

                    int CarteNumber;
                    if (tempList[i].Name.Length == 6) CarteNumber = tempList[i].Name[5] - '0';
                    else CarteNumber = (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0');
                    if (CarteNumber == CarteTobeRemoved)
                    {
                        LocationToRemove = new Point(tempList[i].Location.X, tempList[i].Location.Y);
                        Controls.Remove(tempList[i]);
                        indexToRemove = i;
                    }
                }
                tempList[indexToMove].Location = new Point(LocationToRemove.X, LocationToRemove.Y);
                tempList.RemoveAt(indexToRemove);
            }
            if (!toAdd)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if ((CarteJoueur1[i].Name.Length == 6 && tempList[i].Name[5] - '0' == CarteToBeReceived) || (tempList[i].Name.Length == 7 && (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0') == CarteToBeReceived))
                    {
                        Label temp = tempList[i].Controls.OfType<Label>().First();
                        temp.Text = "x" + (temp.Text[1] - '0' + 1);
                        temp.Visible = true;
                    }
                }
            }
            if (!toRemove)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if ((CarteJoueur1[i].Name.Length == 6 && tempList[i].Name[5] - '0' == CarteTobeRemoved) || (tempList[i].Name.Length == 7 && (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0') == CarteTobeRemoved))
                    {
                        Label temp = tempList[i].Controls.OfType<Label>().First();
                        temp.Text = "x" + (temp.Text[1] - '0' - 1);
                        temp.Visible = true;
                        if (temp.Text[1] == 1) temp.Visible = false;
                    }
                }
            }
        }
        #endregion
    }
}
