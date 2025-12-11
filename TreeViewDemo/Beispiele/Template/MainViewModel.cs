namespace TreeViewDemo.Beispiele.Template
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    internal sealed class MainViewModel : NotifyBase
    {
        public ICommand CategoryMenuCommand { get; }
        public ICommand GroupMenuCommand { get; }
        public ICommand ItemMenuCommand { get; }

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public ICommand ItemClickedCommand { get; }

        public MainViewModel()
        {
            ItemClickedCommand = new RelayCommand(this.OnItemClicked);
            CategoryMenuCommand = new RelayCommand(OnCategoryMenu);
            GroupMenuCommand = new RelayCommand(OnGroupMenu);
            ItemMenuCommand = new RelayCommand(OnItemMenu);

            // Testdaten
            this.Categories.Add(new Category
            {
                Name = "Kategorie A",
                Groups =
            {
                new Group
                {
                    Name = "Gruppe 1",
                    Items =
                    {
                        new Item { Name = "Item 1" },
                        new Item { Name = "Item 2" }
                    }
                },
                new Group
                {
                    Name = "Gruppe 2",
                    Items =
                    {
                        new Item { Name = "Item 3" }
                    }
                }
            }
            });

            Categories.Add(new Category
            {
                Name = "Kategorie B",
                Groups =
            {
                new Group
                {
                    Name = "Gruppe 3",
                    Items =
                    {
                        new Item { Name = "Item 4" }
                    }
                }
            }
            });

            CategoryMenuCommand = new RelayCommand(OnCategoryMenu);

        }

        private void OnCategoryMenu(object obj)
        {
            if (obj is Category cat)
            {
                MessageBox.Show($"Kategorie Menü: {cat.Name}");
            }
        }

        private void OnGroupMenu(object obj)
        {
            if (obj is Group grp)
            {
                MessageBox.Show($"Gruppen Menü: {grp.Name}");
            }
        }

        private void OnItemMenu(object obj)
        {
            if (obj is Item itm)
            {
                MessageBox.Show($"Item Menü: {itm.Name}");
            }
        }
        private void OnItemClicked(object obj)
        {
            // Hier wird alles ausgeführt beim Klick!
            MessageBox.Show($"Geklickt: {obj?.ToString()}");
        }
    }
}
