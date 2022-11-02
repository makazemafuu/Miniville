using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniVille_GraphiqueWF
{
    public class Player
    {
        public List<Cards> CarteAcquises;
        public List<Cards> CarteAcquisesUniques;
        public bool hasGare = false, hasCentre = false, hasTour = false, hasParc = false;
        public int Pieces;
        public Player()
        {
            Pieces = 3;
            CarteAcquises = new List<Cards>() { new Cards(NomCarte.ChampsDeBle), new Cards(NomCarte.Boulangerie)};
            CarteAcquisesUniques = new List<Cards>() { new Cards(NomCarte.ChampsDeBle), new Cards(NomCarte.Boulangerie) };
        }
        public bool CanBuyCard(List<Piles> CartesDisponibles)
        {
            for(int i = 0; i < CartesDisponibles.Count; i++)
            {
                if (CartesDisponibles[i].PileCartes.Count != 0 && CartesDisponibles[i].PileCartes.Peek().Cost <= Pieces)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CanBuy(Cards Carte)
        {   
            if ( Carte.Cost <= Pieces)  return true;
            return false;
        }
        public void BuyCard(Piles pile)
        {
            Cards temp = pile.PileCartes.Peek();
            Pieces -= temp.Cost;
            if (!CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name)) CarteAcquisesUniques.Add(temp);
            CarteAcquises.Add(pile.PileCartes.Pop());
        }
        public bool[] TradingChange(int indexToReceive, int indexToGive)
        {
            #region Carte A donner 
            if (indexToReceive == indexToGive)
            {
                MessageBox.Show("To add = " + false + " to remove = " + false);
                return new bool[] { false, false };
            }
            int count = 0;
            for(int i = CarteAcquises.Count-1; i >= 0; i--)
            {
                if((int) CarteAcquises[i].Name == indexToGive)
                {
                    if(count == 0) CarteAcquises.RemoveAt(i);
                    count++;
                }
            }
            bool ToAdd = false, toRemove = false;
            if (count == 1)
            {
                toRemove = true;
                for (int i = CarteAcquisesUniques.Count- 1; i>= 0; i--)
                {
                    if ((int)CarteAcquisesUniques[i].Name == indexToGive)
                    {
                        CarteAcquisesUniques.RemoveAt(i);
                        break;
                    }
                }
            }
            #endregion
            #region Carte à recevoir
            Cards temp = new Cards((NomCarte)indexToReceive);
            CarteAcquises.Add(temp);
            if (!CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name)) { 
                CarteAcquisesUniques.Add(temp); 
                ToAdd = true; }
            MessageBox.Show("To add = " + ToAdd + " to remove = " + toRemove);
            return new bool[] {ToAdd,toRemove };
            #endregion
        }
    }
}

