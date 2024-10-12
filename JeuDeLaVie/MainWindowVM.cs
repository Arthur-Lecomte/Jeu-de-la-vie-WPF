using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuDeLaVie {
    public class MainWindowVM : BasePropertyChanged{

        private int tailleCellule = 10;
        public int TailleCellule {
            get { return tailleCellule; }
            set { tailleCellule = value; RaisePropertyChanged(); }
        }

        private bool[,] etat;
        public bool[,] Etat {
            get { return etat; }
            set { etat = value; RaisePropertyChanged(); }
        }
    }
}
