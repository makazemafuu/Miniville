using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miniville
{
	internal class Pile
	{
		private static List<Card> cards = new List<Card>()
		{
			new Card("Champs de blé", 0),
			new Card("Ferme", 0),
			new Card("Boulangerie", 1),
			new Card("Café", 2),
			new Card("Supérette", 1),
			new Card("Forêt", 0),
			new Card("Stade", 3),
			new Card("Centre d'affaires", 3),
			new Card("Chaîne de télévision", 3),
			new Card("Fromagerie", 1),
			new Card("Fabrique de meubles", 1),
			new Card("Mine", 0),
			new Card("Restaurant", 2),
			new Card("Verger", 0),
			new Card("Marché de fruits et légumes", 1),
		};

		private Stack<Card> pileCards = new Stack<Card>();

		public Pile(Card toFillIn)
		{
			for (int i = 0; i < 6; i++)
				this.pileCards.Push(toFillIn);
		}

		public Stack<Card> PileCards
		{
			get { return pileCards; }
		}
		public static List<Card> Cards { get { return cards; } }

		public static Dictionary<Pile, string> InitPile()
		{
			Dictionary<Pile, string> result = new Dictionary<Pile, string>();
			Pile pile;
			for (int i = 0; i < 15; i++)
			{
				pile = new Pile(Cards[i]);
				result.Add(pile, Cards[i].Name);
			}
			return result;
		}
	}
}
