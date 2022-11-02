﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
	internal class Game
	{

		private List<Player> listPlayer;
		private bool isGameOver = false;
		private bool TrueEnding = true;
		private int playerRound = 0;
		private Bank bank;
		private Dice dice;
		private Random random = new Random();

		public Bank Bank { get { return bank; } }

		public Dice Dice { get { return dice; } }

		public List<Player> ListPlayer { get { return listPlayer; } }
		public Game()
		{
			listPlayer = new List<Player>();
			bank = new Bank(this, 250);
			dice = new Dice();
		}

		public void Run()
		{
			int nbJoueurReel = 0;
			int nbJoueurMax = -1;
			int nbIA = 4;
            Console.WriteLine("Combien de joueurs êtes-vous ? Vous pouvez jouer de 1 à 4 joueurs. Dans le cas où vous joueriez seul, il y aura au minimum une IA.");
			while(nbJoueurReel < 1 || nbJoueurReel > 4)
				while (!int.TryParse(Console.ReadLine(), out nbJoueurReel))
					Console.WriteLine("Combien de joueurs êtes-vous ?");
			if (nbJoueurReel < 4)
			{
				Console.WriteLine("Combien de joueurs voulez-vous ajouter ? Vous pouvez être ajouter jusqu'à {0} joueurs pour être au maximum 4.", nbIA - nbJoueurReel);
				while (nbIA - nbJoueurReel < nbJoueurMax || nbJoueurMax == -1 || nbJoueurReel + nbJoueurMax < 2)
					while (!int.TryParse(Console.ReadLine(), out nbJoueurMax))
						Console.WriteLine("Combien de joueurs voulez-vous être au total ? Vous pouvez être ajouter jusqu'à {0} joueurs pour être au maximum 4.", nbIA - nbJoueurReel);
            }

            for (int i = 0; i < nbJoueurReel; i++)
            {
                Console.WriteLine("Joueur " + (i + 1) + " veuillez écrire votre nom : ");
                string namePlayer = Console.ReadLine();
                listPlayer.Add(new Player(namePlayer, false, this, 3, Bank));
            }
		/*	foreach (Player player in listPlayer) //Display Ressources for tests. 
			{
				player.DisplayRessources(player.NamePlayer);
				player.Trade(Bank, player, "Card", "Mine");
				Bank.DisplayRessources("Bank");
				player.DisplayRessources(player.NamePlayer);
			}*/
            for (int i = nbJoueurReel; i < nbJoueurMax; i++)
            {
                listPlayer.Add(new Player("IA " + (nbJoueurReel - 1 - i), true, this, 3, Bank));
            }
            while (!isGameOver)
            {
                Console.WriteLine("Tour du joueur {0}", playerRound + 1);
                Round(playerRound);
                playerRound = (playerRound + 1) % nbJoueurMax;
            }

            Console.WriteLine("Le joueur {0} gagne la partie.", isEndgame(TrueEnding) + 1);
        }
		private void Round(int PlayerRound)
		{
			listPlayer[playerRound].IsPlaying = true;
			if (!listPlayer[PlayerRound].IsAI)
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
					Console.WriteLine("Le dés a fait un score de {0}", ScoreDesTotal);
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
					foreach (var Cards in listPlayer[i].CardsAvailable)
					{
						if (Cards.Value.PileCards.Count != 0)
							if (Cards.Value.PileCards.Peek().ActivationValue.Item1 == ScoreDesTotal || Cards.Value.PileCards.Peek().ActivationValue.Item2 == ScoreDesTotal)
							{
								if (Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Blue)
									Cards.Value.PileCards.Peek().ActiveEffect();
								else if ((Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Purple || Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Green) && PlayerRound == i)
									Cards.Value.PileCards.Peek().ActiveEffect();
								else if (Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Red && PlayerRound != i)
									Cards.Value.PileCards.Peek().ActiveEffect();
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
				int nbDesChoice = hasGare(listPlayer[playerRound]) ? random.Next(1, 3) : 1;
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
					foreach (var Cards in listPlayer[i].CardsAvailable)
					{
                        if (Cards.Value.PileCards.Count != 0)
                            if (Cards.Value.PileCards.Peek().ActivationValue.Item1 == ScoreDesTotal || Cards.Value.PileCards.Peek().ActivationValue.Item2 == ScoreDesTotal)
							{
								if (Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Blue)
									Cards.Value.PileCards.Peek().ActiveEffect();
								else if ((Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Purple || Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Green) && PlayerRound == i) 
									Cards.Value.PileCards.Peek().ActiveEffect();
								else if (Cards.Value.PileCards.Peek().Type == (int)Card.Colorcard.Red && PlayerRound != i) 
									Cards.Value.PileCards.Peek().ActiveEffect();
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
			listPlayer[playerRound].IsPlaying = false;
		}
		private bool hasGare(Player player)
		{
			foreach (var item in player.Monuments)
				if (item.Name == "Gare" && item.IsActive) return true;
			return false;
		}
		private bool hasTour(Player player)
		{
			foreach (var item in player.Monuments)
				if (item.Name == "Tour" && item.IsActive) return true;
			return false;
		}
		private bool hasParc(Player player)
		{
			foreach (var item in player.Monuments)
				if (item.Name == "Parc" && item.IsActive)
					return true;
			return false;
		}
		private bool hasCentre(Player player)
		{
			foreach (var item in player.Monuments)
				if (item.Name == "Centre Commercial" && item.IsActive)
					return true;
			return false;
		}
		private int isEndgame(bool TrueEnding)
		{
			if (!TrueEnding)
			{
				for (int i = 0; i < listPlayer.Count; i++)
					if (hasGare(listPlayer[i]) && hasTour(listPlayer[i]) && hasParc(listPlayer[i]) && hasCentre(listPlayer[i]))
					{
						isGameOver = true;
						return i;
					}
			}
			else
			{
				for (int i = 0; i < listPlayer.Count; i++)
				{
					if (listPlayer[i].CoinsAvailable >= 20)
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