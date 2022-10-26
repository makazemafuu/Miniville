using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        int activationValue; // = Dice.face
        string name;
        int type;
        int cost;

        // string not used in this class
        string effectDescription;

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
                    // Bank.coinsAvailable += 2;
                }
            }
            else if (activationValue == 4)
            {
                name = "Supérette";
                type = 2;
                cost = 2;
                // Bank.coinsAvailable += 2;
            }
            else if (activationValue == 5)
            {
                name = "Forêt";
                type = 1;
                cost = 3;
                // Bank.coinsAvailable += 3;
            }
            else if (activationValue == 6)
            {
                if (name == "Stade" && type == 4 && cost == 6)
                {
                    // Bank.coinsAvailable += 6;
                }
                else if (name == "Centre d'Affaires" && type == 4 && cost == 8)
                {
                    // Bank.coinsAvailable += 8;
                }
                else if (name == "Chaîne de Télévision" && type == 4 && cost == 7)
                {
                    // Bank.coinsAvailable += 7;
                }
            }
            else if (activationValue == 7)
            {
                name = "Fromagerie";
                type = 2;
                cost = 5;
                // Bank.coinsAvailable += 5;
            }
            else if (activationValue == 8)
            {
                name = "Fabrique de Meubles";
                type = 2;
                cost = 3;
                // Bank.coinsAvailable += 3;
            }
            else if (activationValue == 9)
            {
                if (name == "Mine" && type == 1 && cost == 6)
                {
                    // Bank.coinsAvailable += 6;
                }
                else if (name == "Restaurant" && type == 3 && cost == 3)
                {
                    // Bank.coinsAvailable += 3;
                }

            }
            else if (activationValue == 10)
            {
                if (name == "Restaurant" && type == 3 && cost == 3)
                {
                    // Bank.coinsAvailable += 3;
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
                // Bank.coinsAvailable += 2;
            }
            else if (activationValue == 12)
            {
                name = "Marche de fruits et légumes";
                type = 2;
                cost = 2;
                // Bank.coinsAvailable += 2;
            }
        }
    }
}
