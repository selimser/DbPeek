using System;
using System.Diagnostics;

namespace DbPeek.Models
{
    internal class ErrorData
    {
        public string ErrorMessage => $"An error has occurred.\n{(int)ErrorCode} - {Enum.GetName(typeof(ErrorCodes), ErrorCode)}";
        public ErrorCodes ErrorCode { get; set; }
        public Exception ExceptionData { internal get; set; }

        [DebuggerStepThrough]
        public override string ToString()
        {
            var builtString = $"ERROR!!! {(int)ErrorCode} | {ExceptionData.Message} | {ExceptionData.StackTrace}";
            Debug.WriteLine(builtString);
            return builtString;
        }
    }
}
