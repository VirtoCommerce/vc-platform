using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Customers.Model;


namespace VirtoCommerce.ManagementClient.Customers.Services
{
	public class MockCaseService
	{
		private List<string> _customersAgentsList = new List<string>();

		public MockCaseService()
		{
			GenerateCustomersAndAgentslist();
		}

		private Case[] GenerateCases(int count)
		{
			List<Case> retVal = new List<Case>();

			Random r = new Random();

			for (int i = 0; i < count; i++)
			{
				Case createdCase = new Case();
				createdCase.Title = string.Format("Title {0}", i);
				createdCase.Description = string.Format("Description {0}", i);

				if (i % 2 == 0)
				{
					createdCase.Status = "1";
					createdCase.Channel = "1";
					createdCase.Priority = 0;
				}
				else
				{
					createdCase.Status = "2";
                    createdCase.Channel = "1";
					createdCase.Priority = 1;
				}

				if (i % 3 == 0)
				{
					createdCase.Status = "3";
                    createdCase.Channel = "1";
					createdCase.Priority = 2;
				}

				//HistoryItemDto[] histories = new HistoryItemDto[]{
				//	new HistoryItemDto(){ModifyDate=DateTime.Now},
				//	new HistoryItemDto(){ModifyDate=new DateTime(2011,12,12)}


				//LabelDto[] labels = new LabelDto[] { };

				//int customerId = r.Next(0, 101);
				//createdCase.CustomerId = customerId;
				int agentId = r.Next(0, 101);
				createdCase.AgentId = agentId.ToString();
				createdCase.AgentName = _customersAgentsList[agentId];

				//createdCase.Histories = histories;
				//createdCase.Items = GetLabels().Skip(9).ToArray();

				retVal.Add(createdCase);
			}

			return retVal.ToArray();
		}

		private void GenerateCustomersAndAgentslist()
		{
			for (int i = 0; i < 100; i++)
			{
				_customersAgentsList.Add(string.Format("User {0}", i));
			}
		}

		#region ICaseService Members

		public CaseSearchResult SearchCases(SearchCaseCriteria criteria, int startIndex, int count)
		{
			CaseSearchResult retVal = new CaseSearchResult()
			{
				CaseInfos = GenerateCases(count).Skip(startIndex).ToArray(),
				TotalResults = count
			};

			System.Threading.Thread.Sleep(1000);
			return retVal;
		}

		public Label[] GetLabels()
		{

			List<Label> labels = new List<Label>();

			for (int i = 0; i < 15; i++)
			{
				Label lab = new Label() { Name = string.Format("Label {0}", i), Description = string.Format("labelDescription {0}", i) };
				labels.Add(lab);
			}


			return labels.ToArray();

		}

		#endregion

	}
}
