using EVA.Auditing.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace EVA.Auditing
{
    public class Result
    {
        public bool IsSuccessful { get; set; }
        public string Message => Errors.FirstOrDefault();
        public List<string> Errors { get; set; } = new List<string>();

        public Result Success() => this.With(x => x.IsSuccessful = true);
        public Result Error(string message) => this.With(x => x.IsSuccessful = false, x => x.Errors.Add(message));

        public Result(bool isSuccessful = false) => IsSuccessful = isSuccessful;
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result<T> Success(T data) => this.With(x => x.IsSuccessful = true, x => x.Data = data);
        public new Result<T> Success() => this.With(x => x.IsSuccessful = true);
        public new Result<T> Error(string message) => this.With(x => x.IsSuccessful = false, x => x.Errors.Add(message));
    }
}