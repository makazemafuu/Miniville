using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
    class Game
    {
        List<Player> listPlayer;
        bool isGameOver = false;
        bool TrueEnding = true;
        int playerRound = 0;
        Bank bank;
        Dice dice;
        Random random = new Random();
        public Game(int nbJoueurReel, int nbJoueurMax)
        {
            listPlayer = new List<Player>();
            bank = new Bank();
            dice = new Dice();


            for (int i = 0; i < nbJoueurReel; i++)
            {
                Console.WriteLine("Joueur " + (i + 1) + "veuillez écrire votre nom : ");
                string namePlayer = Console.ReadLine();
                listPlayer.Add(new Player(namePlayer, false));
            }
            for (int i = nbJoueurReel; i < nbJoueurMax; i++)
            {
                listPlayer.Add(new Player("IA " + (nbJoueurReel - 1 - i), true));
            }
            while (!isGameOver)
            {
                Round(playerRound);
                playerRound = (playerRound + 1) % nbJoueurMax;
            }

            Console.WriteLine("Le joueur {0} gagne la partie.", isEndgame(TrueEnding) + 1);
        }

        private void Round(int PlayerRound)
        {
            if (!listPlayer[PlayerRound].isAI)
            {
                #region Lancé de dé
                string nbDes;
                int nbDesChoice = 1;
                if (hasGare(listPlayer[PlayerRound]))
                {
                    Console.WriteLine("Voulez vous lancer un ou deux dés ? ");
                    nbDes = Console.ReadLine();
                    //Tant que l'input n'est pas un chiffre valide on redemande à l'utilisateur de taper.
                    while (!int.TryParse(nbDes, out nbDesChoice) || !(nbDesChoice == 1 || nbDesChoice == 2))
                    {
                        Console.WriteLine();
                        Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper 1, ou 2", nbDes);
                        nbDes = Console.ReadLine();
                        Console.WriteLine();
                    }
                }
                int ScoreDesTotal, ScoreDes1 = 0, scoreDes2 = 0;
                if (nbDesChoice == 1)
                {
                    ScoreDesTotal = dice.Roll();
                    Console.WriteLine("Les dés ont fait un score de {0}", ScoreDesTotal);
                }
                else
                {
                    ScoreDes1 = dice.Roll();
                    scoreDes2 = dice.Roll();
                    ScoreDesTotal = ScoreDes1 + scoreDes2;
                    Console.WriteLine("Les dés ont fait un score de {0} + {1} = {3} ", ScoreDes1, scoreDes2, ScoreDesTotal);
                }

                if (hasTour(listPlayer[PlayerRound]))
                {
                    Console.WriteLine("Voulez-vous relancer les dés ? (y/n)");
                    string YesNo = Console.ReadLine();
                    while (YesNo.Length != 1 || !(YesNo == "y" || YesNo == "n"))
                    {
                        Console.WriteLine();
                        Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper y, ou n", YesNo);
                        YesNo = Console.ReadLine();
                        Console.WriteLine();
                    }
                    if (YesNo == "y")
                    {
                        Console.WriteLine("Voulez vous lancer un ou deux dés ? ");
                        nbDes = Console.ReadLine();
                        //Tant que l'input n'est pas un chiffre valide on redemande à l'utilisateur de taper.
                        while (!int.TryParse(nbDes, out nbDesChoice) || !(nbDesChoice == 1 || nbDesChoice == 2))
                        {
                            Console.WriteLine();
                            Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper 1, ou 2", nbDes);
                            nbDes = Console.ReadLine();
                            Console.WriteLine();
                        }
                        if (nbDesChoice == 1)
                        {
                            ScoreDesTotal = dice.Roll();
                            Console.WriteLine("Les dés ont fait un score de {0}", ScoreDesTotal);
                        }
                        else
                        {
                            ScoreDes1 = dice.Roll();
                            scoreDes2 = dice.Roll();
                            ScoreDesTotal = ScoreDes1 + scoreDes2;
                            Console.WriteLine("Les dés ont fait un score de {0} + {1} = {3} ", ScoreDes1, scoreDes2, ScoreDesTotal);
                        }
                    }
                }
                #endregion
                #region Activation des cartes
                for (int i = 0; i < listPlayer.Count; i++)
                {
                    foreach (var Cards in listPlayer[i].cardsAvailable)
                    {
                        if (Cards.Key.piles.Peek().activationValue == ScoreDesTotal)
                        {
                            if (Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Bleu) Cards.Key.piles.Peek().ActiveEffect();
                            else if ((Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Violet || Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Vert) && PlayerRound == i) Cards.Key.piles.Peek().ActiveEffect();
                            else if (Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Rouge && PlayerRound != i) Cards.Key.piles.Peek().ActiveEffect();
                        }
                    }
                }

                #endregion
                if (isEndgame(TrueEnding) != -1) return;
                #region Achat
                Console.WriteLine("Voici les cartes que vous pouvez acheter, veuillez taper le numéro de la pile que vous souhaitez acheter");
                listPlayer[PlayerRound].DisplayChoice();
                string PlayerChoice = Console.ReadLine();
                int nbPileChoice;
                while (!int.TryParse(PlayerChoice, out nbPileChoice) || !(nbPileChoice > 0 && nbPileChoice < 16))
                {
                    Console.WriteLine();
                    Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper une pile valide", nbPileChoice);
                    PlayerChoice = Console.ReadLine();
                    Console.WriteLine();
                }
                listPlayer[PlayerRound].Shop(nbPileChoice);
                #endregion
                //rejoue si parc d'attraction
                if (hasParc(listPlayer[PlayerRound]) && ScoreDes1 == scoreDes2 && ScoreDes1 != 0) Round(PlayerRound);
            }
            else
            {
                #region Lancé de dé
                int nbDesChoice = random.Next(1, 3);
                int ScoreDesTotal, ScoreDes1 = 0, scoreDes2 = 0;
                if (nbDesChoice == 1)
                {
                    ScoreDesTotal = dice.Roll();
                    Console.WriteLine("Les dés ont fait un score de {0}", ScoreDesTotal);
                }
                else
                {
                    ScoreDes1 = dice.Roll();
                    scoreDes2 = dice.Roll();
                    ScoreDesTotal = ScoreDes1 + scoreDes2;
                    Console.WriteLine("Les dés ont fait un score de {0} + {1} = {3} ", ScoreDes1, scoreDes2, ScoreDesTotal);
                }

                if (hasTour(listPlayer[PlayerRound]))
                {
                    string YesNo = (random.Next(1, 3) == 1 ? "y" : "n");
                    if (YesNo == "y")
                    {
                        Console.WriteLine("Voulez vous lancer un ou deux dés ? ");
                        nbDesChoice = random.Next(1, 3);
                        if (nbDesChoice == 1)
                        {
                            ScoreDesTotal = dice.Roll();
                            Console.WriteLine("Les dés ont fait un score de {0}", ScoreDesTotal);
                        }
                        else
                        {
                            ScoreDes1 = dice.Roll();
                            scoreDes2 = dice.Roll();
                            ScoreDesTotal = ScoreDes1 + scoreDes2;
                            Console.WriteLine("Les dés ont fait un score de {0} + {1} = {3} ", ScoreDes1, scoreDes2, ScoreDesTotal);
                        }
                    }
                }
                #endregion
                #region Activation des cartes
                for (int i = 0; i < listPlayer.Count; i++)
                {
                    foreach (var Cards in listPlayer[i].cardsAvailable)
                    {
                        if (Cards.Key.piles.Peek().activationValue == ScoreDesTotal)
                        {
                            if (Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Bleu) Cards.Key.piles.Peek().ActiveEffect();
                            else if ((Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Violet || Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Vert) && PlayerRound == i) Cards.Key.piles.Peek().ActiveEffect();
                            else if (Cards.Key.piles.Peek().CouleurCarte == CouleurCarte.Rouge && PlayerRound != i) Cards.Key.piles.Peek().ActiveEffect();
                        }
                    }
                }

                #endregion
                if (isEndgame(TrueEnding) != -1) return;
                #region Achat
                Console.WriteLine("Voici les cartes que vous pouvez acheter, veuillez taper le numéro de la pile que vous souhaitez acheter");
                listPlayer[PlayerRound].DisplayChoice();
                int nbPileChoice = random.Next(1, 16); // a modifié car pas convaincu par le displayChoice de la banque
                listPlayer[PlayerRound].Shop(nbPileChoice);
                #endregion
                //rejoue si parc d'attraction
                if (hasParc(listPlayer[PlayerRound]) && ScoreDes1 == scoreDes2 && ScoreDes1 != 0) Round(PlayerRound);
            }

        }
        private bool hasGare(Player player)
        {
            foreach (var item in player.cardsAvailable)
            {
                if (item.Key.piles.Peek().name == "Gare" && item.Key.piles.Peek().isActive) return true;
            }
            return false;
        }
        private bool hasTour(Player player)
        {
            foreach (var item in player.cardsAvailable)
            {
                if (item.Key.piles.Peek().name == "Tour" && item.Key.piles.Peek().isActive) return true;
            }
            return false;
        }
        private bool hasParc(Player player)
        {
            foreach (var item in player.cardsAvailable)
            {
                if (item.Key.piles.Peek().name == "Parc" && item.Key.piles.Peek().isActive) return true;
            }
            return false;
        }
        private bool hasCentre(Player player)
        {
            foreach (var item in player.cardsAvailable)
            {
                if (item.Key.piles.Peek().name == "CentreCommercial" && item.Key.piles.Peek().isActive) return true;
            }
            return false;
        }
        private int isEndgame(bool TrueEnding)
        {
            if (!TrueEnding)
            {
                for (int i = 0; i < listPlayer.Count; i++)
                {
                    if (hasGare(listPlayer[i]) && hasTour(listPlayer[i]) && hasParc(listPlayer[i]) && hasCentre(listPlayer[i]))
                    {
                        isGameOver = true;
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < listPlayer.Count; i++)
                {
                    int sum = 0;
                    foreach (var item in listPlayer[i].coinsAvailable)
                    {
                        sum += (item.Key * item.Value);
                    }
                    if (sum >= 20)
                    {
                        isGameOver = true;
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}