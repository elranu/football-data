using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballData.Core
{
    public interface IOperationResult { }

    public interface IOperationResult<T> { }

    public interface ISucceededOperationResult : IOperationResult { }

    public interface ISucceededOperationResult<T> : IOperationResult, IOperationResult<T>
    {
        T Value { get; }
    }

    public interface IFailedOperationResult : IOperationResult
    {
        IReadOnlyList<OperationError> GetErrors();
        void AddError(OperationError error);
    }

    public enum OperationResultType
    {
        Successfull,
        AlreadyDone,
        NotFound
    }

    public interface IFailedOperationResult<T> : IFailedOperationResult, IOperationResult<T> { }

    public static class OperationResult
    {
        public static ISucceededOperationResult Succeeded()
            => new SucceededOperationResult();

        public static ISucceededOperationResult<T> Succeeded<T>(T value)
            => new SucceededOperationResult<T>(value);

        public static IFailedOperationResult Failed()
            => new FailedOperationResult();

        public static IFailedOperationResult Failed(string code, string description = null)
            => new FailedOperationResult(new[] { new OperationError(code, description) });

        public static IFailedOperationResult Failed(IEnumerable<OperationError> errors)
            => new FailedOperationResult(errors);

        public static IFailedOperationResult Failed(params OperationError[] errors)
            => new FailedOperationResult(errors);
    }

    public static class OperationResult<T>
    {
        public static IFailedOperationResult<T> Failed()
            => new FailedOperationResult<T>();

        public static IFailedOperationResult<T> Failed(string code, string description = null)
            => new FailedOperationResult<T>(new[] { new OperationError(code, description) });

        public static IFailedOperationResult<T> Failed(IEnumerable<OperationError> errors)
            => new FailedOperationResult<T>(errors);

        public static IFailedOperationResult<T> Failed(params OperationError[] errors)
            => new FailedOperationResult<T>(errors);
    }

    public class SucceededOperationResult : ISucceededOperationResult { }

    public class SucceededOperationResult<T> : ISucceededOperationResult<T>
    {
        public SucceededOperationResult(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    public class FailedOperationResult : IFailedOperationResult
    {
        private static readonly List<OperationError> _emptyErrorList = new List<OperationError>();

        private List<OperationError> _errors;

        public FailedOperationResult() { }

        public FailedOperationResult(IEnumerable<OperationError> errors)
        {
            _errors = errors.ToList();
        }

        public IReadOnlyList<OperationError> GetErrors() => _errors ?? _emptyErrorList;

        public void AddError(OperationError error)
            => (_errors ?? (_errors = new List<OperationError>())).Add(error);

        public void AddError(string code, string description = null)
            => AddError(new OperationError(code, description));
    }

    public class FailedOperationResult<T> : FailedOperationResult, IFailedOperationResult<T>
    {
        public FailedOperationResult() { }
        public FailedOperationResult(IEnumerable<OperationError> errors) : base(errors) { }
    }

    public static class OperationResultExtensions
    {
        public static T GetSucceededValueOrDefault<T>(this IOperationResult<T> operationResult)
        {
            return (operationResult is ISucceededOperationResult<T> result) ? result.Value : default;
        }

        public static T GetSucceededValue<T>(this IOperationResult<T> operationResult)
        {
            var result = operationResult as ISucceededOperationResult<T>;
            if (result == null)
            {
                var failed = (IFailedOperationResult)operationResult;
                throw new Exception("An error ocurred on the operation, check the following errors: " + string.Join(" - ", failed.GetErrors().Select(x => x.Description)));
            }

            return result.Value;
        }
    }
}
