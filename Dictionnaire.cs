using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Mot_Mele
{
    class Dictionnaire
    {
        private string Langage;             // Langue du dictionnaire
        private string[] dico;              // Tableau pour la création de la liste
        private List<string> dicoList;      // Variable de list dictionnaire, à utiliser pour accéder à la liste de tout les mots, séparé et en MAJUSCULE
        private string path;                // Chemin du fichier
        private int Imaxliste;              // Index max du dictionnaire

        public Dictionnaire(string Langage)                                             // Constructeur
        {
            this.Langage = Langage;
            if(Langage == "français")                                                   // En fonction de la langue, on initialise le bon dictionnaire
            {
                try
                {
                    path = ("MotsPossiblesFR.txt");                                     // Récupération du fichier
                    dico = File.ReadAllLines(path);
                    dicoList = new List<string>();
                    for (int i = 0; i < dico.Length; i++)
                    {
                        string[] toutLesMots = dico[i].Split(" ");
                        if (toutLesMots.Length > 1)
                        {
                            for (int j = 0; j < toutLesMots.Length; j++)                // Sépare par taille de mots
                            {
                                string[] motParMot = toutLesMots[j].Split(" ");
                                for (int l = 0; l < motParMot.Length; l++)              // Sépare mot par mot
                                {
                                    dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                    this.Imaxliste = dicoList.Count();
                }
                catch (Exception f)                                                     // En cas d'erreur, pour savoir où déboguer
                {
                    Console.WriteLine(f);
                }
                

            }
            else if (Langage == "anglais")                                              // Idem mais pour le dico anglais
            {
                try
                {
                    path = ("MotsPossiblesEN.txt");                                     // Récupération du fichier
                    dico = File.ReadAllLines(path);
                    dicoList = new List<string>();
                    for (int i = 0; i < dico.Length; i++)
                    {
                        string[] toutLesMots = dico[i].Split(" ");
                        if (toutLesMots.Length > 1)
                        {
                            for (int j = 0; j < toutLesMots.Length; j++)                // Sépare par taille de mots
                            {
                                string[] motParMot = toutLesMots[j].Split(" ");
                                for (int l = 0; l < motParMot.Length; l++)              // Sépare mot par mot
                                {
                                    dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                    this.Imaxliste = dicoList.Count();
                }
                catch (Exception f)                                                     // En cas d'erreur, pour savoir où déboguer
                {
                    Console.WriteLine(f);
                }

            }
            else Console.WriteLine("Langage non existant");                             // Si le langage n'existe pas
        }
        public string GLangage                      // Déclaration de fonctions get, pour accéder à certaines variables hors de la classe
        {
            get { return this.Langage; }
        }
        public List<string> GDicoList
        {
            get { return dicoList; }
        }
        
        /// <summary>
        /// Permet de compter le nombre de mots par rapport à leurs tailles, avec comme méthode de lecture du fichier le streamreader
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string r = "Le dictionnaire est en " + Langage + " :\n";
            for (int i = 0; i < dico.Length; i++)
            {
                string[] toutLesMots = dico[i].Split(" ");
                if (toutLesMots.Length > 1)
                {
                    r += "qui contient " + toutLesMots.Length + " mots de " + toutLesMots[i].Length + " lettres\n";
                }

            }
            return r;
        }


        /// <summary>
        /// check si le mot existe dans le dictionnaire
        /// </summary>
        /// <param name="mot">mot à checker EN MAJUSCULE</param>
        /// <returns></returns>
        /// 
        /*public bool RechDichoRecursif(string mot, int Imin = 0, int b = -2, int mot_taille = 0)
        {

            int milieu = (Imin + b) / 2;
            if (b == -2)
            {
                mot = mot.ToUpper();
                mot_taille = mot.Length ;
                return RechDichoRecursif(mot, Imin, this.dico[mot_taille].Length - 1, mot_taille);
            }
            else if ((Imin + 1 == b || b < 0) && (Imin != 0 && b != this.dico[mot_taille].Length - 1)) { return false; }
            else if (this.dico[mot_taille][milieu].CompareTo(mot) < 0)
            {
                return RechDichoRecursif(mot, milieu, b, mot_taille);
            }
            else if (this.dico[mot_taille][milieu].CompareTo(mot) > 0)
            {
                return RechDichoRecursif(mot, Imin, milieu, mot_taille);
            }
            else
            {
                return true;
            }
        }*/
       public bool RechDichoRecursif(string mot)
        {
             bool r = false;

             if (this.dicoList.Contains(mot)) r = true;
             return r;
            
        }
    }
}
