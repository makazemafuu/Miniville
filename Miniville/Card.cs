using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Miniville
{
    internal class Card
    {
        public enum Colorcard
        {
            Blue = 0,
            Green = 1,
            Red = 2,
            Purple = 3
        }

        private Game game;
        private Player player;

        public Player Player { get { return this.player; } set { this.player = value; } }

        public Game Party
        {
            get { return game; }
            set {
                if (this.game == null)
                    game = value;
                else
                    Console.WriteLine("Vous ne pouvez pas redéfinir game, il a déjà été défini.");
            }
        }

        private (int, int) activationValue; // = Dice.face
        public (int, int) ActivationValue { get; }

        protected string name;
        public string Name { get { return this.name; } }


        private int type;
        public int Type { get; }

        protected int cost;
        public int Cost { get; }

        // string not used in this class
        private string effectDescription;

        public string EffectDescription { get; set; }

        public Card((int, int) ActivationValue, string Name, int Type, int Cost)
        {
            this.activationValue = ActivationValue;
            this.name = Name;
            this.type = Type;
            this.cost = Cost;
        }

        public Card(string Name, int Cost)
        {
            this.name = Name;
            this.cost = Cost;
        }

        public Card() { }
        /*
        public void ActiveEffect()
        {

            if (activationValue.Item1 == 1)
            {
                // obtenez 1 pièce par la banque
            }

            else if (activationValue.Item1 == 2 || activationValue.Item2 == 2)

            {
                if (name == "Ferme" && type == 1 && cost == 1)
                {
                    
                    // obtenez 1 pièce par la banque
                }
                else if (name == "Boulangerie" && type == 2 && cost == 1)
                {

                    

                    // obtenez 1 pièce par la banque
                }
            }
            else if (activationValue.Item1 == 3 || activationValue.Item2 == 3)
            {
                if (name == "Boulangerie" && type == 2 && cost == 1)
                {

                    // obtenez 1 pièce par la banque
                }
                else if (name == "Café" && type == 3 && cost == 2)
                {


                    // obtenez 2 pièces par un autre joueur
                }
            }
            else if (activationValue.Item1 == 4 || activationValue.Item2 == 4)
            {

                // obtenez 3 pièces par la banque
            }
            else if (activationValue.Item1 == 5 || activationValue.Item2 == 5)
            {


                // obtenez 1 pièces par la banque
            }
            else if (activationValue.Item1 == 6 || activationValue.Item2 == 6)
            {
                if (name == "Stade" && type == 4 && cost == 6)
                {


                    // obtenez 5 pièces par un autre joueur
                }
                else if (name == "Centre d'Affaires" && type == 4 && cost == 8)
                {
                    // Player.cardsAvailable += carte établissement d'un joueur au choix qui ne soit pas de type 4;
                }
                else if (name == "Chaîne de Télévision" && type == 4 && cost == 7)
                {


                    // obtenez 5 pièces par la banque
                }
            }
            else if (activationValue.Item1 == 7 || activationValue.Item2 == 7)
            {
                foreach (var item in Player.CardsAvailable)
                {
                    if (item.Key == "Ferme")
                    {

                        // obtenez 3 pièces par la banque pour chaque établissements "Ferme" que vous possédez
                    }
                }
            }
            else if (activationValue.Item1 == 8 || activationValue.Item2 == 8)
            {
                foreach (var item in Player.cardsAvailable)
                {
                    if (item.Value == "Forêt" || item.Value == "Mine")
                    {

                        // obtenez 3 pièces par la banque pour chaque établissements "Forêt" et "Mine" que vous possédez
                    }
                }
            }
            else if (activationValue == 9)
            {
                if (name == "Mine" && type == 1 && cost == 6)
                {


                    // obtenez 5 pièces par la banque
                }
                else if (name == "Restaurant" && type == 3 && cost == 3)
                {

                    // obtenez 2 pièces par un autre joueur
                }

            }
            else if (activationValue == 10)
            {
                if (name == "Restaurant" && type == 3 && cost == 3)
                {


                    // obtenez 2 pièces par un autre joueur
                }
                else if (name == "Verger" && type == 1 && cost == 3)
                {

                    // obtenez 3 pièces par la banque

                }
            }
            else if (activationValue == 11)
            {
                foreach (var item in Player.cardsAvailable)
                {
                    if (item.Value == "Verger" || item.Value == "Champs de Blé")
                    {

                        // obtenez 2 pièces par la banque pour chaque établissements "Verger" et "Champs de Blé" que vous possédez
                    }
                }

            }
            else if (activationValue == 12)
            {
                foreach (var item in Player.cardsAvailable)
                {
                    if (item.Value == "Verger" || item.Value == "Champs de Blé")
                    {

                        // obtenez 2 pièces par la banque pour chaque établissements "Verger" et "Champs de Blé" que vous possédez
                    }
                }

            }
        }*/
    }
}
