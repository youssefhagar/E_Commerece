using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerece.Application.Common
{
    //public class Result
    //{

    //    public bool IsSuccess { get; }
    //    public IReadOnlyList<Error> Errors { get; }

    //    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    //    {
    //        IsSuccess = isSuccess;
    //        Errors = errors;
    //    }
    //    public static Result Success()=> new (true, new List<Error>());

    //    public static Result Fail(Error error)=> new (false, new[] {error});
    //    public static Result Fail(IReadOnlyList<Error> errors)=> new (false, errors);



    //}

    //public class Result<T> : Result
    //{
    //    private IReadOnlyList<Error> errors;

    //    private T _value { get; }
    //    public T Data => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");
    //    protected Result( T value) : base(true, Array.Empty<Error>())
    //    {
    //        _value = value;
    //    }
    //    private Result(Error error) : base(false, new[] { error })
    //    {
    //        _value = default!;
    //    }

    //    public Result(IReadOnlyList<Error> errors) : base(false, Array.Empty<Error>())
    //    {
    //        this.errors = errors;
    //    }

    //    public static Result<T> Ok(T value) => new Result<T>(value);
    //    public static new Result<T> Fail(Error error) => new Result<T>(error);
    //    public static new Result<T> Fail(IReadOnlyList<Error> errors) => new Result<T>(errors);
    //}

}
