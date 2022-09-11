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

using TSMoreland.Extensions.Decorators.Tests.TestData;

namespace TSMoreland.Extensions.Decorators.Tests;

public sealed class ManagedObjectTests
{
    [Fact]
    public void HasValueShouldReturnFalseWhenBuiltUsingDefaultConstructor()
    {
        ManagedObject<IDisposable> managedObject = new();
        Assert.False(managedObject.HasValue);
    }

    [Fact]
    public void HasValueShouldReturnTrueWhenBuiltWithNonDefaultValue()
    {
        Mock<IDisposable> value = new();
        ManagedObject<IDisposable> managedObject = new(value.Object);
        Assert.True(managedObject.HasValue);
    }

    [Fact]
    public void HasValueShouldReturnTrueWhenBuiltWithDefaultValue()
    {
        ManagedObject<IDisposable> managedObject = new(default!);
        Assert.True(managedObject.HasValue);
    }

    [Fact]
    public void ValueShouldReturnDefaultWhenHasValueIsFalse()
    {
        IDisposable? expected = default;
        ManagedObject<IDisposable> managedObject = new();
        IDisposable? actual = managedObject.Value;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValueShouldReturnGivenWhenHasValueIsTrue()
    {
        Mock<IDisposable> value = new();
        IDisposable expected = value.Object;
        ManagedObject<IDisposable> managedObject = new(expected);
        IDisposable? actual = managedObject.Value;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(ResetArgumentTestData))]
    public void ResetShouldDisposeCurrentValue(bool hasNewValue, IDisposable? value)
    {
        Mock<IDisposable> initialValue = new();
        ManagedObject<IDisposable> managedObject = new(initialValue.Object);

        if (hasNewValue)
        {
            managedObject.Reset(value);
        }
        else
        {
            managedObject.Reset();
        }

        initialValue.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public void ResetShouldTakeOwnershipWhenGivenNewReferenceTypeValue()
    {
        ReferenceDisposable initialValue = new();
        ReferenceDisposable newValue = new();
        ManagedObject<ReferenceDisposable> managedObject = new(initialValue);

        managedObject.Reset(newValue);

        Assert.True(managedObject.HasValue);
        Assert.Equal(newValue, managedObject.Value);
    }

    [Fact]
    public void ResetShouldTakeOwnershipWhenGivenNewValueTypeValue()
    {
        ValueDisposable initialValue = new();
        ValueDisposable newValue = new();
        ManagedObject<ValueDisposable> managedObject = new(initialValue);

        managedObject.Reset(newValue);

        Assert.True(managedObject.HasValue);
        Assert.Equal(newValue, managedObject.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ReleaseShouldNotDisposeCurrentValueIfHasValue(bool hasValue)
    {
        Mock<IDisposable> disposable = new();
        ManagedObject<IDisposable> managedObject = hasValue
            ? new ManagedObject<IDisposable>(disposable.Object)
            : new ManagedObject<IDisposable>();

        _ = managedObject.Release();

        disposable.Verify(m => m.Dispose(), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ReleaseShouldValueOrNull(bool hasValue)
    {
        Mock<IDisposable> expected = new();
        ManagedObject<IDisposable> managedObject = hasValue
            ? new ManagedObject<IDisposable>(expected.Object)
            : new ManagedObject<IDisposable>();

        IDisposable? actual = managedObject.Release();

        if (hasValue)
        {
            Assert.Equal(expected.Object, actual);
        }
        else
        {
            Assert.Null(actual);
        }
    }

#if DEBUG
    [Fact]
    public void OrElseShouldReturnValueWhenHasValueIsTrue()
    {
        Mock<IDisposable> expected = new();
        Mock<IDisposable> other = new();
        ManagedObject<IDisposable> managedObject = new(expected.Object);

        IDisposable actual = managedObject.OrElse(other.Object);

        Assert.Equal(expected.Object, actual);
    }

    [Fact]
    public void OrElseShouldReturnElseValueWhenHasValueIsFalse()
    {
        Mock<IDisposable> expected = new();
        ManagedObject<IDisposable> managedObject = new();

        IDisposable actual = managedObject.OrElse(expected.Object);

        Assert.Equal(expected.Object, actual);
    }

    [Fact]
    public void OrThrowShouldReturnValueWhenHasValueIsTrue()
    {
        Mock<IDisposable> expected = new();
        ManagedObject<IDisposable> managedObject = new(expected.Object);

        IDisposable actual = managedObject.OrThrow();

        Assert.Equal(expected.Object, actual);
    }

    [Fact]
    public void OrThrowShouldThrowInvalidOperationExceptionWhenHasValueIsFalse()
    {
        ManagedObject<IDisposable> managedObject = new();
        Assert.Throws<InvalidOperationException>(() => _ = managedObject.OrThrow());
    }

    [Fact]
    public void OrThrowWithSupplierShouldReturnValueWhenHasValueIsTrue()
    {
        Exception ex = new();
        Mock<IDisposable> expected = new();
        ManagedObject<IDisposable> managedObject = new(expected.Object);

        IDisposable actual = managedObject.OrThrow(() => ex);

        Assert.Equal(expected.Object, actual);
    }

    [Fact]
    public void OrThrowWithSupplierShouldThrowWhenHasValueIsFalse()
    {
        Exception expected = new();
        ManagedObject<IDisposable> managedObject = new();

        Exception? actual = Assert.Throws<Exception>(() => _ = managedObject.OrThrow(() => expected));
        Assert.Equal(expected, actual);
    }

#endif

    [Fact]
    public void DispooseShouldDisposeValueWhenProvidedByConstructor()
    {
        Mock<IDisposable> value = new();
        ManagedObject<IDisposable> managedObject = new(value.Object);

        managedObject.Dispose();

        value.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public void DiposeShouldDisposeValueProvidedByReset()
    {
        Mock<IDisposable> value = new();
        Mock<IDisposable> resetValue = new();
        ManagedObject<IDisposable> managedObject = new(value.Object);
        managedObject.Reset(resetValue.Object);

        managedObject.Dispose();

        resetValue.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public void DisposeShouldDisposeValueProvidedByConstructorWhenReset()
    {
        Mock<IDisposable> value = new();
        Mock<IDisposable> resetValue = new();
        ManagedObject<IDisposable> managedObject = new(value.Object);
        managedObject.Reset(resetValue.Object);

        managedObject.Dispose();

        value.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public void DisposeShouldNotDisposeValueWhenReleased()
    {
        Mock<IDisposable> value = new();
        ManagedObject<IDisposable> managedObject = new(value.Object);
        _ = managedObject.Release();

        managedObject.Dispose();

        value.Verify(m => m.Dispose(), Times.Never);
    }

}
