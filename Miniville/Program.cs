﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Miniville
{
    internal class Program
    {
        public static void Main()
        {
            Game game = new Game();
            game.Bank.DisplayRessources();
          /*  Bank bank = new Bank();
            bank.DisplayRessources();
            bank.CardsAvailable["Verger"].PileCards.Pop();
            bank.DisplayRessources();*/

        }
    }
}
