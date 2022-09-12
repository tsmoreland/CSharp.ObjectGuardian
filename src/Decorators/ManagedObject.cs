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

using System.Diagnostics.CodeAnalysis;

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
        ArgumentNullException.ThrowIfNull(value);
    }

    private ManagedObject(bool hasValue, T? value)
    {
        HasValue = hasValue;
        Value = value;
    }

    /// <inheritdoc />
    public void Reset()
    {
        if (!HasValue)
        {
            return;
        }

        IDisposable? disposable = Value;
        (HasValue, Value) = (false, default);
        disposable?.Dispose();
    }

    /// <inheritdoc />
    public void Reset(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Reset();
        (HasValue, Value) = (true, value);
    }

    /// <inheritdoc />
    public T? Release()
    {
        T? value = Value;
        (HasValue, Value) = (false, default);
        return value;
    }

    /// <inheritdoc />
    public T? Value { get; private set; }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; private set; }

    /// <inheritdoc />
    public T OrElse(T other)
    {
        return HasValue
            ? Value
            : other;
    }

    /// <inheritdoc />
    public T OrElse(Func<T> supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        return HasValue
            ? Value
            : supplier();
    }

    /// <inheritdoc />
    public T OrThrow()
    {
        return HasValue
            ? Value
            : throw new InvalidOperationException("Does not contain value");
    }

    /// <inheritdoc />
    public T OrThrow(Func<Exception> supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        return HasValue
            ? Value
            : throw supplier();
    }

    ~ManagedObject() => Dispose(false);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        _ = disposing;
        Reset();
    }
}
