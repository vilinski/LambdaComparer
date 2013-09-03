using System;
using System.Collections;
using System.Collections.Generic;

using FP.Properties;

namespace FP
{
	/// <summary>
	///     Defines methods to support the comparison of two objects.
	/// </summary>
	/// <remarks>
	///     Usefull by sorting on any object properties, or to create overloads for extension methods like
	///     <see cref="EnumerableEx.Distinct{TSource,TValue}" />, etc.
	/// </remarks>
	/// <typeparam name="T">The type of objects to compare.</typeparam>
	/// <typeparam name="TProp">The type of the compared values, returned from the value selector function.</typeparam>
	public class LambdaComparer<T, TProp> : IEqualityComparer<T>, IComparer<T>, IComparer
	{
		private readonly Func<T, T, int> _compare;
		private readonly Func<T, int> _hash;

		/// <summary>
		///     Initializes a new instance of the <see cref="LambdaComparer{T,TProp}" /> class.
		/// </summary>
		/// <param name="valueSelector">The value selector. Not necessary just a property value selector.</param>
		/// <param name="descending">The descending sort direction. Allows to invert the comparison result.</param>
		/// <exception cref="ArgumentNullException"><paramref name="valueSelector" /> is <c>null</c>.</exception>
		public LambdaComparer(Func<T, TProp> valueSelector, bool descending = false)
		{
			if (valueSelector == null)
				throw new ArgumentNullException("valueSelector");

			_hash = obj => valueSelector(obj).GetHashCode();
			Comparer<TProp> comparer = Comparer<TProp>.Default;
			if (!descending)
				_compare = (x, y) => comparer.Compare(valueSelector(x), valueSelector(y));
			else
				_compare = (x, y) => comparer.Compare(valueSelector(y), valueSelector(x));
		}

		#region Implementation of IEqualityComparer<T>

		/// <summary>
		///     Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <typeparamref name="T" /> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if the specified objects are equal; otherwise, false.
		/// </returns>
		public bool Equals(T x, T y)
		{
			return _compare(x, y) == 0;
		}

		/// <summary>
		///     Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The object for which a hash code is to be returned.</param>
		/// <returns>
		///     A hash code for the specified object.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///     The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.
		/// </exception>
		public int GetHashCode([NotNull] T obj)
		{
// ReSharper disable CompareNonConstrainedGenericWithNull
			if (obj == null)
// ReSharper restore CompareNonConstrainedGenericWithNull
				throw new ArgumentNullException("obj");
			return _hash(obj);
		}

		#endregion

		#region Implementation of IComparer<T>

		/// <summary>
		///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		///     <list type="table">
		///         <listheader>
		///             <term>Value</term>
		///             <description>Condition</description>
		///         </listheader>
		///         <item>
		///             <term>Less than zero</term>
		///             <description><paramref name="x" /> is less than <paramref name="y" />.</description>
		///         </item>
		///         <item>
		///             <term>Zero</term>
		///             <description><paramref name="x" /> equals <paramref name="y" />.</description>
		///         </item>
		///         <item>
		///             <term>Greater than zero</term>
		///             <description><paramref name="x" /> is greater than <paramref name="y" />.</description>
		///         </item>
		///     </list>
		/// </returns>
		public int Compare(T x, T y)
		{
			return _compare(x, y);
		}

		#endregion

		/// <summary>
		///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		///     <list type="table">
		///         <listheader>
		///             <term>Value</term>
		///             <description>Condition</description>
		///         </listheader>
		///         <item>
		///             <term>Less than zero</term>
		///             <description><paramref name="x" /> is less than <paramref name="y" />.</description>
		///         </item>
		///         <item>
		///             <term>Zero</term>
		///             <description><paramref name="x" /> equals <paramref name="y" />.</description>
		///         </item>
		///         <item>
		///             <term>Greater than zero</term>
		///             <description><paramref name="x" /> is greater than <paramref name="y" />.</description>
		///         </item>
		///     </list>
		/// </returns>
		public int Compare(object x, object y)
		{
			return Compare((T) x, (T) y);
		}
	}
}