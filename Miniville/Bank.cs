using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Miniville
{
	internal class Bank 
	{
		protected int coinsAvailable = 250;
		protected Dictionary<String, Pile> cardsAvailable = Pile.InitPile();

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

		public void DisplayRessources()
		{
			Console.WriteLine("There is {0} coins remaining in the Bank", CoinsAvailable);
			foreach (var card in cardsAvailable)
				Console.WriteLine("There is {0} cards of the type {1} remaining of the type in the Bank", card.Value.PileCards.Count, card.Key);
		}

		public void Trade()
		{

		}
    }
}
