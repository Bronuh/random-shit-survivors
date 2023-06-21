
namespace Scripts.Libs.ModApi
{
	/// <summary>
	///		Classes marked with this attribute will have their static constructor called after post-initialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class StaticConstructorOnStartup : Attribute { }


}
