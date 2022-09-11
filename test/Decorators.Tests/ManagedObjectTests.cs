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

#if DEBUG

    [Theory]
    [ClassData(typeof(ResetArgumentTestData))]
    public void ResetShouldDisposeCurrentValue(bool hasNewValue, ManagedObject<IDisposable>? value)
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
        Mock<ReferenceDisposable> initialValue = new();
        ReferenceDisposable newValue = new();
        ManagedObject<ReferenceDisposable> managedObject = new(initialValue.Object);

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

    // Dispose Should Dispose Owned Value When Provided by constructor
    // Dispose Should Dispose Owned Reference Type with Default value provided by Reset
    // Dispose Should But Dispose Owned Value Type with Default value provided by Reset

#endif

}
