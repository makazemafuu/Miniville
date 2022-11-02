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
			new Card((1,1), "Champs de Blé", 0, 1 , "Pendant le tour de n'importe quel joueur : Recevez une pièce de la banque."),
			new Card((2,2), "Ferme", 0, 1, "Pendant le tour de n'importe quel joueur : Recevez une pièce de la banque."),
            new Card((2,3), "Boulangerie", 1, 1, "Pendant votre tour uniquement : Recevez une pièce de la banque."),
			new Card((3,3), "Café", 2, 2, "Recevez une pièce du joueur qui a lancé les dés."),
			new Card((4,4), "Supérette", 1, 2, "Pendant votre tour uniquement : Recevez trois pièces de la banque."),
			new Card((5,5), "Forêt", 0, 3, "Pendant le tour de n'importe quel joueur : Recevez une pièce de la banque."),
            new Card((6,6), "Stade", 3, 6, "Pendant votre tour uniquement : Recevez deux pièces de la part de chaque autre joueur."),
            new Card((6,6), "Centre d'Affaires", 3, 8, "Pendant votre tour uniquement : Vous pouvez échanger la carte de votre choix avec le joueur de votre choix."),
            new Card((6,6), "Chaîne de Télévision", 3, 7, "Pendant votre tour uniquement : Recevez 5 pièces du joueur de votre choix."),
			new Card((7,7), "Fromagerie", 1, 5, "Pendant votre tour uniquement : Recevez 3 pièces de la banque pour chaque ferme que vous possédez."),
			new Card((8,8), "Fabrique de Meubles", 1, 3, "Pendant votre tour uniquement : Recevez 3 pièces de la banque pour chaque Forêt ou Mine que vous possédez. "),
			new Card((9,9), "Mine", 0, 6, "Pendant le tour de n'importe quel joueur : Recevez 5 pièces de la banque."),
            new Card((9,10), "Restaurant", 2, 3, "Recevez 2 pièces de la part du joueur qui a lancé les dés."),
			new Card((10,10), "Verger", 0, 3, "Pendant le tour de n'importe quel joueur : Recevez 3 pièces de la banque."),
			new Card((11,12), "Marché de Fruits et Légumes", 1, 2, "Pendant votre tour uniquement : Recevez 2 pièces de la banque pour chaque Champs de Blé ou Verger que vous possédez."),
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
