using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Services;

namespace VirtoCommerce.OrderModule.Test
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
            var repository = new CommerceRepositoryImpl("VirtoCommerce");
            repository.Database.ExecuteSqlCommand(sql);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            const string sql =
                @"IF OBJECT_ID('dbo.UniqueSequence', 'U') IS NOT NULL
                    DROP TABLE [dbo].[UniqueSequence]";
            var repository = new CommerceRepositoryImpl("VirtoCommerce");
            repository.Database.ExecuteSqlCommand(sql);

        }


        [TestMethod]
        [DeploymentItem("connectionStrings.config")]
        [DeploymentItem("Configs/AppConfig.config", "Configs")]
        public void RunSequencesPerformance()
        {
            var repository = new CommerceRepositoryImpl("VirtoCommerce");
            var sequence = new SequenceUniqueNumberGeneratorServiceImpl(() => repository);

            for (var i = 1; i < SequenceUniqueNumberGeneratorServiceImpl.SequenceReservationRange; i++)
            {
                var result = sequence.GenerateNumber("CO{0:yyMMdd}-{1:D5}");
                Debug.WriteLine(result);

                //This would fail if any duplicate generated
                Assert.IsFalse(GlobalNumbers.ContainsKey(result));
                GlobalNumbers.Add(result, result);

                const string sql = "INSERT UniqueSequence VALUES(@p0);";
                //This would fail if any duplicate generated beause we use primary key
                var sqlResult = repository.Database.ExecuteSqlCommand(sql, result);
                Assert.AreEqual(1, sqlResult);
            }
        }
    }
}
