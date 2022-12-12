using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Mot_Mele
{
    public class Dictionnaire
    {
        private string Langage;             // Langue du dictionnaire
        private string[] dico;              // Tableau pour la création de la liste
        private List<string> dicoList=new List<string>();      // Variable de list dictionnaire, à utiliser pour accéder à la liste de tout les mots, séparé et en MAJUSCULE
        private string path;                // Chemin du fichier
        private int Imaxliste;              // Index max du dictionnaire

        public Dictionnaire(string Langage)                                             // Constructeur
        {
            this.Langage = Langage;
            if(this.Langage == "français")                                                   // En fonction de la langue, on initialise le bon dictionnaire
            {
                try
                {
                    path = ("MotsPossiblesFR.txt");                                     // Récupération du fichier
                    dico = File.ReadAllLines(path);
                    
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
                                    this.dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                    this.Imaxliste = this.dicoList.Count();
                    this.dicoList.Sort();
                }
                catch (Exception f)                                                     // En cas d'erreur, pour savoir où déboguer
                {
                    Console.WriteLine(f);
                }
                

            }
            else if (this.Langage == "anglais")                                              // Idem mais pour le dico anglais
            {
                try
                {
                    path = ("MotsPossiblesEN.txt");                                     // Récupération du fichier
                    dico = File.ReadAllLines(path);
                    
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
                                    this.dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                    this.Imaxliste = this.dicoList.Count();
                    this.dicoList.Sort();

                }
                catch (Exception f)                                                     // En cas d'erreur, pour savoir où déboguer
                {
                    Console.WriteLine(f);
                }
                
            }
            else Console.WriteLine("Langage non existant");  
                                                                    // Si le langage n'existe pas
        }
        public string GLangage                      // Déclaration de fonctions get, pour accéder à certaines variables hors de la classe
        {
            get { return this.Langage; }
        }
        public List<string> GDicoList
        {
            get { return this.dicoList; }
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
        
        public bool RechDichoRecursif(string mot, int Imin, int IFin)
        {
            
            if (mot != null && mot != "" && mot != " ")
            {
                int mid = (IFin + Imin) / 2;

                if (this.dicoList[mid] != mot)
                {
                    if (IFin - Imin <= 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (string.Compare(this.dicoList[mid], mot) < 0)
                        {
                            return RechDichoRecursif(mot, mid, IFin);
                        }
                        else
                        {
                            return RechDichoRecursif(mot, Imin, mid);
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            else return false;



        }
    }
}
