namespace TreeViewDemo
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TreeViewDynamicDlg.xaml
    /// </summary>
    public partial class TreeViewDynamicDlg : Window
    {
        private const string FOLDER_CLOSE = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
        private const string FOLDER_OPEN = "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z";

        public TreeViewDynamicDlg()
        {
            this.InitializeComponent();
            this.CreateTreeContent();
        }

        private static Dictionary<int, string> CreateDemoData()
        {
            Dictionary<int, string> nodes = new()
                        {
                            { 1, "Child 1.1" },
                            { 2, "Child 1.2" },
                            { 3, "Child 2.1" },
                            { 4, "Child 2.2" },
                            { 11, "Child 1.1 - Sub A" },
                            { 12, "Child 1.1 - Sub B" }
                        };

            return nodes;
        }

        private void CreateTreeContent()
        {
            // Root
            var root = CreateFolderItem("Root");
            root.IsExpanded = true;

            // Statische Ebene 1
            var child1 = CreateFolderItem("Child 1");
            var child2 = CreateFolderItem("Child 2");

            // Lade Ebene 2 aus Dictionary (IDs 1x und 2x)
            AddChildrenFromDictionary(child1, parentId: 1);
            AddChildrenFromDictionary(child2, parentId: 2);

            // Struktur zusammenbauen
            root.Items.Add(child1);
            root.Items.Add(child2);

            this.MyTreeView.Items.Add(root);
        }

        /// <summary>
        /// Lädt alle Elemente aus dem Dictionary, deren ID mit parentId beginnt.
        /// Beispiel:
        /// parentId = 1 → lädt 1, 11, 12 (zweite Ebene)
        /// </summary>
        private static void AddChildrenFromDictionary(TreeViewItem parentItem, int parentId)
        {
            // 2. Ebene (IDs wie 1, 2, 3 ...)
            var children = CreateDemoData().Where(x => x.Key.ToString(CultureInfo.CurrentCulture).Length == 1 && x.Key.ToString(CultureInfo.CurrentCulture) == parentId.ToString(CultureInfo.CurrentCulture));

            foreach (var child in children)
            {
                var item = CreateFolderItem(child.Value);

                // 3. Ebene (IDs wie 11, 12, 21, 22 ...)
                AddSubChildrenFromDictionary(item, parentId);

                parentItem.Items.Add(item);
            }
        }

        /// <summary>
        /// Lädt alle Elemente aus dem Dictionary, deren ID mit parentId beginnt.
        /// Beispiel:
        /// parentId = 1 → lädt 1, 11, 12 (zweite Ebene)
        /// </summary>
        private static void AddSubChildrenFromDictionary(TreeViewItem parentItem, int parentId)
        {
            string prefix = parentId.ToString(CultureInfo.CurrentCulture);

            var children = CreateDemoData().Where(x => x.Key.ToString(CultureInfo.CurrentCulture).StartsWith(prefix,StringComparison.CurrentCultureIgnoreCase) && x.Key.ToString(CultureInfo.CurrentCulture).Length == 2
            );

            foreach (var child in children)
            {
                parentItem.Items.Add(CreateFileItem(child.Value));
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
    }
}
