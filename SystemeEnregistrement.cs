using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.Threading;
using System.Diagnostics;

namespace Mot_Mele
{
    class SystemeEnregistrement
    {

        private Dictionnaire dico;
        private Joueur j1, j2;
        private Plateau plateau;
        private string path="PlateauEnregistré";
        
        private DateTime temps;
        private List<string> rendu;
        private string[,] grille;
         public SystemeEnregistrement(Plateau plateau)
         {
           // rendu = ;
         }

        public void EnregistrerTableau()
        {
            rendu[0] = plateau.GDifficulte + ";" + plateau.GGetLength0 + ";" + plateau.GGetLength1+";"+plateau.GMotATrouver.Count;
            foreach(string a in plateau.GGrilleRemplie)
            {

            }

            File.WriteAllLines(path + temps + ".csv", rendu);
        }
        public  void ReprendreJeu()
        {
            try
            {
                string[] infos = File.ReadAllLines("test.csv");
                //Recup et création info Joueur1
                string[] decoupeJ1 = infos[0].Split(" ");
                Joueur j1 = new Joueur(decoupeJ1[0]);
                for (int i = 1; i < decoupeJ1.Length - 1; i++)
                {
                    j1.Add_Mot(decoupeJ1[i]);
                }
                j1.Add_Score(Convert.ToInt32(decoupeJ1[decoupeJ1.Length - 1]));
                Console.WriteLine(j1.ToString());
                //Recup et création info Joueur2
                string[] decoupeJ2 = infos[1].Split(" ");
                Joueur j2 = new Joueur(decoupeJ2[0]);
                for (int i = 1; i < decoupeJ2.Length - 1; i++)
                {
                    j2.Add_Mot(decoupeJ2[i]);
                }
                j2.Add_Score(Convert.ToInt32(decoupeJ2[decoupeJ2.Length - 1]));
                Console.WriteLine(j2.ToString());
                //création dico
                Dictionnaire dictionnaire = new Dictionnaire(infos[2]);
                //continuer ici
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("FILE NOT FOUND, CHECK L'EMPLECEMENT, ou le jeu n'as pas été enregistré");
            }
        }
        public  void EnregisterJeu(Joueur j1, Joueur j2, Dictionnaire dico)
        {
            string r = j1.GNom + " ";
            if (j1.GMotTrouve != null)
            {
                foreach (string mot in j1.GMotTrouve)
                {
                    r += mot + " ";
                }
            }

            else r += "null ";

            r += j1.GScore + "\n";

            r += j2.GNom + " ";
            if (j2.GMotTrouve != null)
            {
                foreach (string mot in j2.GMotTrouve)
                {
                    r += mot + " ";
                }
            }
            else r += "null";
            r += j2.GScore + "\n";

            r += dico.GLangage + "\n";
            //continuer ici pour l'enregistrement

            File.WriteAllText("test.csv", r);
        }
    }
}
