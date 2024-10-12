using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JeuDeLaVie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int nbrLigne;
        private int nbrColonne;
        private bool[,] etatPrecedent;
        DispatcherTimer gameTimer;
        bool isGameRunning = false;
        public MainWindow() {
            InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(300); 
            gameTimer.Tick += GameTick;
        }

        private MainWindowVM vm {
            get { return this.DataContext as MainWindowVM; }
        }

        private void GameTick(object sender, EventArgs e) {
            EtatGrilleSuivant();
            DessineJeu();
        }

        private void StartClick(object sender, RoutedEventArgs e) {
            if (isGameRunning) {
                gameTimer.Stop();
                StartStopButton.Content = "Start";
                isGameRunning = false;
            } else {
                gameTimer.Start();
                StartStopButton.Content = "Pause";
                isGameRunning = true;
            }
        }

        private void InitialiseJeu() {
            nbrLigne = (int)Math.Floor(Canvas.ActualHeight / vm.TailleCellule);
            nbrColonne = (int)Math.Floor(Canvas.ActualWidth / vm.TailleCellule);

            vm.Etat = new bool[nbrLigne, nbrColonne];
        }

        private void DessineJeu() {
            Canvas.Children.Clear();
            for (int i = 0; i < vm.Etat.GetLength(0); i++) {
                for (int j = 0; j < vm.Etat.GetLength(1); j++) {
                    var cell = new Rectangle {
                        Width = vm.TailleCellule,
                        Height = vm.TailleCellule,
                        Fill = vm.Etat[i, j] ? Brushes.White : Brushes.Black,
                        Stroke = Brushes.Gray
                    };

                    Canvas.SetTop(cell, i * vm.TailleCellule);
                    Canvas.SetLeft(cell, j * vm.TailleCellule);

                    int currentRow = i;
                    int currentCol = j;

                    cell.MouseLeftButtonDown += (sender, e) => CellClicked(sender, e, currentRow, currentCol);

                    Canvas.Children.Add(cell);
                }
            }
        }

        private void EtatGrilleSuivant() {
            etatPrecedent = new bool[vm.Etat.GetLength(0),vm.Etat.GetLength(1)];
            Array.Copy(vm.Etat, etatPrecedent, etatPrecedent.Length);

            for (int i = 0; i < vm.Etat.GetLength(0); ++i) {
                for (int j = 0; j < vm.Etat.GetLength(1); ++j) {
                    EtatCelluleSuivant(i, j);
                }
            }
        }

        private int CompteVoisinVivant(int row, int col) {
            int count = 0;
            for (int i = -1; i <= 1; ++i) {
                for (int j = -1; j <= 1; ++j) {
                    if (i == 0 && j == 0) continue;

                    int rowVoisin = row + i;
                    int colVoisin = col + j;

                    if (rowVoisin < 0 || colVoisin < 0 || rowVoisin >= nbrLigne || colVoisin >= nbrColonne) continue;

                    if (etatPrecedent[rowVoisin, colVoisin]) count++;
                }
            }
            return count;
        }

        private void EtatCelluleSuivant(int row, int col) {
            int count = CompteVoisinVivant(row, col);

            if (count == 3) 
                vm.Etat[row, col] = true;
            else if 
                (count != 2) vm.Etat[row, col] = false;
        }

        private void CellClicked(object sender, MouseButtonEventArgs e, int row, int col) {
            vm.Etat[row, col] = !vm.Etat[row, col];
            DessineJeu();
        }

        private void CanvasLoaded(object sender, RoutedEventArgs e) {
            InitialiseJeu();
            DessineJeu();
        }

        private void AléatoireClick(object sender, RoutedEventArgs e) {
            if (isGameRunning) return;

            Random rand = new Random();
            for (int i = 0; i < vm.Etat.GetLength(0); i++) {
                for (int j = 0; j < vm.Etat.GetLength(1); j++) {
                    vm.Etat[i, j] = rand.Next(0, 2) == 1;
                }
            }

            DessineJeu();
        }
    }
}
