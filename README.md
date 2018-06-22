LambdaComparer
==============

[![Build status](https://ci.appveyor.com/api/projects/status/2eun1831f3odmf85/branch/master?svg=true)](https://ci.appveyor.com/project/vilinski/lambdacomparer/branch/master)


LambdaComparer class is a flexible `IComparer<T>` implementation, which compares the values returned from the specified selector.

````csharp
var equalityComparer = new LambdaComparer<string,int>(x=>x.Length);
var dict = new Dictionary<string, string>(new LambdaComparer<string, int>(x => x.Length));
// can contain just one string of each length
````

There are also some extension methods additionall to System.Linq.Enumerable methods, using this comparer.

````csharp
var list = new[] { "a", "aa", "aaa", "b", "bb", "bbb" };
var distinct = list.Distinct(x => x.Length); // returns { "a", "aa", "aaa" }
````
````csharp
var list1 = new[] { "a", "aa", "aaa", "aaaa" };
var list2 = new[] { "dd", "eeee" };
var except = list1.Except(list2, x => x.Length); // returns { "a", "aaa" }
````
````csharp
var list1 = new[] { "a", "aa", "aaa", "aaaa" };
var list2 = new[] { "dd", "eeee" };
var intersect = list1.Intersect(list2, x => x.Length); // returns { "aa", "aaaa" }
````

Build
=====

Build the project using latest dotnet CLI (Currently 2.1.300)

````bash
dotnet build
dotnet test LambdaComparer.Test/
dotnet pack
dotnet nuget push -k mySecretNugetKey LambdaComparer.0.2.0.nupkg  -s "https://api.nuget.org/v3/index.json"
````
