namespace TSMoreland.TransferableOwnership.MaybeMovables;

/// <summary>
/// Container class storing at most 1 item, and is responsible for the disposal of that item if present
/// </summary>
/// <typeparam name="T">The type of the stored value</typeparam>
public interface IMaybeOwned<T> : IDisposable
    where T : IDisposable
{
    /// <summary>
    /// Returns <see langword="true"/> if this instance was constructed with a value
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// The contained value <see cref="HasValue"/> is true; otherwise default of <typeparamref name="T"/>
    /// which will be <see langword="null"/> if <typeparamref name="T"/> is a reference type.
    /// </summary>
    T? Value { get; }

    /// <summary>
    /// Returns <see cref="Value"/> if <see cref="HasValue"/> is <see langword="true"/> or
    /// <paramref name="other"/>
    /// </summary>
    /// <param name="other">alternative value used if <see cref="HasValue"/> is <see langword="false"/></param>
    /// <returns></returns>
    T OrElse(T other);

    /// <summary>
    /// Returns <see cref="Value"/> if <see cref="HasValue"/> is <see langword="true"/> or
    /// value supplied by <paramref name="supplier"/>
    /// </summary>
    /// <param name="supplier">
    /// used to create an instance of <typeparamref name="T"/> if <see cref="HasValue"/> is <see langword="false"/>
    /// </param>
    /// <returns></returns>
    T OrElse(Func<T> supplier);

    /// <summary>
    /// Returns <see cref="Value"/> if <see cref="HasValue"/> is <see langword="true"/> or
    /// throw <see cref="InvalidOperationException"/>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// if <see cref="HasValue"/> is <see langword="false"/>
    /// </exception>
    T OrThrow();

    /// <summary>
    /// Returns <see cref="Value"/> if <see cref="HasValue"/> is <see langword="true"/> or
    /// throw exception supplied by <paramref name="supplier"/>
    /// </summary>
    /// <param name="supplier">
    /// function used to build exception thrown if value is not present
    /// </param>
    /// <returns>
    /// Contained <typeparamref name="T"/> if present
    /// </returns>
    T OrThrow(Func<Exception> supplier);

    /// <summary>
    /// If a value is present, apply the
    /// provided Maybe-bearing mapping function to it,
    /// return that result, otherwise return
    /// an empty <see cref="IMaybeOwned{T}"/>.
    /// </summary>
    /// <typeparam name="TMapped"></typeparam>
    /// <param name="mapper">
    /// mapper used to map <typeparamref name="T"/> to <typeparamref name="TMapped"/>; mapped function
    /// should not dispose the owned type as that remains the responsibility of this instance
    /// </param>
    /// <returns>
    /// <see cref="IMaybeOwned{TMapped}"/> containing mapped result if present for this instance;
    /// otherwise an empty <see cref="IMaybeOwned{TMapped}"/>
    /// </returns>
    IMaybeOwned<TMapped> Map<TMapped>(Func<T, TMapped> mapper) where TMapped : IDisposable;
}