namespace TreeViewDemo
{
    using System;
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

        public TreeViewListenDlg()
        {
            this.InitializeComponent();
            this.CreateTreeContent();
        }

        private void CreateTreeContent()
        {
            foreach (var item in TreeData.Level0())
            {
                var node = this.CreateNode(item, level: 0);
                this.MyTreeView.Items.Add(node);
            }
        }

        private TreeViewItem CreateNode(LevelItem item, int level)
        {
            TreeViewItem node = new TreeViewItem();
            node.Header = CreateNodeHeader(item.Name, FOLDER_CLOSE);
            node.Tag = new NodeTag { Item = item, Level = level };

            if (item.ChildIds.Count > 0)
            {
                node.Items.Add("Loading...");
            }

            return node;
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

            List<LevelItem> nextList = GetListForLevel(nextLevel);
            if (nextList == null)
            {
                return;
            }

            var children = nextList.Where(x => item.ChildIds.Contains(x.Id)).ToList();

            foreach (LevelItem child in children)
            {
                parentNode.Items.Add(CreateNode(child, nextLevel));
            }
        }

        private List<LevelItem> GetListForLevel(int level)
        {
            return level switch
            {
                0 => TreeData.Level0(),
                1 => TreeData.Level1(),
                2 => TreeData.Level2(),
                _ => null  // weitere Level möglich
            };
        }

        private sealed class NodeTag
        {
            public LevelItem Item { get; set; }
            public int Level { get; set; }
        }
    }


    public class TreeData
    {
        public static List<LevelItem> Level0()
        {
            // Root-Ebene
            return new()
            {
                new() { Id = 1, Name = "Root A", ChildIds = new(){ 10, 11 }},
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
                new() { Id = 20, Name = "B-Child 1", ChildIds = new(){}}
            };
        }

        public static List<LevelItem> Level2()
        {
            // 3. Ebene
            return new()
            {
                new() { Id = 100, Name = "A-Child 1 - Subchild", ChildIds = new() },
                new() { Id = 110, Name = "A-Child 2 - Subchild", ChildIds = new() }
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
