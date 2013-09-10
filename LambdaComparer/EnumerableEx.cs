using System;
using System.Collections.Generic;
using System.Linq;

using FP.Properties;

namespace FP
{
	/// <summary>
	///     Static class with extension methods you might miss in <see cref="System.Linq.Enumerable" />.
	/// </summary>
	public static class EnumerableEx
	{
		/// <summary>
		///     Default part size for use in <see cref="Partition{T}" />.
		/// </summary>
		public const int PartSize = 64;

		/// <summary>
		///     Returns the minimum value in a generic sequence or the default value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="defaultValue">The default value to return if the <paramref name="source" /> has no elements.</param>
		/// <returns>
		///     The minimum value in the sequence or the default value.
		/// </returns>
		public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue = default (TSource))
		{
			return source.DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		///     Invokes a transform function on each element of a generic sequence and returns the minimum resulting value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <param name="source">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value to return if the <paramref name="source" /> has no elements.</param>
		/// <returns>
		///     The minimum value in the sequence or the default value.
		/// </returns>
		public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector,
			TResult defaultValue = default (TResult))
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
		}

		/// <summary>
		///     Returns the maximum value in a generic sequence or the default value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="defaultValue">The default value to return if the <paramref name="source" /> has no elements.</param>
		/// <returns>
		///     The maximum value in the sequence or the default value.
		/// </returns>
		public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue = default (TSource))
		{
			return source.DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		///     Invokes a transform function on each element of a generic sequence and returns the maximum resulting value.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
		/// <param name="source">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		/// <param name="defaultValue">The default value to return if the <paramref name="source" /> has no elements.</param>
		/// <returns>
		///     The maximum value in the sequence or the default value.
		/// </returns>
		public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector,
			TResult defaultValue = default (TResult))
		{
			return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
		}

		/// <summary>
		///     Produces the set difference of two sequences by using the specified function to calculate compared values.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <typeparam name="TValue">The type of the compared values.</typeparam>
		/// <param name="first">
		///     An <see cref="IEnumerable{T}" /> whose elements that are not also in <paramref name="second" />
		///     will be returned.
		/// </param>
		/// <param name="second">
		///     An <see cref="IEnumerable{T}" /> whose elements that also occur in the first sequence will cause
		///     those elements to be removed from the returned sequence.
		/// </param>
		/// <param name="valueSelector">The value selector to calculate compared values.</param>
		/// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <c>null</c>.</exception>
		public static IEnumerable<TSource> Except<TSource, TValue>(this IEnumerable<TSource> first,
			IEnumerable<TSource> second, Func<TSource, TValue> valueSelector)
		{
			return first.Except(second, new LambdaComparer<TSource, TValue>(valueSelector));
		}

		/// <summary>
		///     Appends the specified <paramref name="item" /> to the <paramref name="source" /> sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <param name="source">An input <see cref="IEnumerable{T}" />.</param>
		/// <param name="item">An item to append to the <paramref name="source" /> sequence.</param>
		/// <returns>A <paramref name="source" /> sequence with appended <paramref name="item" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="item" /> is <c>null</c>.</exception>
		public static IEnumerable<TSource> ConcatOne<TSource>([NotNull] this IEnumerable<TSource> source, TSource item)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return source.Concat(new[] {item});
		}

		/// <summary>
		///     Returns distinct elements from a sequence by using a specified lambda value selector function.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
		/// <typeparam name="TValue">The type of the compared values.</typeparam>
		/// <param name="source">The sequence to remove duplicate elements from.</param>
		/// <param name="valueSelector">The value selector, calculates the values to be compared.</param>
		/// <returns>An <see cref="IEnumerable{TSource}" /> that contains distinct elements from the source sequence.</returns>
		/// <exception cref="ArgumentNullException">
		///     <paramref name="source" /> is <c>null</c>.
		///     <paramref name="valueSelector" /> is <c>null</c>.
		/// </exception>
		public static IEnumerable<TSource> Distinct<TSource, TValue>(this IEnumerable<TSource> source,
			Func<TSource, TValue> valueSelector)
		{
			return source.Distinct(new LambdaComparer<TSource, TValue>(valueSelector));
		}


		/// <summary>
		/// Determines whether a sequence contains a specified element by using a specified lambda value selector function.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <typeparam name="TValue">The type of the compared values.</typeparam>
		/// <param name="first">A sequence in which to locate a value.</param>
		/// <param name="value">The value to locate in the sequence.</param>
		/// <param name="valueSelector">The value selector, calculates the values to be compared.</param>
		/// <returns><c>true</c> if the source sequence contains an element that has the specified value; otherwise, false.</returns>
		public static bool Contains<TSource, TValue>([NotNull]this IEnumerable<TSource> first, TSource value, [NotNull]Func<TSource, TValue> valueSelector)
		{
			return first.Contains(value, new LambdaComparer<TSource, TValue>(valueSelector));
		}


		/// <summary>
		/// Produces the set union of two sequences by using a specified lambda value selector function.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <typeparam name="TValue">The type of the compared values.</typeparam>
		/// <param name="first">An <see cref="IEnumerable{T}"/> whose distinct elements form the first set for the union.</param>
		/// <param name="second">An <see cref="IEnumerable{T}"/> whose distinct elements form the first set for the union.</param>
		/// <param name="valueSelector">The value selector, calculates the values to be compared.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements from both input sequences, excluding duplicates.</returns>
		public static IEnumerable<TSource> Union<TSource, TValue>([NotNull]this IEnumerable<TSource> first, [NotNull]IEnumerable<TSource> second, [NotNull]Func<TSource, TValue> valueSelector)
		{
			return first.Union(second, new LambdaComparer<TSource, TValue>(valueSelector));
		}

		/// <summary>
		///     Produces the set intersection of two sequences by using the specified lambda value selector function.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
		/// <typeparam name="TValue">The type of the compared values.</typeparam>
		/// <param name="first">
		///     An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in
		///     <paramref name="second" /> will be returned.
		/// </param>
		/// <param name="second">
		///     An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence
		///     will be returned.
		/// </param>
		/// <param name="valueSelector">The value selector, calculates the values to be compared.</param>
		/// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="first" /> or <paramref name="second" /> is <c>null</c>.</exception>
		public static IEnumerable<TSource> Intersect<TSource, TValue>(this IEnumerable<TSource> first,
			IEnumerable<TSource> second, Func<TSource, TValue> valueSelector)
		{
			return first.Intersect(second, new LambdaComparer<TSource, TValue>(valueSelector));
		}

		/// <summary>
		///     Partitions the specified <paramref name="source" /> sequence into part sequences of specified
		///     <paramref name="size" />.
		/// </summary>
		/// <remarks>
		///     If the specified <paramref name="source" /> is <c>null</c> or <paramref name="size" /> is not a positive number
		///     returns an empty sequence.
		/// </remarks>
		/// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
		/// <param name="source">The source sequence to partition.</param>
		/// <param name="size">The optional maximal size of one partition. Default value is <see cref="PartSize" />.</param>
		/// <returns>
		///     Sequence of sequences of specified <paramref name="size" /> or an empty sequence if <paramref name="source" />
		///     is <c>null</c>.
		/// </returns>
		public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size = PartSize)
		{
			if (source == null || size <= 0)
				yield break;

			List<T> part = null;
			int count = 0;
			foreach (T item in source)
			{
				if (part == null)
					part = new List<T>(size);
				part.Add(item);
				if (++count == size)
				{
					yield return part;
					count = 0;
					part = null;
				}
			}
			if (part != null)
				yield return part;
		}

		/// <summary>
		///     Returns <c>null</c> if the specified <paramref name="source" /> array is <c>null</c> or has no elements.
		/// </summary>
		/// <remarks>
		///     Used to prevent serializing empty optional XML elements if there are no entries.
		/// </remarks>
		/// <typeparam name="T">Type of array element.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns><c>null</c> if the specified <paramref name="source" /> array is <c>null</c> or has no elements.</returns>
		[CanBeNull]
		public static T[] NullIfEmpty<T>(this T[] source)
		{
			return source != null && source.Length > 0 ? source : null;
		}

		/// <summary>
		///     Determines whether the specified <paramref name="source" /> is <c>null</c> or contains no elements.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns>
		///     <c>true</c> if the specified <paramref name="source" /> is <c>null</c> or contains no elements; otherwise,
		///     <c>false</c>.
		/// </returns>
		[ContractAnnotation("null => true")]
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any();
		}

		/// <summary>
		///     Concatenates the specified <paramref name="source" /> collection of type <see cref="string" />,
		///     using the specified <paramref name="separator" /> between each member.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="separator">The string to use as a separator.</param>
		/// <returns>A string that consists of the members of values delimited by the separator string.</returns>
		[NotNull]
		public static string StringJoin(this IEnumerable<string> source, string separator)
		{
			if (source == null)
				return string.Empty;
			return string.Join(separator, source.DefaultIfEmpty(string.Empty).ToArray());
		}

		/// <summary>
		///     Compared to <see cref="Enumerable.Take{TSource}">Take&lt;string&gt;</see> for
		///     <see cref="IEnumerable{TSource}">IEnumerable&lt;string&gt;</see>, adds an <paramref name="ellipsis" /> if there are
		///     more than <paramref name="count" /> items.
		/// </summary>
		/// <param name="source">The sequence to return elements from.</param>
		/// <param name="count">The number of elements to return.</param>
		/// <param name="ellipsis">The trailing element returned if there are more elements.</param>
		/// <returns>
		///     An <see cref="IEnumerable{T}" /> that contains the specified number of elements from the start of the input
		///     sequence, with trailing <paramref name="ellipsis" /> if there are more.
		/// </returns>
		public static IEnumerable<string> TakeWithEllipsis(this IEnumerable<string> source, int count, string ellipsis = "...")
		{
			if (source == null)
				throw new ArgumentNullException("source");
			return TakeWithEllipsisIterator(source, count, ellipsis);
		}

		private static IEnumerable<string> TakeWithEllipsisIterator(IEnumerable<string> source, int count, string ellipsis)
		{
			if (count > 0)
				foreach (string item in source)
				{
					if (--count > 0)
						yield return item;
					else
					{
						yield return ellipsis;
						break;
					}
				}
		}
	}
}