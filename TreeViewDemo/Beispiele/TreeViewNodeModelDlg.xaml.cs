namespace TreeViewDemo
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TreeViewNodeModelDlg.xaml
    /// </summary>
    public partial class TreeViewNodeModelDlg : Window
    {
        private const string FOLDER_CLOSE = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
        private const string FOLDER_OPEN = "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z";
        private const string FILE = "M14 2H6C4.89 2 4 2.9 4 4V20C4 21.11 4.89 22 6 22H18C19.11 22 20 21.11 20 20V8L14 2M18 20H6V4H13V9H18V20M9.54 15.65L11.63 17.74L10.35 19L7 15.65L10.35 12.3L11.63 13.56L9.54 15.65M17 15.65L13.65 19L12.38 17.74L14.47 15.65L12.38 13.56L13.65 12.3L17 15.65Z";
        private const string MENU = "M3,6H21V8H3V6M3,11H21V13H3V11M3,16H21V18H3V16Z";

        public TreeViewNodeModelDlg()
        {
            this.InitializeComponent();

            WeakEventManager<TreeView, RoutedPropertyChangedEventArgs<object>>.AddHandler(this.MyTreeView, "SelectedItemChanged", this.OnSelectedItemChanged);

            this.CreateTreeContent();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.MyTreeView.SelectedItem is TreeViewItem item && item.Tag is Node node)
            {
                MessageBox.Show($"Geklickt: {node.Name}\nID = {node.Id}\nParent = {node.ParentId}");
            }
        }

        private static List<Node> CreateDemoData()
        {
            List<Node> nodes = new()
                {
                    new Node { Id = 0, ParentId = null, Name = "Root", IsFolder = true },

                    new Node { Id = 1, ParentId = 0, Name = "Child 1", IsFolder = true },
                    new Node { Id = 2, ParentId = 0, Name = "Child 2", IsFolder = true },

                    new Node { Id = 10, ParentId = 1, Name = "Child 1.1", IsFolder = true },
                    new Node { Id = 11, ParentId = 1, Name = "Child 1.2", IsFolder = false },

                    new Node { Id = 20, ParentId = 2, Name = "Child 2.1", IsFolder = false },

                    new Node { Id = 100, ParentId = 10, Name = "Child 1.1 – Sub A", IsFolder = false },
                    new Node { Id = 101, ParentId = 10, Name = "Child 1.1 – Sub B", IsFolder = false },
                };

            return nodes;
        }

        private void CreateTreeContent()
        {
            var rootNode = CreateDemoData().First(n => n.ParentId == null);
            var rootItem = this.CreateItem(rootNode);
            rootItem.IsExpanded = true;

            BuildTree(rootItem, rootNode.Id);
            this.MyTreeView.Items.Add(rootItem);
        }

        private void BuildTree(TreeViewItem parentControl, int parentId)
        {
            var children = CreateDemoData().Where(n => n.ParentId == parentId);

            foreach (var node in children)
            {
                var item = this.CreateItem(node);

                if (node.IsFolder)
                {
                    BuildTree(item, node.Id);
                }

                parentControl.Items.Add(item);
            }
        }

        private TreeViewItem CreateItem(Node node)
        {
            TreeViewItem item = new TreeViewItem
            {
                Tag = node,   // <-- Node am TreeViewItem speichern
                 ContextMenu = CreateContextMenu(node)   // <-- Kontextmenü hier
            };

            if (node.IsFolder)
            {
                item.Header = CreateNodeHeader(node.Name, FOLDER_OPEN);

                item.Expanded += (s, e) =>
                {
                    ImageSource imgSource = GeometryTools.GeometryToImageSource(Geometry.Parse(FOLDER_OPEN), Brushes.Orange);
                    ((Image)((StackPanel)item.Header).Children[0]).Source = imgSource;
                    e.Handled = true;
                };

                item.Collapsed += (s, e) =>
                {
                    ImageSource imgSource = GeometryTools.GeometryToImageSource(Geometry.Parse(FOLDER_CLOSE), Brushes.Orange);
                    ((Image)((StackPanel)item.Header).Children[0]).Source = imgSource;
                    e.Handled = true;
                };

                return item;
            }
            else
            {
                item.Header = CreateNodeHeader(node.Name, FILE);
                return item;
            }
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

        private static TreeViewItem CreateFileItem(string headerText)
        {
            var item = new TreeViewItem
            {
                Header = CreateNodeHeader(headerText, FOLDER_CLOSE)
            };

            return item;
        }

        private static MenuItem CreateMenuItemWithIcon(string text, string iconMenu)
        {
            MenuItem menuItem = new MenuItem();

            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image img = GeometryTools.GeometryToImage(Geometry.Parse(iconMenu), Brushes.Orange);

            TextBlock txt = new TextBlock { Text = text, Margin = new Thickness(5, 0, 0, 0) };

            panel.Children.Add(img);
            panel.Children.Add(txt);
            menuItem.Header = panel;
            return menuItem;
        }

        private ContextMenu CreateContextMenu(Node node)
        {
            var menu = new ContextMenu();

            // Öffnen
            var openItem = CreateMenuItemWithIcon("Öffnen",MENU);
            openItem.Click += (s, e) => MessageBox.Show($"Öffnen von ID {node.Id}: {node.Name}");
            menu.Items.Add(openItem);

            // Info
            var infoItem = new MenuItem { Header = "Informationen" };
            infoItem.Click += (s, e) =>
                MessageBox.Show($"Name: {node.Name}\nID: {node.Id}\nParentId: {node.ParentId}");
            menu.Items.Add(infoItem);

            // Löschen
            var deleteItem = new MenuItem { Header = "Löschen" };
            deleteItem.Click += (s, e) => MessageBox.Show($"Löschen von ID {node.Id}");
            menu.Items.Add(deleteItem);

            return menu;
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }   // null = Root
        public string Name { get; set; }
        public bool IsFolder { get; set; } = true;
    }
}
