using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public interface IHaveDimension
	{
		string WeightUnit { get; set; }
		decimal? Weight { get; set; }

		string MeasureUnit { get; set; }
		decimal? Height { get; set; }
		decimal? Length { get; set; }
		decimal? Width { get; set; }

	}
}
