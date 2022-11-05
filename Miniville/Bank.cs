using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Miniville
{
	internal class Bank 
	{
		protected int coinsAvailable;
		protected Game game;
        protected Dictionary<string, Pile> cardsAvailable;
		private Pile pile;

        public Bank(Game Game, int money) // Constructeur de Bank.
		{
			pile = new Pile();
			this.coinsAvailable = money;
			this.game = Game;
			cardsAvailable = pile.InitPile(game); // On déclare et instancie les piles de carte tout en leur donnant l'instance de la partie dans laquelle nous nous trouvons et on l'attribue à bank.
		}

        public void DisplayCards()
		{
			foreach (var card in cardsAvailable)
				Console.WriteLine(card.Value);
		}

		public Dictionary<String, Pile> CardsAvailable
		{
			get { return this.cardsAvailable; }
			set { this.cardsAvailable = value; }
		}

		public int CoinsAvailable
		{
			get { return this.coinsAvailable; }
			set { this.coinsAvailable = value; }
		}

		public void DisplayRessources(string from)
		{
			Console.WriteLine("{1} dispose de {0} pièces.", CoinsAvailable, from);
			foreach (var card in cardsAvailable)
			{
				if (card.Value.PileCards.Count > 0)
					Console.WriteLine("Il y a {0} cartes {1} restantes dans la réserve de {2}.", card.Value.PileCards.Count, card.Key, from);
            }
		}

        public List<Player> DisplayCardsOtherPLayer(string from)
        {
			List<Player> result = new List<Player>();
            foreach (var card in cardsAvailable)
            {
				result.Add(card.Value.PileCards.Peek().Owner);
				if (card.Value.PileCards.Peek().Type != 3)
					Console.WriteLine("{2} a {0} cartes {1}.", card.Value.PileCards.Count, card.Key, from);
            }
			return result;
        }
		public List<Card> ChooseCardOtherPlayer(string from)
        {
			List<Card> result = new List<Card>();
			int i = 0;
            foreach (var card in cardsAvailable)
            {
				if (card.Value.PileCards.Peek().Type != 3)
				{
					Console.WriteLine("Il y a {0} cartes {1} restantes dans la réserve de {2}, entrez \"{3}\" si vous voulez choisir celle là.", card.Value.PileCards.Count, card.Key, from, i);
					i++;
					result.Add(card.Value.PileCards.Peek());
				}
            }
			return result;
        }

        public List<Card> DisplayYourCards()
        {
			List<Card> list = new List<Card>();
            int i = 0;
            foreach (var card in cardsAvailable)
            {
                if (card.Value.PileCards.Peek().Type != 3)
				{
                    Console.WriteLine("Vous disposez de {0} carte(s) {1} , entrez \"{3}\" si vous voulez donner celle là", card.Value.PileCards.Count, card.Key, i);
					i++;
					list.Add(card.Value.PileCards.Peek());
				}
			}
			return list;
        }

		public void DisplayMoney(Player from)
		{
			Console.WriteLine(from.NamePlayer + "a {0} pièces, voulez-vous le dépouiller ?", from.CoinsAvailable);
		}

        public void Trade(Bank from, Bank to, string type, string value)
		{
			if (type == "Card")
			{
				from.CardsAvailable[value].PileCards.Peek().Owner = (Player)to; // (nom du type) devant une variable, c'est un cast, ce qui permet de traiter la variable comme si elle appartenait à un autre type.
				to.CardsAvailable[value].PileCards.Push(from.CardsAvailable[value].PileCards.Pop());
			}
			else if (type == "Coin")
			{
				to.CoinsAvailable = to.CoinsAvailable + int.Parse(value);
				from.CoinsAvailable = from.CoinsAvailable - int.Parse(value);
			}
		}
    }
}
