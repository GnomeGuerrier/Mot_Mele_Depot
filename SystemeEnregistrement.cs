using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Data;

namespace Mot_Mele
{
    class SystemeEnregistrement
    {

        private Dictionnaire dico;
        private Joueur j1, j2;
        private Plateau plateau;
        private string path="PlateauEnregistré";
        private List<string> motATrouver = new List<string>();
        private DateTime temps=DateTime.Now;
        private int aQuiTour;
        private List<string> rendu=new List<string>();
        private string[,] grille;
         public SystemeEnregistrement(Plateau plateau)//Enregistre le tableau donné 
         {
           // rendu = ;
           this.plateau = plateau;
           this.motATrouver = plateau.GMotATrouver;
            this.grille = plateau.GGrilleRemplie;
            
         }
        public SystemeEnregistrement(Plateau plateau, Joueur j1, Joueur j2, Dictionnaire dico,int aQuiTour)
        {
            this.dico= dico;
            this.j1 = j1;
            this.j2 = j2;
            this.plateau = plateau;
            this.motATrouver = plateau.GMotATrouver;
            this.grille = plateau.GGrilleRemplie;
            this.aQuiTour= aQuiTour;
           
        }





          /// <summary>
          /// Permet d'enregistrer tout les tableaux générées aléatoirement
          /// </summary>
        public void EnregistrerTableau()
        {
            this.rendu.Clear();
            this.rendu.Add( this.plateau.GDifficulte + ";" + this.plateau.GGetLength0 + ";" + this.plateau.GGetLength1+";"+this.plateau.GMotATrouver.Count);
            string mots="";
            string ligne = "";
            foreach(string i in this.motATrouver)
            {
                mots += i + ";";
            }
            this.rendu.Add(mots);
            for (int i = 0; i < this.grille.GetLength(0); i++)
            {
                for (int j = 0; j < this.grille.GetLength(1); j++)
                {
                    ligne += this.grille[i, j]+";";

                }
                this.rendu.Add(ligne);
                ligne = "";
            }

            
            File.WriteAllLines(path +"_"+ temps.Day + "_" + temps.Month + "_" + temps.Year + "_" + temps.Hour + "_" +"_"+temps.Minute+"_"+temps.Second+ ".csv", this.rendu); //Enregistre le tableau avec comme syntaxe PlateauEnregistreJour_Mois_Anne_Heure_Minute_Seconde
        }
       
        public  void EnregisterJeu(Joueur j1, Joueur j2, Dictionnaire dico,int difficulte,string typeJeu)
        {
            /*string r = j1.GNom + " ";
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

            File.WriteAllText("test.csv", r);*/
            this.rendu.Clear();

            string r = j1.GNom + ";";       //enregistrement J1
            if (j1.GMotTrouve != null)
            {
                foreach (string mot in j1.GMotTrouve)
                {
                    r += mot + ";";
                }
            }

            else r += "null;";
            r += j1.GScore + ";";
            this.rendu.Add(r);
            r = "";
            r += j2.GNom + ";";     //enregistrement J2
            if (j2.GMotTrouve != null)
            {
                foreach (string mot in j2.GMotTrouve)
                {
                    r += mot + ";";
                }
            }
            else r += "null";
            r += j2.GScore + ";";
            this.rendu.Add(r);
            r = "";
            r += dico.GLangage ;//enregistrement dico
            this.rendu.Add(r);
            r = "";
            r = Convert.ToString(this.aQuiTour);//enregistrement du prochain joueur à jouer
            this.rendu.Add(r);
            r = "";
            r = Convert.ToString(difficulte+1);//on fait plus 1 pour ajouter la difficultée non comptée lors de l'enregistrement
            this.rendu.Add(r);
            r = "";
            this.rendu.Add(typeJeu);
            File.WriteAllLines("JeuEnregistre.csv", this.rendu);
            rendu.Add(typeJeu);
        }
        
    }
}
