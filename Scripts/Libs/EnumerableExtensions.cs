using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}

}
