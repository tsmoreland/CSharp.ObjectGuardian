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

namespace TSMoreland.MaybeManaged;

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
    T? Value { get; }

    /// <summary>
    /// Returns <see langword="true"/> if <see cref="Value"/> is currently owned.
    /// </summary>
    bool HasValue { get; }


}
