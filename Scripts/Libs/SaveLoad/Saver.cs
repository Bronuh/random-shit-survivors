

using Newtonsoft.Json.Linq;

namespace Scripts.Libs.SaveLoad
{
	public class Saver
	{
		public JObject jObject { get; private set; } = new JObject();

		public void InitSave()
		{
			jObject = new JObject();

		}
	}
}
