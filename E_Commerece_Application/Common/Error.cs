using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Common
{
    //    public record Error(
    //        string Code,
    //        string Description,
    //        ErrorType ErrorType)
    //    {
    //        public static readonly Error None =
    //            new(string.Empty, string.Empty, ErrorType.None);

    //        public static Error Failure(
    //            string code = "General.Failure",
    //            string description = "A general failure has occurred.")
    //            => new(code, description, ErrorType.Failure);

    //        public static Error Validation(
    //            string code = "General.Validation",
    //            string description = "A validation error has occurred.")
    //            => new(code, description, ErrorType.Validation);

    //        public static Error NotFound(
    //            string code = "General.NotFound",
    //            string description = "The requested resource was not found.")
    //            => new(code, description, ErrorType.NotFound);

    //        public static Error Conflict(
    //            string code = "General.Conflict",
    //            string description = "A conflict occurred with the current state of the resource.")
    //            => new(code, description, ErrorType.Conflict);

    //        public static Error Unauthorized(
    //            string code = "General.Unauthorized",
    //            string description = "Access is denied due to lack of authentication.")
    //            => new(code, description, ErrorType.Unauthorized);

    //        public static Error Forbidden(
    //            string code = "General.Forbidden",
    //            string description = "You do not have permission to perform this action.")
    //            => new(code, description, ErrorType.Forbidden);
    //    }
    //    public enum ErrorType
    //    {
    //        None = 0,

    //        Validation = 1,

    //        Failure = 2,

    //        NotFound = 3,

    //        Conflict = 4,

    //        Unauthorized = 5,

    //        Forbidden = 6
    //    }

}