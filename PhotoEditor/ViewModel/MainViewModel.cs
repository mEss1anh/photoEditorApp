using Microsoft.Win32;
using PhotoEditor.Model;
using PhotoEditor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PhotoEditor.ViewModel
{
    class MainViewModel
    {
        public MainViewModel()
        {

        }

        public class FileDialogClass : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            OpenFileDialog dialog = new OpenFileDialog();
            ModelClassImage m = new ModelClassImage();

            private void OpenFile(object sender, RoutedEventArgs e)
            {
                // Исходная директория
                dialog.InitialDirectory = Environment.CurrentDirectory;
                dialog.Filter = "XML files (*.xml)|*.xml";
                if (dialog.ShowDialog().Value)
                {
                    try
                    {
                        
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка загрузки данных");
                    }
                }
            }
        }



        public class Command : ICommand
        {
            #region Constructor

            public Command(Action<object> action)
            {
                ExecuteDelegate = action;
            }

            #endregion


            #region Properties

            public Predicate<object> CanExecuteDelegate { get; set; }
            public Action<object> ExecuteDelegate { get; set; }

            #endregion


            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                if (CanExecuteDelegate != null)
                {
                    return CanExecuteDelegate(parameter);
                }

                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                if (ExecuteDelegate != null)
                {
                    ExecuteDelegate(parameter);
                }
            }

            #endregion
    }
}
}
