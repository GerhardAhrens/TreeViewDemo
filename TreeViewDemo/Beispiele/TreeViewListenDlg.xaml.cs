namespace TreeViewDemo
{
    using System;
    using System.Windows;

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
            throw new NotImplementedException();
        }
    }
}
