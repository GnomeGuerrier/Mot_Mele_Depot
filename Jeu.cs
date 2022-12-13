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


            int taille = 7;             //Choix de la taille de la première manche, le reste de la taille en dépendra
            int nbmot = 8;             //Choix du nombre de mot de la première manche, le reste des mots en dépendront
            int difficulte = 1;         // choix de la difficulté du premier niveau, le reste et le nombre de tours en dépendra
            int tempsTimer = 120;       //choix du temps par manche par joueur, du temps s'ajoutera selon le niveau de difficulté
            bool recommencerJeu = true; //permet de savor si un joueur veut recommencer un jeu
            
            int tempsJ1 = 0;            //temps de jeu du joueur1
            int tempsJ2 = 0;            //temps de jeu du joueur1







            do                              //Début du jeu
            {
            DebutJeu:
                Console.WriteLine("Bonjour, voulez vous commencer une nouvelle partie [N] ou reprendre un jeu [R] ?");
                Console.ForegroundColor = ConsoleColor.Red;
                if (Console.ReadLine().ToUpper() == "R")  //Début du jeu si on veut reprendre une partie
                {
                    Console.ResetColor();
                    if (!File.Exists("JeuEnregistre.csv")) //check si une partie a été enregistrée
                    {
                        Console.WriteLine("Vous n'avez pas encore enregistré de jeu");
                        goto DebutJeu;
                    }
                        string[] infos = File.ReadAllLines("JeuEnregistre.csv"); //partie où on extrait toutes les infos pour recommencer un jeu
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
                        
                        
                        difficulte = Convert.ToInt32(infos[4]);                 //connaitre la difficulte où ils etaient
                        string typeJeuReprendre = infos[5];
                        if (typeJeuReprendre == "G")                          //meme systeme que sans l'enregistrement mais on passe la phase d'initialisation
                        {
                            
                            
                            do
                            {

                            #region TourJ1      
                                                                                //début du tour J1 généré dans le cas où on reprend un jeu
                            TourJ1Simple:
                                Plateau plateau = new Plateau(dico, "CasJ1" + difficulte + ".csv"); //création du nouveau plateau
                                List<string> listMotATrouver = plateau.GMotATrouver; //récupération des mots à trouver
                                Stopwatch swJ1 = new Stopwatch();                   //création d'un chronomètre
                                Console.WriteLine();
                                swJ1.Start();                                       //chrono start
                                string motDonne, directionDonne;                    //initialisation des variables pour trouver le mot
                                string posXDonne, posYDonne;                        //initialisation des variables pour trouver le mot
                                string[] dataDonne = new string[4];                 
                                bool reussi = false;
                                SystemeEnregistrement sysEnregistrementJ1 = new SystemeEnregistrement(plateau, j1, j2, dico, 1);  //Systeme d'enregistremement de tableau et de plateau
                                sysEnregistrementJ1.EnregistrerTableau();

                                do
                                {
                                    if (listMotATrouver.Count == 0)
                                    {
                                        Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                        reussi = true;
                                        j1.Add_Score(20);
                                        goto FinJ1;
                                    }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                    plateau.AfficherGrille();Console.ResetColor();
                                    Console.WriteLine(j1.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                                    directionDonne = Console.ReadLine().ToUpper();
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

                                    if (!dico.RechDichoRecursif(motDonne,0,plateau.GDicoListCount)) //test si le mot est dans le dictionnaire
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ1;
                                    }
                                    else if (plateau.VerifData(dataDonne) && listMotATrouver.Contains(motDonne)) //test si le mot est au bon endroit dans le plateau
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                        listMotATrouver.Remove(motDonne);
                                        j1.Add_Mot(motDonne);
                                        j1.Add_Score(motDonne.Length);
                                    }
                                    else if (listMotATrouver.Contains(motDonne)) //test si le mot est à trouver
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ1:;
                                } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ1.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé !");Console.ResetColor();
                            tempsJ1 += (int)(swJ1.ElapsedMilliseconds / 1000);

                                swJ1.Stop();
                                swJ1.Reset();
                                Console.WriteLine("Appuyez sur n'importe quelle touche pour commencer le tour du prochain joueur");
                                Console.ReadKey();  
                                Console.Clear();


                            #endregion
                            #region TourJ2
                            TourJ2Simple:
                                                                                                         //TOUR JOUEUR 2
                                Plateau plateau2 = new Plateau(dico, "CasJ2" + difficulte + ".csv");     //Création J2
                                List<string> listMotATrouver2 = plateau2.GMotATrouver;                  //récupération des mots à chercher
                                Stopwatch swJ2 = new Stopwatch();
                                SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);//système d'enregistrement
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
                                        Console.WriteLine("Bravo tu as trouvé tout les mots de la liste");
                                        reussi2 = true;
                                        j2.Add_Score(20);
                                        goto FinJ2;
                                    }
                                    plateau2.AfficherGrille();
                                    Console.WriteLine(j2.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                                
                                directionDonne2 = Console.ReadLine().ToUpper();Console.ResetColor();
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

                                    if (!dico.RechDichoRecursif(motDonne2, 0, plateau.GDicoListCount))
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ2;
                                    }
                                    else if (plateau2.VerifData(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                        listMotATrouver2.Remove(motDonne2);
                                        j2.Add_Mot(motDonne2);
                                        j2.Add_Score(motDonne2.Length);
                                    }
                                    else if (listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); 
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ2:;
                                } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ2.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé!"); Console.ResetColor();
                            tempsJ2 += (int)(swJ2.ElapsedMilliseconds / 1000);
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
                            Console.WriteLine("Appuyez sur n'importe quelle touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                                #endregion

                                difficulte++;//ajout de la difficultée
                                nbmot+=5;//ajout de plus de mots pour la prochaine manche
                                taille++;//ajout de la taille en plus pour la prochaine manche
                            tempsTimer += 5 * difficulte;//ajout de temps pour la prochaine manche
                        } while (difficulte <= 5);
                            #region FinJeu
                            Console.WriteLine("Tous les tours sont terminés ! Bravo à vous deux");
                            Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                            if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                            }
                            else if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                            }
                        else if (tempsJ1 > tempsJ2)
                        {
                            Console.WriteLine($"Vous êtes tous les deux trop fort! Il y a eu égalité au niveau des points, mais {j2.GNom} a résolu les plateaux en moins de temps.");
                        }
                        else
                        {
                            Console.WriteLine($"Vous êtes tout les deux trop fort! Il y a eu égalité au niveau des points, mais {j1.GNom} a résolu les plateaux en moins de temps.");
                        }

                        #endregion FinJeu
                    }
                        if (typeJeuReprendre == "A") //Jeu récupéré de type Aléatoire
                        {
                            do
                            {

                             #region TourJ1
                        TourJ1:
                            Plateau plateau = new Plateau(dico, difficulte, taille, nbmot);//
                            
                                plateau.ToFile("PlateauEnregistre");
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
                                        Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                        reussi = true;
                                        j1.Add_Score(20);
                                        goto FinJ1;
                                    }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                    plateau.AfficherGrille();Console.ResetColor();
                                    Console.WriteLine(j1.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                               
                                directionDonne = Console.ReadLine().ToUpper(); Console.ResetColor();
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

                                    if (!dico.RechDichoRecursif(motDonne, 0, plateau.GDicoListCount))
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ1;
                                    }
                                    else if (plateau.Test_Plateau(dataDonne) && listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                        listMotATrouver.Remove(motDonne);
                                        j1.Add_Mot(motDonne);
                                        j1.Add_Score(motDonne.Length);
                                    }
                                    else if (listMotATrouver.Contains(motDonne))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); //DONNE TJRS CE RESULTAT
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ1:;
                                } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ1.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé !");Console.ResetColor();
                            tempsJ1 += (int)(swJ1.ElapsedMilliseconds / 1000);
                            swJ1.Stop();
                                swJ1.Reset();
                            Console.WriteLine("Appuyez sur n'importe quelle touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                                #endregion TourJ1
                                #region TourJ2
                                //TOUR JOUEUR 2
                                Plateau plateau2 = new Plateau(dico, difficulte, taille, nbmot);
                            SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                            plateau2.ToFile("PlateauEnregistre");
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
                                        Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                        reussi2 = true;
                                        j2.Add_Score(20);
                                        goto FinJ2;
                                    }
                                    Console.ForegroundColor= ConsoleColor.Green;
                                    plateau2.AfficherGrille();Console.ResetColor();
                                    Console.WriteLine(j2.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                               
                                directionDonne2 = Console.ReadLine().ToUpper(); Console.ResetColor();
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

                                    if (!dico.RechDichoRecursif(motDonne2, 0, plateau.GDicoListCount))
                                    {
                                        Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                        goto FinJ2;
                                    }
                                    else if (plateau2.Test_Plateau(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                        listMotATrouver2.Remove(motDonne2);
                                        j2.Add_Mot(motDonne2);
                                        j2.Add_Score(motDonne2.Length);
                                    }
                                    else if (listMotATrouver2.Contains(motDonne2))
                                    {
                                        Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); //DONNE TJRS CE RESULTAT
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                    }
                                FinJ2:;
                                } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ2.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé!");  Console.ResetColor();
                            tempsJ2 += (int)(swJ2.ElapsedMilliseconds / 1000);
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
                            Console.WriteLine("Appuyez sur n'importe quel touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                                #endregion
                                
                                difficulte++;//ajout de la difficultée

                            nbmot += 5;//ajout de mot pour la prochaine manche
                            taille ++;//ajout de la taille pour la prochaine manche
                            tempsTimer += 5 * difficulte;//ajout de temps pour la prochaine manche
                        } while (difficulte <= 5);
                            #region FinJeu
                            Console.WriteLine("Tous les tours sont terminés ! Bravo à vous deux");
                            Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                            if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                            }
                            else if (j2.GScore > j1.GScore)
                            {
                                Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                            }
                        else if (tempsJ1 > tempsJ2)
                        {
                            Console.WriteLine($"Vous êtes tous les deux trop fort! Il y a eu égalité au niveau des points, mais {j2.GNom} a résolu les plateaux en moins de temps.");
                        }
                        else
                        {
                            Console.WriteLine($"Vous êtes tous les deux trop fort! Il y a eu égalité au niveau des points, mais {j1.GNom} a résolu les plateaux en moins de temps.");
                        }

                        #endregion FinJeu
                    }








                    
                }
                else   //Partie non reprise, initialisation de la nouvelle partie
                {
                    Console.ResetColor();
                typeJeu:
                    Console.WriteLine("Voulez vous jouez avec des grilles pré-générées [G] ou des grilles générées aléatoirement [A] ?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    
                    string typeJeu = Console.ReadLine().ToUpper();Console.ResetColor();
                    if (typeJeu != "G" && typeJeu != "A")
                    {
                        Console.WriteLine("Veuillez choisir un type de jeu valide");
                        goto typeJeu;
                    }
                    if (typeJeu == "G")                      //JeuGénérée
                    {
                                                             //initialisation d'un nouveau jeu généré 
                        #region IntitialisationJeuSimple
                        Dictionnaire dico = new Dictionnaire("français");
                        string entree;
                    J1entre:
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        if (entree == null || entree == "") goto J1entre;
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        J2entre:
                        Console.WriteLine("Veuillez donner le nom du deuxième joueur");
                        Console.ForegroundColor = ConsoleColor.Red;
                        
                        entree = Console.ReadLine();Console.ResetColor();
                        if (entree == null || entree == "") goto J2entre;
                        Joueur j2 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                        Console.WriteLine("Parfait tout est prêt, voici les règles du jeu : \nChaque joueur a une grille de mot caché qu’il doit trouver dans le temps imparti d’une minute pour le premier tableau. \r\nLe jouer remporte un bonus s’il trouve tous les mots de sa grille, sinon, il remporte un score égal au nombre de lettres des mots trouvés.\r\nLe joueur suivant fera de même sur une nouvelle grille.\r\nA chaque tour, la dimension de la grille et le nombre de mots cachés augmentent, ainsi que la difficulté et le temps imparti.\r\nLe gagnant est celui qui sera le plus rapide pour trouver tous les mots cachés ou celui qui aura le score le plus élevé.\n Chargement de la grille en cours...");

                        #endregion IntitialisationJeuSimple             
                        do
                        {

                        #region TourJ1

                        TourJ1Simple:
                            Plateau plateau = new Plateau(dico, "CasJ1"+difficulte+".csv");
                            List<string> listMotATrouver = plateau.GMotATrouver;
                            Stopwatch swJ1 = new Stopwatch();
                            Console.WriteLine();
                            swJ1.Start();
                            string motDonne, directionDonne;
                            string posXDonne, posYDonne;
                            string[] dataDonne = new string[4];
                            bool reussi = false;
                            plateau.ToFile("PlateauEnregistre");                    //enregistrement du plateau

                            do
                            {
                                if (listMotATrouver.Count == 0)
                                {
                                    Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                    reussi = true;
                                    j1.Add_Score(20);
                                    goto FinJ1;
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                plateau.AfficherGrille();Console.ResetColor();
                                Console.WriteLine(j1.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                                
                                directionDonne = Console.ReadLine().ToUpper();Console.ResetColor();
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

                                if (!dico.RechDichoRecursif(motDonne, 0, plateau.GDicoListCount))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ1;
                                }
                                else if (plateau.VerifData(dataDonne) && listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                    listMotATrouver.Remove(motDonne);
                                    j1.Add_Mot(motDonne);
                                    j1.Add_Score( motDonne.Length );
                                }
                                else if (listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!");
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                }
                            FinJ1:;
                            } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ1.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                            tempsJ1 += (int)(swJ1.ElapsedMilliseconds / 1000);
                            swJ1.Stop();
                            swJ1.Reset();
                            Console.WriteLine("Appuyez sur n'importe quelle touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();


                            #endregion
                            #region TourJ2

                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, "CasJ2" + difficulte + ".csv");
                            List<string> listMotATrouver2 = plateau2.GMotATrouver;
                            Stopwatch swJ2 = new Stopwatch();
                            SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau, j1, j2, dico, 0);
                            Console.WriteLine();
                            swJ2.Start();
                            string motDonne2, directionDonne2;
                            string posXDonne2, posYDonne2;
                            string[] dataDonne2 = new string[4];
                            bool reussi2 = false;
                            plateau.ToFile("PlateauEnregistre");
                            do
                            {

                                if (listMotATrouver2.Count == 0)
                                {
                                    Console.WriteLine("Bravo tu as trouvé tout les mots de la liste");
                                    reussi2 = true;
                                    j2.Add_Score(20);
                                    goto FinJ2;
                                }
                                Console.ForegroundColor= ConsoleColor.Green;
                                plateau2.AfficherGrille();Console.ResetColor();
                                Console.WriteLine(j2.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                                
                                directionDonne2 = Console.ReadLine().ToUpper();Console.ResetColor();
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

                                if (!dico.RechDichoRecursif(motDonne2, 0, plateau.GDicoListCount))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ2;
                                }
                                else if (plateau2.VerifData(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                    listMotATrouver2.Remove(motDonne2);
                                    j2.Add_Mot(motDonne2);
                                    j2.Add_Score( motDonne2.Length);
                                }
                                else if (listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                }
                            FinJ2:;
                            } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ2.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé!"); Console.ResetColor();
                            tempsJ2 += (int)(swJ2.ElapsedMilliseconds / 1000);
                            swJ2.Stop();
                            swJ2.Reset();
                            Console.WriteLine("Voulez vous enregistrer la partie?[Y]/[N]");//enregistrement de la partie
                            Console.ForegroundColor = ConsoleColor.Red;
                            
                            if (Console.ReadLine() == "Y")
                            {

                                sysEnregistrementJ2.EnregisterJeu(j1, j2, dico, difficulte, "G");
                                goto FinComplete;
                            }
                            Console.ResetColor();
                            Console.WriteLine("Appuyez sur n'importe quel touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                            #endregion
                            
                            difficulte++;//nouvelle difficultée pour le tour prochain
                            
                            nbmot += 5;//rajout de plus de mot pour la prochaine manche
                            taille ++;//rajout de taille pour la prochaine partie
                            tempsTimer += 5*difficulte;//rajout de temps pour la prochaine manche
                        } while (difficulte <= 5);
                        #region FinJeu
                        Console.WriteLine("Tous les tours sont terminés ! Bravo à vous deux");
                        Console.WriteLine($"Les scores sont : {j1.GScore} pour {j1.GNom} et {j2.GScore} pour {j2.GNom}");
                        if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j2.GNom} tu as gagné!");
                        }
                        else if (j2.GScore > j1.GScore)
                        {
                            Console.WriteLine($"Bravo à {j1.GNom} tu as gagné!");
                        }
                        else if (tempsJ1 > tempsJ2)
                        {
                            Console.WriteLine($"Vous êtes tous les deux trop fort! Il y a eu égalité au niveau des points, mais {j2.GNom} a résolu les plateaux en moins de temps");
                        }
                        else
                        {
                            Console.WriteLine($"Vous êtes tous les deux trop fort! Il y a eu égalité au niveau des points, mais {j1.GNom} a résolu les plateaux en moins de temps");
                        }

                        #endregion FinJeu

                    }
                    else                                          //JeuComplexe
                    {
                        #region initialisationDuJeuComplex
                        string entree;
                    J1entre:
                        Console.WriteLine("D'accord, veuillez nous donner le nom du premier joueur");
                        Console.ForegroundColor = ConsoleColor.Red;

                        entree = Console.ReadLine(); Console.ResetColor();
                        if (entree == null||entree=="") goto J1entre;
                        Joueur j1 = new Joueur(entree.First().ToString().ToUpper() + entree.Substring(1).ToLower());
                    J2entre:
                        Console.WriteLine("Veuillez donner le nom du deuxième joueur");
                        Console.ForegroundColor = ConsoleColor.Red;

                        entree = Console.ReadLine(); Console.ResetColor();
                        if (entree == null || entree == "") goto J2entre;
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

                        Console.WriteLine("Parfait tout est prêt, voici les règles du jeu : \nChaque joueur a une grille de mot caché qu’il doit trouver dans le temps imparti d’une minute pour le premier tableau. \r\nLe jouer remporte un bonus s’il trouve tous les mots de sa grille, sinon, il remporte un score égal au nombre de lettres des mots trouvés.\r\nLe joueur suivant fera de même sur une nouvelle grille.\r\nA chaque tour, la dimension de la grille et le nombre de mots cachés augmentent, ainsi que la difficulté et le temps imparti.\r\nLe gagnant est celui qui sera le plus rapide pour trouver tous les mots cachés ou celui qui aura le score le plus élevé.\n Chargement de la grille en cours...");
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
                                    Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                    reussi = true;
                                    j1.Add_Score(20);
                                    goto FinJ1;
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                
                                plateau.AfficherGrille();Console.ResetColor();
                                Console.WriteLine(j1.GNom + " à toi de jouer\nLes mots à trouver sont :");
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
                               
                                directionDonne = Console.ReadLine().ToUpper(); Console.ResetColor();
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

                                if (!dico.RechDichoRecursif(motDonne, 0, plateau.GDicoListCount))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ1;
                                }
                                else if (plateau.Test_Plateau(dataDonne) && listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne);
                                    listMotATrouver.Remove(motDonne);
                                    j1.Add_Mot(motDonne);
                                    j1.Add_Score(motDonne.Length);
                                }
                                else if (listMotATrouver.Contains(motDonne))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                }
                            FinJ1:;
                            } while (swJ1.ElapsedMilliseconds / 1000 <= tempsTimer && reussi == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ1.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                                Console.WriteLine("Tour terminé!");Console.ResetColor();
                            tempsJ1 += (int)(swJ1.ElapsedMilliseconds / 1000);
                            swJ1.Stop();
                            swJ1.Reset();
                            Console.WriteLine("Appuyez sur n'importe quel touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                            #endregion TourJ1
                            #region TourJ2
                            //TOUR JOUEUR 2
                            Plateau plateau2 = new Plateau(dico, difficulte, taille, nbmot);
                            SystemeEnregistrement sysEnregistrementJ2 = new SystemeEnregistrement(plateau2, j1, j2, dico, 0);
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
                                    Console.WriteLine("Bravo tu as trouvé tous les mots de la liste");
                                    reussi2 = true;
                                    j2.Add_Score(20);
                                    goto FinJ2;
                                }
                                Console.ForegroundColor= ConsoleColor.Green;
                                plateau2.AfficherGrille();Console.ResetColor();
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
                               
                                directionDonne2 = Console.ReadLine().ToUpper(); Console.ResetColor();
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

                                if (!dico.RechDichoRecursif(motDonne2, 0, plateau.GDicoListCount))
                                {
                                    Console.WriteLine("Ce mot n'est pas dans le dictionnaire " + dico.GLangage);
                                    goto FinJ2;
                                }
                                else if (plateau2.VerifData(dataDonne2) && listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Bien joué, tu as bien trouvé le mot " + motDonne2);
                                    listMotATrouver2.Remove(motDonne2);
                                    j2.Add_Mot(motDonne2);
                                    j2.Add_Score(motDonne2.Length);
                                }
                                else if (listMotATrouver2.Contains(motDonne2))
                                {
                                    Console.WriteLine("Ce mot est à trouver mais tu l'as mal placé, réessaie!"); //DONNE TJRS CE RESULTAT
                                }
                                else
                                {
                                    Console.WriteLine("Ce mot n'étais pas à trouver ou alors tu l'as déja trouvé, réessaie!");

                                }
                            FinJ2:;
                            } while (swJ2.ElapsedMilliseconds / 1000 <= tempsTimer && reussi2 == false); //Crhonomètre et condition de victoire
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            if (swJ2.ElapsedMilliseconds / 1000 >= tempsTimer) Console.WriteLine("Vous avez dépassé le temps donné");
                            Console.WriteLine("Tour terminé!");Console.ResetColor();
                            tempsJ2 += (int)(swJ2.ElapsedMilliseconds / 1000);
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
                            Console.WriteLine("Appuyez sur n'importe quel touche pour commencer le tour du prochain joueur");
                            Console.ReadKey();
                            Console.Clear();
                            #endregion
                            difficulte++;//nouvelle difficultée pour le tour prochain

                            nbmot += 5;//rajout de plus de mot pour la prochaine manche
                            taille++;//rajout de taille pour la prochaine partie
                            tempsTimer += 5 * difficulte;//rajout de temps pour la prochaine manche
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
                        else if (tempsJ1>tempsJ2)
                        {
                            Console.WriteLine($"Vous êtes tout les deux trop fort! Il y a eu égalité au niveau des points, mais {j2.GNom} a résolu les plateaux en moins de temps");
                        }
                        else
                        {
                            Console.WriteLine($"Vous êtes tout les deux trop fort! Il y a eu égalité au niveau des points, mais {j1.GNom} a résolu les plateaux en moins de temps");
                        }

                        #endregion FinJeu
                    }
                }
                Console.WriteLine("Voulez vous recommencer une partie [Y]/[N] ?");//début de la méthode pour recommencer la partie
                Console.ForegroundColor = ConsoleColor.Red;
                
                string r = Console.ReadLine().ToUpper();Console.ResetColor();
                r.ToUpper();
                if (r == "N") recommencerJeu = false;
                else
                {
                    Console.WriteLine("Super, c'est parti pour une nouvelle partie !");
                    Console.Clear();
                    goto DebutJeu;
                }
            } while (recommencerJeu);
        FinComplete:   //fin du jeu
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Merci d'avoir joué à ce jeu créé par Hugo Bonnell et Eliott Coutaz, au plaisir de vous revoir jouer !");Console.ResetColor();
            
        }
        
        
    }
}
