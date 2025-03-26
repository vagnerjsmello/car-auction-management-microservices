namespace CAMS.Common.ResponseApi;

/// <summary>
/// Represents the result of an operation.
/// It shows if the operation was successful, holds the result data,
/// and lists any error messages.
/// </summary>
/// <typeparam name="T">The type of data returned when the operation is successful.</typeparam>

public class ResponseResult<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public IEnumerable<string> Errors { get; }

    private ResponseResult(bool isSuccess, T data, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Errors = errors;
    }

    public static ResponseResult<T> Success(T data) => new ResponseResult<T>(true, data, Enumerable.Empty<string>());
    public static ResponseResult<T> Fail(FluentValidation.Results.ValidationResult validationResult)
        => new ResponseResult<T>(false, default!, validationResult.Errors.Select(e => e.ErrorMessage));
}

