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
        string historique = "Tour du joueur 1";
        Label HistoriqueLabel1, HistoriqueLabel2, HistoriqueLabel3, HistoriqueLabel4;
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
                Anchor = AnchorStyles.None, //Permet d'ancrer l'image à un point, ici .None = le centre
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f), //Oui c'est bien du COMIC SANS MS
                ForeColor = Color.DarkBlue,
            };
            //Ajout des events pour le bouton
            StartGame_Button.Click += new EventHandler(buttonStartGame_Button_Click) + new EventHandler(MenuStart_Button_Click_SFX); 
            StartGame_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            StartGame_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            StartGame_Button.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            //Idem pour le quit bouton
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
            //Idem events
            Leave_Button.Click += new EventHandler(buttonLeave_Click) + new EventHandler(MenuStart_Button_Click_SFX);
            Leave_Button.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Leave_Button.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            Leave_Button.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            //Ajout dans le Forms des boutons 
            this.Controls.Add(StartGame_Button);
            this.Controls.Add(Leave_Button);
            StartMenu.SendToBack(); //Permet la gestion des 'superpositions'. Send to Back => sera l'image la plus reculée / les autres controls sont SUR cette image
            //Animation d'apparition des boutons (d'une size 0 à la size normale). Fait une sorte de Pop
            Button_Enter_Anim(StartGame_Button);
            Button_Enter_Anim(Leave_Button);
        }
        async void buttonStartGame_Button_Click(object sender, EventArgs e) //Event pour gérer le clique sur Start Game
        {
            Button StartGameButton = (Button)sender; //On récupère la ref du bouton cliqué
            var LeaveGameButton = this.Controls.Find("LGButton", true).FirstOrDefault(); //Recherche du bouton Leave Game pour le détruire également (alternativement il aurait fallu réunir les 2 boutons dans le même parent et détruire le parent => permet d'optimiser et de pas rechercher le bouton)
            //Animation de sortie pour les boutons (de la size normale à 0, semblant de disparition)
            Button_Leave_Anim(StartGameButton);
            Button_Leave_Anim((Button)LeaveGameButton);
            await Task.Delay(800); //Attend 800ms pour la fin de l'animation de sortie (alternativement on aurait pu faire un Task du button_leave et faire un await Button_leave)
            nbPlayerChoice(); //On passe à la nouvelle scène pour le choix du nombre de joueur
        }
        private void buttonLeave_Click(object sender, EventArgs e) //Event pour gérer le clique du leave Game
        {
            //Nouvelle picturebox pour servir de fonds (utile lorsque le joueur veut quitter le jeu avec une partie en cours)
            var NewFonds = new PictureBox()
            {
                BackColor = this.BackColor, //On prend la couleur du fonds
                Size = new Size(this.Width, this.Height),
                Anchor = AnchorStyles.None,
            };

            //Logo de sortie
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

            //Bouton oui
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

            //Event du bouton Oui
            buttonYes.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            buttonYes.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            buttonYes.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);

            //Bouton non
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

            //Event du bouton non
            buttonNo.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            buttonNo.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            buttonNo.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);

            //Si le joueur veut quitter ==> envoie à la scène de fin de jeu + crédit
            buttonYes.Click += new EventHandler((sender, e) => {
                EndGameCredit();
                buttonYes.Enabled = false;
                buttonNo.Enabled = false;
            }) + MenuStart_Button_Click_SFX; //Petit SFX
            //Si le joueur ne veut pas quitter 
            buttonNo.Click += new EventHandler((sender, e) => {
                this.Controls.Remove(buttonNo.Parent); //On prend le parent du bouton (donc la picturebox fonds + logo + bouton yes + bouton non) et on l'a supprime de notre form
                buttonYes.Enabled = false;  //On désactive les boutons pour éviter les petits malins qui spam click
                buttonNo.Enabled = false;
            }) + MenuStart_Button_Click_SFX;

            //Ajout des boutons sur le fonds
            NewFonds.Controls.Add(AreYouSure);
            NewFonds.Controls.Add(buttonYes);
            NewFonds.Controls.Add(buttonNo);

            //Ajout du fonds dans le forms
            this.Controls.Add(NewFonds);
            NewFonds.BringToFront(); //Le fonds sera par dessus le jeu en cours
            AreYouSure.BringToFront();

            //Animation d'entrée des boutons YES/NO
            Button_Enter_Anim(buttonYes);
            Button_Enter_Anim(buttonNo);
        }
        void MenuStart_Button_OnEnter(object sender, EventArgs e) //Event pour gérer l'entrée de la souris sur le bouton
        {
            Button button = (Button)sender;
            button.ForeColor = Color.White; //Police en blanc
            button.BackColor = Color.DarkBlue; //Fonds en bleu foncé
            button.Size = new Size(button.Width + 14, button.Height + 14); //Augmentation de la taille
            button.Location = new Point(button.Location.X - 7, button.Location.Y - 7); //On bouge la location de l'augmentation / 2 pour recentrer le bouton
        }
        void MenuStart_Button_OnHover_SFX(object sender, EventArgs e) //SFX du hover bouton. Fonction différente du OnEnter pour plus de fluidité et moins de pollution sonore
        {
            readerButtonHover = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonHover.mp3");
            waveOutButtonHover.Init(readerButtonHover);
            waveOutButtonHover.Play();
        }
        void MenuStart_Button_Click_SFX(object sender, EventArgs e) //SFX bouton clique
        {
            readerButtonClick = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/ButtonClick.mp3");
            waveOutButtonClick.Init(readerButtonClick);
            waveOutButtonClick.Play();
        }
        void MenuStart_Button_OnLeave(object sender, EventArgs e) //Event pour gérer la sortie de la souris sur le bouton
        {
            Button button = (Button)sender;
            button.ForeColor = Color.DarkBlue; //On remet la couleur de la police en bleu foncé
            button.BackColor = Color.White; //On remet la couleur du fonds en blanc
            button.Size = new Size(button.Width - 14, button.Height - 14); //Réduction de la taille
            button.Location = new Point(button.Location.X + 7, button.Location.Y + 7); //On recentre 
        }
        async void endingTypeChoice() //Choix du type de fin de jeu 
        {
            await Task.Delay(800); //Attente de l'anim de fin des boutons (alternativement faire un await Task "Fonction Anim" comme dit précedemment)
            //Premiere fin = 20 pièces
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
            //Ajout des events du bouton
            Ending1.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending1.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending1.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            //Deuxieme fin = 30 pièces
            Button Ending2 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending1.Location.X, Ending1.Location.Y + Ending1.Height + 20), //On le place sous le bouton Ending1 + 20 de marge
                Text = "30 pièces pour gagner",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            //Ajout des events du bouton
            Ending2.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending2.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending2.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            //Troisieme fin = 20 pièces + chaque carte obtenue
            Button Ending3 = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending2.Location.X, Ending2.Location.Y + Ending2.Height + 20), //Placé sous le bouton de la fin 2 + 20 de marge
                Text = "20 pièces + toutes les cartes (1 fois)",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            //Ajout des events du bouton
            Ending3.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Ending3.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Ending3.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            //Quatrième fin = Tous les monuments
            Button TrueEnding = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(Ending3.Location.X, Ending3.Location.Y + Ending3.Height + 20), //Sous le bouton de la 3e fin + 20 de marge
                Text = "Vraies règles du jeux (4 Monuments)",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            //Ajout des events du bouton
            TrueEnding.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            TrueEnding.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            TrueEnding.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);


            //Event click (différent pour chaque bouton dans l'assignation de la variable EndingChoice)
            Ending1.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 1;   // <--- iCI
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending2.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 2; // <--- iCI
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            Ending3.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 3; // <--- iCI
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TrueEnding.Click += new EventHandler((sender, e) =>
            {
                Button_Leave_Anim(Ending1); Button_Leave_Anim(Ending2); Button_Leave_Anim(Ending3); Button_Leave_Anim(TrueEnding);
                EndingChoice = 4; // <--- iCI
                Ending1.Enabled = false; Ending2.Enabled = false; Ending3.Enabled = false; TrueEnding.Enabled = false;
                StartGame();
            }) + new EventHandler(MenuStart_Button_Click_SFX);

            //Ajout des boutons dans le forms
            this.Controls.Add(Ending1);
            this.Controls.Add(Ending2);
            this.Controls.Add(Ending3);
            this.Controls.Add(TrueEnding);

            //On les place sur le tas
            Ending1.BringToFront();
            Ending2.BringToFront();
            Ending3.BringToFront();
            TrueEnding.BringToFront();

            //Animation d'apparition
            Button_Enter_Anim(Ending1);
            Button_Enter_Anim(Ending2);
            Button_Enter_Anim(Ending3);
            Button_Enter_Anim(TrueEnding);
        }
        void nbPlayerChoice() //Choix du nombre de joueur
        {
            // Bouton 2 joueurs
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

            //Ajout des events
            TwoPlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            TwoPlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            TwoPlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            // Bouton 3 joueurs
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

            //Ajout des events
            ThreePlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            ThreePlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            ThreePlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            // Bouton 4 joueurs
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

            //Ajout des events
            FourPlayers.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            FourPlayers.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            FourPlayers.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);

            //Event Click , similaire pour tous les boutons SAUF pour l'assignation de la variable nbJoueur (comme pour le choix du type de fin)
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

            //Ajout des boutons dans le forms
            this.Controls.Add(TwoPlayers);
            this.Controls.Add(ThreePlayers);
            this.Controls.Add(FourPlayers);

            //On les place devant
            TwoPlayers.BringToFront();
            ThreePlayers.BringToFront();
            FourPlayers.BringToFront();

            //Animation d'entrée des boutons
            Button_Enter_Anim(TwoPlayers);
            Button_Enter_Anim(ThreePlayers);
            Button_Enter_Anim(FourPlayers);
        }
        #endregion
        #region Core Game

        #region Menu à droite
        private void AddMenu() //Ajout du menu à droite pour les parties dans notre forms
        {

            //PictureBox pour un background d'une couleure différente à celui du fonds 
            PictureBox MenuBackGround = new PictureBox
            {
                Size = new Size(this.ClientSize.Width - BoardSizeWidth, BoardSizeHeight), //Le width sera de 250 ==> si on veut modifier on le change dans le constructeur Form1
                BackColor = Color.CornflowerBlue, //Couleur diff 
                Location = new Point(BoardSizeWidth, 0), //On le place à droite du board
                Name = "MenuDroite",
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right)
            };

            //Bouton nouvelle partie 
            Button NewGame = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Location = new Point(25, 25),
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Text = "NOUVELLE PARTIE",
                Font = new Font("COMIC SANS MS", 10),
            };

            //Event du bouton nouvelle partie
            NewGame.Click += new EventHandler(buttonNewGame_Click);
            NewGame.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            NewGame.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);

            //Bouton modif du type de partie
            Button ChangeMode = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Location = new Point(25, NewGame.Location.Y + 60),
                Text = "Retour choix nb \nJoueurs",
                Font = new Font("COMIC SANS MS", 10),
            };

            //Event du bouton modif
            ChangeMode.Click += new EventHandler(buttonChangeMode_Click);
            ChangeMode.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            ChangeMode.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);

            //Bouton quitter la partie
            Button LeaveButton = new Button
            {
                Size = new Size(MenuBackGround.Width - 50, 50),
                BackColor = Color.LightSteelBlue,
                Anchor = (AnchorStyles.Top | AnchorStyles.Right),
                Location = new Point(25, ChangeMode.Location.Y + 60),
                Text = "QUITTER",
                Font = new Font("COMIC SANS MS", 10),
            };

            //event du bouton quitter
            LeaveButton.Click += new EventHandler(buttonLeave_Click);
            LeaveButton.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LeaveButton.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);

            //Event du bouton lancer le dé
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

            //Event du bouton lancer le dé
            LancerDe.Click += new EventHandler(buttonLancer_Click);
            LancerDe.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            LancerDe.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            LancerDe.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);

            //Check box pour le choix de 1 ou deux dés
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
            //Event du checkbox
            CheckBoxDe.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);

            //Bouton Fin de tour
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

            //Event fin de tour
            FinDuTour.Click += new EventHandler(buttonFinTour_Click);
            FinDuTour.MouseEnter += new EventHandler(Menu_Game_Button_OnEnter);
            FinDuTour.MouseLeave += new EventHandler(Menu_Game_Button_OnLeave);
            FinDuTour.EnabledChanged += new EventHandler(Menu_Game_Button_EnabledChanged);

            //Ajout dans le pictureBox menu
            MenuBackGround.Controls.Add(CheckBoxDe);
            MenuBackGround.Controls.Add(LancerDe);
            MenuBackGround.Controls.Add(FinDuTour);
            MenuBackGround.Controls.Add(NewGame);
            MenuBackGround.Controls.Add(ChangeMode);
            MenuBackGround.Controls.Add(LeaveButton);

            //Label historique
            HistoriqueLabel1 = new Label
            {
                Size = new Size(MenuBackGround.Width - 5, 50),
                Text = "Tour du joueur 1",
                Location = new Point(5, LeaveButton.Location.Y + LeaveButton.Height + 5),
                Font = new Font("COMIC SANS MS", 9f),
                ForeColor = Color.Yellow,
                Anchor = AnchorStyles.Right,
            };
            HistoriqueLabel2 = new Label
            {
                Size = new Size(MenuBackGround.Width-5, 50),
                Text = "Tour du joueur 2",
                Location = new Point(5, HistoriqueLabel1.Location.Y + HistoriqueLabel1.Height + 15),
                Font = new Font("COMIC SANS MS", 9f),
                Anchor = AnchorStyles.Right,
            };
            HistoriqueLabel3 = new Label
            {
                Size = new Size(MenuBackGround.Width-5, 50),
                Text = "Tour du joueur 3",
                Location = new Point(5, HistoriqueLabel2.Location.Y + HistoriqueLabel2.Height + 15),
                Visible = false,
                Font = new Font("COMIC SANS MS", 9f),
                Anchor = AnchorStyles.Right,
            };
            HistoriqueLabel4 = new Label
            {
                Size = new Size(MenuBackGround.Width-5, 50),
                Text = "Tour du joueur 4",
                Location = new Point(5, HistoriqueLabel3.Location.Y + HistoriqueLabel3.Height + 15),
                Visible = false,
                Font = new Font("COMIC SANS MS", 9f),
                Anchor = AnchorStyles.Right,
            };
            MenuBackGround.Controls.Add(HistoriqueLabel1);
            MenuBackGround.Controls.Add(HistoriqueLabel2);
            if (nbJoueur >= 3) { HistoriqueLabel3.Visible = true; MenuBackGround.Controls.Add(HistoriqueLabel3); }
            if (nbJoueur == 4) { HistoriqueLabel4.Visible = true; MenuBackGround.Controls.Add(HistoriqueLabel4); }
            //Ajout du menu dans le forms
            this.Controls.Add(MenuBackGround);
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e) //Event du checkBox 1 ou 2 dés
        {
            CheckBox checkDe = (CheckBox)sender;
            if (checkDe.Checked) //Si la checkbox est coché alors 
            {
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LES DES "; //Modif du bouton Lancer
                game.NombreDe = 2; //Modif du nombre de dé dans game pour lancer 2 dés
            }
            else //Sinon
            {
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Text = " LANCER LE DE "; //Modif du texte
                game.NombreDe = 1; //On lance un dé
            }
        }
        private void buttonChangeMode_Click(object sender, EventArgs e) //Event du Changement de mode
        {
            //MessageBox.YESNO pour demander confirmation au joueur 
            DialogResult result = MessageBox.Show("Changer de mode recommencera une nouvelle partie, es-tu sûr?", "Changer de Mode", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) // Si confirmation
            {
                activeMusicBackGround.Stop(); //On désactive la musique in-Game et on met la musique Start-Game
                activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Start.wav");
                activeMusicBackGround.PlayLooping(); Controls.Clear();
                nbPlayerChoice(); // On revient au choix du nb de joueur
            }
        }
        private void buttonNewGame_Click(object sender, EventArgs e) //Event du click nouvelle partie
        {
            //Demande de confirmation du joueur
            DialogResult result = MessageBox.Show("Veux-tu lancer une nouvelle partie ?", "Nouvelle partie", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) //Si confirmation
            {
                //On parcourt la liste des controls du forms et on supprime tout SAUF le menu à droite
                List<Control> temp = Controls.OfType<Control>().ToList();
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].Name != "MenuDroite") Controls.Remove(temp[i]);
                }

                //On reprend la taille de l'écran (peut-être obsolète ici vu l'event Resize)
                BoardSizeHeight = this.ClientSize.Height;
                BoardSizeWidth = this.ClientSize.Width - 250; // La taille du menu à gauche = 250
                
                game = new Game(nbJoueur); //On refait une nouvelle parti
                ZoomCardStart(); //Initialisation des grosses cartes zoomées
                DistributionCard_StartGame(); //Animation des distributions des cartes
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = true; //On active le bouton Lancer dé (qui aurait pu être désactivé)
            }
        }
        private async void buttonLancer_Click(object sender, EventArgs e) //Event du bouton lancer le dé
        {
            diceScore2 = 0; //On met le score du dé n°2 à 0
            Button thisButton = (Button)sender;
            thisButton.Enabled = false; //Si on clique sur le bouton on le désactive (pour les petits malins qui spam)
            if (game.NombreDe == 1) //Si on lance un dé
            {
                game.scoreDes = game.die.Lancer();
                diceScore1 = game.scoreDes; //On prend le diceScore
                DiceAnim(game.scoreDes, 0); //Animation des dés
                historique += "\nScore lancé : " +  game.scoreDes;
            }
            else //On lance deux dés
            {
                diceScore1 = game.die.Lancer(); //On récupère chaque score pour le cas ou y'a un double, pour l'animation etc.. obligé de garder les scores quoi
                diceScore2 = game.die.Lancer();
                game.scoreDes = diceScore1 + diceScore2; //Somme des deux pour l'activation value
                DiceAnim(diceScore1, diceScore2); //Animation des dés
                historique += "\nScore lancé : " + diceScore1 + " + " + diceScore2 + " = " + game.scoreDes;
            }
            //SFX
            readerLanceDe = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Dice Roll.mp3");
            waveOutLanceDe.Init(readerLanceDe);
            waveOutLanceDe.Play();

            await Task.Delay(2000); //On attend 2s c'est le temps de l'animation

            //Dans le cas ou le joueur 1 possède la tour
            if (game.playerList[0].hasTour)
            {
                await TourRadioActivation(); //On demande au joueur s'il veut relancer les dés
                if (RelanceDe) //Si le joueur veut relancer les dés
                {
                    //Même code que ci-dessus
                    if (game.NombreDe == 1)
                    {
                        diceScore2 = 0;
                        game.scoreDes = game.die.Lancer();
                        diceScore1 = game.scoreDes;
                        DiceAnim(game.scoreDes, 0);
                        historique += "\nScore relancé : " + game.scoreDes;
                    }
                    else
                    {
                        diceScore1 = game.die.Lancer();
                        diceScore2 = game.die.Lancer();
                        game.scoreDes = diceScore1 + diceScore2;
                        DiceAnim(diceScore1, diceScore2);
                        historique += "\nScore relancé : " + diceScore1 + " + " + diceScore2 + " = " + game.scoreDes;
                    }
                    readerLanceDe = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/Dice Roll.mp3");
                    waveOutLanceDe.Init(readerLanceDe);
                    waveOutLanceDe.Play();
                }
            }

            await UpdateGame(game.scoreDes, 1); //Update des scores en fonction de l'activation value et de la personne qui a lancé le dé (ici le joueur 1)
            game.DieThrowed = true; //Le dé a été lancé ==> variable permettant de débloquer l'achat de carte
            buyingPhase = true; //En phase d'achat
            UpdateLabels(); //Update des LABELS de score (UpdateGame est une fonction du jeu console, updatelabel permet de prendre les valeurs du jeu backend et le mettre en front)

            if (isEnded().Count != 0) //Après l'update des score, check si nous avons un ou des vainqueurs. Dans le cas où il y a au moins un vainqueurs (donc différent de 0 vainqueur)
            {
                List<int> vainqueurs = isEnded();
                //Affichage des vainqueurs
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
                //On désactive les boutons lancer le dé et passer le tour, la partie est terminée
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                return;
            }
            //Si la partie n'est pas terminée on active le bouton passer le tour 
            this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = true;
        }
        private void buttonFinTour_Click(object sender, EventArgs e) //Event du bouton fin de tour
        {
            game.DieThrowed = false; //On remet le statut du dé lancé à faux (anticipation du prochain tour du joueur)
            //MessageBox.Show("Fin tour");
            Button FinTour = (Button)sender;
            FinTour.Enabled = false; //On désactive le bouton fin de tour
            TourNext(); //On passe au tour du prochain joueur
        }
        private void Menu_Game_Button_OnEnter(object sender, EventArgs e) //Event Entrée de la souris sur le bouton 
        {
            Button button = (Button)sender;
            button.BackColor = Color.DarkBlue; //Changement de couleur
            button.ForeColor = Color.White;
            button.Size = new Size(button.Width + 14, button.Height + 14); //On aggrandit le bouton
            button.Location = new Point(button.Location.X - 7, button.Location.Y - 7); //On recentre le bouton de l'aggrandissement /2
        }
        private void Menu_Game_Button_OnLeave(object sender, EventArgs e) //Event sortie de la souris du bouton
        {
            Button button = (Button)sender;
            button.BackColor = Color.LightSteelBlue; //Changement de couleur
            button.ForeColor = Color.Black;
            button.Size = new Size(button.Width - 14, button.Height - 14); //Diminution de la taille
            button.Location = new Point(button.Location.X + 7, button.Location.Y + 7); //Recentrage
        }
        private void Menu_Game_Button_EnabledChanged(object sender, EventArgs e) // Event Changement de couleur lors de l'activation / Désactivation d'un bouton
        {
            Button button = (Button)sender;
            button.BackColor = button.Enabled == false ? Color.LightSlateGray : Color.LightSteelBlue;
        }
        #endregion
        private async void StartGame() //Début du jeu visuellement
        {
            await Task.Delay(1000); //Petit temps d'attente
            Controls.Clear(); //On Clear tout ce qui a dans le form (typiquement les controls du Menu Start)

            //On remet en variable la taille du board
            BoardSizeHeight = this.ClientSize.Height;
            BoardSizeWidth = this.ClientSize.Width - 250;

            //On stoppe la musique de fond de départ et on met celle In Game
            if (activeMusicBackGround != null) activeMusicBackGround.Stop();
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/InGame.wav");
            activeMusicBackGround.PlayLooping();

            game = new Game(nbJoueur); //Initialisation du jeu
            ZoomCardStart(); //Les cartes en gros quand on Hover
            AddMenu(); //Le menu à droite
            Time.Interval = 3; //Définition de l'interval en MS pour la distribution de carte
            DistributionCard_StartGame(); //Animation de distribution

            //TesteurFonction(); // Fonction pour le debugging et play test des scénarios

        }
        private async Task UpdateGame(int scoreDes, int tourJoueur) //Fonction pour Update des scores (anciennement dans la classe game mais re implémenté dans la classe form pour polus de maniabilité des controls)
        {
            for (int i = 0; i < game.playerList.Count; i++) //Pour chaque joueur dans le jeu
            {
                Player player = game.playerList[i];
                for (int j = 0; j < player.CarteAcquises.Count; j++) // Pour chaque carte que le joueur possède
                {
                    if (player.CarteAcquises[j].ActivationValue[0] == scoreDes || (player.CarteAcquises[j].ActivationValue.Length > 1 && player.CarteAcquises[j].ActivationValue[1] == scoreDes)) //Si l'activation value corresponds au score du/des dé(s)
                    {
                        if (player.CarteAcquises[j].Color == CouleurCarte.Bleu) //Si la carte est bleue
                        {
                            player.Pieces += player.CarteAcquises[j].Gain; //On ajoute les gains car activation pour tous les tours
                        }
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Vert && tourJoueur == (i + 1)) //Si la carte est verte et que le tour correspond au joueur actuel
                        {
                            if (player.CarteAcquises[j].Name == NomCarte.Fromagerie) //Si la carte est la fromagerie
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++) //On itère à travers les cartes pour savoir si ils sont du même type que la ferme (type 2 dans notre cas)
                                {
                                    if (player.CarteAcquises[p].Type == 2) count++;
                                }
                                player.Pieces += player.CarteAcquises[j].Gain * count; //Le joueur gagne alors le gain * le nombre de carte de ce type qu'il possède
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Fabrique) //Si la carte est la fabrique de meuble
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++) //idem pour le type 5
                                {
                                    if (player.CarteAcquises[p].Type == 5) count++;
                                }
                                player.Pieces += player.CarteAcquises[j].Gain * count;
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Marche) //Si la carte est le marché aux fruits et légumes
                            {
                                int count = 0;
                                for (int p = 0; p < player.CarteAcquises.Count; p++) //idem pour le type 1
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
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Rouge && tourJoueur != 1) //Si la carte est rouge et que ce n'est pas le tour du joueur
                        {
                            if (player.hasCentre && player.CarteAcquises[j].Type == 4) //Si le joueur possède un centre commercial et que la carte est du type restaurant (type 4) les gains sont +1 pour ce joueur et cette carte
                            {
                                player.Pieces += Math.Min(game.playerList[tourJoueur - 1].Pieces, player.CarteAcquises[j].Gain + 1); // On ne peut pas voler plus que ce le joueur visé possède
                                game.playerList[tourJoueur - 1].Pieces = Math.Max(0, game.playerList[tourJoueur - 1].Pieces - player.CarteAcquises[j].Gain - 1); //On ne peut pas être en sous négatif / endetté
                            }
                            else //Si pas de centre commercial 
                            {
                                player.Pieces += Math.Min(game.playerList[tourJoueur - 1].Pieces, player.CarteAcquises[j].Gain);
                                game.playerList[tourJoueur - 1].Pieces = Math.Max(0, game.playerList[tourJoueur - 1].Pieces - player.CarteAcquises[j].Gain);
                            }

                        }
                        else if (player.CarteAcquises[j].Color == CouleurCarte.Violet && tourJoueur == (i + 1)) //Si La carte est violette et que c'est le tour du joueur
                        {
                            if (player.CarteAcquises[j].Name == NomCarte.Stade) //Si c'est un stade
                            {
                                for (int p = 0; p < game.playerList.Count; p++) //Pour tous les autres joueurs
                                {
                                    if (p != i)
                                    {
                                        player.Pieces += Math.Min(game.playerList[p].Pieces, player.CarteAcquises[j].Gain); //Le joueur gagne +2 ou ce qui reste de pièce du joueur ciblé
                                        game.playerList[p].Pieces = Math.Max(0, game.playerList[p].Pieces - player.CarteAcquises[j].Gain); //Le joueur gagne -2 ou ce qui lui reste de pièce
                                    }
                                }
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Television) //Si c'est une télévision 
                            {
                                if (tourJoueur == 1) // Si c'est le tour du joueur principal
                                {
                                    await TelevisionActivation(); //On attend la tâche Television action ==> Sélectionne le joueur ciblé qui permet de changer la valeur de TargetTelevision
                                    player.Pieces += Math.Min(game.playerList[TargetTelevision - 1].Pieces, player.CarteAcquises[j].Gain); //On vole au TargetTelevision les pièces
                                    game.playerList[TargetTelevision - 1].Pieces = Math.Max(0, game.playerList[TargetTelevision - 1].Pieces - player.CarteAcquises[j].Gain);
                                    UpdateLabels(); //Update des labels juste après le vol
                                }
                                else  //Ici l'IA choisit sa cible aléatoirement (à modif pour une IA encore plus développée)
                                {
                                    int Target = random.Next(0, game.playerList.Count);
                                    while (Target == i) //tant qu'il se cible lui même (quel con aussi)
                                    {
                                        Target = random.Next(0, game.playerList.Count);
                                    }
                                    player.Pieces += Math.Min(game.playerList[Target].Pieces, player.CarteAcquises[j].Gain);
                                    game.playerList[Target].Pieces = Math.Max(0, game.playerList[Target].Pieces - player.CarteAcquises[j].Gain);
                                }
                            }
                            else if (player.CarteAcquises[j].Name == NomCarte.Affaire) //Si la carte est le centre d'affaire
                            {
                                if (tourJoueur == 1) //Si c'est le joueur principal
                                {
                                    await CentreAffaireActivation(); //Tache CentreAffaire qui va définir le joueur ciblé (TargetCentre), la carte à recevoir (CarteRecue), la carte à échanger (CarteEnvoyee)
                                    bool[] changePlayer = game.playerList[i].TradingChange(CarteRecue, CarteEnvoyee); //booléen permettant de savoir les changements dans l'affichage => array de dimension 2, bool[0] => une carte à ajouter, bool[1] => une carte à supprimer
                                    bool[] changeTarget = game.playerList[TargetCentreAffaire - 1].TradingChange(CarteEnvoyee, CarteRecue); //même booléen mais pour notre cible
                                    ChangeDisplayAfterExchange(0, changePlayer[0], changePlayer[1], CarteEnvoyee, CarteRecue); //On modifie l'affichage selon notre booléen définie precedemment
                                    ChangeDisplayAfterExchange(TargetCentreAffaire - 1, changeTarget[0], changeTarget[1], CarteRecue, CarteEnvoyee); //idem pour notre cible
                                }
                                else  //A dev (franchement la flemme pour le moment on fait comme si l'IA ne veut jamais échanger)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
        private void TesteurFonction() //Bouton pour faire des tests / debugging. Ici notre bouton ajoute 100 gold à tout le monde
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
                    game.playerList[i].Pieces += 10;
                }

            });

            Controls.Add(Test);
            Test.BringToFront();
        }
        private async void TourNext() //Passage au prochain tour
        {
            //Si le joueur du tour actuel possède le monument parc et a fait un double, alors il va rejouer. Sinon on augmente la variable tourjoueur de 1 (modulo le nb de joueur pour boucler)
            buyingPhase = false;
            if (!(game.playerList[game.tourJoueur - 1].hasParc && diceScore1 == diceScore2)) game.tourJoueur = game.tourJoueur % nbJoueur + 1;
            diceScore2 = 0;
            historique = "Tour du joueur " + game.tourJoueur;
            if (game.tourJoueur == 1) //Si le tour du joueur est celui du joueur principal, on met le label du tour du joueur précédent en noir
            {
                if (nbJoueur == 4) { TourJoueur4.ForeColor = Color.Black; HistoriqueLabel4.ForeColor = Color.Black; }
                if (nbJoueur == 3) {TourJoueur3.ForeColor = Color.Black; HistoriqueLabel3.ForeColor = Color.Black; }
                if (nbJoueur == 2) {TourJoueur2.ForeColor = Color.Black; HistoriqueLabel2.ForeColor = Color.Black; }
                TourJoueur1.ForeColor = Color.Yellow; // On met le label du joueur actuel en jaune
                HistoriqueLabel1.ForeColor = Color.Yellow;
                this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = true; //On active le bouton lancer de dé
                return;
            }
            else //Si c'est le tour d'un joueur IA, on met l'IA en jaune et le joueur précédent en  noir
            {
                if (game.tourJoueur == 2) 
                {
                    TourJoueur1.ForeColor = Color.Black;
                    TourJoueur2.ForeColor = Color.Yellow;

                    HistoriqueLabel1.ForeColor = Color.Black;
                    HistoriqueLabel2.ForeColor = Color.Yellow;
                }
                if (game.tourJoueur == 3)
                {
                    TourJoueur2.ForeColor = Color.Black;
                    TourJoueur3.ForeColor = Color.Yellow;

                    HistoriqueLabel2.ForeColor = Color.Black;
                    HistoriqueLabel3.ForeColor = Color.Yellow;
                }
                if (game.tourJoueur == 4)
                {
                    TourJoueur3.ForeColor = Color.Black;
                    TourJoueur4.ForeColor = Color.Yellow;

                    HistoriqueLabel3.ForeColor = Color.Black;
                    HistoriqueLabel4.ForeColor = Color.Yellow;
                }


                bool hasBuy = false; //Initialisation du booléen pour les différentes priorités d'achat de l'IA
                #region Tour de l'IA

                game.NombreDeIA = 1; //On remet le nombre de dé à 1 à chaque nouveau tour de IA

                if (game.playerList[game.tourJoueur - 1].hasGare) game.NombreDeIA = random.Next(1, 3); //Si l'IA possède la gare, il choisit aléatoirement entre 1 ou 2 dés
                if (game.NombreDeIA == 1) //Lancé de 1 dé
                {
                    game.scoreDes = game.die.Lancer();
                    diceScore1 = game.scoreDes;
                    DiceAnim(game.scoreDes);
                    historique += "\nScore lancé : " + game.scoreDes;
                }
                else //Lancé de 2 dés
                {
                    diceScore1 = game.die.Lancer();
                    diceScore2 = game.die.Lancer();
                    game.scoreDes = diceScore1 + diceScore2;
                    DiceAnim(diceScore1, diceScore2);
                    historique += "\nScore lancé : " + diceScore1 + " + " + diceScore2 + " = " + game.scoreDes ;
                }

                await Task.Delay(2000); //Petite attente pour les dés
                await UpdateGame(game.scoreDes, game.tourJoueur); //Update des scores du jeu avec le score des dés + le tour du joueur
                UpdateLabels(); //Update des Label pour la fenetre graphique
                if (isEnded().Count != 0) //S'il y a un vainqueur
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
                    if (!game.playerList[game.tourJoueur - 1].hasGare && game.playerList[game.tourJoueur - 1].Pieces >= MonuCost[1]) //Si l'IA n'a pas la gare et qu'il peut l'acheter
                    {
                        hasBuy = true; //il va acheter la carte donc le boléen a true
                        game.playerList[game.tourJoueur - 1].hasGare = true; //booléen hasGare dans game a true (permet de lancer débloquer le lancer de 2 dés)
                        PictureBox MonuToChange; //On récupère la liste des pictureBox du Monument du joueur visé
                        if (game.tourJoueur == 2) MonuToChange = MonuJoueur2[0];
                        else if (game.tourJoueur == 3) MonuToChange = MonuJoueur3[0];
                        else MonuToChange = MonuJoueur4[0];

                        //On trouve la carte zoom et on modifie son image (de image lock à unlock)
                        var ZoomPictureOld = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();
                        MonuToChange.Name = MonuToChange.Name.Remove(5);
                        MonuToChange.ImageLocation = "Images/" + MonuToChange.Name + ".png";
                        var ZoomPictureNew = this.Controls.Find("Zoom" + MonuToChange.Name, true).FirstOrDefault();

                        if (ZoomPictureNew != null) //Pour que l'image ne dépasse pas de l'écran, on check si son coté gauche et sa hauteur soit OK par rapport à la taille de la carte zoomé
                        {
                            if (MonuToChange.Location.X > this.Width - MonuToChange.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(MonuToChange.Location.X - (ZoomPictureNew.Width + 30),
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);
                            else ZoomPictureNew.Location = new Point(MonuToChange.Location.X + MonuToChange.Width + 10,
                                this.Height - MonuToChange.Location.Y > ZoomPictureNew.Height ? MonuToChange.Location.Y : MonuToChange.Location.Y + MonuToChange.Height - ZoomPictureNew.Height);

                            if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                        }
                        if (ZoomPictureOld != null) //On cache l'ancienne image (image locked)
                        {
                            ZoomPictureOld.Visible = false;
                        }

                        game.playerList[game.tourJoueur - 1].Pieces -= MonuCost[1]; //On actualise le score du joueur
                        historique += "\nAchat de la Gare";
                        UpdateLabels(); //Actualisation de l'affichage du score                
                    }

                    //Même chose que ci-dessus mais pour le Centre Commercial --> en terme de prioté il faut d'abord avoir une gare pour que l'IA achete le centre
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
                        historique += "\nAchat du Centre Commercial";
                        UpdateLabels();                       
                    }

                    //Même chose que ci-dessus mais pour la tour radio --> nécessite la gare + le centre commercial
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
                        historique += "\nAchat de la Tour Radio";
                        UpdateLabels();
                    }

                    //Même chose que ci-dessus mais pour le parc d'attraction --> nécessite la gare + le centre commercial + la tour radio
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
                        historique += "\nAchat du Parc d'Attraction";
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
                    int[] CheckCarteAcquises = new int[15]; //Array de longueur 15 permettant de checker les cartes obtenues
                    for (int i = 0; i < game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count; i++) //Si l'IA possède la carte, alors on met un 1 à l'index de l'array. Une IA qui a juste une boulangerie et un champs de blé c'est donc [1,0,1,0,...,0] car boulangerie c'est la carte 3
                    {
                        CheckCarteAcquises[(int)game.playerList[game.tourJoueur - 1].CarteAcquisesUniques[i].Name - 1] = 1;
                    }

                    for (int i = 0; i < 15; i++) //Itération à travers l'array que nous avons crée précedemment
                    {
                        Cards temp; List<PictureBox> tempList = new List<PictureBox>();
                        if (CheckCarteAcquises[i] == 0 && game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces >= temp.Cost) //Si l'IA ne possède pas la carte et qu'il peut l'acheter, alors il l'achète
                        {
                            if (game.tourJoueur == 2)
                            {
                                tempList = CarteJoueur2; //On l'ajoute à notre liste de carte différente et on la positionne au bon endroit selon le joueur
                                if (nbJoueur == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count + 1), i + 1, 2)); //Cas différent pour nbJoueur == 2 car le joueur 2 se place à l'opposé donc en haut à droite
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
                            hasBuy = true; //on met la carte acheté à true pour pas faire acheter à l'IA plusieurs cartes
                            game.playerList[game.tourJoueur - 1].BuyCard(game.CartesDisponibles[i]); //Ajout de la carte dans la partie console + gestion de la monnaie directement dans la fonction
                            if (game.CartesDisponibles[i].PileCartes.Count == 0) //Si l'IA achete la derniere carte de la pile, on ne l'affiche plus
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

                            historique += "\nAchat de la carte " + temp.Name;
                            break; //On sort de la boucle directement si une carte est achetée
                        }
                        else if (CheckCarteAcquises[i] == 0 && game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces < temp.Cost) 
                        {
                            break; //En vrai else if qui ne sert pas à grand chose mais mon IA si elle ne peut pas acheter la 1ere carte non possédée, cela s'arrete
                        }

                    }

                    UpdateLabels(); //Update des label des scores
                    
                    //Check sur la condition de victoire
                    if (game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count == 15 && game.playerList[game.tourJoueur - 1].Pieces >= 20)
                    {
                        //Si victoire alors affichage des joueurs victorieux
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
                        //En cas de fin de partie désactivation des boutons lancer et passer le tours
                        this.Controls.Find("BoutonLancer", true).FirstOrDefault().Enabled = false;
                        this.Controls.Find("BoutonPasserTour", true).FirstOrDefault().Enabled = false;
                        return;
                    }
                }

                #endregion

                #region Achat IA
                if (!hasBuy) //Si l'IA n'a pas acheté de monument ou de carte pour le type de fin 3
                {
                    for (int i = 0; i < game.CartesDisponibles.Count; i++) //L'IA va check les cartes disponibles sur le terrain dans l'ordre
                    {
                        Cards temp; List<PictureBox> tempList = new List<PictureBox>();
                        //Si l'IA a la thune pour acheter une carte et qu'il ne possède pas 5 cartes différentes (limitation arbitrairement choisie par le lead dev) alors il achete la carte
                        if (game.CartesDisponibles[i].PileCartes.TryPeek(out temp) && game.playerList[game.tourJoueur - 1].Pieces >= temp.Cost && game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Count < 5)
                        {
                            if (!game.playerList[game.tourJoueur - 1].CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name)) //Si l'IA ne possède pas la carte dans son deck, on va la faire spawn + ajout dans la liste des cartes de son deck
                            {
                                if (game.tourJoueur == 2) //Condition pour le bon positionnement et ancrage de la carte à spawn en fonction du joueur
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
                            else //Si l'IA possède déjà la carte, on fait +1 au label en haut à droite de la carte
                            {
                                //On choisit le bon deck selon le joueur
                                if (game.tourJoueur == 2) tempList = CarteJoueur2;
                                if (game.tourJoueur == 3) tempList = CarteJoueur3;
                                if (game.tourJoueur == 4) tempList = CarteJoueur4;
                                for (int j = 0; j < tempList.Count; j++) //On itère à travers le deck pour trouver la carte à augmenter
                                {
                                    //Si name.Length == 6 alors c'est un numéro de carte à 1 chiffre, sinon à 2 chiffres. Alternativement on aurait pu faire une concaténation de string + comparaison
                                    if ((tempList[j].Name.Length == 6 && tempList[j].Name[5] - '0' == (i + 1)) || (tempList[j].Name.Length == 7 && (tempList[j].Name[5] - '0') * 10 + (tempList[j].Name[6] - '0') == (i + 1)))
                                    {
                                        Label tempLabel = tempList[j].Controls.OfType<Label>().First(); //Je prends le first car les cartes n'ont qu'un seul label qui est le nombre de cartes empilé
                                        tempLabel.Text = "x" + (tempLabel.Text[1] - '0' + 1); //On ajoute 1 au label
                                        tempLabel.Visible = true; //Je mets le label visible à true (le label x1 est invisible sinon)
                                    }
                                }

                            }
                            game.playerList[game.tourJoueur - 1].BuyCard(game.CartesDisponibles[i]); //Action console d'acheter la carte, cela actualise le score du jeu et les listes de posséssions
                            if (game.CartesDisponibles[i].PileCartes.Count == 0) //Cas ou le joueur achete la derniere carte de la pile, on la rend non visible
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
                            historique += "\nAchat de la carte " + temp.Name;
                            UpdateLabels(); //Update de l'affiche graphique des scores    
                            hasBuy = true;
                            break; //Si une carte est achetée on sort de la boucle directement
                        }
                    }
                }
                #endregion
                UpdateLabels();
                await Task.Delay(1500);
            }

            TourNext();
        }
        private void ZoomCardStart() //Création des cartes zoomées que l'on utilisera tout le long de la partie en cas de Hover
        {
            for (int i = 1; i < 16; i++) //On itère de 1 à 15 pour toutes les cartes normales
            {
                PictureBox picturebox = new PictureBox //Création de la grosse carte 
                {
                    ImageLocation = "Images/Carte" + i + ".png",
                    Visible = false,
                    Size = new Size(400, 600),
                    Name = "ZoomCarte" + i,
                    SizeMode = PictureBoxSizeMode.StretchImage, //Permet d'adapter l'image à la taille de la picturebox 
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                this.Controls.Add(picturebox); //Ajout de la carte au forms
            }
            for (int i = 1; i < 5; i++) //On itère de 1 à 5 pour les monuments
            {
                PictureBox pictureMonument = new PictureBox //Création du monument
                {
                    ImageLocation = "Images/Monu" + i + ".png",
                    Visible = false,
                    Name = "ZoomMonu" + i,
                    Size = new Size(400, 600),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                PictureBox pictureMonumentLocked = new PictureBox //Création du monument mais vérouillé
                {
                    ImageLocation = "Images/Monu" + i + "Locked.png",
                    Visible = false,
                    Name = "ZoomMonu" + i + "Locked",
                    Size = new Size(400, 600),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    WaitOnLoad = true,
                };
                this.Controls.Add(pictureMonument); //ajout de la carte au forms
                this.Controls.Add(pictureMonumentLocked);
            }
        }
        private void Carte_OnHover(object sender, EventArgs e) //Event lors du hover des cartes 
        {
            PictureBox thisPicture = (PictureBox)sender; //on reprend l'object qui envoie l'event et on le cast en PictureBox

            thisPicture.Size = new Size(thisPicture.Width + 20, thisPicture.Height + 20); //On grossit la carte qu'on hover (meilleur effet visuel)
            thisPicture.Location = new Point(thisPicture.Location.X - 10, thisPicture.Location.Y - 10); //On recentre 
            if (thisPicture.Name[0] != 'M') thisPicture.BringToFront(); //Condition pour ne pas changer l'ordre des monuments, sinon on amene la carte en premier plan pour bien la voir
            var ZoomPicture = this.Controls.Find("Zoom" + thisPicture.Name, true).FirstOrDefault(); //On cherche la pictureBoX Zoom (celle qui a été crée dans la fonction ZoomCardStart()
            if (ZoomPicture != null) //Condition pour le debugging mais non nécessaire sinon
            {
                ZoomPicture.Visible = true; //On affiche la carte zoomé, et on la positionne en faisant attention qu'elle ne sorte pas du terrain
                if (thisPicture.Location.X > this.Width - thisPicture.Width - (ZoomPicture.Width + 30)) ZoomPicture.Location = new Point(thisPicture.Location.X - (ZoomPicture.Width + 30),
                    this.Height - thisPicture.Location.Y - 10 > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);
                else ZoomPicture.Location = new Point(thisPicture.Location.X + thisPicture.Width + 10,
                    this.Height - thisPicture.Location.Y - 10 > ZoomPicture.Height ? thisPicture.Location.Y : thisPicture.Location.Y + thisPicture.Height - ZoomPicture.Height);

                if (ZoomPicture.Location.Y < -5) ZoomPicture.Location = new Point(ZoomPicture.Location.X, (this.Height - ZoomPicture.Height) / 2);
                ZoomPicture.BringToFront(); //On la met en 1er plan
            }
        }
        private void Carte_OnLeave(object sender, EventArgs e) //Event lorsque la souris quitte la carte
        {
            PictureBox thisPicture = (PictureBox)sender; //récupération de l'object sender et casting en picture box
            thisPicture.Size = new Size(thisPicture.Width - 20, thisPicture.Height - 20); //On rétrécit la carte à sa taille originelle (on l'avait aggrandit en cas de hover)
            thisPicture.Location = new Point(thisPicture.Location.X + 10, thisPicture.Location.Y + 10); //on recentre la carte
            var ZoomPicture = this.Controls.Find("Zoom" + thisPicture.Name, true).FirstOrDefault(); //On retrouve la carte zoomée pour la rendre invisible
            if (ZoomPicture != null) //Condition mise en place pour le debugging
            {
                ZoomPicture.Visible = false;
            }
        }
        private void Card_OnClick(object sender, EventArgs e) //Event lors d'un clique sur les cartes
        {
            if (!buyingPhase) //Si ce n'est pas la phase d'achat
            {
                MessageBox.Show("Tu ne peux pas acheter de carte pour le moment ! Attends ton tour et lance tes dés.\nNB : On ne peut acheter qu'une seule carte par tour.","Essai d'achat");
                return;
            }

            PictureBox picture = (PictureBox)sender;
            int numberCard = Int32.Parse(picture.Name.Remove(0, 5)); //On prend le numéro de la carte en supprimant les 5 premiers caractères ==> exemple Carte1 -> 1 ; Carte15 -> 15
            
            if (game.CartesDisponibles[numberCard - 1].PileCartes.Peek().Cost > game.playerList[0].Pieces) //Petit trick, le numéro de la carte c'est le numéro de la pile, donc on check si le coût de la carte > au pièces du joueurs
            {
                MessageBox.Show("Tu es trop pauvre pour acheter cette carte !", "Essai d'achat");
                return;
            }

            //Sinon la carte est achetée
            if (!game.playerList[0].CarteAcquisesUniques.Any(Cards => Cards.Name == (NomCarte)numberCard)) //Si le joueur ne possède pas la carte
            {
                //On fait spawn la carte au bon endroit et on l'ajoute à notre liste de picturebox de carte
                CarteJoueur1.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * game.playerList[0].CarteAcquisesUniques.Count, numberCard));
            }
            else //Si le joueur possède la carte, on fait +1 au label pour voir l'effet de stacking
            {
                for (int i = 0; i < CarteJoueur1.Count; i++) //On itère à travers les picturebox du joueur
                {
                    //On retrouve la picturebox qui correspond au numéro de la carte achetée
                    if ((CarteJoueur1[i].Name.Length == 6 && CarteJoueur1[i].Name[5] - '0' == numberCard) || (CarteJoueur1[i].Name.Length == 7 && (CarteJoueur1[i].Name[5] - '0') * 10 + (CarteJoueur1[i].Name[6] - '0') == numberCard))
                    {
                        Label temp = CarteJoueur1[i].Controls.OfType<Label>().First(); //Un seul label par picturebox donc on peut choisir le 1er trouvé
                        temp.Text = "x" + (temp.Text[1] - '0' + 1); 
                        temp.Visible = true; 
                    }
                }
            }
            game.playerList[0].BuyCard(game.CartesDisponibles[numberCard - 1]); //Fonction Achat en console
            buyingPhase = false; //La carte a été acheté donc la phase d'achat s'arrête (le joueur ne peut pas acheter 2 cartes d'affilées)
            if (game.CartesDisponibles[numberCard - 1].PileCartes.Count == 0) //Si c'est la dernière carte de la pile, on cache la pile
            {
                picture.Visible = false;
                picture.Enabled = false;
            };
            historique += "\nAchat de la carte " + (NomCarte)numberCard;
            UpdateLabels(); //Update des labels des scores
        }
        private void MonumentBuy(object sender, EventArgs e) //Event pour l'achat de Monument
        {
            if (!buyingPhase) //Si le joueur ne peut pas acheter (ce n'est pas son tour/il a déjà acheté une carte/il n'a pas lancé les dés)
            {
                MessageBox.Show("Tu ne peux pas acheter de carte pour le moment ! Attends ton tour et lance tes dés.\nNB : On ne peut acheter qu'une seule carte par tour.", "Essai d'achat");
                return;
            }
            var picturebox = (PictureBox)sender;
            int numberMonu = (picturebox.Name)[4] - '0'; //On récupère le numéro du monument
            if (MonuCost[numberMonu] > game.playerList[0].Pieces) //Si le coût du monument est supérieur aux pièces du joueur
            {
                MessageBox.Show("Tu es trop pauvre pour acheter cette carte !", "Essai d'achat");
                return;
            }
            //Sinon la carte est achetée
            var ZoomPictureOld = this.Controls.Find("Zoom" + picturebox.Name, true).FirstOrDefault(); //On retrouve la GROSSE carte LOCKED zoomée
            picturebox.Name = picturebox.Name.Remove(5); //On remove à partir de 5 pour avoir le nom unlocked --> Ex Monu3Locked => Monu3
            picturebox.ImageLocation = "Images/" + picturebox.Name + ".png"; //L'image de la PETITE carte est modifiée en carte unlocked
            var ZoomPictureNew = this.Controls.Find("Zoom" + picturebox.Name, true).FirstOrDefault(); //On retrouve la GROSSE carte UNLOCKED zoomée
            if (ZoomPictureNew != null) //Pour le debugging
            {
                ZoomPictureNew.Visible = true; //La grosse carte unlocked est affichée
                if (picturebox.Location.X > this.Width - picturebox.Width - (ZoomPictureNew.Width + 30)) ZoomPictureNew.Location = new Point(picturebox.Location.X - (ZoomPictureNew.Width + 30),
                    this.Height - picturebox.Location.Y > ZoomPictureNew.Height ? picturebox.Location.Y : picturebox.Location.Y + picturebox.Height - ZoomPictureNew.Height);
                else ZoomPictureNew.Location = new Point(picturebox.Location.X + picturebox.Width + 10,
                    this.Height - picturebox.Location.Y > ZoomPictureNew.Height ? picturebox.Location.Y : picturebox.Location.Y + picturebox.Height - ZoomPictureNew.Height);

                if (ZoomPictureNew.Location.Y < -5) ZoomPictureNew.Location = new Point(ZoomPictureNew.Location.X, (this.Height - ZoomPictureNew.Height) / 2);
                ZoomPictureNew.BringToFront(); //Mise en premier plan
            }
            if (ZoomPictureOld != null) //Debugging
            {
                ZoomPictureOld.Visible = false; //La grosse carte LOCKED en visibilté fausse ( on ne la remove pas du forms dans le pour les autres joueurs qui ne l'ont pas unlock)
                ZoomPictureOld.SendToBack(); 
            }
            picturebox.Click -= new EventHandler(MonumentBuy); //On supprime l'event click, cela évite que le joueur achete plusieurs fois le même monument
            game.playerList[0].Pieces -= MonuCost[numberMonu]; //On actualise les pièces en console
            buyingPhase = false; //La phase d'achet est terminée
            if (numberMonu == 1)  //Si il achète la gare
            {
                game.playerList[0].hasGare = true; //booléen en console
                this.Controls.Find("CheckBoxDe", true).FirstOrDefault().Enabled = true; //Le bouton checkbox devient utilisable pour lancer deux dés
                historique += "\nAchat de la Gare";
            }
            if (numberMonu == 2) { game.playerList[0].hasCentre = true; historique += "\nAchat du Centre Commercial"; }
            if (numberMonu == 3) { game.playerList[0].hasTour = true; historique += "\nAchat de la Tour Radio"; }
            if (numberMonu == 4) { game.playerList[0].hasParc = true; historique += "\nAchat du Parc d'Attraction"; }
            UpdateLabels(); //Update des labels des scores
            if (EndingChoice == 4 && isEnded().Count != 0) //S'il y a un vainqueur (dans le cas où c'est la fin nb 4)
            {
                //Affichage des vainqueurs
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
        private void UpdateLabels() //Fonction pour update les golds des joueurs
        {
            //on prend le texte du label et on le met égal au nb de pièces en console
            MoneyJoueur1.Text = game.playerList[0].Pieces.ToString();
            MoneyJoueur2.Text = game.playerList[1].Pieces.ToString();
            if (nbJoueur >= 3) MoneyJoueur3.Text = game.playerList[2].Pieces.ToString();
            if (nbJoueur >= 4) MoneyJoueur4.Text = game.playerList[3].Pieces.ToString();
            if(game.tourJoueur == 1) HistoriqueLabel1.Text = historique;
            else if (game.tourJoueur == 2) HistoriqueLabel2.Text = historique;
            else if (game.tourJoueur == 3) HistoriqueLabel3.Text = historique;
            else if (game.tourJoueur == 4) HistoriqueLabel4.Text = historique;

        }
        private List<int> isEnded() //Check si quelqu'un à gagner et nous renvoie la liste des vainqueurs
        {
            List<int> Vainqueurs = new List<int>(); //La liste à renvoyer 
            if (EndingChoice == 1) 
            {
                for (int i = 0; i < nbJoueur; i++) //itération dans la liste des joueurs
                {
                    if (game.playerList[i].Pieces >= 20) Vainqueurs.Add(i + 1); //On ajoute l'index du vainqueur s'il a + de 20 pièces
                }
            }
            else if (EndingChoice == 2) 
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].Pieces >= 30) Vainqueurs.Add(i + 1); //On ajoute l'index du vainqueur s'il a + de 30 pièces
                }
            }
            else if (EndingChoice == 3)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].Pieces >= 20 && game.playerList[i].CarteAcquisesUniques.Count == 15) Vainqueurs.Add(i + 1); //On ajoute l'index du vainqueur s'il a + de 20 pièces et qu'il possède 15 cartes uniques (donc toutes les cartes)
                }
            }
            else if (EndingChoice == 4)
            {
                for (int i = 0; i < nbJoueur; i++)
                {
                    if (game.playerList[i].hasCentre && game.playerList[i].hasGare && game.playerList[i].hasParc && game.playerList[i].hasTour) Vainqueurs.Add(i + 1); //On ajoute l'index du vainqueur s'il possède les 4 monuments (4 booléens)
                }
            }
            return Vainqueurs;
        }
        private async Task TelevisionActivation() //Tâche sélection de la cible de l'activation de la carte Télévision
        {
            bool choosed = false; //booléen pour savoir si le joueur à choisi ou non

            //Cadre à afficher pour faire un popup menu
            PictureBox TargetToSteal = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(TargetToSteal);

            //Bouton sélection de la cible joueur 2 
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

            //Label pour le titre du cadre / Instructions à faire
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
            //Ajout des event
            Joueur2.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            Joueur2.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            Joueur2.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            //Si on clique sur joueur 2
            Joueur2.Click += new EventHandler((sender, e) =>
            {
                TargetTelevision = 2; //La cible de la télévisin sera le joueur 2
                choosed = true; //Le joueur à choisi sa cible
                Controls.Remove(TargetToSteal);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            TargetToSteal.Controls.Add(Joueur2);
            
            //Si le nb de joueur est 3 ou +
            if (nbJoueur >= 3)
            {
                Button Joueur3 = new Button
                {
                    Size = new Size(200, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + Joueur2.Height + 20), //Le bouton sera sous le bouton 2
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
            //Si le nb de joueur est 4
            if (nbJoueur == 4)
            {
                Button Joueur4 = new Button
                {
                    Size = new Size(200, 60),
                    Location = new Point(Joueur2.Location.X, Joueur2.Location.Y + 2 * (Joueur2.Height + 20)), //Le bouton sera sous le bouton 3
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
            while (!choosed) //Tant que le joueur n'a pa choisi, on attend (ainsi ce la ne finit pas la tache ==> permet d'attendre avant de continuer dans les autres étapes)
            {
                await Task.Delay(1000);
            }
        }
        private async Task CentreAffaireActivation() //Tâche sélection de la cible de l'activation de la carte Centre affaire + des cartes à échanger
        {
            #region Choix Target
            bool TargetChosen = false; //booléen pour le choix du joueur ciblé

            //Idem que pour le choix de la cible de la television 
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

            //Bouton dans le cas ou le joueur ne veut pas échanger de carte
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
                TargetCentreAffaire = -1; //Mise du target à -1 pour futur condition plus loin dans le code. Si la target est à -1 on annulera l'action
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
            while (!TargetChosen) //Tant que la cible n'a pas été choisi on attend
            {
                await Task.Delay(1000);
            }

            #endregion

            if (TargetCentreAffaire == -1) //Si la cible choisie est -1 cela veut dire que le joueur ne souhaite pas échanger de carte. On met les carte recue et envoyée à 0, cela permet de ne pas échanger de carte
            {
                TargetCentreAffaire = 2;
                CarteRecue = 0;
                CarteEnvoyee = 0;
                return;
            }

            #region Choix notre Carte
            bool CardSelfChosen = false; //booléen pour le choix de carte que l'on va donner 

            //Cadre de sélection
            PictureBox CardToGive = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(CardToGive);

            //Titre / Instruction du Cadre
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

            int index = 0; //Index nous permet de bien placer la carte dans le cadre. Nécessaire étant donné l'itération ignorant certaines cartes exclues
            for (int i = 0; i < game.playerList[0].CarteAcquisesUniques.Count; i++) //On itère à travers les cartes uniques du joueur principale
            {
                int numberCard = (int)game.playerList[0].CarteAcquisesUniques[i].Name;
                if (numberCard == 7 || numberCard == 8 || numberCard == 9) continue; //On ignore les cartes 7, 8 et 9 qui sont les cartes exclues des échanges
                //On crée une pictureBox pour afficher la carte disponible à donner
                PictureBox CarteToExchange = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    ImageLocation = "Images/Carte" + numberCard + ".png",
                    Location = new Point((index % 7 + 1) * CardWidth, GiveCardText.Location.Y + 40 + CardHeight * (index / 7)), //On peut placer 7 cartes en lignes, d'où l'utilisation d'index
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte" + numberCard,
                    Anchor = AnchorStyles.None,
                    WaitOnLoad = true,
                };
                index++; 
                //Ajout des events pour les petites cartes (très similaires aux events de carte normale)
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

                //Event click
                CarteToExchange.Click += new EventHandler((sender, e) =>
                {
                    CardSelfChosen = true; //La carte a été choisie 
                    if (CarteToExchange.Name.Length == 6) CarteEnvoyee = CarteToExchange.Name[5] - '0'; //On récupère le numéro de la carte choisie 
                    else CarteEnvoyee = (CarteToExchange.Name[5] - '0') * 10 + (CarteToExchange.Name[6] - '0');
                    Controls.Remove(CardToGive);
                });

                CardToGive.Controls.Add(CarteToExchange);
            }

            CardToGive.BringToFront();

            //Tant que la carte n'a pas été choisie, on attend
            while (!CardSelfChosen)
            {
                await Task.Delay(1000);
            }
            #endregion

            #region Choix Carte adversaire
            //Même code que pour le choix de ntore propre carte, mais affichage du deck selon le joueur ciblé et assignation à une variable différente pour la carte à recevoir
            bool CardChosen = false; //booléen pour choisir la carte à recevoir

            //Cadre
            PictureBox CardToReceive = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(CardToReceive);
            //Titres / Instructions
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
            //On itère à travers les cartes uniques du joueur ciblé (même façon que pour notre carte à envoyée)
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
        private async Task TourRadioActivation() //Tâche sélection de la cible de l'activation de la carte Tour Radio
        {
            bool choosed = false; //booléen pour la condition de sortie une réponse choisie

            //Cadre
            PictureBox YesNo = new PictureBox
            {
                Location = new Point(BoardSizeWidth / 4, BoardSizeHeight / 4 - 30),
                Size = new Size(BoardSizeWidth / 2, BoardSizeHeight / 2 + 30),
                BackColor = Color.Gray,
                Anchor = AnchorStyles.None,
            };
            Controls.Add(YesNo);

            //On récupère l'image du score des anciens dés
            PictureBox OldScore1 = new PictureBox
            {
                Size = new Size(100, 100),
                ImageLocation = @"Images\Dice" + diceScore1 + ".png",
                Location = new Point(5, YesNo.Height / 2 - 100),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Anchor = AnchorStyles.None,
            };
            YesNo.Controls.Add(OldScore1);

            //Si on a lancé deux dés on récupère l'image du 2e dés également
            if (game.NombreDe == 2)
            {
                PictureBox OldScore2 = new PictureBox
                {
                    Size = new Size(100, 100),
                    ImageLocation = @"Images\Dice" + diceScore2 + ".png",
                    Location = new Point(5, OldScore1.Location.Y + OldScore1.Height + 10), //On place le dé sous le 1er
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Anchor = AnchorStyles.None,
                };
                YesNo.Controls.Add(OldScore2);
            }

            //Bouton Oui pour relancer
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

            //Titres / Instructions 
            Label TextRelance = new Label
            {
                Size = new Size(400, 30),
                Text = "Veux-tu relancer le(s) dé(s) ?",
                Location = new Point(YesNo.Width / 3, YesButton.Location.Y - 80), //Placement au dessus du bouton oui
                Font = new Font("COMIC SANS MS", 12f),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top,
            };


            YesNo.Controls.Add(TextRelance);
            //Ajout des event
            YesButton.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            YesButton.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            YesButton.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            YesButton.Click += new EventHandler((sender, e) =>
            {
                RelanceDe = true; //Si oui on relance les dés
                choosed = true; //booléen à true
                Controls.Remove(YesNo); //On ferme le cadre
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            YesNo.Controls.Add(YesButton);

            //Bouton non
            Button NoButton = new Button
            {
                Size = new Size(200, 60),
                Location = new Point(YesButton.Location.X, YesButton.Location.Y + YesButton.Height + 20), //Placement sous le bouton oui
                Text = "Non",
                Anchor = AnchorStyles.None,
                BackColor = Color.White,
                Font = new Font("COMIC SANS MS", 12f),
                ForeColor = Color.DarkBlue,
            };

            //event bouton non
            NoButton.MouseEnter += new EventHandler(MenuStart_Button_OnEnter);
            NoButton.MouseHover += new EventHandler(MenuStart_Button_OnHover_SFX);
            NoButton.MouseLeave += new EventHandler(MenuStart_Button_OnLeave);
            NoButton.Click += new EventHandler((sender, e) =>
            {
                RelanceDe = false; //On ne relance pas les dés
                choosed = true; //réponse choisie
                Controls.Remove(YesNo);
            }) + new EventHandler(MenuStart_Button_Click_SFX);
            YesNo.Controls.Add(NoButton);

            YesNo.BringToFront();
            while (!choosed) //tant que le joueur ne choisit pas on attend
            {
                await Task.Delay(1000);
            }

        }
        #endregion
        #region Affichage / Animation / VFX
        private void DiceAnim(int DiceScore1, int DiceScore2 = 0) //Animation pour le lancement de dé
        {
            Timer Time = new Timer(); 
            Time.Interval = 50; 
            //Totaltime ==> le temps du défilement des dés random, elapsedtime ==> check le temps passé, finaldisplaytime - totalTIme = le temps à montrer le résultat des dés 
            float totalTime = 1000, elapsedTime = 0, finalDisplayTime = 1200; 

            //Image du dé
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

            //Si on a un 2e dé, on décale le 1er dé et on ajoute un 2e dé
            if (DiceScore2 != 0)
            {
                pictureDice.Location = new Point(BoardSizeWidth / 2 - 100, BoardSizeHeight / 2 - 3 * (70 + 5) / 2 - 125); //Décalage
                //2e dé
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
                    if (elapsedTime > totalTime + finalDisplayTime) //Si le temps passé est supérieur au temps total de défilement + le temps de l'affichage du résultat
                    {
                        pictureDice2.Visible = false; //alors on cache le dé
                    }
                    else if (elapsedTime > totalTime) //Si le temps dépasse le temps total de défilement
                    {
                        pictureDice2.ImageLocation = @"Images\Dice" + DiceScore2 + ".png"; //On affiche le résultat
                    }
                    else //Si le temps passé est inférieur au temps de défilement
                    {
                        pictureDice2.ImageLocation = @"Images\Dice" + random.Next(1, 7) + ".png"; //on affiche tout les time.interval une nouvelle image random de dé
                    }
                });
            }
            //Idem que pour le tick du dés 2 mais on ajoute le Stop et l'augmentation de l'elapsedtime
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime + finalDisplayTime)
                {
                    pictureDice.Visible = false;
                    Time.Stop(); //Arrete le timer
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
                elapsedTime += Time.Interval; //augmentation du temps passé de time.interval
            });
            Time.Start();
        }
        private void Button_Enter_Anim(Button button) //Animation pour l'apparition des boutons (croissance de la taille)
        {
            int originalX = button.Size.Width, originalY = button.Size.Height; //on récupère la taille finale voulue des boutons (ou taille d'origine du coup)
            button.Location = new Point(button.Location.X + originalX / 2 - 1, button.Location.Y + originalY / 2 - 1);
            button.Size = new Size(1, 1);
            Timer Time = new Timer(); //Setting du timer
            Time.Interval = 5;
            float totalTime = 50, elapsedTime = 0; //On va voir 10 ticks
            Time.Tick += new EventHandler((sender, e) =>
            {
                if (elapsedTime > totalTime) //Une fois les 10 ticks passés stop
                {
                    Time.Stop();
                }
                else //A chaque tick on augmente la taille de 1/11e et on déplace la localisation pour centré (centré lors d'un aggrandissement se fait en diagonale donc c'est l'aggrandissement /2)
                {
                    button.Location = new Point(button.Location.X - originalX / 22, button.Location.Y - originalY / 22);
                    button.Size = new Size(button.Size.Width + originalX / 11, button.Size.Height + originalY / 11);
                }
                elapsedTime += Time.Interval;
            });
            Time.Start();
        }
        private void Button_Leave_Anim(Button button) //Animation pour la disparition des boutons (décroissance de la taille)
        {
            //Même principe que pour l'animation d'apparition des boutons mais on réduit la taille plutot que de l'aggrandir
            button.Enabled = false;
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
        private PictureBox Spawn_Card(int height, int width, int numberCard, int NumeroJoueur = 1) //Fonction pour faire apparaître une carte spéicifique à l'endroit donné et retourne la picturebox créee, en l'ancrant au bon joueur
        {
            //Création de la carte
            PictureBox picture = new PictureBox
            {
                Size = new Size(CardWidth, CardHeight),
                BackColor = Color.Transparent,
                Location = new Point(width, height), //On place la carte à l'endroit voulu
                ImageLocation = "Images/Carte" + numberCard + ".png", //La carte voulue
                SizeMode = PictureBoxSizeMode.StretchImage,
                Name = "Carte" + numberCard,
                WaitOnLoad = true,
            };
            //On ajoute à la carte son label (initialisé à x1 quand on spawn une nouvelle carte, logique)
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

            //Ajout des event pour le hover (aggrandissement et centre)
            picture.MouseEnter += new EventHandler(Carte_OnHover);
            picture.MouseLeave += new EventHandler(Carte_OnLeave);


            if (nbJoueur == 2) //Si le nombre joueur est à 2
            {
                if (NumeroJoueur == 1) //Joueur 1 => ancrage en bas à gauche (BON OK je l'ai mis dans tous les IF j'aurais pu le sortir celui là)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2) //Joueur 2 => ancrage en haut à droite. On réadapte également la position de la carte
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 3) //Si 3 joueurs
            {
                if (NumeroJoueur == 1) //Joueur 1 => ancrage en bas à gauche
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2) //Joueur 2 => ancrage en haut à gauche
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3) //Joueur 3 => ancrage en haut à droite
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            else if (nbJoueur == 4) // Si 4 joueurs
            {
                if (NumeroJoueur == 1) //Joueur 1 => ancrage en bas à gauche
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                }
                else if (NumeroJoueur == 2) //Joueur 2 => ancrage en haut à gauche
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    height = BoardSizeHeight - picture.Height - height;
                }
                else if (NumeroJoueur == 3) //Joueur 3 => ancrage en haut à droite
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    height = BoardSizeHeight - picture.Height - height;
                    width = BoardSizeWidth - picture.Width - width;
                }
                else if (NumeroJoueur == 4) //Joueur 4 => ancrage en bas à droite
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                    width = BoardSizeWidth - picture.Width - width;
                }
            }
            this.Controls.Add(picture);
            return picture;
        }
        private void Move_Cards(int height, int width, List<PictureBox> CarteList, int index, int NumeroJoueur = 0) //Animation du déplacement d'une carte => utilisée uniquement dans la distribution en début de partie
        {
            PictureBox picture = CarteList[index];
            //Placement de la carte avec Ancrage au bon joueur
            //Les placements de base sont pour le joueur 1, en cas d'autre joueur height et width est modifiée selon le nb de joueur
            if (nbJoueur == 2)
            {
                if (NumeroJoueur == 1)
                {
                    picture.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; //Ancrage en bas à gauche (quoi qu'il arrive en fait)
                }
                else if (NumeroJoueur == 2) //Joueur 2
                {
                    picture.Anchor = AnchorStyles.Top | AnchorStyles.Right; //Ancrage en bas à droite
                    height = BoardSizeHeight - picture.Height - height; //On réadapte l'emplacement
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

            //Variable direction pour obtenir le nombre à incrémenter pour envoyer la carte au bon endroit (A SAVOIR QUE chaque carte sera spawn au même endroit avant distribution => au milieu en bas de l'écran)
            int directionY = height - picture.Location.Y, directionX = width - picture.Location.X;
            float totalTime = 15, elapsedTime = 0;
            EventHandler handler = null;
            handler = (sender, e) =>
            {
                if (elapsedTime > totalTime)
                {
                    Time.Tick -= handler; //Lorsqu'on arrive au bout du timer, on enleve l'event
                }
                else
                {
                    picture.Location = new Point(picture.Location.X + directionX / 6, picture.Location.Y + directionY / 6); //A chaque tick on envoie la carte de 1/6 du chemin
                }
                elapsedTime += Time.Interval;
            };
            Time.Tick += handler;
            Time.Start();
        }
        private async void DistributionCard_StartGame() //Animation de distribution des cartes
        {
            //Spawn des cartes sur le terrain (les 15 cartes différentes)
            CarteBoardList = new List<PictureBox>();
            for (int CarteNom = 1; CarteNom < 16; CarteNom++)
            {
                PictureBox picture = new PictureBox
                {
                    Size = new Size(CardWidth, CardHeight),
                    BackColor = Color.Transparent,
                    Location = new Point(BoardSizeWidth / 2 - CardWidth / 2, BoardSizeHeight - CardHeight), //Toute les cartes partiront du milieu du bas de l'écran
                    ImageLocation = "Images/Carte" + CarteNom + ".png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Name = "Carte" + CarteNom,
                    Tag = "Pile" + CarteNom,
                    Anchor = AnchorStyles.None,
                };

                //Ajout des events
                picture.Click += new EventHandler(Card_OnClick);
                picture.MouseEnter += new EventHandler(Carte_OnHover);
                picture.MouseLeave += new EventHandler(Carte_OnLeave);
                CarteBoardList.Add(picture);
                this.Controls.Add(picture);
            }
            //Animation distribution des cartes 
            for (int i = 0; i < 15; i++) 
            {
                //5 cartes par colonnes
                Move_Cards(BoardSizeHeight / 2 - (CardHeight / 2 * 3) + CardHeight * (i / 5), BoardSizeWidth / 2 - (CardWidth / 2 * 5) + CardWidth * (i % 5), CarteBoardList, i); 
                await Task.Delay(50);
            }
            //Pour chaque joueur, 4 monuments, champs de blé et boulangerie + image des golds
            for (int i = 1; i <= nbJoueur; i++)
            {
                List<PictureBox> JoueurMonumentList = new List<PictureBox>();
                List<PictureBox> JoueurStartCardList = new List<PictureBox>();
                #region Monument
                for (int j = 1; j < 5; j++)
                {
                    //Ajout des monuments Locked
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
                //Ajout des event
                pictureChamps.MouseEnter += new EventHandler(Carte_OnHover);
                pictureChamps.MouseLeave += new EventHandler(Carte_OnLeave);
                pictureBoulang.MouseEnter += new EventHandler(Carte_OnHover);
                pictureBoulang.MouseLeave += new EventHandler(Carte_OnLeave);
                JoueurStartCardList.Add(pictureChamps);
                this.Controls.Add(pictureChamps);
                JoueurStartCardList.Add(pictureBoulang);
                this.Controls.Add(pictureBoulang);

                //On assigne aux variables du jeux afin de pouvoir repiocher dedans au cours de la partie (utile pour récupérer les bonnes picturesbox sans passé par des find controls)
                if (i == 1) { CarteJoueur1 = JoueurStartCardList; MonuJoueur1 = JoueurMonumentList; }
                else if (i == 2) { CarteJoueur2 = JoueurStartCardList; MonuJoueur2 = JoueurMonumentList; }
                else if (i == 3) { CarteJoueur3 = JoueurStartCardList; MonuJoueur3 = JoueurMonumentList; }
                else if (i == 4) { CarteJoueur4 = JoueurStartCardList; MonuJoueur4 = JoueurMonumentList; }
                #endregion
                #region Animation Distribution
                //Animation des monuments
                for (int j = 0; j < 4; j++) //Pas mis directement dans la boucle précédente car pour des soucis de fluidité des images on préfère d'abord les afficher, puis les déplacer (au lieu de faire en même temps)
                {
                    Move_Cards(BoardSizeHeight - 220, 5 + j * 30, JoueurMonumentList, j, i);
                    await Task.Delay(50);
                }
                //Animation champs de blé et boulangerie
                Move_Cards(BoardSizeHeight - CardHeight - 10, 5, JoueurStartCardList, 0, i);
                await Task.Delay(50);
                Move_Cards(BoardSizeHeight - CardHeight - 10, 5 + 5 + CardWidth, JoueurStartCardList, 1, i);

                #endregion
                //Positionnement des Sacoches à gold  + Nom joueurs
                #region Image GOLD 

                //Image de la sacoche à or
                PictureBox Gold = new PictureBox
                {
                    Size = new Size(CardWidth + 15, CardHeight + 15),
                    ImageLocation = "Images/Gold.png",
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                //Placement de la sacoche selon le joueur
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
                    if (this.nbJoueur == 2) //Toujours le cas où s'il y a que deux joueurs ou 3+
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

                //Label du nom des joueurs
                Label NomJoueur = new Label
                {
                    Size = new Size(100, 30),
                    Text = "Joueur " + i,
                    Font = new Font("COMIC SANS MS", 12f),
                };

                //Placement des noms + ancrages selon le joueur
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
        private async void EndGameCredit() //Animation crédit et fin de jeu
        {
            Controls.Clear(); //Clear de tous les controls de la forms
            //Activation de la musique du fond pour le endgame
            activeMusicBackGround = new SoundPlayer("Sound Design & SFX/Carefree.wav");
            activeMusicBackGround.PlayLooping();
            Font FontName = new Font(new FontFamily("Verdana"), 10);

            //Image du thank you (ouais c'est une image c'est pas avec un du text ou label)
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

            //Premier gif de la danse fortnite
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

            //On peint le nom en haut du gif
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
                    Fornite1.Location = new Point(Fornite1.Location.X + 20, Fornite1.Location.Y); //On fait déplacer le gif de gauche à droite tous les tick
                }
            });
            Time1.Start();
            //tant que le gif du haut n'est pas arrivé à destination on attend
            while (Time1.Enabled == true) await Task.Delay(1000);

            //Idem mais 2e gif et déplacement de droite à gauche
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
                e.Graphics.DrawString("DIANE", FontName, Brushes.Black, 0, 40);
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
            //Tant que le 2e gif n'est pas arrivé à destination on attend
            while (Time2.Enabled == true) await Task.Delay(1000);

            //Idem mais déplacement gauche à droite
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
            //Tant que le gif précédent n'est pas arrivé à destination on attend
            while (Time3.Enabled == true) await Task.Delay(1000);

            //Idem déplacement droite à gauche
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
            readerButtonHover = new NAudio.Wave.Mp3FileReader("Sound Design & SFX/SUIII.mp3");
            waveOutButtonHover.Init(readerButtonHover);
            waveOutButtonHover.Play();
            //On remet leur pos ensemble à 4 pour la fin
            Fornite1.Location = new Point(this.Width / 2 - (Fornite1.Width * 2), Fornite1.Location.Y);
            Fornite2.Location = new Point(this.Width / 2 - Fornite2.Width, Fornite2.Location.Y);
            Fornite3.Location = new Point(this.Width / 2, Fornite3.Location.Y);
            Fornite4.Location = new Point(this.Width / 2 + Fornite4.Width, Fornite4.Location.Y);
            await Task.Delay(8000); //Petite attente de 8s pour profiter des crédits
            Application.Exit(); //Sortie systeme
        }
        private void ChangeDisplayAfterExchange(int indexPlayer, bool toAdd, bool toRemove, int CarteTobeRemoved, int CarteToBeReceived) //Permet d'ajouter, supprimer ou substituer des cartes lors de l'activation de la carte centre affaire
        {
            //On récupère la liste des picturebox du joueur
            List<PictureBox> tempList = new List<PictureBox>();
            if (indexPlayer == 0) tempList = CarteJoueur1;
            if (indexPlayer == 1) tempList = CarteJoueur2;
            if (indexPlayer == 2) tempList = CarteJoueur3;
            if (indexPlayer == 3) tempList = CarteJoueur4;

            if (CarteTobeRemoved == CarteToBeReceived) return; //Il ne se passe rien dans ce cas
            if (toAdd && toRemove) //Si on ajoute et on enleve une carte ==> on fait l'action de supprimer + ajouter (voir les commentaires ci-dessous)
            {
                Point LocationToRemove = new Point();
                int Maxwidth = tempList[0].Location.X, indexToMove = 0, indexToRemove = 0;
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (indexPlayer == 0 || (indexPlayer == 1 && nbJoueur > 2)) //Si c'est le joueur 1 ou alors le joueur 2 (dans le cas 3+ joueurs)
                    {
                        if (Maxwidth < tempList[i].Location.X) //Cette condition permet de trouver la carte la plus à droite de son deck
                        {
                            indexToMove = i; //On récupère l'index de la carte la plus à droite
                            Maxwidth = tempList[i].Location.X;
                        }
                    }
                    else //Sinon on check la carte la plus à gauche de son deck
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

                if (indexPlayer == 0) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived));
                else if (indexPlayer == 1 && nbJoueur == 2) tempList.Add(Spawn_Card(9, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 2));
                else if (indexPlayer == 1) tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 3));
                else if (indexPlayer == 3) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 4));
            }
            else if (toAdd && !toRemove) //Si on ajoute une carte il suffit de faire spawn une carte à la suite des cartes déjà existentes
            {
                if (indexPlayer == 0) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived));
                else if (indexPlayer == 1 && nbJoueur == 2) tempList.Add(Spawn_Card(9, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 2));
                else if (indexPlayer == 1) tempList.Add(Spawn_Card(10, 11 + (CardWidth + 5) * (tempList.Count), CarteToBeReceived, 2));
                else if (indexPlayer == 2) tempList.Add(Spawn_Card(10, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 3));
                else if (indexPlayer == 3) tempList.Add(Spawn_Card(BoardSizeHeight - CardHeight - 5, BoardSizeWidth - (CardWidth + 5) * (tempList.Count+1), CarteToBeReceived, 4));

                for (int i = 0; i < tempList.Count; i++) //Idem que pour add mais avec -
                {
                    if ((tempList[i].Name.Length == 6 && tempList[i].Name[5] - '0' == CarteTobeRemoved) || (tempList[i].Name.Length == 7 && (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0') == CarteTobeRemoved))
                    {
                        Label temp = tempList[i].Controls.OfType<Label>().First();
                        temp.Text = "x" + (temp.Text[1] - '0' - 1);
                        temp.Visible = true;
                        if (temp.Text[1] == 1) temp.Visible = false;
                    }
                }
            }
            else if (toRemove && !toAdd) // Si on enlève un carte
            {              
                Point LocationToRemove = new Point();
                int Maxwidth = tempList[0].Location.X, indexToMove = 0, indexToRemove = 0;
                for (int i = 0; i < tempList.Count; i++) //On itère à travers les cartes du joueur
                {
                    if (indexPlayer == 0 || (indexPlayer == 1 && nbJoueur > 2)) //Si c'est le joueur 1 ou alors le joueur 2 (dans le cas 3+ joueurs)
                    {
                        if (Maxwidth < tempList[i].Location.X) //Cette condition permet de trouver la carte la plus à droite de son deck
                        {
                            indexToMove = i; //On récupère l'index de la carte la plus à droite
                            Maxwidth = tempList[i].Location.X;
                        }
                    }
                    else //Sinon on check la carte la plus à gauche de son deck
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
                    if (CarteNumber == CarteTobeRemoved) //Si la carte correspond à la carte à supprimer
                    {
                        LocationToRemove = new Point(tempList[i].Location.X, tempList[i].Location.Y); //On récupère sa localisation
                        Controls.Remove(tempList[i]); //On la supprime
                        indexToRemove = i;
                    }
                }
                tempList[indexToMove].Location = new Point(LocationToRemove.X, LocationToRemove.Y); //On place la carte à l'extrémité du deck à l'endroit de la carte supprimée
                tempList.RemoveAt(indexToRemove); //On delete du deck la carte à delete


                for (int i = 0; i < tempList.Count; i++) //Boucle pour itérer à travers le deck et trouver la carte à incrémenter pour son label
                {
                    if ((tempList[i].Name.Length == 6 && tempList[i].Name[5] - '0' == CarteToBeReceived) || (tempList[i].Name.Length == 7 && (tempList[i].Name[5] - '0') * 10 + (tempList[i].Name[6] - '0') == CarteToBeReceived))
                    {
                        Label temp = tempList[i].Controls.OfType<Label>().First();
                        temp.Text = "x" + (temp.Text[1] - '0' + 1);
                        temp.Visible = true;
                    }
                }
            }
        }
        #endregion
    }
}
