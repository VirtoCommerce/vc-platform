using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.AppConfig.Services;

namespace PerformanceTests.AppConfig
{
    [TestClass]
    public class SequencesScenarios
    {

        public static Dictionary<string,string> generatedNumbers = new Dictionary<string, string>() ;

        [TestMethod]
        [DeploymentItem("connectionStrings.config")]
        [DeploymentItem("Configs/AppConfig.config", "Configs")]
        public void Run_sequences_performance()
        {
            var repository = new EFAppConfigRepository("VirtoCommerce");
            var sequence = new SequenceService(repository);

            //Get first
            var result = sequence.GetNext("test");
            Assert.IsNotNull(result);

            for (var i = 1; i < SequenceService.SequenceReservationRange; i++)
            {
                result = sequence.GetNext("test");
                Debug.Write(result);
                //This would fail if any duplicate generated
                Assert.IsFalse(generatedNumbers.ContainsKey(result));
                generatedNumbers.Add(result, result);
            }

            //var utcNow = DateTime.UtcNow;
            //var startCount = repository.Sequences.Single(x => x.ObjectType.Equals("test")).Value - SequenceService.SequenceReservationRange + 1;

            //var generatedItems = new Stack<string>();
            //for (var index = startCount; index < SequenceService.SequenceReservationRange + startCount; index++)
            //{
            //    var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(SequenceService.CounterLength, '0');
            //    generatedItems.Push(string.Format(SequenceService.IdTemplate, "test".ToUpper(), utcNow.ToString(SequenceService.DateFormat), strCount));
            //}

            //var reversedStack = new Stack<string>(generatedItems);


        }
    }
}
