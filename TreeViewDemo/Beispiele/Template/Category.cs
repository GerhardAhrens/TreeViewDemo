namespace TreeViewDemo.Beispiele.Template
{
    using System.Collections.ObjectModel;

    internal sealed class Category : NotifyBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
    }
}
