namespace Scripts.Libs
{
	public interface IPrototype<T>
	{
		public T Prototype { get; }
		public bool IsInstance { get; }

		T Instantiate();
		void MakePrototype();
	}
}