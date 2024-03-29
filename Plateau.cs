﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.Collections;

namespace Mot_Mele
{
    public class Plateau
    {                                                                        // Déclaration des variables
        private List<string> motATrouver = new List<string>();               // Liste des mots à trouver (seulement les mots, sans leurs données associées)
        private Dictionnaire dico;                                     
        private int difficulte; 
        private int nbmot;
        private int taille;
        private string mot;
        
        private List<string> lettreSimple = new List<string>();             // Liste servant à la séparation des mots du fichier dictonnaire
        private string[,] grilleVide;
        private string[,] grilleFinieMotSansLettre;
        
        private string[,] grilleRemplie;
        private List<string> infoSimple = new List<string>();
        private List<PropMot> listeMot;
        private PropMot motAjoute;
        public struct PropMot                           //Initiation structure PropMot pour garder toutes les données liées aux mots
        {
            public string mot;              // Mot
            public int orientation;         // Orientation [1,8]
            public int posX;                // Coordonnées
            public int posY;
        }
        

        
        public Plateau(Dictionnaire dico,string path)       // Constructeur simple (plateau pré-généré)
        {
            this.dico = dico;
            ToRead(path);
        }
        public Plateau(Dictionnaire dico,int difficulte,int taille,int nbmot)       // Constructeur pour un plateau vierge
        {
            
            this.dico = dico;
            this.difficulte=difficulte;
            this.nbmot = nbmot;
            this.taille = taille;
            this.grilleVide = GenererGrille(this.taille);                   // On génère la grille
            this.listeMot = new List<PropMot>();
            this.grilleFinieMotSansLettre = RemplirGrille(this.grilleVide,dico, nbmot,difficulte);      // Grille avec les mots placés                                                                                              //On remplit la grille avec 12 mots du dico donné
            RemplirGrilleRandom(this.grilleFinieMotSansLettre);                                         // Grille avec mots et lettres aléatoires autour
        }
        public void ToRead(string path)                             // Fonction Lecture de fichier plateau de jeu
        {
            string[] fichier = File.ReadAllLines(path);             // Récupération d'un plateau
            string[] lettreSimpleTab;                           
            foreach (string mot in fichier[0].Split(";"))           // Découpage des mots ...
            {
                this.infoSimple.Add(mot);                           // Permet d'avoir les infos de difficultée,colonne,ligne et nb de mots à trouver
            }
            this.grilleRemplie = new string[Convert.ToInt32(infoSimple[1]), Convert.ToInt32(infoSimple[2])];
            foreach (string mot in fichier[1].Split(";"))
            {
                this.motATrouver.Add(mot);                      // Construit la liste des mots à trouver
            }
            this.motATrouver = this.motATrouver.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            for (int i = 2; i < fichier.Length; i++)
            {
                lettreSimpleTab = new string[fichier[i].Split(";").Length];
                lettreSimpleTab = fichier[i].Split(";");
                for (int k = 0; k < lettreSimpleTab.Length; k++)
                {
                    lettreSimple.Add(lettreSimpleTab[k]);               // List avec toutes les lettres du plateau
                }
                this.lettreSimple = this.lettreSimple.Where(s => !string.IsNullOrWhiteSpace(s)).ToList(); // Permet de retirer tous les espaces et lettres vides


            }
            int count = 0;
            for (int l = 0; l < Convert.ToInt32(infoSimple[1]); l++)
            {
                for (int o = 0; o < Convert.ToInt32(infoSimple[2]); o++)
                {
                    this.grilleRemplie[l, o] = lettreSimple[count];         // Remplit la grille avec toutes les lettres
                    count++;
                }
            }
        }
        public override string ToString()           // Fonction imposée ToString()
        {
            string r = "Ce tableau est composé de " + this.GGetLength0 + " lignes et de " + this.GGetLength1 + " colones\n";
            r += "Il est de difficulté " + this.GDifficulte + " et a " + this.GMotATrouver.Count + " mots à trouver\n";
            return r;
        }
        public List<PropMot> GListeMotPropMot       // Fonctions d'accès GET
        {
            get { return this.listeMot; }
        }
        public string[,] GGrilleRemplie
        {
            get { return this.grilleRemplie; }
        }
        public List<string> GMotATrouver
        {
            get { return motATrouver; }
        }
        public int GDifficulte
        {
            get { return this.difficulte; }
        }
        public int GDicoListCount
        {
            get { return this.dico.GDicoList.Count(); }
        }
        public int GGetLength0
        {
            get { return this.grilleRemplie.GetLength(0); }
        }
        public int GGetLength1
        {
            get { return this.grilleRemplie.GetLength(1); }
        }

        string[,] RemplirGrille(string[,] grille, Dictionnaire dico, int nombreMots, int difficulte)                 //Fonction pour remplir la grille avec les mots du dictionnaire en fonction de la difficulté
        {
             this.mot = MotAleatoire(dico, grille);                            //On initialise un premier mot choisi aléatoirement
            int x = NombreAleatoire(0, grille.GetLength(0));            //On prend un x aléatoire dans la grille                Notez que les coordonnées de la grille
            int y = NombreAleatoire(0, grille.GetLength(1));            //On prend un y aléatoire dans la grille                se notent ( y , x )
            bool verif = true;                                          //On initialise la variable verification à true (utile et expliqué plus tard)     
            int orientation = 1;                                        //On initialise l'orientation à 1
            // Orientation :
            // 1 : Haut/Bas
            // 2 : Bas/Haut
            // 3 : Droite/Gauche
            // 4 : Gauche/Droite
            // 5 : NO / SE
            // 6 : SO / NE
            // 7 : NE / SO
            // 8 : SE / NO

            for (int k = 0; k < nombreMots; k++)                         //Boucle de remplissage de mots, tourant 'nombreMots' fois
            {
                int n = 0;                                                      //On initialise le compteur n, qui compte combien de mots on été testé.
                this.motAjoute = new PropMot();
                switch (orientation)                                            //En fonction de l'orientation on a 4 cas différents, mais similaires dans la structure
                {
                    case 1:                                                     //Pour l'orientation "Haut/Bas"
                        if (grille.GetLength(0) - x <= this.mot.Length)              //On vérifie que le mot a l'espace nécessaire pour rentrer entre les coordonnées d'origine et le bord du plateau
                        {                                                           //Si c'est le cas, rien ne change
                            x = grille.GetLength(0) - this.mot.Length - 1;               //Sinon on décale le x originel pour que le mot puisse entrer dans le tableau
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)                                                                        //Pour i entre 0 et la longueur du mot choisi
                            {
                                if (x + i < grille.GetLength(0) && y < grille.GetLength(1))                                               //DEBUG -> si sortie de matrice, on change de mot et on recommence
                                {
                                    if (grille[x + i, y] != " " && Convert.ToString(this.mot[i]) != grille[x + i, y] && n <= dico.GDicoList.Count)    //Grace à la boucle for(int i ...) on défile à travers toutes les cases que le mot va occuper
                                    {                                                                                                   //Si une case n'est NI égale à un espace, NI à la lettre correspondante à la position et que la variable compteur n est inferieure à la longueur du dictionnaire alors
                                        this.mot = MotAleatoire(dico, grille);                                                                           //On change de mot
                                        verif = false;                                                                                      //Verification à false pour rester dans le do while
                                        n++;                                                                                                //On augmente le compteur de mots testés sur cette position
                                        break;                                                                                              //On sort de la boucle for pour la recommencer à 0 avec le nouveau mot
                                    }
                                    else if (n > dico.GDicoList.Count)                                                                      //Si n devient supérieur à la longueur du dictionnaire (ne signifie pas qu'on a testé tous les mots de ce dernier, car ils sont choisi aléatoirement, mais un certain nombre)
                                    {                                                                                                       //alors on change les coordonnées d'insertion du mot
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(0) - x < mot.Length)                                                           //On décale la coordonnée x si le mot ne rentre pas dans la grille
                                        {
                                            x = grille.GetLength(0) - this.mot.Length;
                                        }
                                        n = 0;                                                                                              //On remet le nombre de mots testés sur la nouvelle position à 0
                                    }
                                    else
                                    {
                                        verif = true;                                                                                   //Sinon, (donc si les cases sont égales à des espaces ou à la lettre correspondante à la position de la case
                                    }                                                                                                       //On met verif sur true pour sortir du do/while
                                                                                                                                            //On a donc vérifié que le mot peut entrer dans la direction designée, mais il n'est pas encore placé !
                                }
                                else
                                {
                                                
                                    verif = false;                                  //La vérification est false pour rester dans la boucle do/while
                                    this.mot = MotAleatoire(dico, grille);                       //On change de mot aléatoirement
                                }
                            }
                            if(this.mot != null && this.mot.Length >= 1)                                    //Double condition servant à du débogage...
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de mot aléatoirement
                            }
                            
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)                                        //Après avoir vérifié que le mot pouvait entrer sur des coordonnées données pour une direction donnée
                        {
                            grille[x + l, y] = Convert.ToString(this.mot[l]);                            //On remplit la grille
                        }

                        break;                                                      //Fin du cas "orientation 1"
                    case 2:                                                         //Les autres cas sont les mêmes, change uniquement les coordonnées modifiées pour correspondre à la bonne orientation du mot
                        if (x - this.mot.Length <= 0)
                        {
                            x = this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x - i >= 0 && y < grille.GetLength(1))
                                {
                                    if (grille[x - i, y] != " " && Convert.ToString(this.mot[i]) != grille[x - i, y] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (x - this.mot.Length <= 0)
                                        {
                                            x = this.mot.Length - 1;
                                        }
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }


                                }
                                else
                                {
                                    
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x - l, y] = Convert.ToString(this.mot[l]);
                        }

                        break;
                    case 3:
                        if (grille.GetLength(1) - y <= this.mot.Length)
                        {
                            y = grille.GetLength(1) - this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x < grille.GetLength(0) && y + i < grille.GetLength(1))
                                {
                                    if (grille[x, y + i] != " " && Convert.ToString(this.mot[i]) != grille[x, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y < this.mot.Length)
                                        {
                                            y = grille.GetLength(1) - this.mot.Length;
                                        }
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }


                                }
                                else
                                {
                                    
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x, y + l] = Convert.ToString(this.mot[l]);
                        }

                        break;
                    case 4:
                        if (y - this.mot.Length <= 0)
                        {
                            y = this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x < grille.GetLength(0) && y - i >= 0)
                                {
                                    if (grille[x, y - i] != " " && Convert.ToString(this.mot[i]) != grille[x, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (y - this.mot.Length <= 0)
                                        {
                                            y = this.mot.Length - 1;
                                        }
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }


                                }
                                else
                                {
                                    
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de this.mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x, y - l] = Convert.ToString(this.mot[l]);
                        }

                        break;

                    case 5:
                        if (grille.GetLength(1) - y <= this.mot.Length)
                        {
                            y = grille.GetLength(1) - this.mot.Length - 1;
                        }
                        if (grille.GetLength(0) - x <= this.mot.Length)
                        {
                            x = grille.GetLength(0) - this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x + i < grille.GetLength(0) && y + i < grille.GetLength(1))
                                {
                                    if (grille[x + i, y + i] != " " && Convert.ToString(this.mot[i]) != grille[x + i, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y <= this.mot.Length)
                                        {
                                            y = grille.GetLength(1) - this.mot.Length - 1;
                                        }
                                        if (grille.GetLength(0) - x <= this.mot.Length)
                                        {
                                            x = grille.GetLength(0) - this.mot.Length - 1;
                                        }
                                        verif = false;
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }

                                }
                                else
                                {
                                    
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }

                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de this.mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x + l, y + l] = Convert.ToString(this.mot[l]);
                        }

                        break;
                    case 6:
                        if (grille.GetLength(1) - y <= this.mot.Length)
                        {
                            y = grille.GetLength(1) - this.mot.Length - 1;
                        }
                        if (x - this.mot.Length <= 0)
                        {
                            x = this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x - i >= 0 && y + i < grille.GetLength(1))
                                {
                                    if (grille[x - i, y + i] != " " && Convert.ToString(this.mot[i]) != grille[x - i, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y <= this.mot.Length)
                                        {
                                            y = grille.GetLength(1) - this.mot.Length - 1;
                                        }
                                        if (x - this.mot.Length <= 0)
                                        {
                                            x = this.mot.Length - 1;
                                        }
                                        verif = false;
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }

                                }
                                else
                                {
                                   
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de this.mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x - l, y + l] = Convert.ToString(this.mot[l]);
                        }

                        break;

                    case 7:
                        if (y - this.mot.Length <= 0)
                        {
                            y = this.mot.Length - 1;
                        }
                        if (grille.GetLength(0) - x <= this.mot.Length)
                        {
                            x = grille.GetLength(0) - this.mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x + i < grille.GetLength(0) && y - i >= 0)
                                {
                                    if (grille[x + i, y - i] != " " && Convert.ToString(this.mot[i]) != grille[x + i, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (y - this.mot.Length <= 0)
                                        {
                                            y = this.mot.Length - 1;
                                        }
                                        if (grille.GetLength(0) - x <= this.mot.Length)
                                        {
                                            x = grille.GetLength(0) - this.mot.Length - 1;
                                        }
                                        verif = false;
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }

                                }
                                else
                                {
                                   
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de this.mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x + l, y - l] = Convert.ToString(this.mot[l]);
                        }

                        break;
                    case 8:
                        if (x - this.mot.Length - 1 <= 0)
                        {
                            x = this.mot.Length;
                        }
                        if (y - this.mot.Length - 1 <= 0)
                        {
                            y = this.mot.Length;
                        }


                        do
                        {
                            for (int i = 0; i < this.mot.Length; i++)
                            {
                                if (x - i >= 0 && y - i >= 0)
                                {
                                    if (grille[x - i, y - i] != " " && Convert.ToString(this.mot[i]) != grille[x - i, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        this.mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (x - this.mot.Length - 1 <= 0)
                                        {
                                            x = this.mot.Length;
                                        }
                                        if (y - this.mot.Length - 1 <= 0)
                                        {
                                            y = this.mot.Length;
                                        }
                                        verif = false;
                                        n = 0;
                                    }
                                    else
                                    {
                                        verif = true;
                                    }
                                }
                                else
                                {
                                  
                                    verif = false;
                                    this.mot = MotAleatoire(dico, grille);
                                }

                            }
                            if (this.mot != null && this.mot.Length >= 1)
                            {
                                if (grille[x, y] != Convert.ToString(this.mot[0]) && grille[x, y] != " ")
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                                this.mot = MotAleatoire(dico, grille);                       //On change de this.mot aléatoirement
                            }
                        } while (verif == false);

                        for (int l = 0; l < this.mot.Length; l++)
                        {
                            grille[x - l, y - l] = Convert.ToString(this.mot[l]);
                        }

                        break;

                    default:
                        Console.WriteLine("Default case in first switch");
                        break;
                }
                this. motAjoute.mot = this.mot;
                this.motAjoute.orientation = orientation;
                this.motAjoute.posX = x;
                this.motAjoute.posY = y;
                this.listeMot.Add(this.motAjoute);
                                                                                             // Une fois le this.mot placé
                this.mot = MotAleatoire(dico, grille);                                       // On choisit un nouveau this.mot

                Random RAND = new Random();                         
                RendDiff:
                orientation = RAND.Next(1,9);                                               // On change l'orientation
                switch (difficulte)
                {
                    case 1:                                                                 // On adapte l'orientation en fonction de la difficulté
                        if (orientation == 2)
                        {
                            orientation = 3;
                        }
                        else if (orientation > 3)
                        {
                            goto RendDiff;
                        }
                        break;
                    case 2:
                        if (orientation > 4)
                        {
                            goto RendDiff;
                        }
                        break;
                    case 3:
                        if (orientation > 4 && orientation != 7)
                        {
                            orientation = 7;
                        }
                        else if (orientation > 4)
                        {
                            goto RendDiff;
                        }
                        break;
                    case 4:
                        if (orientation > 5 && orientation != 7)
                        {
                            orientation = 7;
                        }
                        else if (orientation > 5)
                        {
                            goto RendDiff;
                        }
                        break;
                    case 5:
                        if (orientation > 8)
                        {
                            goto RendDiff;
                        }
                        break;
                }
                x = NombreAleatoire(0, grille.GetLength(0));                    //On re selectionne de nouvelles coordonnées aléatoires
                y = NombreAleatoire(0, grille.GetLength(1));

            }
            
            foreach (PropMot a in this.listeMot)
            {
                this.motATrouver.Add(a.mot);
            }
            this.grilleRemplie = grille;
                return grille;                                                      // A la fin de la boucle de remplissge des mots, on retourne la grille
         }
        /// <summary>
        /// Fonction servant à l'enregistrement d'une partie sous forme d'un fichier CVS
        /// </summary>
        /// <param name="path">Nom de tête pour enregistrer le plateat</param>
         public void ToFile(string path)                                            
        {
            List<string> rendu = new List<string>();                //La liste rendu contient toutes les informations qui seront enregistrées
            string[,] grille=this.GGrilleRemplie;
            DateTime temps = DateTime.Now;
            rendu.Clear();
            rendu.Add(this.GDifficulte + ";" + this.GGetLength0 + ";" + this.GGetLength1 + ";" + this.GMotATrouver.Count);
            string mots = "";
            string ligne = "";
            foreach (string i in this.motATrouver)
            {
                mots += i + ";";
            }
            rendu.Add(mots);
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    ligne += grille[i, j] + ";";

                }
                rendu.Add(ligne);
                ligne = "";
            }


            File.WriteAllLines(path + "_" + temps.Day + "_" + temps.Month + "_" + temps.Year + "_" + temps.Hour + "_" + "_" + temps.Minute + "_" + temps.Second + ".csv", rendu); //Enregistre le tableau avec comme syntaxe PlateauEnregistreJour_Mois_Anne_Heure_Minute_Seconde

        }

        int NombreAleatoire(int min, int max)        //Fonction qui retourne un entier aléatoire entre min et max
        {
            Random rand = new Random();
            int n = rand.Next(min, max);
            return n;
        }
        string MotAleatoire(Dictionnaire dico, string[,] grille)           //Foncton qui retourne un mot aléatoire du dictionnaire donné en paramètre
        {

        //n'est pas dedans
        MotAleatoire:
            Random rand = new Random();
            int n;
            string motRandom;
            do
            {
                n = rand.Next(0, dico.GDicoList.Count);
                motRandom = dico.GDicoList[n];

            } while (dico.GDicoList[n].Length >= grille.GetLength(0));      // On choisit un mot aléatoirement jusqu'à tomber sur un mot pouvant entrer dans le plateau
            if (this.listeMot != null)                                      // Si la liste est vide, le mot n'est pas dedans
            { 
                for (int i = 0; i < this.listeMot.Count; i++)               // Débogage
                {
                    if (motRandom == this.listeMot[i].mot)
                    {
                       
                        goto MotAleatoire;                                  // On relace MotAléatoire si nécessaire...
                    }
                }
            }
            return dico.GDicoList[n];                                       // On renvoie le mot aléatoire

        }
        public string[,] GenererGrille(int tailleGrille)                    //Fonction générant une grille vide de taille 'tailleGrille'
        {
            string[,] grille = new string[tailleGrille, tailleGrille];       //Initialisation de la grille
            for (int i = 0; i < tailleGrille; i++)                            //On boucle pour la remplir d'espaces, qui seront traités comme du vide par le reste du code
            {
                for (int j = 0; j < tailleGrille; j++)
                {
                    grille[i, j] = " ";
                }
            }
            return grille;                                                  //On retourne la grille
        }
         public void AfficherGrille()                        //Fonction pour afficher une grille
        {
            Console.Write("   ");
            for(int i = 0; i <this.grilleRemplie.GetLength(0); i++)
            {
                if(i <= 9)
                {
                    Console.Write("  " + i + " ");
                }
                else
                {
                    Console.Write("  " + i);
                } 

            }
            Console.Write("  Y \n");
            for (int i = 0; i < this.grilleRemplie.GetLength(0); i++)                      
            {                    
                if(i<=9) Console.Write(" " + i + " |");                                                                  
                else Console.Write(i+"|");                                             
                for (int j = 0; j < this.grilleRemplie.GetLength(1); j++)                      
                { 
                     Console.Write(" "+this.grilleRemplie[i, j] + " |"); 
                                                                       
                }
                Console.WriteLine();
            }
            Console.WriteLine("X");
        }
         void RemplirGrilleRandom(string[,] grille)
        {
            for(int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if(grille[i,j]==" ")
                    {
                        grille[i, j] = LettreAleatoire();
                    }
                }
            }
        }
        public string LettreAleatoire()
        {
            string r;
            Random rnd = new Random();
            int ascii_index = rnd.Next(65, 91); //ASCII character codes 65-90
            r  = Convert.ToString(Convert.ToChar(ascii_index));
            return r;
        }

        public void AfficherListePropMot()
        {
            foreach(PropMot p in this.listeMot)
            {
                Console.WriteLine(p.mot + "\n" + p.orientation + "\n" + p.posX + "\n" + p.posY + "\n--------------");
            }
        }
        public bool Test_Plateau(string[] data)         // Vérifie que les données entrées par l'utilisateur correspondent à un mot à trouver
        {
            //Data : data[0] mot | data[1] orientation | data[2] posX | data[3] posY
            bool verif = false;

            foreach(PropMot p in this.listeMot)
            {
                if(p.mot == data[0] && p.orientation == Convert.ToInt32(data[1]) && p.posX == Convert.ToInt32(data[2]) && p.posY == Convert.ToInt32(data[3]))
                {
                    verif = true;
                }
            }

            return verif;
        }

        public bool VerifData(string[] data)            // Verifie si des données correspondent à un mot du plateau sans que ce dernier ne soit enregistré dans une structure propMot
        {
            bool verif = true;
            string[,] grille = this.grilleRemplie;
            if(data == null || data[0] == null || data[0] == "" || data[1] == null || data[1] == "" || data[2] == null || data[2] == "" || data[3] == null || data[3] == "")
            {
                verif = false;
            }
            else
            {
                int x = Convert.ToInt32(data[2]);
                int y = Convert.ToInt32(data[3]);
                switch (data[1])
                {
                    case "1":
                        for(int i = 0; i < data[0].Length; i++)
                        {
                            if(x+i < grille.GetLength(0))
                            {
                                if (Convert.ToString(data[0][i]) != grille[x + i, y])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }
                        }
                        break;
                    case "2":
                        for(int i = 0; i< data[0].Length; i++)
                        {
                            if(x-i >= 0)
                            {
                                if (Convert.ToString(data[0][i]) != grille[x - i, y])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                          
                        }
                        break;
                    case "3":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(y+i < grille.GetLength(1))
                            {
                                if (Convert.ToString(data[0][i]) != grille[x, y + i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                            
                        }
                        break;
                    case "4":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(y-i >= 0)
                            {
                                if (Convert.ToString(data[0][i]) != grille[x, y - i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }  
                        }
                        break;
                    case "5":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(x+i < grille.GetLength(0) && y+i < grille.GetLength(1))
                            {
                                if (Convert.ToString(data[0][i]) != grille[x + i, y + i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                           
                        }
                        break;
                    case "6":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(x-i >= 0 && y + i < grille.GetLength(1))
                            {
                                if (Convert.ToString(data[0][i]) != grille[x - i, y + i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                          
                        }
                        break;
                    case "7":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(x+i < grille.GetLength(0) && y -i >= 0)
                            {
                                if (Convert.ToString(data[0][i]) != grille[x + i, y - i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                            
                        }
                        break;
                    case "8":
                        for (int i = 0; i < data[0].Length; i++)
                        {
                            if(x-i >= 0 && y -i >= 0)
                            {
                                if (Convert.ToString(data[0][i]) != grille[x - i, y - i])
                                {
                                    verif = false;
                                }
                            }
                            else
                            {
                                verif = false;
                            }                           
                        }
                        break;
                    default:
                        verif = false;
                        break;
                }
            }
            return verif;
        }

    }
    
}
