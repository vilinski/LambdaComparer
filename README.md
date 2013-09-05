LambdaComparer
==============

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

