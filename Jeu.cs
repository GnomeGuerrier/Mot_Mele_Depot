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

            //Console.WriteLine(dictionnaire.toString());


            //if (dictionnaire.RechDichoRecursif("HELLO")) Console.WriteLine("oui");
            //else Console.WriteLine("non");
            //Enregister(j1, j2, dictionnaire);
            //Recommencer();
            




            
           
            int taille = 10; int nbmot = 10;
            
           
            Console.WriteLine("Bonjour, voulez vous commencer une nouvelle partie [N] ou reprendre un jeu [R] ?");
            if (Console.ReadLine() == "R") Reprendre();
            else
            {
                typeJeu:
                Console.WriteLine("Voulez vous jouez avec des grilles générée avant [G] ou aléatorement [A] ?");
                string typeJeu = Console.ReadLine();
                if(typeJeu!="G"&&typeJeu!= "A")
                {
                    Console.WriteLine("Veuillez choisir un tye de jeu valide");
                    goto typeJeu;
                }
                if (typeJeu == "G")
                {
                    Dictionnaire dico = new Dictionnaire("français");
                    Plateau plateau = new Plateau(dico, "CasSimple.csv");
                    Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                    Joueur j1 = new Joueur(Console.ReadLine());
                    Console.WriteLine("veuillez donner le nom du deuxième joueur");
                    Joueur j2 = new Joueur(Console.ReadLine());
                    Console.WriteLine("Parfait tout est prêt,voici les règles du jeu : \nblaaaaaaablabla");
                    List<string> listMotATrouver = plateau.GMotATrouver;
                    Stopwatch swJ1 = new Stopwatch();
                    Stopwatch swJ2 = new Stopwatch();
                    plateau.AfficherGrille();
                    Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                    foreach (string m in listMotATrouver)
                    {
                        Console.Write(m + " ");
                    }
                    Console.WriteLine();
                    swJ1.Start();
                    string motDonne, directionDonne;
                    string posXDonne, posYDonne;
                    string[] dataDonne = new string[4];
                    bool reussi = false;
                }
                else
                {
                    #region initialisationDuJeuComplex
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
                    Console.WriteLine("Pour commencer, quelle difficultée voulez vous?");
                    int difficulte = -1;
                    do
                    {
                        if (int.TryParse(Console.ReadLine(), out difficulte))
                        {
                            if (difficulte >= 0 && difficulte <= 5) break;
                            else Console.WriteLine("Veuillez sélectionner une difficultée entre 1 et 5");
                        }
                        else Console.WriteLine("Veuillez donner un chiffre");
                    } while (difficulte <= 0 || difficulte > 5);
                    



                    Plateau plateau = new Plateau(dico, difficulte, taille, nbmot);
                    List<string> listMotATrouver = plateau.GMotATrouver;
                    #endregion
                    Stopwatch swJ1 = new Stopwatch();
                    Stopwatch swJ2 = new Stopwatch();
                    Console.WriteLine();
                    swJ1.Start();
                    string motDonne, directionDonne;
                    string posXDonne, posYDonne;
                    string[] dataDonne = new string[4];
                    bool reussi = false;
                    do
                    {
                        if (listMotATrouver.Count == 0)
                        {
                            Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                            reussi = true;
                            goto Fin;
                        }
                        plateau.AfficherGrille();
                        Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                        
                        foreach (string m in listMotATrouver)
                        {
                            Console.Write(m + " ");
                        }
                        Console.WriteLine();
                        Console.WriteLine("Donnez le mot que vous voulez trouver");
                        motDonne = Console.ReadLine().ToUpper();

                    Direction:
                        Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                        directionDonne = Console.ReadLine();
                        if (directionDonne == "S") directionDonne = "1";
                        else if (directionDonne == "N") directionDonne = "2";
                        else if (directionDonne == "O") directionDonne = "4";
                        else if (directionDonne == "E") directionDonne = "3";
                        else if (directionDonne == "NE") directionDonne = "6";
                        else if (directionDonne == "NO") directionDonne = "8";
                        else if (directionDonne == "SE") directionDonne = "5";
                        else if (directionDonne == "SO") directionDonne = "7";
                        else
                        {
                            Console.WriteLine("La direction donnée n'est pas bonne, veuillez la ressaisir");
                            goto Direction;
                        }
                    posX:
                        Console.WriteLine("Et enfin la position X de la première lettre du mot");

                        posXDonne = (Console.ReadLine());
                        if (!int.TryParse(posXDonne, out int n))
                        {
                            Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                            goto posX;
                        }
                    posY:
                        Console.WriteLine("Et la postion Y de la première lettre du mot ");
                        posYDonne = (Console.ReadLine());
                        if (!int.TryParse(posYDonne, out int nn))
                        {
                            Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                            goto posY;
                        }
                        dataDonne[0] = motDonne;
                        dataDonne[1] = directionDonne;
                        dataDonne[2] = posXDonne;
                        dataDonne[3] = posYDonne;

                        if (!dico.RechDichoRecursif(motDonne))
                        {
                            Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                            goto Fin;
                        }
                        else if (plateau.Test_Plateau(dataDonne) && listMotATrouver.Contains(motDonne))
                        {
                            Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                            listMotATrouver.Remove(motDonne);
                        }
                        else if (listMotATrouver.Contains(motDonne))
                        {
                            Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                        }
                        else
                        {
                            Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                        }
                    Fin:;
                    } while (swJ1.ElapsedMilliseconds / 1000 <= 600 || reussi == true); //Crhonomètre et condition de victoire
                    Console.WriteLine("Tour terminé!");

                }
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
