# Decorators

## CI Status

### Main

[![Continuous-Integration](https://github.com/tsmoreland/TSMoreland.TransferableOwnership/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/tsmoreland/TSMoreland.TransferableOwnership/actions/workflows/dotnet-ci.yml)

### Development

[![CI: Development Branch](https://github.com/tsmoreland/MaybeManaged/actions/workflows/dotnet-ci.yml/badge.svg?branch=development)](https://github.com/tsmoreland/MaybeManaged/actions/workflows/dotnet-ci.yml)

## Introduction

This class library provides generic collectors providing functionality similar to C++23 library classes such as [expected (C++23)](https://en.cppreference.com/w/cpp/utility/expected/expected) and [```std::unique_ptr<T>```](https://en.cppreference.com/w/cpp/memory/unique_ptr), with additional ones added if/when a need arises.
The objects also draw from ```java.util.Optional<T>``` providing additional methods to aid with functional programming.

### Expected

The ```Expected<TValue, TEnum>``` and ```ExpectedResult<T>``` serve as a form of nullabale (or optional) storoage class but with the addiontion of an ```enum``` or ```Exception``` respectively.  Then intent is to either provide the "expected" type or a reason why it's not present.

### ManagedObject

Providing similar purpose as ```std::unique_ptr<T>``` without the ability to enforce uniqueness.  The intent is to offer a type that implements ```IDisposable``` a way to be used in a using statement while also allow a way to transfer ownership making the Dispose call a no-op.
