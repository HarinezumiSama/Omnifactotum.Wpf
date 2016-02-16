using System;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf.Commands
{
    /// <summary>
    ///     Represents the asynchronous implementation of <see cref="ICommand"/> that uses external methods
    ///     for its logic.
    /// </summary>
    public sealed class AsyncRelayCommand : RelayCommandBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <param name="canExecute">
        ///     A reference to a method that determines if the command can be executes, or <c>null</c> if the
        ///     command is always available for execution.
        /// </param>
        /// <param name="handleException">
        ///     A reference to a method that handles an exception occurred in the command's logic, if any.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public AsyncRelayCommand(
            [NotNull] Action<object, CancellationToken> execute,
            [CanBeNull] Func<object, bool> canExecute,
            [CanBeNull] Action<Exception> handleException)
            : base(execute, canExecute, handleException)
        {
            CancelCommand = new CancelAsyncOperationCommand(this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <param name="canExecute">
        ///     A reference to a method that determines if the command can be executes, or <c>null</c> if the
        ///     command is always available for execution.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public AsyncRelayCommand(
            [NotNull] Action<object, CancellationToken> execute,
            [CanBeNull] Func<object, bool> canExecute)
            : this(execute, canExecute, null)
        {
            // Nothing to do
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <param name="handleException">
        ///     A reference to a method that handles an exception occurred in the command's logic, if any.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public AsyncRelayCommand(
            [NotNull] Action<object, CancellationToken> execute,
            [CanBeNull] Action<Exception> handleException)
            : this(execute, null, handleException)
        {
            // Nothing to do
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public AsyncRelayCommand([NotNull] Action<object, CancellationToken> execute)
            : this(execute, null, null)
        {
            // Nothing to do
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets an <see cref="ICommand"/> that, when executed, cancels this
        ///     <see cref="AsyncRelayCommand"/> if the latter is being executed.
        /// </summary>
        [NotNull]
        public ICommand CancelCommand
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Executes the logic of the command.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can
        ///     be set to <c>null</c>.
        /// </param>
        public override void Execute(object parameter)
        {
            ExecuteInternal(parameter, true);
        }

        /// <summary>
        ///     Cancels the command if it is being executed.
        /// </summary>
        public void Cancel()
        {
            CancelInternal();
        }

        #endregion

        #region CancelAsyncOperationCommand Class

        private sealed class CancelAsyncOperationCommand : ICommand
        {
            private readonly AsyncRelayCommand _owner;

            internal CancelAsyncOperationCommand([NotNull] AsyncRelayCommand owner)
            {
                if (owner == null)
                {
                    throw new ArgumentNullException(nameof(owner));
                }

                _owner = owner;
                _owner.IsExecutingChanged += OnIsExecutingChanged;
            }

            public void Execute(object parameter)
            {
                _owner.Cancel();
            }

            public bool CanExecute(object parameter)
            {
                return _owner.IsExecuting;
            }

            public event EventHandler CanExecuteChanged;

            private void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            private void OnIsExecutingChanged(object sender, EventArgs eventArgs)
            {
                RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}