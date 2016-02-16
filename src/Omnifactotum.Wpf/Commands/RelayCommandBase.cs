using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf.Commands
{
    /// <summary>
    ///     The base class for <see cref="RelayCommand"/> and <see cref="AsyncRelayCommand"/>.
    /// </summary>
    public abstract class RelayCommandBase : ICommand, INotifyPropertyChanged
    {
        #region Constants and Fields

        [NotNull]
        private readonly object _syncLock = new object();

        [NotNull]
        private readonly Action<object, CancellationToken> _execute;

        [NotNull]
        private readonly Func<object, bool> _canExecute;

        [NotNull]
        private readonly Action<Exception> _handleException;

        [NotNull]
        private readonly Dispatcher _dispatcher;

        [CanBeNull]
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isExecuting;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommandBase"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <param name="canExecute">
        ///     A reference to a method that determines if the command can be executes, or
        ///     <c>null</c> if the command is always available for execution.
        /// </param>
        /// <param name="handleException">
        ///     A reference to a method that handles an exception occurred in the command's logic, if any.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        internal RelayCommandBase(
            [NotNull] Action<object, CancellationToken> execute,
            [CanBeNull] Func<object, bool> canExecute,
            [CanBeNull] Action<Exception> handleException)
        {
            #region Argument Check

            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            #endregion

            _execute = execute;
            _canExecute = canExecute ?? CanExecuteByDefault;
            _handleException = handleException ?? HandleExceptionByDefault;
            _dispatcher = Dispatcher.CurrentDispatcher.EnsureNotNull();
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when <see cref="IsExecuting"/> has changed.
        /// </summary>
        public event EventHandler IsExecutingChanged;

        #endregion

        #region INotifyPropertyChanged Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ICommand Events

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region ICommand Methods

        /// <summary>
        ///     Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to <c>null</c>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if this command can be executed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return !IsExecuting && _canExecute(parameter);
        }

        /// <summary>
        ///     Executes the logic of the command.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to <c>null</c>.
        /// </param>
        public abstract void Execute(object parameter);

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether the command is being executed.
        /// </summary>
        public bool IsExecuting
        {
            [DebuggerStepThrough]
            get
            {
                return _isExecuting;
            }

            private set
            {
                lock (_syncLock)
                {
                    if (_isExecuting == value)
                    {
                        return;
                    }

                    _isExecuting = value;
                }

                _dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            RaiseIsExecutingChanged();
                            OnPropertyChanged(nameof(IsExecuting));
                            RaiseCanExecuteChanged();
                        }));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Notifies the command that the external changes may have affected the ability of the
        ///     command to be executed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Notifies <see cref="INotifyPropertyChanged"/> subscribers that a property with the
        ///     specified name has changed its value.
        /// </summary>
        /// <param name="propertyName">
        ///     The name of the property that has changed.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///     Executes a wrapper for the command's logic.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter to pass to the command's logic.
        /// </param>
        /// <param name="executeAsynchronously">
        ///     A <see cref="bool"/> value that indicates whether the command's logic should be
        ///     executed asynchronously.
        /// </param>
        protected void ExecuteInternal(object parameter, bool executeAsynchronously)
        {
            lock (_syncLock)
            {
                _dispatcher.VerifyAccess();

                if (IsExecuting)
                {
                    return;
                }
            }

            if (executeAsynchronously)
            {
                lock (_syncLock)
                {
                    Factotum.DisposeAndNull(ref _cancellationTokenSource);
                    _cancellationTokenSource = new CancellationTokenSource();

                    var cancellationToken = _cancellationTokenSource.Token;
                    var task = new Task(() => _execute(parameter, cancellationToken), cancellationToken);

                    task.ContinueWith(
                        t =>
                            _dispatcher.Invoke(
                                new Action(() => CleanUpAfterExecution(t.Exception)),
                                DispatcherPriority.Send),
                        CancellationToken.None);

                    IsExecuting = true;
                    task.Start();
                }
            }
            else
            {
                Exception exception = null;

                IsExecuting = true;
                try
                {
                    _execute(parameter, CancellationToken.None);
                }
                catch (Exception ex)
                    when (!ex.IsFatal())
                {
                    exception = ex;
                }
                finally
                {
                    CleanUpAfterExecution(exception);
                }
            }
        }

        /// <summary>
        ///     Cancels the command if it is being executed.
        /// </summary>
        protected void CancelInternal()
        {
            lock (_syncLock)
            {
                if (IsExecuting)
                {
                    _cancellationTokenSource?.Cancel();
                }
            }
        }

        #endregion

        #region Private Methods

        private static bool CanExecuteByDefault(object arg)
        {
            return true;
        }

        private static void HandleExceptionByDefault(Exception exception)
        {
            Trace.TraceError($@"[{nameof(HandleExceptionByDefault)}] {exception}");
        }

        private void RaiseIsExecutingChanged()
        {
            IsExecutingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CleanUpAfterExecution([CanBeNull] Exception exception)
        {
            _dispatcher.VerifyAccess();

            try
            {
                if (exception != null && !exception.IsFatal())
                {
                    _handleException(exception);
                }
            }
            finally
            {
                lock (_syncLock)
                {
                    IsExecuting = false;
                    Factotum.DisposeAndNull(ref _cancellationTokenSource);
                }
            }
        }

        #endregion
    }
}