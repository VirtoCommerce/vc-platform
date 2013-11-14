using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.CronExpressionDescriptor
{
	public class ExpressionParser
	{
		private string m_expression;
		private Options m_options;

		public ExpressionParser(string expression, Options options)
		{
			m_expression = expression;
			m_options = options;
		}

		public string[] Parse()
		{
			string[] parsed = new string[6];

			if (string.IsNullOrEmpty(m_expression))
			{
				throw new MissingFieldException("ExpressionDescriptor", "expression");
			}
			else
			{
				string[] expressionPartsTemp = m_expression.Split(' ');

				if (expressionPartsTemp.Length < 5)
				{
					throw new FormatException(string.Format("Error: Expression only has {0} parts.  At least 5 part are required.", expressionPartsTemp.Length));
				}
				else if (expressionPartsTemp.Length > 6)
				{
					throw new FormatException(string.Format("Error: Expression has too many parts ({0}).  Expression must not have more than 6 parts.", expressionPartsTemp.Length));
				}
				else if (expressionPartsTemp.Length == 5)
				{
					//5 part cron so defualt seconds to empty and shift array
					parsed[0] = string.Empty;
					Array.Copy(expressionPartsTemp, 0, parsed, 1, 5);
				}
				else if (expressionPartsTemp.Length == 6)
				{
					parsed = expressionPartsTemp;
				}
			}

			NormalizeExpression(parsed);

			return parsed;
		}

		private void NormalizeExpression(string[] expressionParts)
		{
			//convert ? to * only for DOM and DOW
			expressionParts[3] = expressionParts[3].Replace("?", "*");
			expressionParts[5] = expressionParts[5].Replace("?", "*");

			//convert 0/, 1/ to */
			expressionParts[0] = expressionParts[0].Replace("0/", "*/"); //seconds
			expressionParts[1] = expressionParts[1].Replace("0/", "*/"); //minutes
			expressionParts[2] = expressionParts[2].Replace("0/", "*/"); //hours
			expressionParts[3] = expressionParts[3].Replace("1/", "*/"); //DOM
			expressionParts[4] = expressionParts[4].Replace("1/", "*/"); //Month
			expressionParts[5] = expressionParts[5].Replace("1/", "*/"); //DOW

			//convert */1 to *
			for (int i = 0; i <= 5; i++)
			{
				if (expressionParts[i] == "*/1")
				{
					expressionParts[i] = "*";
				}
			}

			//convert SUN-SAT format to 0-6 format
			for (int i = 0; i <= 6; i++)
			{
				if (!m_options.DayOfWeekStartIndexZero)
				{
					expressionParts[5] = expressionParts[5].Replace((i + 1).ToString(), i.ToString());
				}

				DayOfWeek currentDay = (DayOfWeek)i;
				string currentDayOfWeekDescription = currentDay.ToString().Substring(0, 3).ToUpper();
				expressionParts[5] = expressionParts[5].Replace(currentDayOfWeekDescription, i.ToString());
			}

			//convert  JAN-DEC format to 1-12 format
			for (int i = 1; i <= 12; i++)
			{
				DateTime currentMonth = new DateTime(DateTime.Now.Year, i, 1);
				string currentMonthDescription = currentMonth.ToString("MMM").ToUpper();
				expressionParts[4] = expressionParts[4].Replace(currentMonthDescription, i.ToString());
			}

			//convert 0 second to (empty)
			if (expressionParts[0] == "0")
			{
				expressionParts[0] = string.Empty;
			}
		}
	}
}
