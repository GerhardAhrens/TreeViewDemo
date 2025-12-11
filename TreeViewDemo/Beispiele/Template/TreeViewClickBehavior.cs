namespace TreeViewDemo.Beispiele.Template
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    internal static class TreeViewClickBehavior
    {
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.RegisterAttached(
                "ItemClickCommand",
                typeof(ICommand),
                typeof(TreeViewClickBehavior),
                new PropertyMetadata(null, OnAttached));

        public static void SetItemClickCommand(DependencyObject obj, ICommand value)
            => obj.SetValue(ItemClickCommandProperty, value);

        public static ICommand GetItemClickCommand(DependencyObject obj)
            => (ICommand)obj.GetValue(ItemClickCommandProperty);

        private static void OnAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeView treeView)
            {
                treeView.SelectedItemChanged += (sender, args) =>
                {
                    ICommand cmd = GetItemClickCommand(treeView);
                    if (cmd != null && cmd.CanExecute(args.NewValue))
                    {
                        cmd.Execute(args.NewValue);
                    }
                };
            }
        }
    }
}
