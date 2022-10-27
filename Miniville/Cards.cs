using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miniville
{
    public class Cards
    {
        enum Color
        {
            Blue,
            Green,
            Red,
            Purple
        }

        private int activationValue; // = Dice.face
        public int ActivationValue { get; }

        private string name;
        public string Name { get; }

        private int type;
        public int Type { get; }

        private int cost;
        public int Cost { get; }

        // string not used in this class
        private string effectDescription;
        public string EffectDescription { get; }

        public Cards(int ActivationValue, string Name, int Type, int Cost)
        {
            this.activationValue = ActivationValue;
            this.name = Name;
            this.type = Type;
            this.cost = Cost;
        }

        public void ActiveEffect()
        {
            if (activationValue == 1)
            {
                name = "Champs de Blé";
                type = 1;
                cost = 1;
                // Bank.coinsAvailable += 1;
            }
            else if (activationValue == 2)
            {
                if (name == "Ferme" && type == 1 && cost == 1)
                {
                    // Bank.coinsAvailable += 1;
                }
                else if (name == "Boulangerie" && type == 2 && cost == 1)
                {
                    // Bank.coinsAvailable += 1;
                }
            }
            else if (activationValue == 3)
            {
                if (name == "Boulangerie" && type == 2 && cost == 1)
                {
                    // Bank.coinsAvailable += 1;
                }
                else if (name == "Café" && type == 3 && cost == 2)
                {
                    // Player.coinsAvailable += 2;
                }
            }
            else if (activationValue == 4)
            {
                name = "Supérette";
                type = 2;
                cost = 2;
                // Bank.coinsAvailable += 3;
            }
            else if (activationValue == 5)
            {
                name = "Forêt";
                type = 1;
                cost = 3;
                // Bank.coinsAvailable += 1;
            }
            else if (activationValue == 6)
            {
                if (name == "Stade" && type == 4 && cost == 6)
                {
                    // Player.coinsAvailable += 2;
                }
                else if (name == "Centre d'Affaires" && type == 4 && cost == 8)
                {
                    // Player.cardsAvailable += carte établissement d'un joueur au choix qui ne soit pas de type 4;
                }
                else if (name == "Chaîne de Télévision" && type == 4 && cost == 7)
                {
                    // Player.coinsAvailable += 5;
                }
            }
            else if (activationValue == 7)
            {
                name = "Fromagerie";
                type = 2;
                cost = 5;
                // Bank.coinsAvailable += 3 pièces pour chaque établissements "Ferme" que vous possédez;
            }
            else if (activationValue == 8)
            {
                name = "Fabrique de Meubles";
                type = 2;
                cost = 3;
                // Bank.coinsAvailable += 3 pièces pour chaque établissements "Forêt" et "Mine" que vous possédez;
            }
            else if (activationValue == 9)
            {
                if (name == "Mine" && type == 1 && cost == 6)
                {
                    // Bank.coinsAvailable += 5;
                }
                else if (name == "Restaurant" && type == 3 && cost == 3)
                {
                    // Player.coinsAvailable += 2;
                }

            }
            else if (activationValue == 10)
            {
                if (name == "Restaurant" && type == 3 && cost == 3)
                {
                    // Player.coinsAvailable += 2;
                }
                else if (name == "Verger" && type == 1 && cost == 3)
                {
                    // Bank.coinsAvailable += 3;
                }
            }
            else if (activationValue == 11)
            {
                name = "Marche de fruits et légumes";
                type = 2;
                cost = 2;
                // Bank.coinsAvailable += 2 pour chaque établissements "Verger" et "Champs de Blé" que vous possédez;
            }
            else if (activationValue == 12)
            {
                name = "Marche de fruits et légumes";
                type = 2;
                cost = 2;
                // Bank.coinsAvailable += 2 pour chaque établissements "Verger" et "Champs de Blé" que vous possédez;
            }
        }

        public void PassiveEffect()
        {
            bool isActive = false;
        }
    }
}
