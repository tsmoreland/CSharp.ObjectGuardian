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

namespace TSMoreland.ObjectGuardians;

public readonly ref struct Expected<TValue, TEnum>
    where TEnum : System.Enum
{

#if NET7_0_OR_GREATER
    private readonly ref TValue _value;
    public required bool HasValue { get; init; }
    public ref TValue Value => ref _value;
#elif NET6_0_OR_GREATER
    public bool HasValue { get; init; }
    public TValue Value { get; init; }

#endif
    public readonly TEnum Reason { get; init; }

#if NET7_0_OR_GREATER
    public Expected()
    {
        HasValue = false;
        _value = default!;
        Reason = default!;
    }

    [SetsRequiredMembers]
    public Expected(TValue value)
    {
        _value = value;
        HasValue = true;
        Reason = default!;
    }

    [SetsRequiredMembers]
    public Expected(ref TValue value)
    {
        _value = ref value;
        HasValue = true;
        Reason = default!;
    }

    [SetsRequiredMembers]
    public Expected(TEnum reason)
    {
        _value = default!;
        Reason = reason;
        HasValue = false;
    }

    /// <summary>
    /// Determines if <paramref name="other"/> is equal to <see cref="Value"/> if <see cref="HasValue"/>
    /// is <see langword="true"/>
    /// </summary>
    /// <param name="other">other value to compare </param>
    /// <returns>
    /// <see langword="true"/> if <see cref="HasValue"/> is <see langword="true"/>
    /// and <see cref="Value"/> is equal to <paramref name="other"/>
    /// </returns>
    public readonly bool Equals(ref TValue other)
    {
        if (!HasValue)
        {
            return false;
        }

        return _value is not null
            ? _value.Equals(other)
            : other is null;
    }


#elif NET6_0_OR_GREATER
    public Expected()
    {
        HasValue = false;
        Value = default!;
        Reason = default!;
    }

    public Expected(TValue value)
    {
        Value = value;
        HasValue = true;
        Reason = default!;
    }

    public Expected(TEnum reason)
    {
        Value = default!;
        Reason = reason;
        HasValue = false;
    }

    /// <summary>
    /// Determines if <paramref name="other"/> is equal to <see cref="Value"/> if <see cref="HasValue"/>
    /// is <see langword="true"/>
    /// </summary>
    /// <param name="other">other value to compare </param>
    /// <returns>
    /// <see langword="true"/> if <see cref="HasValue"/> is <see langword="true"/>
    /// and <see cref="Value"/> is equal to <paramref name="other"/>
    /// </returns>
    public readonly bool Equals(TValue other)
    {
        if (!HasValue)
        {
            return false;
        }

        return Value is not null
            ? Value.Equals(other)
            : other is null;
    }

#endif

    public void IfHasValue(Action<TValue> consumer)
    {
        ArgumentNullException.ThrowIfNull(consumer);
        if (HasValue)
        {
            consumer(Value);
        }
    }

    public TValue OrElseGet(in TValue other)
    {
        return HasValue
            ? Value
            : other;
    }

    public TValue OrElseThrow(Func<Exception> supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);

        return HasValue
            ? Value
            : throw supplier();
    }

    public Expected<TMapped, TEnum> Select<TMapped>(Func<TValue, TMapped> mapper)
    {
        return HasValue
            ? new Expected<TMapped, TEnum>(mapper(Value))
            : new Expected<TMapped, TEnum>(Reason);
    }

}
