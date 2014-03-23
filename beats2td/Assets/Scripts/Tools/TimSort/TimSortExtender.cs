#region Licence

/*
 * Copyright (C) 2008 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
#endregion

#region Notes
// Keripo Edit: The version of MonoDevelop that Unity 4 uses doesn't support default arguments yet,
// hence this file has been edited to remove them (buffered = true).
// In the situations where we do use TimSort, however, we usually want buffered = false anyway.

//------------------------------------------------------------------------------
// Java implementation:
//
// A stable, adaptive, iterative mergesort that requires far fewer than
// n lg(n) comparisons when running on partially sorted arrays, while
// offering performance comparable to a traditional mergesort when run
// on random arrays.  Like all proper mergesorts, this sort is stable and
// runs O(n log n) time (worst case).  In the worst case, this sort requires
// temporary storage space for n/2 object references; in the best case,
// it requires only a small constant amount of space.
// 
// This implementation was adapted from Tim Peters's list sort for
// Python, which is described in detail here:
// http://svn.python.org/projects/python/trunk/Objects/listsort.txt
// 
// Tim's C code may be found here:
// http://svn.python.org/projects/python/trunk/Objects/listobject.c
// 
// The underlying techniques are described in this paper (and may have
// even earlier origins):
// 
// "Optimistic Sorting and Information Theoretic Complexity"
// Peter McIlroy
// SODA (Fourth Annual ACM-SIAM Symposium on Discrete Algorithms),
// pp 467-474, Austin, Texas, 25-27 January 1993.
// 
// While the API to this class consists solely of static methods, it is
// (privately) instantiable; a TimSort instance holds the state of an ongoing
// sort, assuming the input array is large enough to warrant the full-blown
// TimSort. Small arrays are sorted in place, using a binary insertion sort.
// 
// author: Josh Bloch
//------------------------------------------------------------------------------
// C# implementation:
//
// This implementation was adapted from Josh Bloch array sort for Java, 
// which has been found here:
// http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/main/java/util/TimSort.java?view=co
// 
// author: Milosz Krajewski
//------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Diagnostics;
using TimSort;

namespace System.Linq
{
	#region class TimSortExtender

	public static class TimSortExtender
	{
		#region Array (T[])
		
		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		public static void TimSort<T>(this T[] array)
		{
			ArrayTimSort<T>.Sort(array, Comparer<T>.Default.Compare);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		public static void TimSort<T>(this T[] array, int start, int length)
		{
			length = Math.Min(length, array.Length - start);
			ArrayTimSort<T>.Sort(array, start, start + length, Comparer<T>.Default.Compare);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this T[] array, Comparison<T> comparer)
		{
			ArrayTimSort<T>.Sort(array, comparer);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this T[] array, int start, int length, Comparison<T> comparer)
		{
			length = Math.Min(length, array.Length - start);
			ArrayTimSort<T>.Sort(array, start, start + length, comparer);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this T[] array, Comparer<T> comparer)
		{
			ArrayTimSort<T>.Sort(array, comparer.Compare);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start.</param>
		/// <param name="length">The length.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this T[] array, int start, int length, Comparer<T> comparer)
		{
			length = Math.Min(length, array.Length - start);
			ArrayTimSort<T>.Sort(array, start, start + length, comparer.Compare);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of compared expression.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="expression">The compared expression.</param>
		public static void TimSort<T, C>(this T[] array, Func<T, C> expression)
		{
			Comparison<T> comparer = (a, b) => Comparer<C>.Default.Compare(expression(a), expression(b));
			ArrayTimSort<T>.Sort(array, comparer);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of compared expression.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		/// <param name="expression">The compared expression.</param>
		public static void TimSort<T, C>(this T[] array, int start, int length, Func<T, C> expression)
		{
			length = Math.Min(length, array.Length - start);
			Comparison<T> comparer = (a, b) => Comparer<C>.Default.Compare(expression(a), expression(b));
			ArrayTimSort<T>.Sort(array, start, start + length, comparer);
		}


		#endregion

		#region List (IList<T>)

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		public static void TimSort<T>(this IList<T> array, bool buffered)
		{
			if (buffered)
			{
				BufferedListSort(array, 0, array.Count, Comparer<T>.Default.Compare);
			}
			else
			{
				ListTimSort<T>.Sort(array, Comparer<T>.Default.Compare);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		public static void TimSort<T>(this IList<T> array, int start, int length, bool buffered)
		{
			length = Math.Min(length, array.Count - start);
			
			if (buffered)
			{
				BufferedListSort(array, start, length, Comparer<T>.Default.Compare);
			}
			else
			{
				ListTimSort<T>.Sort(array, start, start + length, Comparer<T>.Default.Compare);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this IList<T> array, Comparison<T> comparer, bool buffered)
		{
			if (buffered)
			{
				BufferedListSort(array, 0, array.Count, comparer);
			}
			else
			{
				ListTimSort<T>.Sort(array, comparer);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this IList<T> array, int start, int length, Comparison<T> comparer, bool buffered)
		{
			length = Math.Min(length, array.Count - start);

			if (buffered)
			{
				BufferedListSort(array, start, length, comparer);
			}
			else
			{
				ListTimSort<T>.Sort(array, start, start + length, comparer);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this IList<T> array, Comparer<T> comparer, bool buffered)
		{
			if (buffered)
			{
				BufferedListSort(array, 0, array.Count, comparer.Compare);
			}
			else
			{
				ListTimSort<T>.Sort(array, comparer.Compare);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start.</param>
		/// <param name="length">The length.</param>
		/// <param name="comparer">The comparer.</param>
		public static void TimSort<T>(this IList<T> array, int start, int length, Comparer<T> comparer, bool buffered)
		{
			length = Math.Min(length, array.Count - start);

			if (buffered)
			{
				BufferedListSort(array, start, length, comparer.Compare);
			}
			else
			{
				ListTimSort<T>.Sort(array, start, start + length, comparer.Compare);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of compared expression</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="expression">The compared expression.</param>
		/// <param name="buffered">Set it to <c>true</c> if you need buffered sorting.</param>
		public static void TimSort<T, C>(this IList<T> array, Func<T, C> expression, bool buffered)
		{
			Comparison<T> comparer = (a, b) => Comparer<C>.Default.Compare(expression(a), expression(b));

			if (buffered)
			{
				BufferedListSort(array, 0, array.Count, comparer);
			}
			else
			{
				ListTimSort<T>.Sort(array, comparer);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="C">Type of compared expression.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length.</param>
		/// <param name="expression">The compared expression.</param>
		/// <param name="buffered">Set it to <c>true</c> if you need buffered sorting.</param>
		public static void TimSort<T, C>(this IList<T> array, int start, int length, Func<T, C> expression, bool buffered)
		{
			length = Math.Min(length, array.Count - start);
			Comparison<T> comparer = (a, b) => Comparer<C>.Default.Compare(expression(a), expression(b));

			if (buffered)
			{
				BufferedListSort(array, start, length, comparer);
			}
			else
			{
				ListTimSort<T>.Sort(array, start, start + length, comparer);
			}
		}
		
		#endregion

		#region BufferedListSort

		/// <summary>Buffered list sort.</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="start">The start.</param>
		/// <param name="length">The length.</param>
		/// <param name="comparer">The comparer.</param>
		private static void BufferedListSort<T>(IList<T> list, int start, int length, Comparison<T> comparer)
		{
			Debug.Assert(start >= 0 && start < list.Count);
			Debug.Assert(length >= 0 && length <= list.Count - start);

			T[] array = new T[length];

			int left, src, dst;
			
			if (start == 0 && length >= list.Count)
			{
				list.CopyTo(array, 0); // this might be faster than copying one by one
			}
			else
			{
				left = length; src = start; dst = 0;
				while (left-- > 0) array[dst++] = list[src++];
			}
			
			ArrayTimSort<T>.Sort(array, comparer);
			
			left = length; src = 0; dst = start;
			while (left-- > 0) list[dst++] = array[src++];
		}
		
		#endregion
	}
	
	#endregion
}
