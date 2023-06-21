
namespace Scripts.Libs.ModApi
{
	/// <summary>
	///		Interface describing a single method called BEFORE initialization begins.
	///		Can be used to replace system services (manager or mod loader).
	/// </summary>
	public interface ICoreMod
	{
		/// <summary>
		///		Called BEFORE invoking all initializers.
		/// </summary>
		void Execute();
	}
}
