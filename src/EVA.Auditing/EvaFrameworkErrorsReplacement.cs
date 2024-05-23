using System;
using System.Runtime.CompilerServices;

namespace EVA.Framework.Errors;

public class SortingErrors
{
    public static readonly Error<(string SortingProperty, string ObjectName)> SortingPropertyDoesNotExistInObject = ("Sorting", "SortingPropertyDoesNotExistInObject", "Sortingproperty {0} does not exist in object {1}");
}

public interface IError
{
    string Type { get; }
    string Message { get; }
    string Prefix { get; }
    string Identifier { get; }
}

public class Error<TArg> : IError, IEquatable<IError>
{
    public string Type { get; }
    public string Message { get; }
    public string Prefix { get; }
    public string Identifier => Prefix + ":" + Type;

    public Error(string prefix, string type, string message)
    {
        Type = type;
        Message = message;
        Prefix = prefix;
    }

    private static object[] ProcessArgs(TArg arg)
    {
        if (arg is ITuple tuple)
        {
            var args = new object[tuple.Length];

            for (var i = 0; i < tuple.Length; i++)
            {
                args[i] = tuple[i];
            }

            return args;
        }

        return new object[] { arg };
    }

    public static implicit operator Error<TArg>((string prefix, string type, string message) tuple) => new Error<TArg>(tuple.prefix, tuple.type, tuple.message);

    public static bool operator ==(Error<TArg> first, IError second) => Equals(first, second);

    public static bool operator !=(Error<TArg> first, IError second) => !Equals(first, second);

    public string ToString(TArg args) => Message.TryFormatWith(ProcessArgs(args));

    public static bool Equals(IError first, IError second)
    {
        var firstIsNull = ReferenceEquals(null, first);
        var secondIsNull = ReferenceEquals(null, second);
        if (firstIsNull && secondIsNull) return true;
        if (firstIsNull || secondIsNull) return false;
        return first.Type == second.Type && first.Prefix == second.Prefix;
    }

    public override bool Equals(object obj) => Equals(this, obj as IError);

    public bool Equals(IError other) => Equals(this, other);

    public Exception ToException(params object[] args) => new Exception(Message);
}