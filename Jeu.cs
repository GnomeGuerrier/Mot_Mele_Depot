﻿using System;
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
            
           
            int taille = 5; 
            int nbmot = 1;
            int difficulte = 1;
            int tempsTimer = 600;
            bool recommencerJeu = true;
            int aQuiTour = 0;
            string ouAller;
           /* Dictionnaire dico = new Dictionnaire("français");
            Plateau plateau = new Plateau(dico, 1, 5, 2);
            plateau.AfficherGrille();*/
            
          

            
            

            do
            {
            DebutJeu:
                Console.WriteLine("Bonjour, voulez vous commencer une nouvelle partie [N] ou reprendre un jeu [R] ?");
                Console.ForegroundColor = ConsoleColor.Red;
                if (Console.ReadLine() == "R")
                {
                    Console.ResetColor();
                    if (!File.Exists("JeuEnregistre.csv"))
                    {
                        Console.WriteLine("Vous n'avez pas encore enregistré de jeu");
                        goto DebutJeu;
                    }
                        string[] infos = File.ReadAllLines("JeuEnregistre.csv");
                        //Recup et création info Joueur1
                        string[] decoupeJ1 = infos[0].Split(";");
                        Joueur j1 = new Joueur(decoupeJ1[0]);
                        for (int i = 1; i < decoupeJ1.Length - 1; i++)
                        {
                            j1.Add_Mot(decoupeJ1[i]);
                        }
                        j1.Add_Score(Convert.ToInt32(decoupeJ1[decoupeJ1.Length - 2]));

                        //Recup et création info Joueur2
                        string[] decoupeJ2 = infos[1].Split(";");
                        Joueur j2 = new Joueur(decoupeJ2[0]);
                        for (int i = 1; i < decoupeJ2.Length - 1; i++)
                        {
                            j2.Add_Mot(decoupeJ2[i]);
                        }
                        j2.Add_Score(Convert.ToInt32(decoupeJ2[decoupeJ2.Length - 2]));

                        //création dico
                        Dictionnaire dico = new Dictionnaire(infos[2]);
                        //Savoir à qui est le tour
                        aQuiTour = Convert.ToInt32(infos[3]);
                        difficulte = Convert.ToInt32(infos[4]);//connaitre la difficulte où ils etaient
                        string typeJeuReprendre = infos[5];
                        if (typeJeuReprendre == "G")    //meme systeme que sans l'enregistrement mais on passe la phase d'initialisation
                        {
                            
                            
                            do
                            {

                            #region TourJ1

                            TourJ1Simple:
                                Plateau plateau = new Plateau(dico, "CasSimple.csv");
                                List<string> listMotATrouver = plateau.GMotATrouver;
                                Stopwatch swJ1 = new Stopwatch();
                                Console.WriteLine();
                                swJ1.Start();
                                string motDonne, directionDonne;
                                string posXDonne, posYDonne;
                                string[] dataDonne = new string[4];
                                bool reussi = false;
                                SystemeEnregistrement sysEnregistrementJ1 = new SystemeEnregistrement(plateau, j1, j2, dico, 1);
                                sysEnregistrementJ1.EnregistrerTableau();

                                do
                                {
                                    if (listMotATrouver.Count == 0)
                                    {
                                        Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                        reussi = true;
                                        j1.Add_Score(100);
                                        goto FinJ1;
                                    }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                    plateau.AfficherGrille();Console.ResetColor();
                                    Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                               
                                foreach (string m in listMotATrouver)
                                    {
                                        Console.Write(m + " ");
                                    } Console.ResetColor();
                                    Console.WriteLine();
                                    Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                motDonne = Console.ReadLine().ToUpper();
                                Console.ResetColor();
                                Direction:
                                    Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    directionDonne = Console.ReadLine();
                                     Console.ResetColor();
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                    posXDonne = (Console.ReadLine());
                                    Console.ResetColor();
                                    if (!int.TryParse(posXDonne, out int n))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posX;
                                    }
                                posY:
                                    Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posYDonne = (Console.ReadLine());Console.ResetColor();
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
                                        goto FinJ1;
                                    }
                                    else if (plateau.VerifData(dataDonne) && listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                        listMotATrouver.Remove(motDonne);
                                        j1.Add_Mot(motDonne);
                                        j1.Add_Score(10 + Convert.ToInt32(directionDonne) * 5);
                                    }
                                    else if (listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ1:;
                                } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                                swJ1.Stop();
                                swJ1.Reset();
                                
                                Console.Clear();


                            #endregion
                            #region TourJ2
                            TourJ2Simple:
                                //TOUR JOUEUR 2
                                Plateau plateau2 = new Plateau(dico, "CasSimple.csv");
                                List<string> listMotATrouver2 = plateau2.GMotATrouver;
                                Stopwatch swJ2 = new Stopwatch();
                                SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                                Console.WriteLine();
                                swJ2.Start();
                                string motDonne2, directionDonne2;
                                string posXDonne2, posYDonne2;
                                string[] dataDonne2 = new string[4];
                                bool reussi2 = false;

                                do
                                {

                                    if (listMotATrouver2.Count == 0)
                                    {
                                        Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                        reussi2 = true;
                                        j2.Add_Score(100);
                                        goto FinJ2;
                                    }
                                    plateau2.AfficherGrille();
                                    Console.WriteLine(j2.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver2)
                                    {
                                        Console.Write(m + " ");
                                    }Console.ResetColor();
                                    Console.WriteLine();
                                    Console.WriteLine("Donnez le mot que vous voulez trouver");
                                    motDonne2 = Console.ReadLine().ToUpper();

                                Direction:
                                    Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                directionDonne2 = Console.ReadLine();Console.ResetColor();
                                    if (directionDonne2 == "S") directionDonne2 = "1";
                                    else if (directionDonne2 == "N") directionDonne2 = "2";
                                    else if (directionDonne2 == "O") directionDonne2 = "4";
                                    else if (directionDonne2 == "E") directionDonne2 = "3";
                                    else if (directionDonne2 == "NE") directionDonne2 = "6";
                                    else if (directionDonne2 == "NO") directionDonne2 = "8";
                                    else if (directionDonne2 == "SE") directionDonne2 = "5";
                                    else if (directionDonne2 == "SO") directionDonne2 = "7";
                                    else
                                    {
                                        Console.WriteLine("La direction donnée n'est pas bonne, veuillez la ressaisir");
                                        goto Direction;
                                    }
                                posX:
                                    Console.WriteLine("Et enfin la position X de la première lettre du mot");
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                posXDonne2 = (Console.ReadLine()); Console.ResetColor();
                                    if (!int.TryParse(posXDonne2, out int n))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posX;
                                    }
                                posY:
                                    Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posYDonne2 = (Console.ReadLine());Console.ResetColor();
                                    if (!int.TryParse(posYDonne2, out int nn))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posY;
                                    }
                                    dataDonne2[0] = motDonne2;
                                    dataDonne2[1] = directionDonne2;
                                    dataDonne2[2] = posXDonne2;
                                    dataDonne2[3] = posYDonne2;

                                    if (!dico.RechDichoRecursif(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ2;
                                    }
                                    else if (plateau2.VerifData(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                        listMotATrouver2.Remove(motDonne2);
                                        j2.Add_Mot(motDonne2);
                                        j2.Add_Score(10 + Convert.ToInt32(directionDonne2) * 5);
                                    }
                                    else if (listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ2:;
                                } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                           
                            Console.WriteLine("Tour terminé!"); Console.ResetColor();
                                swJ2.Stop();
                                swJ2.Reset();
                                Console.WriteLine("Voulez vous enregistrer la partie?[Y]/[N]");//enregistrement de la partie
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            if (Console.ReadLine() == "Y")
                                {
                                    Console.ResetColor();
                                    sysEnregistrementJ2.EnregisterJeu(j1, j2, dico, difficulte, "G");
                                    goto FinComplete;
                                }
                            Console.ResetColor();
                                Console.Clear();
                                #endregion

                                difficulte++;
                            } while (difficulte <= 5);
                            #region FinJeu
                            Console.WriteLine("Tout les tours sont finis! Bravo à vous deux");
                            Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                            if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                            }
                            else if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                            }
                            else
                            {
                                Console.WriteLine("Vous êtes tout les deux trop fort! Il y a eu égalité");
                            }

                            #endregion FinJeu
                        }
                        if (typeJeuReprendre == "A")
                        {
                            do
                            {

                            #region TourJ1
                            TourJ1:
                                Plateau plateau = new Plateau(dico, difficulte, taille, nbmot);
                                SystemeEnregistrement sysEnregistrementJ1 = new SystemeEnregistrement(plateau, j1, j2, dico, 1);
                                sysEnregistrementJ1.EnregistrerTableau();
                                List<string> listMotATrouver = plateau.GMotATrouver;
                                Stopwatch swJ1 = new Stopwatch();
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
                                        j1.Add_Score(100);
                                        goto FinJ1;
                                    }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                    plateau.AfficherGrille();Console.ResetColor();
                                    Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver)
                                    {
                                        Console.Write(m + " ");
                                    }Console.ResetColor();
                                    Console.WriteLine();
                                    Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                motDonne = Console.ReadLine().ToUpper();Console.ResetColor();

                                Direction:
                                    Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                directionDonne = Console.ReadLine(); Console.ResetColor();
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posXDonne = (Console.ReadLine());Console.ResetColor();
                                    if (!int.TryParse(posXDonne, out int n))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posX;
                                    }
                                posY:
                                    Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                posYDonne = (Console.ReadLine()); Console.ResetColor();
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
                                        goto FinJ1;
                                    }
                                    else if (plateau.Test_Plateau(dataDonne) && listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                        listMotATrouver.Remove(motDonne);
                                        j1.Add_Mot(motDonne);
                                        j1.Add_Score(10 + Convert.ToInt32(directionDonne) * 5);
                                    }
                                    else if (listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ1:;
                                } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                                swJ1.Stop();
                                swJ1.Reset();
                                
                                Console.Clear();
                                #endregion TourJ1
                                #region TourJ2
                                //TOUR JOUEUR 2
                                Plateau plateau2 = new Plateau(dico, difficulte, taille, nbmot);
                                SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                                sysEnregistrementJ2.EnregistrerTableau();
                                List<string> listMotATrouver2 = plateau2.GMotATrouver;
                                Stopwatch swJ2 = new Stopwatch();
                                Console.WriteLine();
                                swJ2.Start();
                                string motDonne2, directionDonne2;
                                string posXDonne2, posYDonne2;
                                string[] dataDonne2 = new string[4];
                                bool reussi2 = false;

                                do
                                {

                                    if (listMotATrouver2.Count == 0)
                                    {
                                        Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                        reussi2 = true;
                                        j2.Add_Score(100);
                                        goto FinJ2;
                                    }
                                    plateau2.AfficherGrille();
                                    Console.WriteLine(j2.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver2)
                                    {
                                        Console.Write(m + " ");
                                    }Console.ResetColor();
                                    Console.WriteLine();
                                    Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                motDonne2 = Console.ReadLine().ToUpper();Console.ResetColor();

                                Direction:
                                    Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                directionDonne2 = Console.ReadLine(); Console.ResetColor();
                                    if (directionDonne2 == "S") directionDonne2 = "1";
                                    else if (directionDonne2 == "N") directionDonne2 = "2";
                                    else if (directionDonne2 == "O") directionDonne2 = "4";
                                    else if (directionDonne2 == "E") directionDonne2 = "3";
                                    else if (directionDonne2 == "NE") directionDonne2 = "6";
                                    else if (directionDonne2 == "NO") directionDonne2 = "8";
                                    else if (directionDonne2 == "SE") directionDonne2 = "5";
                                    else if (directionDonne2 == "SO") directionDonne2 = "7";
                                    else
                                    {
                                        Console.WriteLine("La direction donnée n'est pas bonne, veuillez la ressaisir");
                                        goto Direction;
                                    }
                                posX:
                                    Console.WriteLine("Et enfin la position X de la première lettre du mot");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posXDonne2 = (Console.ReadLine());Console.ResetColor();
                                    if (!int.TryParse(posXDonne2, out int n))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posX;
                                    }
                                posY:
                                    Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                    posYDonne2 = (Console.ReadLine());
                                    if (!int.TryParse(posYDonne2, out int nn))
                                    {
                                        Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                        goto posY;
                                    }
                                    dataDonne2[0] = motDonne2;
                                    dataDonne2[1] = directionDonne2;
                                    dataDonne2[2] = posXDonne2;
                                    dataDonne2[3] = posYDonne2;

                                    if (!dico.RechDichoRecursif(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ2;
                                    }
                                    else if (plateau2.Test_Plateau(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                        listMotATrouver2.Remove(motDonne2);
                                        j2.Add_Mot(motDonne2);
                                        j2.Add_Score(10 + Convert.ToInt32(directionDonne2) * 5);
                                    }
                                    else if (listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ2:;
                                } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                          
                            Console.WriteLine("Tour terminé!");  Console.ResetColor();
                                swJ2.Stop();
                                swJ2.Reset();
                                Console.WriteLine("Voulez vous enregistrer la partie?[Y]/[N]");
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            if (Console.ReadLine() == "Y")
                                {
                                Console.ResetColor();
                                    sysEnregistrementJ2.EnregisterJeu(j1, j2, dico, difficulte, "A");
                                    goto FinComplete;
                                }
                            Console.ResetColor();
                                Console.Clear();
                                #endregion

                                difficulte++;
                            } while (difficulte <= 5);
                            #region FinJeu
                            Console.WriteLine("Tout les tours sont finis! Bravo à vous deux");
                            Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                            if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                            }
                            else if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                            }
                            else
                            {
                                Console.WriteLine("Vous êtes tout les deux trop fort! Il y a eu égalité");
                            }

                            #endregion FinJeu
                        }








                    
                }
                else   //Partie non récupérée
                {
                    Console.ResetColor();
                typeJeu:
                    Console.WriteLine("Voulez vous jouez avec des grilles générée avant [G] ou aléatorement [A] ?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    
                    string typeJeu = Console.ReadLine();Console.ResetColor();
                    if (typeJeu != "G" && typeJeu != "A")
                    {
                        Console.WriteLine("Veuillez choisir un type de jeu valide");
                        goto typeJeu;
                    }
                    if (typeJeu == "G")                      //JeuGénérée
                    {
                        #region IntitialisationJeuSimple
                        Dictionnaire dico = new Dictionnaire("français");
                        string entree;
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        Console.WriteLine("veuillez donner le nom du deuxième joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        Joueur j2 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        Console.WriteLine("Parfait tout est prêt,voici les règles du jeu : \nblaaaaaaablabla\nne pas oublier la regles des points");

                        #endregion IntitialisationJeuSimple
                        do
                        {

                        #region TourJ1

                        TourJ1Simple:
                            Plateau plateau = new Plateau(dico, "CasSimple.csv");
                            List<string> listMotATrouver = plateau.GMotATrouver;
                            Stopwatch swJ1 = new Stopwatch();
                            Console.WriteLine();
                            swJ1.Start();
                            string motDonne, directionDonne;
                            string posXDonne, posYDonne;
                            string[] dataDonne = new string[4];
                            bool reussi = false;
                            SystemeEnregistrement sysEnregistrementJ1 = new SystemeEnregistrement(plateau, j1, j2, dico, 1);
                            sysEnregistrementJ1.EnregistrerTableau();

                            do
                            {
                                if (listMotATrouver.Count == 0)
                                {
                                    Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                    reussi = true;
                                    j1.Add_Score(100);
                                    goto FinJ1;
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                plateau.AfficherGrille();Console.ResetColor();
                                Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                foreach (string m in listMotATrouver)
                                {
                                    Console.Write(m + " ");
                                }
                                Console.ResetColor();
                                Console.WriteLine();
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                motDonne = Console.ReadLine().ToUpper();Console.ResetColor();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                directionDonne = Console.ReadLine();Console.ResetColor();
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
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                posXDonne = (Console.ReadLine()); Console.ResetColor();
                                if (!int.TryParse(posXDonne, out int n))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posX;
                                }
                            posY:
                                Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posYDonne = (Console.ReadLine());Console.ResetColor();
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
                                    goto FinJ1;
                                }
                                else if (plateau.VerifData(dataDonne) && listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                    listMotATrouver.Remove(motDonne);
                                    j1.Add_Mot(motDonne);
                                    j1.Add_Score(10 + Convert.ToInt32(directionDonne) * 5);
                                }
                                else if (listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!");
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                }
                            FinJ1:;
                            } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                            swJ1.Stop();
                            swJ1.Reset();
                            
                            Console.Clear();


                            #endregion
                            #region TourJ2

                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, "CasSimple.csv");
                            List<string> listMotATrouver2 = plateau2.GMotATrouver;
                            Stopwatch swJ2 = new Stopwatch();
                            SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                            Console.WriteLine();
                            swJ2.Start();
                            string motDonne2, directionDonne2;
                            string posXDonne2, posYDonne2;
                            string[] dataDonne2 = new string[4];
                            bool reussi2 = false;

                            do
                            {

                                if (listMotATrouver2.Count == 0)
                                {
                                    Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                    reussi2 = true;
                                    j2.Add_Score(100);
                                    goto FinJ2;
                                }
                                plateau2.AfficherGrille();
                                Console.WriteLine(j2.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver2)
                                {
                                    Console.Write(m + " ");
                                }Console.ResetColor();
                                Console.WriteLine();
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                motDonne2 = Console.ReadLine().ToUpper();Console.ResetColor();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                directionDonne2 = Console.ReadLine();Console.ResetColor();
                                if (directionDonne2 == "S") directionDonne2 = "1";
                                else if (directionDonne2 == "N") directionDonne2 = "2";
                                else if (directionDonne2 == "O") directionDonne2 = "4";
                                else if (directionDonne2 == "E") directionDonne2 = "3";
                                else if (directionDonne2 == "NE") directionDonne2 = "6";
                                else if (directionDonne2 == "NO") directionDonne2 = "8";
                                else if (directionDonne2 == "SE") directionDonne2 = "5";
                                else if (directionDonne2 == "SO") directionDonne2 = "7";
                                else
                                {
                                    Console.WriteLine("La direction donnée n'est pas bonne, veuillez la ressaisir");
                                    goto Direction;
                                }
                            posX:
                                Console.WriteLine("Et enfin la position X de la première lettre du mot");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posXDonne2 = (Console.ReadLine());Console.ResetColor();
                                if (!int.TryParse(posXDonne2, out int n))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posX;
                                }
                            posY:
                                Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                posYDonne2 = (Console.ReadLine()); Console.ResetColor();
                                if (!int.TryParse(posYDonne2, out int nn))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posY;
                                }
                                dataDonne2[0] = motDonne2;
                                dataDonne2[1] = directionDonne2;
                                dataDonne2[2] = posXDonne2;
                                dataDonne2[3] = posYDonne2;

                                if (!dico.RechDichoRecursif(motDonne2))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ2;
                                }
                                else if (plateau2.Test_Plateau(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                    listMotATrouver2.Remove(motDonne2);
                                    j2.Add_Mot(motDonne2);
                                    j2.Add_Score(10 + Convert.ToInt32(directionDonne2) * 5);
                                }
                                else if (listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                }
                            FinJ2:;
                            } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                           
                            Console.WriteLine("Tour terminé!"); Console.ResetColor();
                            swJ2.Stop();
                            swJ2.Reset();
                            Console.WriteLine("Voulez vous enregistrer la partie?[Y]/[N]");//enregistrement de la partie
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            if (Console.ReadLine() == "Y")
                            {

                                sysEnregistrementJ2.EnregisterJeu(j1, j2, dico, difficulte, "G");
                                goto FinComplete;
                            }
                            Console.Clear();
                            #endregion
                            Console.ResetColor();
                            difficulte++;
                        } while (difficulte <= 5);
                        #region FinJeu
                        Console.WriteLine("Tout les tours sont finis! Bravo à vous deux");
                        Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                        if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                        }
                        else if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                        }
                        else
                        {
                            Console.WriteLine("Vous êtes tout les deux trop fort! Il y a eu égalité");
                        }

                        #endregion FinJeu

                    }
                    else                                          //JeuComplexe
                    {
                        #region initialisationDuJeuComplex
                        string entree;
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        Console.WriteLine("veuillez donner le nom du deuxième joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        Joueur j2 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        Console.WriteLine("Parfait! Quel dictionnaire voulez vous utiliser? français/anglais");
                        string langage = "";
                        do
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            langage = Console.ReadLine();Console.ResetColor();
                            if (langage != "français" && langage != "anglais") Console.WriteLine("Langue non supportée,veuillez en saisir une autre");
                        } while (langage != "français" && langage != "anglais");

                        Dictionnaire dico = new Dictionnaire(langage);

                        Console.WriteLine("Parfait tout est prêt,voici les règles du jeu : \nblaaaaaaablabla\nne pas oublier la regles des points");
                        #endregion
                        do
                        {

                        #region TourJ1
                        TourJ1:
                            Plateau plateau = new Plateau(dico, difficulte, taille, nbmot);
                            SystemeEnregistrement sysEnregistrementJ1 = new SystemeEnregistrement(plateau, j1, j2, dico, 1);
                            sysEnregistrementJ1.EnregistrerTableau();
                            List<string> listMotATrouver = plateau.GMotATrouver;
                            Stopwatch swJ1 = new Stopwatch();
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
                                    j1.Add_Score(100);
                                    goto FinJ1;
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                plateau.AfficherGrille();Console.ResetColor();
                                Console.WriteLine(j1.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver)
                                {
                                    Console.Write(m + " ");
                                }Console.ResetColor();
                                Console.WriteLine();
                                
                                
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                                motDonne = Console.ReadLine().ToUpper();Console.ResetColor();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                directionDonne = Console.ReadLine(); Console.ResetColor();
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
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posXDonne = (Console.ReadLine());Console.ResetColor();Console.ResetColor();
                                if (!int.TryParse(posXDonne, out int n))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posX;
                                }
                            posY:
                                Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posYDonne = (Console.ReadLine());Console.ResetColor();
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
                                    goto FinJ1;
                                }
                                else if (plateau.Test_Plateau(dataDonne) && listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                    listMotATrouver.Remove(motDonne);
                                    j1.Add_Mot(motDonne);
                                    j1.Add_Score(10 + Convert.ToInt32(directionDonne) * 5);
                                }
                                else if (listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                }
                            FinJ1:;
                            } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                            swJ1.Stop();
                            swJ1.Reset();
                           
                            Console.Clear();
                            #endregion TourJ1
                            #region TourJ2
                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, difficulte, taille, nbmot);
                            SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                            sysEnregistrementJ2.EnregistrerTableau();
                            List<string> listMotATrouver2 = plateau2.GMotATrouver;
                            Stopwatch swJ2 = new Stopwatch();
                            Console.WriteLine();
                            swJ2.Start();
                            string motDonne2, directionDonne2;
                            string posXDonne2, posYDonne2;
                            string[] dataDonne2 = new string[4];
                            bool reussi2 = false;

                            do
                            {

                                if (listMotATrouver2.Count == 0)
                                {
                                    Console.WriteLine("Bravo tu a trouvé tout les mots de la liste");
                                    reussi2 = true;
                                    j2.Add_Score(100);
                                    goto FinJ2;
                                }
                                plateau2.AfficherGrille();
                                Console.WriteLine(j2.GNom + " a toi de jouer\nLes mots à trouver sont :");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                
                                foreach (string m in listMotATrouver2)
                                {
                                    Console.Write(m + " ");
                                }Console.ResetColor();
                                Console.WriteLine();
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                motDonne2 = Console.ReadLine().ToUpper(); Console.ResetColor();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                directionDonne2 = Console.ReadLine(); Console.ResetColor();
                                if (directionDonne2 == "S") directionDonne2 = "1";
                                else if (directionDonne2 == "N") directionDonne2 = "2";
                                else if (directionDonne2 == "O") directionDonne2 = "4";
                                else if (directionDonne2 == "E") directionDonne2 = "3";
                                else if (directionDonne2 == "NE") directionDonne2 = "6";
                                else if (directionDonne2 == "NO") directionDonne2 = "8";
                                else if (directionDonne2 == "SE") directionDonne2 = "5";
                                else if (directionDonne2 == "SO") directionDonne2 = "7";
                                else
                                {
                                    Console.WriteLine("La direction donnée n'est pas bonne, veuillez la ressaisir");
                                    goto Direction;
                                }
                            posX:
                                Console.WriteLine("Et enfin la position X de la première lettre du mot");
                                Console.ForegroundColor = ConsoleColor.Red;
                               
                                posXDonne2 = (Console.ReadLine()); Console.ResetColor();
                                if (!int.TryParse(posXDonne2, out int n))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posX;
                                }
                            posY:
                                Console.WriteLine("Et la postion Y de la première lettre du mot ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                
                                posYDonne2 = (Console.ReadLine());Console.ResetColor();
                                if (!int.TryParse(posYDonne2, out int nn))
                                {
                                    Console.WriteLine("La coordonnée donnée n'est pas un nombre");
                                    goto posY;
                                }
                                dataDonne2[0] = motDonne2;
                                dataDonne2[1] = directionDonne2;
                                dataDonne2[2] = posXDonne2;
                                dataDonne2[3] = posYDonne2;

                                if (!dico.RechDichoRecursif(motDonne2))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ2;
                                }
                                else if (plateau2.VerifData(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                    listMotATrouver2.Remove(motDonne2);
                                    j2.Add_Mot(motDonne2);
                                    j2.Add_Score(10 + Convert.ToInt32(directionDonne2) * 5);
                                }
                                else if (listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé,réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou tu l'as déja trouvé, réessaie!");

                                }
                            FinJ2:;
                            } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                            swJ2.Stop();
                            swJ2.Reset();
                            Console.WriteLine("Voulez vous enregistrer la partie?[Y]/[N]");
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            if (Console.ReadLine() == "Y")
                            {
                                Console.ResetColor();
                                sysEnregistrementJ2.EnregisterJeu(j1, j2, dico, difficulte, "A");
                                goto FinComplete;
                            }
                            Console.Clear();
                            #endregion
                            Console.ResetColor();
                            difficulte++;
                        } while (difficulte <= 5);
                        #region FinJeu
                        Console.WriteLine("Tout les tours sont finis! Bravo à vous deux");
                        Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                        if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                        }
                        else if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                        }
                        else
                        {
                            Console.WriteLine("Vous êtes tout les deux trop fort! Il y a eu égalité");
                        }

                        #endregion FinJeu
                    }
                }
                Console.WriteLine("Voulez vous recommencer une partie [Y]/[N] ?");
                Console.ForegroundColor = ConsoleColor.Red;
                
                string r = Console.ReadLine();Console.ResetColor();
                r.ToUpper();
                if (r == "N") recommencerJeu = false;
                else
                {
                    Console.WriteLine("Super, c'est parti pour une nouvelle partie!!");
                    Console.Clear();
                    goto DebutJeu;
                }
            } while (recommencerJeu);
        FinComplete:
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Merci d'avoir joué ce jeu crée par Hugo Bonnell et Eliott Coutaz, au plaisir de vous revoir jouer!!!!");Console.ResetColor();
            
        }
        
        
    }
}
