using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miniville
{
	internal class Card
	{
		public enum Colorcard
		{
			Blue = 0,
			Green = 1,
			Red = 2,
			Purple = 3
		}

		private Game game;
		private Player owner;

		public Player Owner { get { return this.owner; } set { this.owner = value; } }

		public Game Party
		{
			get { return game; }
			set {
				if (this.game == null)
					game = value;
				else
					Console.WriteLine("Vous ne pouvez pas redéfinir game, il a déjà été défini.");
			}
		}

		private (int, int) activationValue; // = Dice.face
		public (int, int) ActivationValue { get; }

		protected string name;
		public string Name { get { return this.name; } }


		private int type;
		public int Type { get { return type; } }

		protected int cost;
		public int Cost { get { return cost; } }

		// string not used in this class
		protected string effectDescription;

		public string EffectDescription { get { return effectDescription; } }

		public Card((int, int) ActivationValue, string Name, int Type, int Cost, string Description)
		{
			this.activationValue = ActivationValue;
			this.name = Name;
			this.type = Type;
			this.cost = Cost;
			this.effectDescription = Description;
		}

		public Card(string Name, int Cost)
		{
			this.name = Name;
			this.cost = Cost;
		}

		public Card() { }
		
		public void ActiveEffect()
		{
			if (name == "Ferme")
				game.Bank.Trade(game.Bank, owner, "Coin", "1");  // obtenez 1 pièce par la banque
			else if (name == "Boulangerie")
				game.Bank.Trade(game.Bank, owner, "Coin", "1");  // obtenez 1 pièce par la banque
			else if (name == "Café")
			{
				foreach (Player player in game.ListPlayer)
					if (player.IsPlaying && player.NamePlayer != Owner.NamePlayer) // obtenez 1 pièce par le joueur qui a lancé les dés
                        game.Bank.Trade(player, Owner, "Coin", "1");
			}
			else if (name == "Supérette")
				game.Bank.Trade(game.Bank, owner, "Coin", "3");  // obtenez 3 pièces par la banque
            else if (name == "Forêt")
				game.Bank.Trade(game.Bank, owner, "Coin", "1");  // obtenez 1 pièces par la banque
			else if (name == "Stade")
			{
                foreach (Player player in game.ListPlayer)
                    if (owner.IsPlaying && player.NamePlayer != Owner.NamePlayer) // obtenez 1 pièce par le joueur qui a lancé les dés
                        game.Bank.Trade(player, Owner, "Coin", "2");
            }
			else if (name == "Centre d'Affaires")
			{
				int i = 0;
				while (i++ < game.ListPlayer.Count)
				{
					game.ListPlayer[i].DisplayCardsOtherPLayer(game.ListPlayer[i].NamePlayer);
					Console.WriteLine("Enter {0} if you want to choose this player.", i);
				}
                while (i < 0 || game.ListPlayer[i] == null)
                    while (!int.TryParse(Console.ReadLine(), out i)) 
					{
						Console.WriteLine("Sélectionnez un chiffre valable s'il vous plait.");
					}
				int j = -1;
				while (j < 0 || game.ListPlayer[i].CardsAvailable.ElementAt(j).Value.PileCards.Count == 0 || game.ListPlayer[i].CardsAvailable.ElementAt(j).Value.PileCards.Peek().Type == 3)
				{
					while (!int.TryParse(Console.ReadLine(), out j))
						game.ListPlayer[i].ChooseCardOtherPlayer(game.ListPlayer[i].NamePlayer);
				}
				int k = -1;
                while (k < 0 || owner.CardsAvailable.ElementAt(k).Value.PileCards.Count == 0 || owner.CardsAvailable.ElementAt(j).Value.PileCards.Peek().Type == 3)
                {
                    while (!int.TryParse(Console.ReadLine(), out k))
						owner.DisplayYourCards();
                }
				game.Bank.Trade(game.ListPlayer[i], Owner, "Card", game.ListPlayer[i].CardsAvailable.ElementAt(j).Key);
				game.Bank.Trade(Owner, game.ListPlayer[i], "Card", owner.CardsAvailable.ElementAt(k).Key);
                // Le joueur possèdant la carte centre d'affaire échange avec un autre joueur de son choix une carte de son choix sauf carte violette.
            }
			else if (name == "Chaîne de Télévision")
			{
				int i = 0;
                while (i++ < game.ListPlayer.Count)
                {
                    game.ListPlayer[i].DisplayMoney(game.ListPlayer[i].NamePlayer);
                    Console.WriteLine("Enter {0} if you want to choose this player.", i);
                }
                while (i < 0 || game.ListPlayer[i] == null)
                    while (!int.TryParse(Console.ReadLine(), out i))
                    {
                        Console.WriteLine("Sélectionnez un chiffre valable s'il vous plait.");
                    }
				game.Bank.Trade(game.ListPlayer[i], owner, "Coin", "5");
                // obtenez 5 pièces par un autre joueur

            }
            else if (name == "Fromagerie")
			{
                int coins = 0;
                foreach (var item in owner.CardsAvailable)
                    if (item.Key == "Verger")
						coins = item.Value.PileCards.Count;
                game.Bank.Trade(game.Bank, owner, "Coin", (coins * 3).ToString()); // obtenez 3 pièces par la banque pour chaque établissements "Ferme" que vous possédez
            }
			else if (name == "Fabrique de Meubles")
			{
                int coins = 0;
                foreach (var item in owner.CardsAvailable)
                    if (item.Key == "Forêt" || item.Key == "Mine")
                        coins += item.Value.PileCards.Count;
                game.Bank.Trade(game.Bank, owner, "Coin", (coins * 3).ToString()); // obtenez 3 pièces par la banque pour chaque établissements "Forêt" et "Mine" que vous possédez
            }
			else if (name == "Mine")
			{
				game.Bank.Trade(game.Bank, owner, "Coin", "5"); // obtenez 5 pièces par la banque
			}
            else if (name == "Restaurant")
			{
                foreach (Player player in game.ListPlayer)
                    if (player.IsPlaying && player.NamePlayer != Owner.NamePlayer) // obtenez 1 pièce par le joueur qui a lancé les dés
                        game.Bank.Trade(player, Owner, "Coin", "2");
                // obtenez 2 pièces par un autre joueur
            }
			else if (name == "Verger")
			{
                game.Bank.Trade(game.Bank, owner, "Coin", "3"); // obtenez 3 pièces par la banque
            }
			else if (name == "Marché de Fruits et Légumes")
			{
				int coins = 0;
				foreach (var item in owner.CardsAvailable)
				{
					if (item.Key == "Verger" || item.Key == "Champs de Blé")
					{
						coins += item.Value.PileCards.Count;
					}
				}
				game.Bank.Trade(game.Bank, owner, "Coin", coins.ToString()); // obtenez 2 pièces par la banque pour chaque établissements "Verger" et "Champs de Blé" que vous possédez
			}
		}
	}
}
