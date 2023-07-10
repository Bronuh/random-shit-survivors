using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs.Stats
{
	public interface IStatusEffectConsumer
	{
		void ApplyEffect(IStatusEffect effect);
		void RemoveEffect(IStatusEffect effect);
	}
}
