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
        public Player() //Constructeur, Pièce à 3 avec les 2 cartes champs de blé et boulangerie
        {
            Pieces = 3;
            CarteAcquises = new List<Cards>() { new Cards(NomCarte.ChampsDeBle), new Cards(NomCarte.Boulangerie)};
            CarteAcquisesUniques = new List<Cards>() { new Cards(NomCarte.ChampsDeBle), new Cards(NomCarte.Boulangerie) };
        }
        public void BuyCard(Piles pile) //Achete la carte de la pile
        {
            Cards temp = pile.PileCartes.Peek(); //1ère carte de la pile
            Pieces -= temp.Cost; //On enleve le coût (la condition de si achetable on non est déjà check au préalable)
            if (!CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name)) CarteAcquisesUniques.Add(temp); //Si on ne possède pas ce type de carte on l'ajoute à la liste des cartes uniques
            CarteAcquises.Add(pile.PileCartes.Pop()); //On ajoute la carte obtenue dans notre liste de cartes acquises et on pop de la pile
        }
        public bool[] TradingChange(int indexToReceive, int indexToGive) //Renvoie un booléen en fonction des cartes données et recues si l'affichage doit changer, tout en faisant les actions consoles nécessaires
        {
            #region Carte A donner 
            if (indexToReceive == indexToGive) //Si on recoit la même carte qu'on donne, il ne se passe rien
            {
                MessageBox.Show("To add = " + false + " to remove = " + false); //Pour le debugging
                return new bool[] { false, false }; //false, false
            }
            int count = 0;
            for(int i = CarteAcquises.Count-1; i >= 0; i--)
            {
                if((int) CarteAcquises[i].Name == indexToGive)
                {
                    if(count == 0) CarteAcquises.RemoveAt(i); //On enleve la carte de notre liste de cartes acquises
                    count++;
                }
            }
            bool ToAdd = false, toRemove = false;
            if (count == 1) //Si count == 1 cela veut dire qu'on a un seul exemplaire de la carte qu'on veut donner, donc toRemove = true;
            {
                toRemove = true;
                for (int i = CarteAcquisesUniques.Count- 1; i>= 0; i--)
                {
                    if ((int)CarteAcquisesUniques[i].Name == indexToGive)
                    {
                        CarteAcquisesUniques.RemoveAt(i); //On enleve la carte de notre liste de cartes uniques acquises
                        break;
                    }
                }
            }
            #endregion
            #region Carte à recevoir
            Cards temp = new Cards((NomCarte)indexToReceive);
            CarteAcquises.Add(temp);
            if (!CarteAcquisesUniques.Any(Cards => Cards.Name == temp.Name)) {  //Si on ne possède pas la carte alors on devra l'ajouter, ToAdd = true
                CarteAcquisesUniques.Add(temp); 
                ToAdd = true; }
            MessageBox.Show("To add = " + ToAdd + " to remove = " + toRemove); //Pour le debugging
            #endregion
            return new bool[] { ToAdd, toRemove }; //Retour pour l'affichage
        }
    }
}

