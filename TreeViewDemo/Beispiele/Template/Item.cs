namespace TreeViewDemo.Beispiele.Template
{
    internal sealed class Item : NotifyBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
