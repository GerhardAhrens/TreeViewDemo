namespace TreeViewDemo
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TreeViewSearchFilterDlg.xaml
    /// </summary>
    public partial class TreeViewSearchFilterDlg : Window
    {
        public TreeViewSearchFilterDlg()
        {
            this.InitializeComponent();
            WeakEventManager<TreeView, RoutedPropertyChangedEventArgs<object>>.AddHandler(this.MyTreeView, "SelectedItemChanged", this.OnSelectedItemChanged);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnSuchen, "Click", this.OnSearchClick);
            this.MyTreeView.ItemsSource = this.CreateDemoData();
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim().ToLower(CultureInfo.CurrentCulture);
            // Filter + Hervorhebung anwenden
            var filtered = this.ApplySearchAndFilter(this.CreateDemoData(), query);

            this.MyTreeView.ItemsSource = filtered;
        }

        private void OnFilterClick(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim().ToLower(CultureInfo.CurrentCulture);
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.MyTreeView.SelectedItem is TreeItem item)
            {
                MessageBox.Show($"Geklickt: {item.Name}\nChildrens {item.Children.Count}");
            }
        }

        private List<TreeItem> ApplySearchAndFilter(List<TreeItem> items, string query)
        {
            query = query?.Trim();
            bool hasQuery = !string.IsNullOrWhiteSpace(query);

            if (!hasQuery)
            {
                // Keine Suche → originale Daten zurückgeben, ohne Hervorhebung
                this.ClearMatches(items);
                return items;
            }

            return this.FilterAndMark(items, query.ToLower(CultureInfo.CurrentCulture));
        }

        private List<TreeItem> FilterAndMark(List<TreeItem> items, string query)
        {
            List<TreeItem> result = new();

            foreach (var item in items)
            {
                var filteredChildren = FilterAndMark(item.Children, query);

                bool ownMatch = item.Name.Contains(query, StringComparison.OrdinalIgnoreCase);

                if (ownMatch || filteredChildren.Any())
                {
                    var newItem = new TreeItem
                    {
                        Name = item.Name,
                        Children = filteredChildren,
                        IsMatch = ownMatch,
                        SearchQuery = query,

                        // 👉 Knoten öffnen, wenn Treffer oder Kindtreffer existiert
                        IsExpanded = ownMatch || filteredChildren.Any()
                    };

                    result.Add(newItem);
                }
            }

            return result;
        }


        private void ClearMatches(List<TreeItem> items)
        {
            foreach (var item in items)
            {
                item.IsMatch = false;
                ClearMatches(item.Children);
            }
        }


        private List<TreeItem> CreateDemoData()
        {
            List<TreeItem> treeData = new()
            {
                    new TreeItem { Name = "Fahrzeuge", Children = { new TreeItem { Name = "Auto" }, new TreeItem { Name = "Motorrad" }, new TreeItem { Name = "Fahrrad" } }},
                    new TreeItem { Name = "Tiere", Children = { new TreeItem { Name = "Hund" }, new TreeItem { Name = "Katze" }, new TreeItem { Name = "Maus" }}}
            };

            return treeData;
        }

    }

    internal sealed class HighlightTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not TreeItem item || string.IsNullOrWhiteSpace(item.SearchQuery))
            {
                return new Run(((TreeItem)value).Name);
            }

            string text = item.Name;
            string query = item.SearchQuery;
            int index = text.IndexOf(query, StringComparison.OrdinalIgnoreCase);

            // kein Treffer
            if (index < 0)
            {
                return new Run(text);
            }

            var span = new Span();

            // 1. Teil vor dem Treffer
            if (index > 0)
            {
                span.Inlines.Add(new Run(text[..index]));
            }

            // 2. Hervorgehobener Treffer
            span.Inlines.Add(new Run(text.Substring(index, query.Length))
            {
                Background = Brushes.Yellow,
                FontWeight = FontWeights.Bold
            });

            // 3. Teil nach dem Treffer
            int end = index + query.Length;
            if (end < text.Length)
            {
                span.Inlines.Add(new Run(text[end..]));
            }

            return span;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    internal sealed class TreeItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public List<TreeItem> Children { get; set; } = new();

        public bool IsMatch { get; set; }
        public string SearchQuery { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
