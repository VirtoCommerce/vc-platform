using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Результат вычисления МП
	/// </summary>
	public class PromotionResult
	{
		public PromotionResult()
		{
			Rewards = new List<PromotionReward>();
		}

		public ICollection<PromotionReward> Rewards { get; private set; }

		public T GetReward<T>() where T : PromotionReward
		{
			return Rewards.OfType<T>().Where(x => x.IsValid).FirstOrDefault();
		}

		public T GetPotentialReward<T>() where T : PromotionReward
		{
			return Rewards.OfType<T>().Where(x => !x.IsValid).FirstOrDefault();
		}

		public T[] GetRewards<T>() where T : PromotionReward
		{
			return Rewards.OfType<T>().Where(x => x.IsValid).ToArray();
		}

		public T[] GetPotentialRewards<T>() where T : PromotionReward
		{
			return Rewards.OfType<T>().Where(x => !x.IsValid).ToArray();
		}
	}
}
