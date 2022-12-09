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
            
            int taille = 5; 
            int nbmot = 2;
            int difficulte = 1;
            int tempsTimer = 600;
            bool recommencerJeu = true;


            /*Dictionnaire dico = new Dictionnaire("français");
            Plateau plateau = new Plateau(dico, 1, 5, 2);
            plateau.AfficherGrille();
           
            List<string> listMotATrouver = plateau.GMotATrouver;
            foreach(string a in listMotATrouver)
            {
                Console.Write(a+" ");
            }
            Console.WriteLine();
            Plateau plateau2 = new Plateau(dico, 1, 5, 2);
            plateau2.AfficherGrille();
            List<string> listMotATrouver2 = plateau2.GMotATrouver;
            foreach (string a in listMotATrouver2)
            {
                Console.Write(a+" ");
            }
            Console.WriteLine();*/


            

            do
            {
                DebutJeu:
                Console.WriteLine("Bonjour, voulez vous commencer une nouvelle partie [N] ou reprendre un jeu [R] ?");
                if (Console.ReadLine() == "R") Console.WriteLine("rrrr"); //Reprendre();
                else
                {
                typeJeu:
                    Console.WriteLine("Voulez vous jouez avec des grilles générée avant [G] ou aléatorement [A] ?");
                    string typeJeu = Console.ReadLine();
                    if (typeJeu != "G" && typeJeu != "A")
                    {
                        Console.WriteLine("Veuillez choisir un type de jeu valide");
                        goto typeJeu;
                    }
                    if (typeJeu == "G")                      //Générée
                    {
                        #region IntitialisationJeuSimple
                        Dictionnaire dico = new Dictionnaire("français");
                        string entree;
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        entree = Console.ReadLine();
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1));
                        Console.WriteLine("veuillez donner le nom du deuxième joueur");
                        entree = Console.ReadLine();
                        Joueur j2 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1));
                        Console.WriteLine("Parfait tout est prêt,voici les règles du jeu : \nblaaaaaaablabla\nne pas oublier la regles des points");
                        #endregion IntitialisationJeuSimple
                        do
                        {
                        #region TourJ1
                        TourJ1:
                            Plateau plateau = new Plateau(dico, "CasSimple.csv");
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
                            } while (swJ1.ElapsedMilliseconds / 1000 <=  tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.WriteLine("Tour terminé!");
                            swJ1.Stop();
                            swJ1.Reset();
                            Console.ReadKey();
                            Console.Clear();
                            #endregion
                            #region TourJ2

                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, "CasSimple.csv");
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

                                foreach (string m in listMotATrouver2)
                                {
                                    Console.Write(m + " ");
                                }
                                Console.WriteLine();
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                motDonne2 = Console.ReadLine().ToUpper();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                directionDonne2 = Console.ReadLine();
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

                                posXDonne2 = (Console.ReadLine());
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
                            } while (swJ2.ElapsedMilliseconds / 1000 <=  tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.WriteLine("Tour terminé!");
                            swJ2.Stop();
                            swJ2.Reset();
                            Console.ReadKey();
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
                    else                                          //JeuComplexe
                    {
                        #region initialisationDuJeuComplex
                        string entree;
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        entree = Console.ReadLine();
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1));
                        Console.WriteLine("veuillez donner le nom du deuxième joueur");
                        entree = Console.ReadLine();
                        Joueur j2 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1));
                        Console.WriteLine("Parfait! Quel dictionnaire voulez vous utiliser? français/anglais");
                        string langage = "";
                        do
                        {
                            langage = Console.ReadLine();
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
                            } while (swJ1.ElapsedMilliseconds / 1000 <=  tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.WriteLine("Tour terminé!");
                            swJ1.Stop();
                            swJ1.Reset();
                            Console.ReadKey();
                            Console.Clear();
                            #endregion TourJ1
                            #region TourJ2
                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, difficulte, taille, nbmot);
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

                                foreach (string m in listMotATrouver2)
                                {
                                    Console.Write(m + " ");
                                }
                                Console.WriteLine();
                                Console.WriteLine("Donnez le mot que vous voulez trouver");
                                motDonne2 = Console.ReadLine().ToUpper();

                            Direction:
                                Console.WriteLine("Puis la direction( Nord [N], Sud [S], Est [E], Ouest [O], Nord_Est [NE], Nord_Ouest [NO], Sud_Est [SE], Sud_Ouest [SO] )");// A VERIFIER AVEC HUGO
                                directionDonne2 = Console.ReadLine();
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

                                posXDonne2 = (Console.ReadLine());
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
                            } while (swJ2.ElapsedMilliseconds / 1000 <=  tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.WriteLine("Tour terminé!");
                            swJ2.Stop();
                            swJ2.Reset();
                            Console.ReadKey();
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
                Console.WriteLine("Voulez vous recommencer une partie [Y]/[N] ?");
                string r = Console.ReadLine();
                r.ToUpper();
                if (r == "N") recommencerJeu = false;
                else
                {
                    Console.WriteLine("Super, c'est parti pour une nouvelle partie!!");
                    Console.Clear();
                    goto DebutJeu;
                }
            } while (recommencerJeu);
            Console.WriteLine("Merci d'avoir joué ce jeu crée par Hugo Bonnell et Eliott Coutaz, au plaisir de vous revoir jouer!!!!");

        }
        
        
    }
}
