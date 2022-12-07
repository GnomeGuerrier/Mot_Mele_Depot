using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Mot_Mele
{
    class Plateau
    {
        private Dictionnaire dico;
        private int difficulte;
        private int nbmot;
        private int taille;
        
        private string[,] grilleVide;
        private string[,] grilleFinie;
        public struct PropMot
        {
            public string mot;
            public int orientation;
            public int posX;
            public int posY;
        }

        private List<PropMot> listeMot;
        
        public Plateau(Dictionnaire dico,int difficulte,int taille,int nbmot)
        {
            this.dico = dico;
            this.difficulte=difficulte;
            this.nbmot = nbmot;
            this.taille = taille;
            this.grilleVide = GenererGrille(this.taille);
            this.listeMot = new List<PropMot>();
            AfficherGrille(grilleVide);                                                                                                         //Affiche une première fois la grille vide
            this.grilleFinie = RemplirGrille(this.grilleVide,dico, nbmot,difficulte);                                                                                                 //On remplit la grille avec 12 mots du dico donné
            RemplirGrilleRandom(this.grilleFinie);
            
            AfficherGrille(this.grilleFinie);
            AfficherListePropMot();

        }

        public List<PropMot> GListeMot
        {
            get { return this.listeMot; }
        }

         string[,] RemplirGrille(string[,] grille, Dictionnaire dico, int nombreMots, int difficulte)                 //Fonction pour remplir la grille avec les mots du dictionnaire en fonction de la difficulté
        {
            string mot = MotAleatoire(dico, grille);                            //On initialise un premier mot choisi aléatoirement
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
                PropMot motAjoute = new PropMot();
                switch (orientation)                                            //En fonction de l'orientation on a 4 cas différents, mais similaires dans la structure
                {
                    case 1:                                                     //Pour l'orientation "Haut/Bas"
                        if (grille.GetLength(0) - x <= mot.Length)              //On vérifie que le mot a l'espace nécessaire pour rentrer entre les coordonnées d'origine et le bord du plateau
                        {                                                           //Si c'est le cas, rien ne change
                            x = grille.GetLength(0) - mot.Length - 1;               //Sinon on décale le x originel pour que le mot puisse entrer dans le tableau
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)                                                                        //Pour i entre 0 et la longueur du mot choisi
                            {
                                if (x + i < grille.GetLength(0) && y < grille.GetLength(1))                                               //DEBUG -> si sortie de matrice, on change de mot et on recommence
                                {
                                    if (grille[x + i, y] != " " && Convert.ToString(mot[i]) != grille[x + i, y] && n <= dico.GDicoList.Count)    //Grace à la boucle for(int i ...) on défile à travers toutes les cases que le mot va occuper
                                    {                                                                                                   //Si une case n'est NI égale à un espace, NI à la lettre correspondante à la position et que la variable compteur n est inferieure à la longueur du dictionnaire alors
                                        mot = MotAleatoire(dico, grille);                                                                           //On change de mot
                                        verif = false;                                                                                      //Verification à false pour rester dans le do while
                                        n++;                                                                                                //On augmente le compteur de mots testés sur cette position
                                        break;                                                                                              //On sort de la boucle for pour la recommencer à 0 avec le nouveau mot
                                    }
                                    else if (n > dico.GDicoList.Count)                                                                           //Si n devient supérieur à la longueur du dictionnaire (ne signifie pas qu'on a testé tous les mots de ce dernier, car ils sont choisi aléatoirement, mais un certain nombre)
                                    {                                                                                                       //alors on change les coordonnées d'insertion du mot
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(0) - x < mot.Length)                                                           //On décale la coordonnée x si le mot ne rentre pas dans la grille
                                        {
                                            x = grille.GetLength(0) - mot.Length;
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
                                    mot = MotAleatoire(dico, grille);                       //On change de mot aléatoirement
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)                                        //Après avoir vérifié que le mot pouvait entrer sur des coordonnées données pour une direction donnée
                        {
                            grille[x + l, y] = Convert.ToString(mot[l]);                            //On remplit la grille
                        }

                        break;                                                      //Fin du cas "orientation 1"
                    case 2:                                                         //Les autres cas sont les mêmes, change uniquement les coordonnées modifiées pour correspondre à la bonne orientation du mot
                        if (x - mot.Length <= 0)
                        {
                            x = mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x - i >= 0 && y < grille.GetLength(1))
                                {
                                    if (grille[x - i, y] != " " && Convert.ToString(mot[i]) != grille[x - i, y] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (x - mot.Length <= 0)
                                        {
                                            x = mot.Length - 1;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x - l, y] = Convert.ToString(mot[l]);
                        }

                        break;
                    case 3:
                        if (grille.GetLength(1) - y <= mot.Length)
                        {
                            y = grille.GetLength(1) - mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x < grille.GetLength(0) && y + i < grille.GetLength(1))
                                {
                                    if (grille[x, y + i] != " " && Convert.ToString(mot[i]) != grille[x, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y < mot.Length)
                                        {
                                            y = grille.GetLength(1) - mot.Length;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x, y + l] = Convert.ToString(mot[l]);
                        }

                        break;
                    case 4:
                        if (y - mot.Length <= 0)
                        {
                            y = mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x < grille.GetLength(0) && y - i >= 0)
                                {
                                    if (grille[x, y - i] != " " && Convert.ToString(mot[i]) != grille[x, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (y - mot.Length <= 0)
                                        {
                                            y = mot.Length - 1;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x, y - l] = Convert.ToString(mot[l]);
                        }

                        break;

                    case 5:
                        if (grille.GetLength(1) - y <= mot.Length)
                        {
                            y = grille.GetLength(1) - mot.Length - 1;
                        }
                        if (grille.GetLength(0) - x <= mot.Length)
                        {
                            x = grille.GetLength(0) - mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x + i < grille.GetLength(0) && y + i < grille.GetLength(1))
                                {
                                    if (grille[x + i, y + i] != " " && Convert.ToString(mot[i]) != grille[x + i, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y <= mot.Length)
                                        {
                                            y = grille.GetLength(1) - mot.Length - 1;
                                        }
                                        if (grille.GetLength(0) - x <= mot.Length)
                                        {
                                            x = grille.GetLength(0) - mot.Length - 1;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }

                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x + l, y + l] = Convert.ToString(mot[l]);
                        }

                        break;
                    case 6:
                        if (grille.GetLength(1) - y <= mot.Length)
                        {
                            y = grille.GetLength(1) - mot.Length - 1;
                        }
                        if (x - mot.Length <= 0)
                        {
                            x = mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x - i >= 0 && y + i < grille.GetLength(1))
                                {
                                    if (grille[x - i, y + i] != " " && Convert.ToString(mot[i]) != grille[x - i, y + i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (grille.GetLength(1) - y <= mot.Length)
                                        {
                                            y = grille.GetLength(1) - mot.Length - 1;
                                        }
                                        if (x - mot.Length <= 0)
                                        {
                                            x = mot.Length - 1;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x - l, y + l] = Convert.ToString(mot[l]);
                        }

                        break;

                    case 7:
                        if (y - mot.Length <= 0)
                        {
                            y = mot.Length - 1;
                        }
                        if (grille.GetLength(0) - x <= mot.Length)
                        {
                            x = grille.GetLength(0) - mot.Length - 1;
                        }

                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x + i < grille.GetLength(0) && y - i >= 0)
                                {
                                    if (grille[x + i, y - i] != " " && Convert.ToString(mot[i]) != grille[x + i, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (y - mot.Length <= 0)
                                        {
                                            y = mot.Length - 1;
                                        }
                                        if (grille.GetLength(0) - x <= mot.Length)
                                        {
                                            x = grille.GetLength(0) - mot.Length - 1;
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
                                    mot = MotAleatoire(dico, grille);
                                }
                            }
                            if (grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " ")
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x + l, y - l] = Convert.ToString(mot[l]);
                        }

                        break;
                    case 8:
                        if (x - mot.Length - 1 <= 0)
                        {
                            x = mot.Length;
                        }
                        if (y - mot.Length - 1 <= 0)
                        {
                            y = mot.Length;
                        }


                        do
                        {
                            for (int i = 0; i < mot.Length; i++)
                            {
                                if (x - i >= 0 && y - i >= 0)
                                {
                                    if (grille[x - i, y - i] != " " && Convert.ToString(mot[i]) != grille[x - i, y - i] && n <= dico.GDicoList.Count)
                                    {
                                        mot = MotAleatoire(dico, grille);
                                        verif = false;
                                        n++;
                                        break;
                                    }
                                    else if (n > dico.GDicoList.Count)
                                    {
                                        x = NombreAleatoire(0, grille.GetLength(0));
                                        y = NombreAleatoire(0, grille.GetLength(1));
                                        if (x - mot.Length - 1 <= 0)
                                        {
                                            x = mot.Length;
                                        }
                                        if (y - mot.Length - 1 <= 0)
                                        {
                                            y = mot.Length;
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
                                    mot = MotAleatoire(dico, grille);
                                }

                            }
                            if ((grille[x, y] != Convert.ToString(mot[0]) && grille[x, y] != " "))
                            {
                                verif = false;
                            }
                        } while (verif == false);

                        for (int l = 0; l < mot.Length; l++)
                        {
                            grille[x - l, y - l] = Convert.ToString(mot[l]);
                        }

                        break;

                    default:
                        Console.WriteLine("Default case in first switch");
                        break;
                }
                Console.WriteLine(mot);
                motAjoute.mot = mot;
                motAjoute.orientation = orientation;
                motAjoute.posX = x;
                motAjoute.posY = y;
                this.listeMot.Add(motAjoute);
                //Une fois le mot placé
                mot = MotAleatoire(dico, grille);                                       //On choisit un nouveau mot
               
                Console.WriteLine("------------------");
                AfficherGrille(grille);
                orientation += 1;                                               //On change l'orientation
                switch (difficulte)
                {
                    case 1:
                        if (orientation == 2)
                        {
                            orientation = 3;
                        }
                        else if (orientation > 3)
                        {
                            orientation = 1;
                        }
                        break;
                    case 2:
                        if (orientation > 4)
                        {
                            orientation = 1;
                        }
                        break;
                    case 3:
                        if (orientation > 4 && orientation != 7)
                        {
                            orientation = 7;
                        }
                        else if (orientation > 4)
                        {
                            orientation = 1;
                        }
                        break;
                    case 4:
                        if (orientation > 5 && orientation != 7)
                        {
                            orientation = 7;
                        }
                        else if (orientation > 5)
                        {
                            orientation = 1;
                        }
                        break;
                    case 5:
                        if (orientation > 8)
                        {
                            orientation = 1;
                        }
                        break;
                }
                x = NombreAleatoire(0, grille.GetLength(0));                    //On re selectionne de nouvelles coordonnées aléatoires
                y = NombreAleatoire(0, grille.GetLength(1));

            }

            return grille;                                                      //A la fin de la boucle de remplissge des mots, on retourne la grille
        }


         int NombreAleatoire(int min, int max)        //Fonction qui retourne un entier aléatoire entre min et max
        {
            Random rand = new Random();
            int n = rand.Next(min, max);
            return n;
        }
         string MotAleatoire(Dictionnaire dico, string[,] grille)           //Foncton qui retourne un mot aléatoire du dictionnaire donné en paramètre
        {
            Random rand = new Random();
            int n;
            do
            {
                n = rand.Next(0, dico.GDicoList.Count);
            } while (dico.GDicoList[n].Length >= grille.GetLength(0));

            return dico.GDicoList[n];

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
         void AfficherGrille(string[,] grille)                        //Fonction pour afficher une grille
        {

            for (int i = 0; i < grille.GetLength(0); i++)                      //On boucle de sorte à afficher les éléments X de la grille tel que: 
            {                                                                   // |X|X|X|X....|X|X|
                Console.Write("|");                                             // |X|X|X|X....|X|X|
                for (int j = 0; j < grille.GetLength(1); j++)                      // ...
                {                                                               // ...
                    Console.Write(grille[i, j] + "|");                          // |X|X|X|X....|X|X|
                }
                Console.WriteLine();
            }
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
            foreach(PropMot p in listeMot)
            {
                Console.WriteLine(p.mot + "\n" + p.orientation + "\n" + p.posX + "\n" + p.posY + "\n--------------");
            }
        }
        public bool checkMot(string[] data)
        {
            //Data : data[0] mot | data[1] orientation | data[2] posX | data[3] posY
            bool verif = false;

            foreach(PropMot p in listeMot)
            {
                if(p.mot == data[0] && p.orientation == Convert.ToInt32(data[1]) && p.posX == Convert.ToInt32(data[2]) && p.posY == Convert.ToInt32(data[3]))
                {
                    verif = true;
                }
            }

            return verif;
        }
    }
    
}
