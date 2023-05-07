using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerplateProject.Scripts.Libs
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Выводит все строковые значения в одну строку, с разделением запятыми
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static string ToLine(this IEnumerable<string> list)
		{
			string respond = "";
			foreach (string word in list)
				respond += word + (word == list.Last() ? "" : ", ");

			return respond;
		}


		/// <summary>
		/// Возвращает случайный элемент списка
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IList<T> list)
		{
			return list[new Random().Next(0, list.Count)];
		}

		/// <summary>
		/// Возвращает случайный элемент списка
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IEnumerable<T> list)
		{
			return list.ElementAt(new Random().Next(0, list.Count()));
		}
	}
}
