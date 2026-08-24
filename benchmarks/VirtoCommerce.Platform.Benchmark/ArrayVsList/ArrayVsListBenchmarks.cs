using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using A = GenFu.GenFu;

namespace VirtoCommerce.Platform.Benchmark.ArrayVsList;

[MemoryDiagnoser]
public class ArrayVsListBenchmarks
{
    private List<string> _dataList;
    private string[] _dataArray;
    private Repo _repo;

    [GlobalSetup]
    public void Setup()
    {
        const int n = 10000;
        _dataList = A.ListOf<Something>(n).Select(x => x.Id).ToList();
        _dataArray = _dataList.ToArray();
        _repo = new Repo();
        _repo.Ctx.AddRange(A.ListOf<Something>(n));
        _repo.Ctx.SaveChanges();
    }


    [Benchmark]
    public List<string> BenchArrayToList() => _dataArray.ToList();

    [Benchmark]
    public string[] BenchArrayToArray() => _dataArray.ToArray();

    [Benchmark]
    public List<string> BenchListToList() => _dataList.ToList();

    [Benchmark]
    public string[] BenchListToArray() => _dataList.ToArray();

    [Benchmark]
    public List<string> BenchListFromArray() => [.._dataArray];

    [Benchmark]
    public IList<Something> BenchEfToList() => _repo.Somethings.ToList();

    [Benchmark]
    public IList<Something> BenchEfToArray() => _repo.Somethings.ToArray();
}
