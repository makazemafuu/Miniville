using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniVille_GraphiqueWF
{
    public partial class Form1 : Form
    {
        public int nbJoueur;
        public int difficulty;
        //public Game game;
        public int BoardSizeWidth, BoardSizeHeight;
        Label Tourjoueur, MoneyJoueur1, MoneyJoueur2;
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            StartMenu();
        }

        #region Start Menu
        void StartMenu()
        {
            PictureBox StartMenu = new PictureBox
            {
                Anchor = AnchorStyles.None,
                Size = new Size(this.Width, this.Height),
                Name = "StartMenu",
                AutoSize = true,
                BackColor = Color.Transparent,
            };
            this.Controls.Add(StartMenu);
            this.BackColor = Color.SkyBlue;
            PictureBox SunGif = new PictureBox
            {
                Anchor = AnchorStyles.None,
                ImageLocation = "Images/Sun.gif",
                Name = "Sun",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(this.Width, this.Height / 2),
                BackColor = Color.Transparent,
                Location = new Point(0, 0),

            };
            PictureBox Title = new PictureBox
            {
                Anchor = AnchorStyles.None,
                ImageLocation = "Images/MV_Title.png",
                Name = "Title",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(this.Width, this.Height / 2),
                BackColor = Color.Transparent
            };
            SunGif.Controls.Add(Title);
            StartMenu.Controls.Add(SunGif);
            PictureBox CityGif = new PictureBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ImageLocation = "Images/city-unscreen.gif",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(500, 400),
                BackColor = Color.Transparent,
                Location = new Point(this.Width / 8, this.Height / 2),
            };
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
            StartGame_Button.Click += new EventHandler(buttonStartGame_Button_Click);
            StartGame_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            StartGame_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
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
            Leave_Button.Click += new EventHandler(buttonLeave_Click);
            Leave_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Leave_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
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
            await Task.Delay(800);
            nbPlayerChoice();
        }
        private void buttonLeave_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Veux-tu vraiment quitter le jeu ?", "Quitter le jeu", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        void MenuStart_Button_OnEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = Color.White;
            button.BackColor = Color.DarkBlue;
            button.Size = new Size(button.Width + 14, button.Height + 14);
            button.Location = new Point(button.Location.X - 7, button.Location.Y - 7);
        }
        void MenuStart_Button_OnLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.ForeColor = Color.DarkBlue;
            button.BackColor = Color.White;
            button.Size = new Size(button.Width - 14, button.Height - 14);
            button.Location = new Point(button.Location.X + 7, button.Location.Y + 7);
        }
        async void DifficultyChoice()
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
            Ending3.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Button TrueEnding = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending3.Location.X, Ending3.Location.Y + Ending3.Height + 20),
                Text = "Vraies règles du jeux (4 établissements)",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };
            TrueEnding.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            TrueEnding.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            Ending1.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 2; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            });
            Ending2.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 3; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            });
            Ending3.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            });
            TrueEnding.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            });
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
            FourPlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            TwoPlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 2; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                DifficultyChoice();
            });
            ThreePlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 3; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                DifficultyChoice();
            });
            FourPlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                DifficultyChoice();
            });
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
        private async void StartGame()
        {
            await Task.Delay(1000);
            Controls.Clear();
            BoardSizeHeight = this.ClientSize.Height;
            BoardSizeWidth = this.ClientSize.Width - 250; // La taille du menu à gauche = 250
            //game = new Game();
            AddMenu(); //Le menu à droite
            //Board_Display();
            //LabelCardEffect();
        }

        private void Board_Display()
        {

        }

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
                Text = " NOUVELLE PARTIE "
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
                Text = " QUITTER "
            };
            LeaveButton.Click += new EventHandler(buttonLeave_Click);
            LeaveButton.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LeaveButton.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            Button LancerDe = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, MenuBackGround.Height - 175),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = " LANCER LE DE ",
                Name = "BoutonLancer"
            };
            LancerDe.Click += new EventHandler(buttonLancer_Click);
            LancerDe.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LancerDe.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            LancerDe.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);
            CheckBox CheckBoxDe = new CheckBox
            {
                AutoCheck = true,
                Size = new Size(200, 50),
                Location = new Point(25, LancerDe.Location.Y + 50),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = "Cocher pour lancer deux dés"
            };
            CheckBoxDe.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            Button FinDuTour = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, CheckBoxDe.Location.Y + 50),
                Anchor = (AnchorStyles.Right | AnchorStyles.Bottom),
                Text = "FIN DU TOUR",
                Name = "BoutonPasserTour",
                Enabled = false
            };
            FinDuTour.Click += new EventHandler(buttonFinTour_Click);
            FinDuTour.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            FinDuTour.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            FinDuTour.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);
            Tourjoueur = new Label
            {
                Size = new Size(400, 20),
                Location = new Point(0, 300),
                Anchor = AnchorStyles.Right,
                //Text = "C'est le tour du joueur " + game.tourJoueur,
            };
            MoneyJoueur1 = new Label
            {
                Size = new Size(400, 20),
                Location = new Point(0, 320),
                Anchor = AnchorStyles.Right,
                //Text = "Pièces joueur 1 : " + game.player1.Pieces,
            };
            MoneyJoueur2 = new Label
            {
                Size = new Size(400, 20),
                Location = new Point(0, 340),
                Anchor = AnchorStyles.Right,
                //Text = "Pièces joueur 2 : " + game.player2.Pieces,
            };
            MenuBackGround.Controls.Add(MoneyJoueur1);
            MenuBackGround.Controls.Add(MoneyJoueur2);
            MenuBackGround.Controls.Add(Tourjoueur);
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
            //if (game.NombreDe == 1)
            //{
            //    game.NombreDe = 2;
            //    this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LES DES ";
            //}
            //else
            //{
            //    game.NombreDe = 1;
            //    this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LE DE ";
            //}
        }
        private void buttonChangeMode_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Changer de mode recommencera une nouvelle partie es-tu sur?", "Change MODE", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Controls.Clear();
                nbPlayerChoice();
            }
        }
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Veux-tu lancer une nouvelle partie ?", "Nouvelle partie", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                List<PictureBox> temp = Controls.OfType<PictureBox>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].Name != "MenuDroite") Controls.Remove(temp[i]);
                }
                //game = new Game();
                //Board_Display();
                //LabelCardEffect();
            }
        }
        private async void buttonLancer_Click(object sender, EventArgs e)
        {
            //if (game.NombreDe == 1)
            //{
            //    game.scoreDes = game.Lancer(1);
            //    DiceAnim(game.scoreDes, 0);
            //}
            //else
            //{
            //    int firstDe = game.Lancer(1), secondDe = game.Lancer(1);
            //    game.scoreDes = firstDe + secondDe;
            //    DiceAnim(firstDe, secondDe);
            //}
            //game.DieThrowed = true;
            DiceAnim(random.Next(1, 6), 0);   //A enlever lors du merge avec le dice de fait
            await Task.Delay(2000);
            //game.Update(game.scoreDes);
            //UpdateLabels();
            Button thisButton = (Button)sender;
            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = true;
            thisButton.Enabled = false;
        }
        private void buttonFinTour_Click(object sender, EventArgs e)
        {
            //game.DieThrowed = false;
            MessageBox.Show("Fin tour");
            //NextJoueurTour();
            //TourIA();
            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
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
        #endregion
        #region Animation
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
        private void Spawn_Cards(int height, int width) // A changer
        {
            PictureBox picture = new PictureBox
            {
                Text = "text",
            };
            this.Controls.Add(picture);
        }
        #endregion


    }
}
