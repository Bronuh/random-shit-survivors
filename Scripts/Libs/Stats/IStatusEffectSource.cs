using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs.Stats
{
	public interface IStatusEffectSource
	{
		IEnumerable<IStatusEffect> Effects { get;}

		void ApplyEffects(params IStatusEffectConsumer[] targets);
		bool IsAffected(IStatusEffectConsumer target);
	}
}
