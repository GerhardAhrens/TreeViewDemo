//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de yyyy
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace TreeViewDemo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.WindowTitel = "WPF TreeView Demo";
            this.DataContext = this;
        }

        private string _WindowTitel;

        public string WindowTitel
        {
            get { return _WindowTitel; }
            set
            {
                if (this._WindowTitel != value)
                {
                    this._WindowTitel = value;
                    this.OnPropertyChanged();
                }
            }
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnCloseApplication, "Click", this.OnCloseApplication);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielA, "Click", this.OnClickBeispielA);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielB, "Click", this.OnClickBeispielB);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielC, "Click", this.OnClickBeispielC);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielD, "Click", this.OnClickBeispielD);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielE, "Click", this.OnClickBeispielE);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielF, "Click", this.OnClickBeispielF);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnBeispielG, "Click", this.OnClickBeispielG);
        }

        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            MessageBoxResult msgYN = MessageBox.Show("Wollen Sie die Anwendung beenden?", "Beenden", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgYN == MessageBoxResult.Yes)
            {
                App.ApplicationExit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// TreeView Statisch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielA(object sender, RoutedEventArgs e)
        {
            TreeViewStaticDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView dynamisch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielB(object sender, RoutedEventArgs e)
        {
            TreeViewDynamicDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView Node Model Class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielC(object sender, RoutedEventArgs e)
        {
            TreeViewNodeModelDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView Listen für jede Ebene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielD(object sender, RoutedEventArgs e)
        {
            TreeViewListenDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView Rekursiv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielE(object sender, RoutedEventArgs e)
        {
            TreeViewRecursivDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView HierarchicalDataTemplate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielF(object sender, RoutedEventArgs e)
        {
            TreeViewTemplateDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }

        /// <summary>
        /// TreeView HierarchicalDataTemplate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBeispielG(object sender, RoutedEventArgs e)
        {
            TreeViewSearchFilterDlg window = new();
            window.Owner = this;
            window.ShowDialog();
        }
        #region INotifyPropertyChanged implementierung
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
        #endregion INotifyPropertyChanged implementierung
    }
}