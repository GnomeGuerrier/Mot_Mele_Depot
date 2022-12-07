using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;

namespace Mot_Mele
{
    class Joueur
    {
        //CLASSE FINIE SELON L'ENONCE, TEST PUSH3
        private string nom;
        private string[] motTrouve;
        private int score;
        private bool hasRun = false;
        public Joueur(string nom)
        {
            if (nom != null && nom != "")
            {
                this.nom = nom;
            }
            this.score = 0;
            this.motTrouve = null;
        }
        public string GNom{
            get { return this.nom; }
        }
        public string[] GMotTrouve
        {
            get { return this.motTrouve; }
        }
        public int GScore
        {
            get { return this.score; }
        }
        /// <summary>
        /// Permet d'ajouter un mot dans la lsite des mots trouvé, la première fois que cette méthode est éxécuté on initialise l'array motTrouve avec une taille
        /// </summary>
        /// <param name="mot">mot à ajouter</param>
        public void Add_Mot(string mot)
        {
            //check si c'est la première fois que la méthode est appelée
            if (!hasRun)
            {
                motTrouve = new string[0];
                hasRun = true;
            }
            //permet d'agrandir l'array motTrouve
            Array.Resize(ref motTrouve, motTrouve.Length + 1);
            motTrouve[motTrouve.Length - 1] = mot;
        }
        /// <summary>
        /// Donne un string avec toutes les infos du joueur
        /// </summary>
        /// <returns>le string avec les infos</returns>
        public override string ToString()
        {
            
            string aRetrouner = "Le nom du joueur est " + nom + "\nIl/Elle a trouvé ces mots : ";
            //permet d'afficher tout les mots trouvés
             for(int i = 0; i < motTrouve.Length; i++)
             {
                 aRetrouner += motTrouve[i] + "; ";
             }
             aRetrouner += "\nSon score est de : " + score;
            
            return aRetrouner;
        }
        /// <summary>
        /// Ajoute des points au score du joueur
        /// </summary>
        /// <param name="val">nombre de points à ajouter</param>
        public void Add_Score(int val)
        {
            this.score += val;
        }

    }
}
