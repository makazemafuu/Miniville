using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Miniville
{
	internal class Bank 
	{
		private int coinsAvailable = 250;
		private Dictionary<Pile, string> cardsAvailable = Pile.InitPile();

		public void DisplayCards()
		{
			foreach (var card in cardsAvailable)
				Console.WriteLine(card.Value);
		}
		public Dictionary<Pile, string> CardsAvailable
		{
			get { return this.cardsAvailable; }
			set { this.cardsAvailable = value; }
		}
		public int CoinsAvailable
		{
			get { return this.coinsAvailable; }
			set { this.coinsAvailable = value; }
		}

    }
}
