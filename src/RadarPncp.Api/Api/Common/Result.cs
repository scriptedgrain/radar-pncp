namespace RadarPncp.Api.Common;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Resultado de uma operação que pode falhar de forma esperada
/// </summary>
public sealed class Result<T>
{
    /// <summary>
    /// Valor produzido, quando a operação deu certo
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Motivo da falha, quando a operação não deu certo
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Indica se a operação deu certo
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Construtor com valor e erro esperados
    /// </summary>
    private Result(T? value, string? error)
    {
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Em caso de sucesso
    /// </summary>
    public static Result<T> Success(T value) => new(value, null);

    /// <summary>
    /// Em caso de falha
    /// </summary>
    public static Result<T> Failure(string error) => new(default, error);
}