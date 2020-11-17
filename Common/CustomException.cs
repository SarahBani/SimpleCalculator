using System;

namespace Common
{
    public enum ExceptionKey
    {
        NotDefined = -1,

        EmptyArray,
        DividedByZero,
    }

    public class CustomException : Exception
    {

        #region Properties

        public string CustomMessage { get; private set; }

        #endregion /Properties

        #region Constructors

        public CustomException(Exception exception)
        {
            var baseException = exception.GetBaseException();
            this.CustomMessage = GetMessage(baseException);
        }

        public CustomException(ExceptionKey exceptionKey, params object[] args) =>
            this.CustomMessage = string.Format(GetMessage(exceptionKey), args);

        public CustomException(string message) => this.CustomMessage = message;

        #endregion /Constructors

        #region Methods

        private string GetMessage(Exception baseException) =>
            baseException switch
            {
                DivideByZeroException _ => Constant.Exception_DividedByZero,
                OverflowException _ => Constant.Exception_OverFlow,
                _ => Constant.Exception_HasError,
            };

        private string GetMessage(ExceptionKey exceptionKey) =>
          exceptionKey switch
          {
              ExceptionKey.EmptyArray => Constant.Exception_EmptyArray,
              ExceptionKey.DividedByZero => Constant.Exception_DividedByZero,
              ExceptionKey.NotDefined => Constant.Exception_HasError,
              _ => Constant.Exception_HasError,
          };

        #endregion /Methods

    }
}