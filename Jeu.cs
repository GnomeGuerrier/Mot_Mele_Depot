using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Mot_Mele
{
    class Jeu
    {
        static void Main(string[] args)
        {
             /*Joueur j1 = new Joueur("eliott");
            j1.Add_Mot("test1");
            j1.Add_Mot("test1");
            j1.Add_Mot("test1");
            j1.Add_Score(22);
            Joueur j2 = new Joueur("hugo");
            j2.Add_Mot("test2");
            j2.Add_Mot("test2");
            
            Console.WriteLine(j1.ToString());*/
            Dictionnaire dictionnaire = new Dictionnaire("français");
            //Console.WriteLine(dictionnaire.toString());
            int taille = 10;int nbmot = 17;
            Plateau plateau = new Plateau(dictionnaire, 5,taille,nbmot);
            //if (dictionnaire.RechDichoRecursif("HELLO")) Console.WriteLine("oui");
            //else Console.WriteLine("non");
            //Enregister(j1, j2, dictionnaire);
            //Recommencer();
            foreach(string t in plateau.GMotAjoute)
            {
                Console.Write(t+" ");
            }
            




            Console.WriteLine("Bonjour, voulez vous reprendre un  jeu? oui/non");
            if (Console.ReadLine() == "OUI") Reprendre();
            else
            {
                Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                Joueur j1 = new Joueur(Console.ReadLine());
                Console.WriteLine("veuillez donner le nom du deuxième joueur");
                Joueur j2 = new Joueur(Console.ReadLine());
                Console.WriteLine("Parfait! Quel dictionnaire voulez vous utiliser? français/anglais");
                string langage = "";
                do
                {
                    langage = Console.ReadLine();
                    if (langage != "français" && langage != "anglais") Console.WriteLine("Langue non supportée,veuillez en saisir une autre");
                } while (langage != "français" && langage != "anglais");
                Dictionnaire dico = new Dictionnaire(langage);
                Console.WriteLine("Parfait tout est prêt,voici les règles du jeu : \nblaaaaaaablabla");

            
            }




        }
        public static void Enregister(Joueur j1,Joueur j2, Dictionnaire dico)
        {
            string r = j1.GNom+" ";
            if (j1.GMotTrouve != null)
            {
                foreach (string mot in j1.GMotTrouve)
                {
                    r += mot + " ";
                }
            }
            
            else r += "null ";
            
            r += j1.GScore+"\n";
            
            r += j2.GNom + " ";
            if (j2.GMotTrouve != null)
            {
                foreach (string mot in j2.GMotTrouve)
                {
                    r += mot + " ";
                }
            }
            else r += "null";
            r += j2.GScore+"\n";
            
            r += dico.GLangage+"\n";
            //continuer ici pour l'enregistrement

            File.WriteAllText("test.csv", r);
        }
        public static void Reprendre()
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
    }
}
