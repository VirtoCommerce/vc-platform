using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Sequences;

namespace VirtoCommerce.Foundation.Data.AppConfig.Services
{
    public class DbIdentitySequenceService : ISequenceService
    {
        private readonly IAppConfigRepository _repository;
        //********These settings could be saved in database:

        //Prefix_Date_Seq
        public static string IdTemplate = "{0}{1}-{2}";
        //How many sequence items will be stored in-memory
        public const int SequenceReservationRange = 100;
        //Constant length of counter. Trailing zeros are added to left.
        public const int CounterLength = 8;

        public static string DateFormat = "yyyy-MMdd";

        //***********************************************


        public DbIdentitySequenceService(IAppConfigRepository repository)
        {
            _repository = repository;

            /*Uncomment following lines on your own risk. Dont forget these rules
             * 1. Traking number contains three parts {0}{1}{2}
             * {0}. Prefix is option
             * {1}. Date part is required and must contain all three Year/Month/Day
             * {2}  Counter is required
             */
            var template = repository.Settings.Expand(x=>x.SettingValues).FirstOrDefault(x => x.Name == "TrackingNumberFormat");
            if (template != null && template.SettingValues.Any())
            {
                IdTemplate = template.SettingValues[0].ToString();
            }

            var dateFormat = repository.Settings.Expand(x=>x.SettingValues).FirstOrDefault(x => x.Name == "TrackingNumberFormatDateFormat");
            if (dateFormat != null && dateFormat.SettingValues.Any())
            {
                DateFormat = dateFormat.SettingValues[0].ToString();
            }
        }

        public string GetNext(string key)
        {
            var prefix = String.Empty;
            prefix = key.Substring(key.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
            prefix = String.IsNullOrEmpty(prefix) ? "U" : prefix.ToUpper();


            var sql = "INSERT IdentitySequence DEFAULT VALUES;SELECT cast(SCOPE_IDENTITY() as int);";
            var result = ((DbContext)_repository).Database.SqlQuery<int>(sql).First();

            var strCount = result.ToString(CultureInfo.InvariantCulture).PadLeft(CounterLength, '0');
            var formatted = string.Format(IdTemplate, prefix, DateTime.UtcNow.ToString(DateFormat), strCount);

            return formatted;
        }
    }
}
