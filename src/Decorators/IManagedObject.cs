//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace TSMoreland.Extensions.Decorators;

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
    /// disposes of the owned <see cref="Value"/> if <see cref="HasValue"/> is <see langword="true"/>.
    /// <see cref="HasValue"/> will be <see langword="false"/> after this is called.
    /// </summary>
    void Reset();

    /// <summary>
    /// replaces the managed object with <paramref name="value"/>
    /// ensure the original value is disposed if <see cref="HasValue"/> is <see langword="true"/>
    /// </summary>
    /// <param name="value">value to be managed</param>
    void Reset(T? value);

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
    T? Value { get; }

    /// <summary>
    /// Returns <see langword="true"/> if <see cref="Value"/> is currently owned.
    /// </summary>
    bool HasValue { get; }

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
    /// If a <see cref="HasValue"/> is <see langword="true"/>, apply the
    /// provided <see cref="ManagedObject{TMapped}"/>-bearing mapping function to it,
    /// return that result, otherwise return
    /// an empty <see cref="IManagedObject{TMapped}"/>.
    /// </summary>
    /// <typeparam name="TMapped"></typeparam>
    /// <param name="mapper">
    /// mapper used to map <typeparamref name="T"/> to <typeparamref name="TMapped"/>; mapped function
    /// should not dispose the owned type as that remains the responsibility of this instance
    /// </param>
    /// <returns>
    /// <see cref="IManagedObject{TMapped}"/> containing mapped result if present for this instance;
    /// otherwise an empty <see cref="IMaybeManagedObject{TMapped}"/>
    /// </returns>
    IManagedObject<TMapped> Map<TMapped>(Func<T, TMapped> mapper) where TMapped : IDisposable;
}
