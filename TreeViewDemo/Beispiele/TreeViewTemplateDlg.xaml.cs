namespace TreeViewDemo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using TreeViewDemo.Beispiele.Template;

    /// <summary>
    /// Interaktionslogik für TreeViewTemplateDlg.xaml
    /// </summary>
    public partial class TreeViewTemplateDlg : Window
    {
        public TreeViewTemplateDlg()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
        }

    }
}
