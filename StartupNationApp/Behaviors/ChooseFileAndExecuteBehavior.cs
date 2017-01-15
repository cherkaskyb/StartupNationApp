using GalaSoft.MvvmLight.Command;
using StartupNationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace StartupNationApp.Behaviors
{
    public class ChooseFileAndExecuteBehavior : Behavior<Button>
    {
        public RelayCommand<string> CommandToExecute
        {
            get { return (RelayCommand<string>)GetValue(CommandToExecuteDp); }
            set { SetValue(CommandToExecuteDp, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandToExecuteDp =
            DependencyProperty.Register("CommandToExecute", typeof(RelayCommand<string>), 
                typeof(ChooseFileAndExecuteBehavior));

        protected override void OnAttached()
        {
            AssociatedObject.Click += AssociatedObject_Click;
            base.OnAttached();
        }

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var filepath = ChooseFIleDialog.GetFilePath(checkFileExists: false);
            CommandToExecute.Execute(filepath);
        }
    }
}
