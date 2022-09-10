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
/// Factory methods for <see cref="MaybeOwned{T}"/>
/// </summary>
public static class MaybeOwned
{
    /// <summary>
    /// Creates an instance of <see cref="MaybeOwned{T}"/> containing <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static MaybeOwned<T> Of<T>(T value) where T : IDisposable
    {
        return new MaybeOwned<T>(true, value);
    }

    /// <summary>
    /// Creates an empty instance of <see cref="MaybeOwned{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>an empty instance of <see cref="MaybeOwned{T}"/></returns>
    public static MaybeOwned<T> Empty<T>() where T : IDisposable =>
        new();
}

public struct MaybeOwned<T> : IMaybeOwned<T>
    where T : IDisposable
{
    public MaybeOwned()
        :this(false, default)
    {
    }

    internal MaybeOwned(bool hasValue, T? value)
    {
        HasValue = hasValue;
        Value = value;
    }

    /// <inheritdoc />
    public bool HasValue { get; private set; }

    /// <inheritdoc />
    public T? Value { get; private set; }

    /// <inheritdoc />
    public readonly T OrElse(T other) => throw new NotImplementedException();

    /// <inheritdoc />
    public readonly T OrElse(Func<T> supplier) => throw new NotImplementedException();

    /// <inheritdoc />
    public T OrThrow() => throw new NotImplementedException();

    /// <inheritdoc />
    public T OrThrow(Func<Exception> supplier) => throw new NotImplementedException();

    /// <inheritdoc />
    public IMaybeOwned<TMapped> Map<TMapped>(Func<T, TMapped> mapper) where TMapped : IDisposable => throw new NotImplementedException();

    /// <inheritdoc />
    public void Dispose() => throw new NotImplementedException();
}
