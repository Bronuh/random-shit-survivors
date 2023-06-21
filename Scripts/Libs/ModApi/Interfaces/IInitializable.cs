
namespace Scripts.Libs.ModApi
{
	/// <summary>
	///		Interface that defines methods called during initialization. The interface methods are called sequentially.
	/// </summary>
	public interface IInitializable
	{
		void PreInit();

		void Init();

		void PostInit();
	}
}
