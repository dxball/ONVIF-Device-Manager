using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace utils {
    public class RelayCommand : ICommand {

        readonly Action<object> _execute;
        readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) {
            if (_canExecute == null || _canExecute(parameter))
                _execute(parameter);
        }

        public static void InvalidateCanExecute()
        {
            CommandManager.InvalidateRequerySuggested();
        }

    }
}
