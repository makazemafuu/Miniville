using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
	internal class Pile
	{
		private Game game;

		public List<Card> cards = new List<Card>()
		{
			new Card((1,1), "Champs de blé", 0, 1),
			new Card((2,2), "Ferme", 0, 1),
			new Card((2,3), "Boulangerie", 1, 1),
			new Card((3,3), "Café", 2, 2),
			new Card((4,4), "Supérette", 1, 2),
			new Card((5,5), "Forêt", 0, 3),
			new Card((6,6), "Stade", 3, 6),
			new Card((6,6), "Centre d'affaires", 3, 8),
			new Card((6,6), "Chaîne de télévision", 3, 7),
			new Card((7,7), "Fromagerie", 1, 5),
			new Card((8,8), "Fabrique de meubles", 1, 3),
			new Card((9,9), "Mine", 0, 6),
			new Card((9,10), "Restaurant", 2, 3),
			new Card((10,10), "Verger", 0, 3),
			new Card((11,12), "Marché de fruits et légumes", 1, 2),
		};

		private Stack<Card> pileCards = new Stack<Card>();

		public Pile(Card toFillIn, Game game)
		{
			toFillIn.Party = game;
			for (int i = 0; i < 6; i++)
				this.pileCards.Push(toFillIn);
		}

		public Pile() {}

		public Stack<Card> PileCards
		{
			get { return pileCards; }
		}
		public List<Card> Cards { get { return cards; } }

		public Dictionary<string, Pile> InitPile(Game game)
		{
			Dictionary<String, Pile> result = new Dictionary<String, Pile>();
			Pile pile;
			for (int i = 0; i < 15; i++)
			{
				pile = new Pile(Cards[i], game);
				result.Add(Cards[i].Name, pile);
			}
			return result;
		}
	}
}
