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
        private string Langage;
        private string[] dico;//tableau pour la création de la list
        private List<string> dicoList; // variable de list dictionnaire, à utiliser pour accéder à la liste de tout les mots, séparé et en MAJUSCULE
        private string path;//chemin du fichier
        private int Imaxliste;

        public Dictionnaire(string Langage)
        {
            this.Langage = Langage;
            if(Langage == "français")
            {
                try
                {
                    path = ("MotsPossiblesFR.txt");
                    dico = File.ReadAllLines(path);
                    dicoList = new List<string>();
                    for (int i = 0; i < dico.Length; i++)
                    {
                        string[] toutLesMots = dico[i].Split(" ");
                        if (toutLesMots.Length > 1)
                        {
                            for (int j = 0; j < toutLesMots.Length; j++)//sépare par taille de mots
                            {
                                string[] motParMot = toutLesMots[j].Split(" ");
                                for (int l = 0; l < motParMot.Length; l++)//sépare mot par mot
                                {
                                    dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                }
                catch (Exception f)
                {
                    Console.WriteLine(f);
                }
                

            }
            else if (Langage == "anglais")
            {
                try
                {
                    path = ("MotsPossiblesEN.txt");
                    dico = File.ReadAllLines(path);
                    dicoList = new List<string>();
                    for (int i = 0; i < dico.Length; i++)
                    {
                        string[] toutLesMots = dico[i].Split(" ");
                        if (toutLesMots.Length > 1)
                        {
                            for (int j = 0; j < toutLesMots.Length; j++)//sépare par taille de mots
                            {
                                string[] motParMot = toutLesMots[j].Split(" ");
                                for (int l = 0; l < motParMot.Length; l++)//sépare mot par mot
                                {
                                    dicoList.Add(motParMot[l].Trim());
                                }
                            }
                        }
                    }
                    Imaxliste = dicoList.Count();
                }
                  
                catch(Exception f)
                {
                    Console.WriteLine(f);
                }
                
            }
            else Console.WriteLine("Langage non existant");
        }
        public string GLangage
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
        
       public bool RechDichoRecursif(string mot/*, int Imaxliste, int Iminliste = 0*/)
        {
             bool r = false;

             if (this.dicoList.Contains(mot)) r = true;
             return r;
            /*dicoList.Sort();



            ///recherche du mot dans la liste en récursif
            if (dicoList[(Iminliste + Imaxliste) / 2].CompareTo(mot) > 0 && Convert.ToDouble(Math.Abs(Iminliste - Imaxliste)) / 2 >= 1)
            {
                return RechDichoRecursif(mot, Iminliste, (Iminliste + Imaxliste) / 2);
            }
             if (dicoList[(Iminliste + Imaxliste) / 2].CompareTo(mot) < 0 && Convert.ToDouble(Math.Abs(Iminliste - Imaxliste)) / 2 >= 1)
            {
                return RechDichoRecursif(mot, (Iminliste + Imaxliste) / 2, Imaxliste);
            }
             if (dicoList[(Iminliste + Imaxliste) / 2].CompareTo(mot) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }*/




        }
    }
}
