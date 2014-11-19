using System;
using System.Linq;
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

        [TestMethod]
        [DeploymentItem("connectionStrings.config")]
        [DeploymentItem("Configs/AppConfig.config", "Configs")]
        public void Run_sequences_performance()
        {
            var localRunCount = ++RunCount;
            Debug.WriteLine("Run count: " + localRunCount);
            var repository = new EFAppConfigRepository("VirtoCommerce");
            var sequence = new SequenceService(repository);


            for (var i = 1; i < SequenceService.SequenceReservationRange; i++)
            {
                var result = sequence.GetNext("test");
                Debug.WriteLine(result);
                //This would fail if any duplicate generated
                Assert.IsFalse(GlobalNumbers.ContainsKey(result));
                GlobalNumbers.Add(result, result);
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
