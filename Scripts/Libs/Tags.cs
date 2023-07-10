
using System.Collections;

namespace Scripts.Libs
{
	public interface ITagsHolder
	{
		TagsContainer Tags { get; }
	}

	public class TagsMixin : ITagsHolder
	{
		public TagsContainer Tags { get; } = new TagsContainer();
	}

	public interface ITagsContainer : IEnumerable<string>
	{
		public IEnumerable<string> Tags { get; }

		bool HasTag(string tag) => Tags.Contains(tag);
		bool HasAny(params string[] tags) => Tags.HasAnyOf(tags);
		bool HasAll(params string[] tags) => Tags.HasAllOf(tags);

		void Add(string tag);
		void Remove(string tag);
	}

	public class TagsContainer : ITagsContainer
	{
		IEnumerable<string> ITagsContainer.Tags => _tags;
		public string this[int i]
		{
			get => _tags[i];
			set => _tags[i] = value;
		}

		public int Count => _tags.Count;

		private List<string> _tags = new();
		public void Add(string tag)
		{
			if(!((ITagsContainer)this).HasTag(tag))
				_tags.Add(tag);
		}

		public void Remove(string tag)
		{
			_tags.Remove(tag);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _tags.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class TagsFilter
	{
		private readonly List<Func<ITagsContainer, bool>> filters;

		public bool IsAny { get; private set; } = false;
		/// <summary>
		/// Initializes a new instance of the TagsFilter class with the specified filters.
		/// </summary>
		/// <param name="filters">The list of filters to be applied.</param>
		private TagsFilter(List<Func<ITagsContainer, bool>> filters)
		{
			this.filters = filters;
		}

		/// <summary>
		/// Initializes a new instance of the TagsFilter class with the filter, that match anything.
		/// </summary>
		/// <returns>Always true</returns>
		public static TagsFilter Any()
		{
			var filter = new TagsFilter(new List<Func<ITagsContainer, bool>>
		{
			tagsContainer => true
		});
			filter.IsAny = true;
			return filter;
		}

		/// <summary>
		/// Checks if the specified ITagsContainer matches all the filters in the TagsFilter instance.
		/// </summary>
		/// <param name="tagsContainer">The ITagsContainer to be matched against the filters.</param>
		/// <returns>True if the ITagsContainer matches all the filters; otherwise, false.</returns>
		public bool Match(ITagsContainer tagsContainer)
		{
			return filters.All(filter => filter(tagsContainer));
		}

		/// <summary>
		/// Creates a TagsFilter instance that checks if the ITagsContainer has any of the specified tags.
		/// </summary>
		/// <param name="tags">The tags to be checked.</param>
		/// <returns>A TagsFilter instance with the specified filter.</returns>
		public static TagsFilter HasAny(params string[] tags)
		{
			var filter = new TagsFilter(new List<Func<ITagsContainer, bool>>
		{
			tagsContainer => tagsContainer.HasAny(tags)
		});

			return filter;
		}

		/// <summary>
		/// Creates a TagsFilter instance that checks if the ITagsContainer has all of the specified tags.
		/// </summary>
		/// <param name="tags">The tags to be checked.</param>
		/// <returns>A TagsFilter instance with the specified filter.</returns>
		public static TagsFilter HasAll(params string[] tags)
		{
			var filter = new TagsFilter(new List<Func<ITagsContainer, bool>>
		{
			tagsContainer => tagsContainer.HasAll(tags)
		});

			return filter;
		}

		/// <summary>
		/// Combines the current TagsFilter instance with another TagsFilter instance using the logical OR operation.
		/// </summary>
		/// <param name="other">The TagsFilter instance to be combined.</param>
		/// <returns>A new TagsFilter instance with filters from both instances.</returns>
		public TagsFilter Or(TagsFilter other)
		{
			var combinedFilters = filters.Concat(other.filters).ToList();
			var filter = new TagsFilter(combinedFilters);

			return filter;
		}

		/// <summary>
		/// Combines the current TagsFilter instance with another TagsFilter instance using the logical AND operation.
		/// </summary>
		/// <param name="other">The TagsFilter instance to be combined.</param>
		/// <returns>A new TagsFilter instance with filters that check if both filters are satisfied.</returns>
		public TagsFilter And(TagsFilter other)
		{
			var combinedFilters = filters.SelectMany(filter => other.filters.Select(
				otherFilter => (Func<ITagsContainer, bool>)(tagsContainer =>
					filter(tagsContainer) && otherFilter(tagsContainer))
				)).ToList();

			var filter = new TagsFilter(combinedFilters);

			return filter;
		}

		/// <summary>
		/// Creates a new TagsFilter instance with a filter that negates the result of the current TagsFilter instance.
		/// </summary>
		/// <returns>A new TagsFilter instance with a negated filter.</returns>
		public TagsFilter Not()
		{
			var filter = new TagsFilter(new List<Func<ITagsContainer, bool>>
		{
			tagsContainer => !Match(tagsContainer)
		});

			return filter;
		}
	}
}
