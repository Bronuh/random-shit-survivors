using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.ModApi
{
	/// <summary>
	///		Classes marked with this attribute will have their static constructor called after post-initialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class StaticConstructorOnStartup : Attribute { }


}
