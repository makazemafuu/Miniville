using System;
using System.Collections.Generic;

namespace MiniVille_GraphiqueWF
{
    public enum CouleurCarte
    {
        Bleu,
        Rouge,
        Vert,
        Violet
    }
    public enum NomCarte
    {
        ChampsDeBle = 1    ,
        Ferme       = 2    ,
        Boulangerie = 3    ,
        Cafe        = 4    ,
        Superette   = 5    ,
        Foret       = 6    ,
        Stade       = 7    ,
        Affaire     = 8    ,
        Television  = 9    ,
        Fromagerie  = 10   ,
        Fabrique    = 11   ,
        Mine        = 12   ,
        Restaurant  = 13   ,
        Verger      = 14   ,
        Marche      = 15   ,
    }
    public class Cards
    {
        public readonly Dictionary<NomCarte,Object[]> ListeCartes = new Dictionary<NomCarte,Object[]>
         {
            //Nom, Activation Value, Couleur, Coût, Gain, Effet, Type
            { NomCarte.ChampsDeBle, new Object[] { new int[] { 1 },      CouleurCarte.Bleu,   1, 1, "Recevez 1 pièce"                                                       , 1 } },
            { NomCarte.Ferme,       new Object[] { new int[] { 2 },      CouleurCarte.Bleu,   1, 1, "Recevez 1 pièce"                                                       , 2 } },
            { NomCarte.Boulangerie, new Object[] { new int[] { 2, 3 },   CouleurCarte.Vert,   1, 1, "Recevez 2 pièces"                                                      , 3 } },
            { NomCarte.Cafe,        new Object[] { new int[] { 3 },      CouleurCarte.Rouge,  2, 1, "Recevez 1 pièce du joueur qui a lancé le dé"                           , 4 } },
            { NomCarte.Superette,   new Object[] { new int[] { 4 },      CouleurCarte.Vert,   2, 3, "Recevez 3 pièces"                                                      , 3 } },
            { NomCarte.Foret,       new Object[] { new int[] { 5 },      CouleurCarte.Bleu,   3, 1, "Recevez 1 pièce"                                                       , 5 } },
            { NomCarte.Stade,       new Object[] { new int[] { 6 },      CouleurCarte.Violet, 6, 2, "Recevez 2 pièces de la part de chaque autre joueur"                    , 6 } },
            { NomCarte.Affaire,     new Object[] { new int[] { 6 },      CouleurCarte.Violet, 8, 0, "Vous pouvez échanger avec le joueur de votre choix..."                 , 6 } },
            { NomCarte.Television,  new Object[] { new int[] { 6 },      CouleurCarte.Violet, 7, 5, "Recevez 5 pièces du joueur de votre choix"                             , 6 } },
            { NomCarte.Fromagerie,  new Object[] { new int[] { 7 },      CouleurCarte.Vert,   5, 3, "Recevez 3 pièces pour chaque établissement de type 2 que vous possédez", 7 } },
            { NomCarte.Fabrique,    new Object[] { new int[] { 8 },      CouleurCarte.Vert,   3, 3, "Recevez 3 pièces pour chaque établissement de type 5 que vous possédez", 7 } },
            { NomCarte.Mine,        new Object[] { new int[] { 9 },      CouleurCarte.Bleu,   6, 5, "Recevez 5 pièces"                                                      , 5 } },
            { NomCarte.Restaurant,  new Object[] { new int[] { 9, 10 },  CouleurCarte.Rouge,  3, 2, "Recevez 2 pièces du joueur qui a lancé le dé"                          , 4 } },
            { NomCarte.Verger,      new Object[] { new int[] { 10 },     CouleurCarte.Bleu,   3, 3, "Recevez 3 pièces"                                                      , 1 } },
            { NomCarte.Marche,      new Object[] { new int[] { 11, 12 }, CouleurCarte.Bleu,   2, 2, "Recevez 2 pièces pour chaque établissement de type 1 que vous possédez", 8 } }
         };


        public int Cost;
        public int[] ActivationValue;
        public CouleurCarte Color;
        public NomCarte Name;
        public string DescriptionEffect;
        public int Gain;
        public int Type;

        public Cards(NomCarte name = NomCarte.ChampsDeBle)
        {
            Name = name;
            ActivationValue = (int[]) ListeCartes[Name][0];
            Color = (CouleurCarte) ListeCartes[Name][1];
            Cost = (int) ListeCartes[Name][2];
            Gain = (int) ListeCartes[Name][3];
            DescriptionEffect = (string) ListeCartes[Name][4];
            Type = (int)ListeCartes[Name][5];
        }
    }
}
