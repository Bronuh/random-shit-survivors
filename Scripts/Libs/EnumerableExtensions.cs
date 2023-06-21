
namespace Scripts.Libs
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Возвращает случайный элемент списка
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IEnumerable<T> sequence)
		{
			Random random = new Random();
			if (sequence == null)
			{
				throw new ArgumentNullException();
			}

			if (!sequence.Any())
			{
				throw new ArgumentException("The sequence is empty.");
			}

			//optimization for ICollection<T>
			if (sequence is ICollection<T>)
			{
				ICollection<T> col = (ICollection<T>)sequence;
				return col.ElementAt(random.Next(col.Count));
			}

			int count = 1;
			T selected = default(T);

			foreach (T element in sequence)
			{
				if (random.Next(count++) == 0)
				{
					//Select the current element with 1/count probability
					selected = element;
				}
			}

			return selected;
		}

		/// <summary>
		/// Splits an IEnumerable into multiple IEnumerables of a specified size.
		/// </summary>
		/// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
		/// <param name="source">The IEnumerable to split.</param>
		/// <param name="splitSize">The size of each split IEnumerable.</param>
		/// <returns>An IEnumerable of IEnumerables, each containing splitSize elements from the source.</returns>
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int splitSize)
		{
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return InnerSplit(enumerator, splitSize);
				}
			}

		}

		private static IEnumerable<T> InnerSplit<T>(IEnumerator<T> enumerator, int splitSize)
		{
			int count = 0;
			do
			{
				count++;
				yield return enumerator.Current;
			}
			while (count % splitSize != 0
				 && enumerator.MoveNext());
		}


		/// <summary>
		/// Converts a System.IO.Stream to a byte array.
		/// </summary>
		/// <param name="stream">The stream to convert.</param>
		/// <returns>A byte array containing the data from the stream.</returns>
		public static byte[] ConvertToByteArray(this System.IO.Stream stream)
		{
			var streamLength = Convert.ToInt32(stream.Length);
			byte[] data = new byte[streamLength + 1];

			//convert to to a byte array
			stream.Read(data, 0, streamLength);
			stream.Close();

			return data;
		}


		/// <summary>
		/// Returns the first element of a sequence, or null if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="values">The sequence to return the first element of.</param>
		/// <returns>null if source is empty; otherwise, the first element in source.</returns>
		public static T FirstOrNull<T>(this IEnumerable<T> values) where T : class
		{
			return values.DefaultIfEmpty(null).FirstOrDefault();
		}


		/// <summary>
		/// Flattens an <see cref="IEnumerable"/> of <see cref="String"/> objects to a single string, seperated by an optional seperator and with optional head and tail.
		/// </summary>
		/// <param name="strings">The string objects to be flattened.</param>
		/// <param name="seperator">The seperator to be used between each string object.</param>
		/// <param name="head">The string to be used at the beginning of the flattened string. Defaulted to an empty string.</param>        
		/// <param name="tail">The string to be used at the end of the flattened string. Defaulted to an empty string.</param>
		/// <returns>Single string containing all elements seperated by the given seperator, with optionally a head and/or tail.</returns>
		public static string Flatten(this IEnumerable<string> strings, string seperator, string head, string tail)
		{
			// If the collection is null, or if it contains zero elements,
			// then return an empty string.
			if (strings == null || strings.Count() == 0)
				return String.Empty;

			// Build the flattened string
			var flattenedString = new StringBuilder();

			flattenedString.Append(head);
			foreach (var s in strings)
				flattenedString.AppendFormat("{0}{1}", s, seperator); // Add each element with the given seperator.
			flattenedString.Remove(flattenedString.Length - seperator.Length, seperator.Length); // Remove the last seperator
			flattenedString.Append(tail);

			// Return the flattened string
			return flattenedString.ToString();
		}
	}

}
