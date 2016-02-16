using System;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Omnifactotum.Annotations;

namespace Omnifactotum.Wpf.Commands
{
    /// <summary>
    ///     Represents the synchronous implementation of <see cref="ICommand"/> that uses external
    ///     methods for its logic.
    /// </summary>
    public sealed class RelayCommand : RelayCommandBase
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand"/> class.
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
        public RelayCommand(
            [NotNull] Action<object> execute,
            [CanBeNull] Func<object, bool> canExecute,
            [CanBeNull] Action<Exception> handleException)
            : base(ConvertExecuteDelegate(execute), canExecute, handleException)
        {
            // Nothing to do
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <param name="canExecute">
        ///     A reference to a method that determines if the command can be executes, or
        ///     <c>null</c> if the command is always available for execution.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public RelayCommand(
            [NotNull] Action<object> execute,
            [CanBeNull] Func<object, bool> canExecute)
            : this(execute, canExecute, null)
        {
            // Nothing to do
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand"/> class.
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
        public RelayCommand(
            [NotNull] Action<object> execute,
            [CanBeNull] Action<Exception> handleException)
            : this(execute, null, handleException)
        {
            // Nothing to do
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">
        ///     A reference to a method that executes the command's logic.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="execute"/> is <c>null</c>.
        /// </exception>
        public RelayCommand([NotNull] Action<object> execute)
            : this(execute, null, null)
        {
            // Nothing to do
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Executes the logic of the command.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this
        ///     object can be set to <c>null</c>.
        /// </param>
        public override void Execute(object parameter)
        {
            ExecuteInternal(parameter, false);
        }

        #endregion

        #region Private Methods

        private static Action<object, CancellationToken> ConvertExecuteDelegate(Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            return (parameter, token) => execute(parameter);
        }

        #endregion
    }
}