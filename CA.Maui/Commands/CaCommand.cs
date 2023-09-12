using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CA.Maui.Commands
{
    public sealed class CaCommand<T> : CaCommand
    {
        public CaCommand(Action<T> execute) :
            base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            })
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
        }

        public CaCommand(Action<T> execute, Func<T, bool> canExecute)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            }, o => IsValidParameter(o) && canExecute((T)o))
        {
            ArgumentNullException.ThrowIfNull(execute, nameof(execute));
            ArgumentNullException.ThrowIfNull(canExecute, nameof(canExecute));
        }

        static bool IsValidParameter(object o)
        {
            if (o is not null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                return o is T;
            }

            var t = typeof(T);
            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }
            // Not a Nullable, if it's a value type then null is not valid
            return !t.IsValueType;
        }
    }
    public class CaCommand : ICaCommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecuteFunc;
        private bool _canExecute;

        public CaCommand(Action<object> execute)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = true;
        }

        public CaCommand(Action execute) : this(o => execute())
        {
            ArgumentNullException.ThrowIfNull(execute);
        }

        public CaCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            ArgumentNullException.ThrowIfNull(canExecute);
            _canExecuteFunc = canExecute;
        }

        public CaCommand(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            ArgumentNullException.ThrowIfNull(execute);
            ArgumentNullException.ThrowIfNull(canExecute);
        }
        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc?.Invoke(parameter) ?? _canExecute;
        }

        public void Execute(object parameter)
        {
            if(_canExecute) 
                _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void DisableExecution()
        {
            _canExecute = false;
        }

        public void EnableExecution()
        {
            _canExecute = true;
        }
    }
}
