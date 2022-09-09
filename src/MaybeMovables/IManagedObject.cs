namespace TSMoreland.TransferableOwnership.MaybeMovables;

/// <summary>
/// Manages an instance of <see cref="IDisposable"/> ensuring that it is
/// disposed unless that instance is released to be handled elsewhere.
/// </summary>
/// <typeparam name="T">the disposable instance which may be disposed unless moved</typeparam>
/// <remarks>
/// instended to store dispoable objects to ensure their proper release in the event of error or
/// be transfered to another owner if all steps in a process are successful.
///
/// Influenced by the likes of <code>std::unique_ptr&lt;T&gt;</code>
/// </remarks>
public interface IManagedObject<T> : IDisposable
    where T : IDisposable
{

    /// <summary>
    /// replaces the managed object with <paramref name="value"/>
    /// ensure the original value is disposed if <see cref="HasValue"/> is <see langword="true"/>
    /// </summary>
    /// <param name="value">value to be managed</param>
    void Reset(T? value = default);

    /// <summary>
    /// returns <see cref="Value"/> and releases the ownership
    /// </summary>
    /// <returns><see cref="Value"/></returns>
    T? Release();

    /// <summary>
    /// The managed object;
    /// </summary>
    /// <returns>
    /// the managed object if owned; otherwise default value of <typeparamref name="T"/>
    /// which will be <see langword="null"/> for reference types
    /// </returns>
    T? Value();

    /// <summary>
    /// Returns <see langword="true"/> if <see cref="Value"/> is currently owned.
    /// </summary>
    bool HasValue { get; }


}