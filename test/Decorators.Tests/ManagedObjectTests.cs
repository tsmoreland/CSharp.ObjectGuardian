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

namespace TSMoreland.Extensions.Decorators.Tests;

public abstract class ManagedObjectTests<T> where T : IDisposable
{
    /// <summary>
    /// Build an instance of <typeparamref name="T"/> when
    /// </summary>
    protected abstract T Build();

    [Fact]
    public void HasValueShouldReturnFalseWhenBuiltUsingDefaultConstructor()
    {
        ManagedObject<T> managedObject = new();
        Assert.False(managedObject.HasValue);
    }

    [Fact]
    public void HasValueShouldReturnTrueWhenBuiltWithNonDefaultValue()
    {
        ManagedObject<T> managedObject = new(Build());
        Assert.True(managedObject.HasValue);
    }

    [Fact]
    public void HasValueShouldReturnTrueWhenBuiltWithDefaultValue()
    {
        ManagedObject<T> managedObject = new(default!);
        Assert.True(managedObject.HasValue);
    }

    [Fact]
    public void ValueShouldReturnDefaultWhenHasValueIsFalse()
    {
        T? expected = default;
        ManagedObject<T> managedObject = new();
        T? actual = managedObject.Value;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValueShouldReturnGivenWhenHasValueIsTrue()
    {
        T? expected = Build();
        ManagedObject<T> managedObject = new(expected);
        T? actual = managedObject.Value;
        Assert.Equal(expected, actual);
    }
}
