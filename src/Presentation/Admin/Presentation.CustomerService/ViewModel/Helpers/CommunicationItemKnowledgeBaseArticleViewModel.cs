using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoSoftware.CommerceFoundation.Customer.Models;
using Presentation.Customers.Infrastructure.Communications;

namespace Presentation.Customers.ViewModel.Helpers
{
	public class CommunicationItemKnowledgeBaseArticleViewModel : CommunicationItemViewModel
	{
        public CommunicationItemKnowledgeBaseArticleViewModel()
			: base()
		{
			this.Type = CaseType.KnowledgeBaseArticle;
		}
	}
}
