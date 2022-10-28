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
        public int nbJoueur;
        public int difficulty;
        SoundPlayer activeMusicBackGround;

        public int BoardSizeWidth, BoardSizeHeight;
        //Label Tourjoueur, MoneyJoueur1, MoneyJoueur2;
        NAudio.Wave.WaveOut waveOutCards, waveOutButtonHover, waveOutButtonClick, waveOutLanceDe;
        Random random = new Random();
        public Form1()
        {
           InitializeComponent();
            this.MaximizeBox = false;
            //Init des SFX
            var readerCards = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Cards Flip.mp3");
            waveOutCards = new NAudio.Wave.WaveOut();
            waveOutCards.Init(readerCards);
            var readerButtonHover = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonHover.mp3");
            waveOutButtonHover = new NAudio.Wave.WaveOut();
            waveOutButtonHover.Init(readerButtonHover);
            var readerButtonClick = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonClick.mp3");
            waveOutButtonClick = new NAudio.Wave.WaveOut();
            waveOutButtonClick.Init(readerButtonClick);
            var readerLanceDe = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Dice Roll.mp3");
            waveOutLanceDe = new NAudio.Wave.WaveOut();
            waveOutLanceDe.Init(readerLanceDe);
            StartMenu();
           //StartGame(); 
            
        }
        #region Start Menu
        void StartMenu()
        {
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Start.wav");
            activeMusicBackGround.PlayLooping();
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
                //ImageLocation = "Images/Sun.gif",  //en commentaire car nouveau test avec le LoadAsync pour éviter un chargement
                Name = "Sun",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(this.Width, this.Height / 2),
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                WaitOnLoad = false, //A mettre à true si enlever le LoadAsync

            };
            SunGif.LoadAsync("Images/Sun.gif");
            PictureBox Title = new PictureBox
            {
                Anchor = AnchorStyles.None,
                //ImageLocation = "Images/MV_Title.png",
                Name = "Title",
                SizeMode = PictureBoxSizeMode.CenterImage,
                Size = new Size(this.Width, this.Height / 2),
                BackColor = Color.Transparent,
                WaitOnLoad = false, //A mettre à true si enlever le LoadAsync
            };
            Title.LoadAsync("Images/MV_Title.png");
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
                Size = new Size(300,200),
                Location = new Point(this.ClientSize.Width/2 - 150, this.ClientSize.Height / 2 - 300),
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent,
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
            buttonYes.Click += new EventHandler((sender, e) => {
                EndGameCredit();
            });
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
            buttonNo.Click += new EventHandler((sender, e) => {
                this.Controls.Remove(buttonNo.Parent);
            });
            buttonNo.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            buttonNo.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            buttonNo.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);

            NewFonds.Controls.Add(AreYouSure);
            NewFonds.Controls.Add(buttonYes);
            NewFonds.Controls.Add(buttonNo);
            
            this.Controls.Add(NewFonds);
            NewFonds.BringToFront();
            AreYouSure.BringToFront();
            Button_Enter_Anim(buttonYes);
            Button_Enter_Anim(buttonNo);

            //DialogResult result = MessageBox.Show("Veux-tu vraiment quitter le jeu ?", "Quitter le jeu", MessageBoxButtons.YesNo);
            //if (result == DialogResult.Yes)
            //{
            //    EndGameCredit(); 
            //}
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
            waveOutButtonHover.Play();
        }
        void MenuStart_Button_Click_SFX(object sender, EventArgs e)
        {
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
                Text = "Vraies règles du jeux (4 établissements)",
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
                nbJoueur = 2; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending2.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 3; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending3.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TrueEnding.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
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
                DifficultyChoice();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            ThreePlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 3; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                DifficultyChoice();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            FourPlayers.Click += new EventHandler((sender, e) =>
            {
                nbJoueur = 4; Button_Leave_Anim(TwoPlayers); Button_Leave_Anim(ThreePlayers); Button_Leave_Anim(FourPlayers);
                DifficultyChoice();
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
        private async void StartGame()
        {
            await Task.Delay(1000);
            Controls.Clear();
            BoardSizeHeight = this.ClientSize.Height;
            BoardSizeWidth = this.ClientSize.Width - 250; // La taille du menu à gauche = 250

            if(activeMusicBackGround != null) activeMusicBackGround.Stop();
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/InGame.wav");
            activeMusicBackGround.PlayLooping();

            //game = new Game();

            ZoomCardStart(); //Les cartes en gros quand on Hover
            AddMenu(); //Le menu à droite
            Board_Display();
        }

        private async void Board_Display()
        {
            // Affichage des cartes du boards avec animation
            int k = 0;
            for (int i = 0; i < 3; i++) //3 lignes
            {
                for (int j = 0; j < 5; j++) //5 colonnes
                {
                    // Marge de 20 entre les cartes  ==>  ( x/2 * nb carte sur ligne/colonne ) + x * ( % nb carte sur ligne/colonne) avec x la taille + la marge    : ici 20 de marge
                    Spawn_Cards(BoardSizeHeight / 2 - (60 * 3) + 120 * (i % 3), BoardSizeWidth / 2 - (50 * 5) + 100 * (j % 5), (k++ % 15) + 1, true);
                    await Task.Delay(50);
                }
            }
            //Affichage des 2 premières cartes Boulangerie et Champs de blé + les 4 monuments 
            Spawn_Cards(BoardSizeHeight - 110,5,1, true,1); //Champs
            await Task.Delay(50);
            Spawn_Cards(BoardSizeHeight - 110, 5, 1, true, -1); // joueur 2 dernier arg en -1
            await Task.Delay(50);
            //await Task.Delay(100);
            Spawn_Cards(BoardSizeHeight - 110, 100, 3, true, 1); //Boulang
            await Task.Delay(50);
            Spawn_Cards(BoardSizeHeight - 110, 100, 3, true, -1);
            await Task.Delay(50);
            //Les Monuments
            for (int i = 0; i < 4; i++)
            {
                ///await Task.Delay(100);
                Spawn_Cards(BoardSizeHeight - 220, 5 + i * 30, 16+i, true, 1);
                await Task.Delay(50);
                Spawn_Cards(BoardSizeHeight - 220, 5 + i * 30, 16 + i, true, -1);
                await Task.Delay(50);
            }
        }
        private void ZoomCardStart()
        {
            for(int i = 1; i < 16; i++)
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
            for(int i = 1; i < 5; i++)
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
                    Name = "ZoomMonu" + i+"Locked",
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
            var ZoomPicture = this.Controls.Find("Zoom" + thisPicture.Name, true).FirstOrDefault();
            if (ZoomPicture != null)
            {
                ZoomPicture.Visible = true;
                if (thisPicture.Location.X > this.Width - thisPicture.Width - (ZoomPicture.Width + 30)) ZoomPicture.Location = new Point(thisPicture.Location.X - (ZoomPicture.Width + 30),
                    this.Height - thisPicture.Location.Y > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);
                else ZoomPicture.Location = new Point(thisPicture.Location.X + thisPicture.Width + 10,
                    this.Height - thisPicture.Location.Y > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);

                if (ZoomPicture.Location.Y <-5) ZoomPicture.Location = new Point(ZoomPicture.Location.X, ( this.Height - ZoomPicture.Height)/2);
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
                ZoomPicture.SendToBack();
            }
        }
        private void MonumentBuy(object sender, EventArgs e)
        {
            var picturebox = (PictureBox)sender;
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
            //Tourjoueur = new Label
            //{
            //    Size = new Size(400, 20),
            //    Location = new Point(0, 300),
            //    Anchor = AnchorStyles.Right,
            //    //Text = "C'est le tour du joueur " + game.tourJoueur,
            //};
            //MoneyJoueur1 = new Label
            //{
            //    Size = new Size(400, 20),
            //    Location = new Point(0, 320),
            //    Anchor = AnchorStyles.Right,
            //    //Text = "Pièces joueur 1 : " + game.player1.Pieces,
            //};
            //MoneyJoueur2 = new Label
            //{
            //    Size = new Size(400, 20),
            //    Location = new Point(0, 340),
            //    Anchor = AnchorStyles.Right,
            //    //Text = "Pièces joueur 2 : " + game.player2.Pieces,
            //};
            //MenuBackGround.Controls.Add(MoneyJoueur1);
            //MenuBackGround.Controls.Add(MoneyJoueur2);
            //MenuBackGround.Controls.Add(Tourjoueur);
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
                List<PictureBox> temp = Controls.OfType<PictureBox>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].Name != "MenuDroite") Controls.Remove(temp[i]);
                }
                BoardSizeHeight = this.ClientSize.Height;
                BoardSizeWidth = this.ClientSize.Width - 250; // La taille du menu à gauche = 250
                //game = new Game();
                Board_Display();
                ZoomCardStart();
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
            DiceAnim(random.Next(1, 6), 0);   // !!!!!!!!!!!!!! A enlever lors du merge avec le dice de fait
            waveOutLanceDe.Play();
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
        private async void Spawn_Cards(int height, int width, int CarteNom = 0, bool Afficher = false, int Sens = 0) 
        {
            PictureBox picture = new PictureBox
            {
                Size = new Size(80, 100),
                BackColor = Color.Transparent,
                Location = new Point(BoardSizeWidth / 2 - 40, BoardSizeHeight - 100), // width/2 et height * 1
                Anchor = AnchorStyles.None,
                WaitOnLoad = false,
            };
            if (CarteNom != 0 )
            {
                if(CarteNom < 16)
                {
                    //picture.ImageLocation = "Images/Carte" + CarteNom + ".png";
                    picture.LoadAsync("Images/Carte" + CarteNom + ".png");
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture.Name = "Carte" + CarteNom;          
                }
                else
                {
                    //picture.ImageLocation = "Images/Monu" + (CarteNom % 15) + "Locked.png";
                    picture.LoadAsync("Images/Monu" + (CarteNom % 15) + "Locked.png");
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture.Name = "Monu" + (CarteNom%15) +"Locked";
                    picture.Click += new EventHandler(MonumentBuy);
                }
            }
            if(Sens == 1)
            {
                picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            }
            if(Sens == -1)
            {
                picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                height = BoardSizeHeight - picture.Height - height;
                width = BoardSizeWidth - picture.Width - width;
            }
            picture.MouseEnter += new EventHandler(Carte_OnHover);
            picture.MouseLeave += new EventHandler(Carte_OnLeave);
            this.Controls.Add(picture);
            picture.BringToFront();
            await Task.Delay(50); 
            Timer Time = new Timer();

            int directionY = height - picture.Location.Y, directionX = width - picture.Location.X;

            waveOutCards.Play();
            Time.Interval = 3;
            float totalTime = 15, elapsedTime = 0;
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime)
                {
                    Time.Stop();
                    if(!Afficher) Controls.Remove(picture);
                }
                else
                {
                    picture.Location = new Point(picture.Location.X + directionX / 6, picture.Location.Y + directionY / 6);
                }
                elapsedTime += Time.Interval;
            });
            Time.Start();
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
                Size = new Size(400,150),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.Top,
                Location = new Point(this.Width/ 2  - 200, this.Height / 2 - 300)
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
                if (Fornite1.Location.X > this.Width )
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
                Location = new Point(this.Width+200, this.Height / 2 - 350 / 2),
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
            Fornite1.Location = new Point(this.Width/2 - (Fornite1.Width * 2),Fornite1.Location.Y);
            Fornite2.Location = new Point(this.Width / 2 - Fornite2.Width , Fornite2.Location.Y);
            Fornite3.Location = new Point(this.Width / 2 , Fornite3.Location.Y);
            Fornite4.Location = new Point(this.Width / 2 + Fornite4.Width, Fornite4.Location.Y);
            await Task.Delay(8000);
            Application.Exit();
        }
        #endregion
    }
}
