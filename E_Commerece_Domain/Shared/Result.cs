using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, IReadOnlyList<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success()
            => new(true, Array.Empty<Error>());

        public static Result Fail(Error error)
            => new(false, new[] { error });

        public static Result Fail(IReadOnlyList<Error> errors)
            => new(false, errors);
    }


    public class Result<T> : Result
    {
        private readonly T _value;

        public T Data =>
            IsSuccess
                ? _value
                : throw new InvalidOperationException(
                    "Cannot access the value of a failed result.");

        private Result(T value)
            : base(true, Array.Empty<Error>())
        {
            _value = value;
        }

        private Result(Error error)
            : base(false, new[] { error })
        {
            _value = default!;
        }

        private Result(IReadOnlyList<Error> errors)
            : base(false, errors)
        {
            _value = default!;
        }

        public static Result<T> Ok(T value)
            => new(value);

        public static new Result<T> Fail(Error error)
            => new(error);

        public static new Result<T> Fail(
            IReadOnlyList<Error> errors)
            => new(errors);
    }
}
