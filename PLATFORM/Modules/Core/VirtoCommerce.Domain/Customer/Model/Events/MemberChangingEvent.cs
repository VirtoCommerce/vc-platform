using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Events
{
	public class MemberChangingEvent
	{
		public MemberChangingEvent(EntryState state, Member member)
		{
			ChangeState = state;
            Member = member;
		}

		public EntryState ChangeState { get; set; }
		public Member Member { get; set; }
	}
}
