namespace TreeViewDemo
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TreeViewListenDlg.xaml
    /// </summary>
    public partial class TreeViewListenDlg : Window
    {
        private const string FOLDER_CLOSE = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
        private const string FOLDER_OPEN = "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z";
        private const string FILE = "M14 2H6C4.89 2 4 2.9 4 4V20C4 21.11 4.89 22 6 22H18C19.11 22 20 21.11 20 20V8L14 2M18 20H6V4H13V9H18V20M9.54 15.65L11.63 17.74L10.35 19L7 15.65L10.35 12.3L11.63 13.56L9.54 15.65M17 15.65L13.65 19L12.38 17.74L14.47 15.65L12.38 13.56L13.65 12.3L17 15.65Z";
        private const string MENU = "M3,6H21V8H3V6M3,11H21V13H3V11M3,16H21V18H3V16Z";
        private const string DELETE = "M9,3V4H4V6H5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V6H20V4H15V3H9M7,6H17V19H7V6M9,8V17H11V8H9M13,8V17H15V8H13Z";

        public TreeViewListenDlg()
        {
            this.InitializeComponent();
            this.CreateTreeContent();
        }

        private void CreateTreeContent()
        {
            WeakEventManager<TreeView, RoutedPropertyChangedEventArgs<object>>.AddHandler(this.MyTreeView, "SelectedItemChanged", this.OnSelectedItemChanged);

            foreach (var item in TreeItemData.Level0())
            {
                var node = this.CreateNode(item, level: 0);
                this.MyTreeView.Items.Add(node);
            }
        }

        private TreeViewItem CreateNode(LevelItem item, int level)
        {
            TreeViewItem node = new TreeViewItem();
            if (level == 0)
            {
                node.Header = this.CreateNodeHeader(item.Name, FOLDER_CLOSE);
            }
            else if (level == 1)
            {
                node.Header = this.CreateNodeHeader(item.Name, FILE);
            }
            else if (level == 2)
            {
                node.Header = this.CreateNodeHeader(item.Name, MENU);
            }

            node.ContextMenu = this.CreateContextMenu(item);
            node.Tag = new NodeTag { Item = item, Level = level };

            if (item.ChildIds.Count > 0)
            {
                node.Items.Add("Loading...");
            }

            return node;
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item)
            {
                NodeTag node = item.Tag as NodeTag;

                MessageBox.Show($"Geklickt: Level = {node.Level}\nID = {node.Item.Id} ({node.Item.Name})\nChilds = {node.Item.ChildIds.Count}");
            }
        }

        private StackPanel CreateNodeHeader(string text, string nodeIcon)
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

        private void OnTreeExpanded(object sender, RoutedEventArgs e)
        {
            var node = e.OriginalSource as TreeViewItem;
            if (node == null)
            {
                return;
            }

            // Wurde dieser Node bereits geladen?
            if (node.Items.Count == 1 && node.Items[0] is string)
            {
                node.Items.Clear();
                this.LoadChildren(node);
            }
        }

        private void LoadChildren(TreeViewItem parentNode)
        {
            var tag = (NodeTag)parentNode.Tag;
            var item = tag.Item;

            int nextLevel = tag.Level + 1;

            List<LevelItem> nextList = this.GetListForLevel(nextLevel);
            if (nextList == null)
            {
                return;
            }

            var children = nextList.Where(x => item.ChildIds.Contains(x.Id)).ToList();

            foreach (LevelItem child in children)
            {
                parentNode.Items.Add(this.CreateNode(child, nextLevel));
            }
        }

        private ContextMenu CreateContextMenu(LevelItem item)
        {
            var menu = new ContextMenu();

            // Öffnen
            var openItem = this.CreateMenuItemWithIcon("Öffnen", MENU);
            openItem.Click += (s, e) => MessageBox.Show($"Öffnen von ID {item.Id}: {item.Name}");
            menu.Items.Add(openItem);

            // Info
            var infoItem = this.CreateMenuItemWithIcon("Informationen", FILE);
            infoItem.Click += (s, e) =>
                MessageBox.Show($"Name: {item.Name}\nID: {item.Id}");
            menu.Items.Add(infoItem);

            // Löschen
            var deleteItem = this.CreateMenuItemWithIcon("Löschen", DELETE);
            deleteItem.Click += (s, e) => MessageBox.Show($"Löschen von ID {item.Id}");
            menu.Items.Add(deleteItem);

            return menu;
        }

        private MenuItem CreateMenuItemWithIcon(string text, string iconMenu)
        {
            MenuItem menuItem = new MenuItem();

            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            System.Windows.Shapes.Path imgShape = GeometryTools.GetPathGeometry(iconMenu, Colors.Orange);

            TextBlock txt = new TextBlock { Text = text, Margin = new Thickness(5, 0, 0, 0) };

            panel.Children.Add(txt);
            menuItem.Header = panel;
            menuItem.Icon = imgShape;
            return menuItem;
        }

        private List<LevelItem> GetListForLevel(int level)
        {
            return level switch
            {
                0 => TreeItemData.Level0(),
                1 => TreeItemData.Level1(),
                2 => TreeItemData.Level2(),
                _ => null  // weitere Level möglich
            };
        }

        private sealed class NodeTag
        {
            public LevelItem Item { get; set; }
            public int Level { get; set; }
        }
    }


    public class TreeItemData
    {
        public static List<LevelItem> Level0()
        {
            // Root-Ebene
            return new()
            {
                new() { Id = 1, Name = "Root A", ChildIds = new(){ 10, 11,12 }},
                new() { Id = 2, Name = "Root B", ChildIds = new(){ 20 }},
            };
        }


        public static List<LevelItem> Level1()
        {
            // 2. Ebene
            return new()
            {
                new() { Id = 10, Name = "A-Child 1", ChildIds = new(){ 100 }},
                new() { Id = 11, Name = "A-Child 2", ChildIds = new(){ 110 }},
                new() { Id = 12, Name = "A-Child 3", ChildIds = new(){ 120,130,140,150,160 }},
                new() { Id = 20, Name = "B-Child 1", ChildIds = new(){}}
            };
        }

        public static List<LevelItem> Level2()
        {
            // 3. Ebene
            return new()
            {
                new() { Id = 100, Name = "A-Child 1-1 - SubChild", ChildIds = new() },
                new() { Id = 110, Name = "A-Child 2-1 - SubChild", ChildIds = new() },
                new() { Id = 120, Name = "A-Child 3-1 - SubChild", ChildIds = new() },
                new() { Id = 130, Name = "A-Child 3-2 - SubChild", ChildIds = new() },
                new() { Id = 140, Name = "A-Child 3-3 - SubChild", ChildIds = new() },
                new() { Id = 150, Name = "A-Child 3-4 - SubChild", ChildIds = new() },
                new() { Id = 160, Name = "A-Child 3-5 - SubChild", ChildIds = new() },
            };
        }
    }

    public class LevelItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ChildIds { get; set; } = new();
    }
}
