namespace TreeViewDemo.Beispiele.Template
{
    using System.Collections.ObjectModel;

    internal sealed class Group : NotifyBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    }
}
