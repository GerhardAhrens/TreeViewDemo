namespace TreeViewDemo
{
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaktionslogik für TreeViewRecursivDlg.xaml
    /// </summary>
    public partial class TreeViewRecursivDlg : Window
    {
        private const string FOLDER_CLOSE = "M20,18H4V8H20M20,6H12L10,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V8C22,6.89 21.1,6 20,6Z";
        private const string FOLDER_OPEN = "M6.1,10L4,18V8H21A2,2 0 0,0 19,6H12L10,4H4A2,2 0 0,0 2,6V18A2,2 0 0,0 4,20H19C19.9,20 20.7,19.4 20.9,18.5L23.2,10H6.1M19,18H6L7.6,12H20.6L19,18Z";
        private const string FILE = "M14 2H6C4.89 2 4 2.9 4 4V20C4 21.11 4.89 22 6 22H18C19.11 22 20 21.11 20 20V8L14 2M18 20H6V4H13V9H18V20M9.54 15.65L11.63 17.74L10.35 19L7 15.65L10.35 12.3L11.63 13.56L9.54 15.65M17 15.65L13.65 19L12.38 17.74L14.47 15.65L12.38 13.56L13.65 12.3L17 15.65Z";
        private const string MENU = "M3,6H21V8H3V6M3,11H21V13H3V11M3,16H21V18H3V16Z";
        private const string DELETE = "M9,3V4H4V6H5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V6H20V4H15V3H9M7,6H17V19H7V6M9,8V17H11V8H9M13,8V17H15V8H13Z";

        public TreeViewRecursivDlg()
        {
            this.InitializeComponent();
            this.CreateTreeContent();
        }

        private void CreateTreeContent()
        {
            string rootPath = @"C:\_Projekte";   // <<== Anpassen
            var rootItem = this.CreateTreeItem(rootPath, isDirectory: true);
            this.MyTreeView.Items.Add(rootItem);
        }

        private TreeViewItem CreateTreeItem(string path, bool isDirectory)
        {
            var item = new TreeViewItem()
            {
                Header = ShellIconHelper.CreateHeader(path),
                Tag = path
            };

            item.ContextMenu = this.CreateContextMenu(path);

            if (isDirectory == true)
            {
                // Platzhalter-Knoten für Lazy Loading
                item.Items.Add(null);
                item.Expanded += this.Folder_Expanded;
            }

            return item;
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            // Nur laden, wenn Platzhalter existiert
            if (item.Items.Count == 1 && item.Items[0] == null)
            {
                item.Items.Clear();
                string path = item.Tag.ToString();

                // Ordner
                try
                {
                    foreach (var dir in Directory.GetDirectories(path))
                    {
                        item.Items.Add(CreateTreeItem(dir, isDirectory: true));
                    }

                    // Dateien
                    foreach (var file in Directory.GetFiles(path))
                    {
                        item.Items.Add(CreateTreeItem(file, isDirectory: false));
                    }
                }
                catch { /* z.B. Zugriffsfehler ignorieren */ }
            }
        }


        private static MenuItem CreateMenuItemWithIcon(string text, string iconMenu)
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

        private ContextMenu CreateContextMenu(string path)
        {
            var menu = new ContextMenu();

            // Öffnen
            var openItem = CreateMenuItemWithIcon("Öffnen", MENU);
            openItem.Click += (s, e) => MessageBox.Show($"Öffnen: {path}");
            menu.Items.Add(openItem);

            // Info
            var infoItem = CreateMenuItemWithIcon("Informationen", FILE);
            infoItem.Click += (s, e) =>  MessageBox.Show($"Pfad: {path}", "Information");
            menu.Items.Add(infoItem);

            // Löschen
            var deleteItem = CreateMenuItemWithIcon("Löschen", DELETE);
            deleteItem.Click += (s, e) => MessageBox.Show($"Pfad: {path}", "Löschen");
            menu.Items.Add(deleteItem);

            menu.Items.Add(new Separator());

            // Explorer öffnen
            var explorerItem = CreateMenuItemWithIcon("Im Explorer anzeigen", FILE);
            explorerItem.Click += (s, e) => System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{path}\"");
            menu.Items.Add(explorerItem);

            // Pfad kopieren
            var pathCopytem = CreateMenuItemWithIcon("Pfad kopieren", FILE);
            pathCopytem.Click += (s, e) => Clipboard.SetText(path);
            menu.Items.Add(pathCopytem);

            return menu;
        }
    }

    public static class ShellIconHelper
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;
        private const uint FILE_ATTRIBUTE_FILE = 0x80;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        // ----------------------------------------------------
        // Icon + Text im TreeViewItem als Header erzeugen
        // ----------------------------------------------------
        public static StackPanel CreateHeader(string path)
        {
            bool isDirectory = Directory.Exists(path);

            Image icon = new Image
            {
                Source = GetShellIcon(path, isDirectory),
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 5, 0)
            };

            TextBlock text = new TextBlock
            {
                Text = System.IO.Path.GetFileName(path),
                VerticalAlignment = VerticalAlignment.Center
            };

            var panel = new StackPanel { Orientation = Orientation.Horizontal };
            panel.Children.Add(icon);
            panel.Children.Add(text);

            return panel;
        }

        // ----------------------------------------------------
        // Echte Windows-Shell-Icons laden
        // ----------------------------------------------------
        public static ImageSource GetShellIcon(string path, bool isDirectory)
        {
            uint flags = SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;
            uint attr = isDirectory ? FILE_ATTRIBUTE_DIRECTORY : FILE_ATTRIBUTE_FILE;

            var shinfo = new SHFILEINFO();
            IntPtr hImg = SHGetFileInfo(path, attr, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            if (hImg == IntPtr.Zero)
            {
                return null;
            }

            Icon icon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            BitmapSource image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            icon.Dispose();
            return image;
        }
    }
}
