using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using GenFu;
using System.Collections.Generic;
using System.Linq;

namespace ArrayVsListBench
{

    [MemoryDiagnoser]
    public class ArrayVsList
    {
        private List<string> DataList;
        private string[] DataArray;
        private Repo repo;

        [GlobalSetup]
        public void Setup()
        {
            var n = 10000;
            DataList = A.ListOf<Something>(n).Select(x => x.Id).ToList();
            DataArray = DataList.ToArray();
            repo = new Repo();
            repo.Ctx.AddRange(A.ListOf<Something>(n));
            repo.Ctx.SaveChanges();
        }


        [Benchmark]
        public List<string> BenchArrayToList() => DataArray.ToList();

        [Benchmark]
        public string[] BenchArrayToArray() => DataArray.ToArray();

        [Benchmark]
        public List<string> BenchListToList() => DataList.ToList();

        [Benchmark]
        public string[] BenchListoArray() => DataList.ToArray();

        [Benchmark]
        public List<string> BenchListFromArray() => new(DataArray);

        [Benchmark]
        public IList<Something> BenchEFToList() => repo.Somethings.ToList();

        [Benchmark]
        public IList<Something> BenchEFToArray() => repo.Somethings.ToArray();
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ArrayVsList>();
        }
    }
}
