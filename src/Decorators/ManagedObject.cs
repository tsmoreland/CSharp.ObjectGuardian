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
/// Manages a <typeparamref name="T"/> if this instance is the current owner making it responsible
/// for the disposal of the owned object
/// </summary>
/// <typeparam name="T">
/// <see cref="IDisposable"/> object which will be disposed when this instance is, unless ownership is transfer
/// </typeparam>
public sealed class ManagedObject<T> : IManagedObject<T>
    where T : IDisposable
{
    /// <summary>
    /// Instanties a new instance of <see cref="ManagedObject{T}"/> that does not
    /// own an object of type <typeparamref name="T"/>
    /// </summary>
    public ManagedObject()
        : this(false, default)
    {
    }

    /// <summary>
    /// Instanties a new instance of <see cref="ManagedObject{T}"/> that owns
    /// <paramref name="value"/>
    /// </summary>
    /// <param name="value">the object to be managed by this instance.</param>
    public ManagedObject(T value)
        : this(true, value)
    {
    }

    private ManagedObject(bool hasValue, T? value)
    {
        HasValue =  hasValue;
        Value = value;
    }

    /// <inheritdoc />
    public void Reset() => throw new NotImplementedException();

    /// <inheritdoc />
    public void Reset(T? value) => throw new NotImplementedException();

    /// <inheritdoc />
    public T? Release() => throw new NotImplementedException();

    /// <inheritdoc />
    public T? Value { get; private set; }

    /// <inheritdoc />
    public bool HasValue { get; private set; }

    /// <inheritdoc />
    public T OrElse(T other) => throw new NotImplementedException();

    /// <inheritdoc />
    public T OrElse(Func<T> supplier) => throw new NotImplementedException();

    /// <inheritdoc />
    public T OrThrow() => throw new NotImplementedException();

    /// <inheritdoc />
    public T OrThrow(Func<Exception> supplier) => throw new NotImplementedException();

    /// <inheritdoc />
    public IManagedObject<TMapped> Map<TMapped>(Func<T, TMapped> mapper) where TMapped : IDisposable => throw new NotImplementedException();

    /// <inheritdoc />
    public void Dispose() => throw new NotImplementedException();
}
