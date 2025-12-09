namespace TreeViewDemo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TreeViewStaticDlg.xaml
    /// </summary>
    public partial class TreeViewStaticDlg : Window
    {
        private const string FOLDER_CLOSE = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
        private const string FOLDER_OPEN = "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z";

        public TreeViewStaticDlg()
        {
            this.InitializeComponent();
            this.CreateTreeContent();
        }

        private void CreateTreeContent()
        {
            TreeViewItem root = CreateFolderItem("Root");
            root.IsExpanded = true;

            TreeViewItem child1 = CreateFolderItem("Child 1");
            child1.Items.Add(CreateFileItem("Child 1.1"));
            child1.Items.Add(CreateFileItem("Child 1.2"));

            TreeViewItem child2 = CreateFolderItem("Child 2");
            child2.Items.Add(CreateFileItem("Child 2.1"));

            root.Items.Add(child1);
            root.Items.Add(child2);

            this.MyTreeView.Items.Add(root);
        }

        private static TreeViewItem CreateFolderItem(string headerText)
        {
            var item = new TreeViewItem();

            // Default Icon = geschlossen
            var header = CreateNodeHeader(headerText, FOLDER_OPEN);
            item.Header = header;

            // Icon ändern, wenn aufgeklappt
            item.Expanded += (s, e) =>
            {
                ImageSource imgSource = GeometryTools.GeometryToImageSource(Geometry.Parse(FOLDER_OPEN), Brushes.Orange);
                ((Image)((StackPanel)item.Header).Children[0]).Source = imgSource;
                e.Handled = true;
            };

            // Icon ändern, wenn zugeklappt
            item.Collapsed += (s, e) =>
            {
                ImageSource imgSource = GeometryTools.GeometryToImageSource(Geometry.Parse(FOLDER_CLOSE), Brushes.Orange);
                ((Image)((StackPanel)item.Header).Children[0]).Source = imgSource;
                e.Handled = true;
            };

            return item;
        }

        private static StackPanel CreateNodeHeader(string text, string nodeIcon)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image img = GeometryTools.GeometryToImage(Geometry.Parse(nodeIcon), Brushes.Orange);

            TextBlock txt = new TextBlock { Text = text, Margin = new Thickness(5, 0, 0, 0) };

            panel.Children.Add(img);
            panel.Children.Add(txt);

            return panel;
        }

        private static TreeViewItem CreateFileItem(string headerText)
        {
            var item = new TreeViewItem
            {
                Header = CreateNodeHeader(headerText, FOLDER_CLOSE)
            };

            return item;
        }
    }
}
