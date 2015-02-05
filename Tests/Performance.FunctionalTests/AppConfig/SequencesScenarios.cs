using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Services;

namespace PerformanceTests.AppConfig
{
    [TestClass]
    public class SequencesScenarios
    {

        public static Dictionary<string, string> GlobalNumbers = new Dictionary<string, string>();
        public static int RunCount = 0;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            const string sql =
                @"IF OBJECT_ID('dbo.UniqueSequence', 'U') IS NULL
                    CREATE TABLE [dbo].[UniqueSequence]([Sequence] [nvarchar](255) NOT NULL,CONSTRAINT [PK_UniqueSequence] PRIMARY KEY CLUSTERED ([Sequence] ASC)
                    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON))";
            var repository = new EFAppConfigRepository("VirtoCommerce");
            repository.Database.ExecuteSqlCommand(sql);

        }

        [ClassCleanup]
        public static void Cleanup()
        {
            const string sql =
                @"IF OBJECT_ID('dbo.UniqueSequence', 'U') IS NOT NULL
                    DROP TABLE [dbo].[UniqueSequence]";
            var repository = new EFAppConfigRepository("VirtoCommerce");
            repository.Database.ExecuteSqlCommand(sql);

        }


        [TestMethod]
        [DeploymentItem("connectionStrings.config")]
        [DeploymentItem("Configs/AppConfig.config", "Configs")]
        public void Run_sequences_performance()
        {
            var repository = new EFAppConfigRepository("VirtoCommerce");
            var sequence = new SequenceService(repository);


            for (var i = 1; i < SequenceService.SequenceReservationRange; i++)
            {
                var result = sequence.GetNext("test");
                Debug.WriteLine(result);

                //This would fail if any duplicate generated
                Assert.IsFalse(GlobalNumbers.ContainsKey(result));
                GlobalNumbers.Add(result, result);

                const string sql = "INSERT UniqueSequence VALUES(@p0);";
                //This would fail if any duplicate generated beause we use primary key
                var sqlResult = repository.Database.ExecuteSqlCommand(sql, result);
                Assert.AreEqual(1,sqlResult);
            }
        }
    }
}
