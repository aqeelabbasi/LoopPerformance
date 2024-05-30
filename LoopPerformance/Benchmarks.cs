using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace LoopPerformance
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private readonly List<string> _list = [];
        private readonly int _size = 1000000;


        //    | Method  | Mean       | Error    | StdDev   | Allocated |
        //|-------- |-----------:|---------:|---------:|----------:|
        //| For     |   710.0 us | 14.01 us | 17.71 us |         - |
        //| Foreach |   727.1 us | 13.60 us | 28.69 us |         - |
        //| ForEach | 1,561.6 us | 27.60 us | 69.76 us |      94 B |
        //| While   |   679.1 us | 15.14 us | 42.44 us |         - |
        //| Span    |   350.4 us | 13.60 us | 21.17 us |         - |

        [GlobalSetup]
        public void SetUp()
        {
            var random=new Random(420);
            for (var i = 0; i < _size; i++)
            {
                _list.Add(random.Next().ToString());
            }
        }

        [Benchmark]
        public string For()
        {
            var result = string.Empty;
            var size= _list.Count;
            for (var i = 0; i < size; i++)
            {
                result= _list[i];
            }
            return result;
        }

        [Benchmark]
        public string Foreach()
        {
            var result=string.Empty;
            foreach (var item in _list)
            {

                result = item;
            }

            return result;
        }

        [Benchmark]
        public string ForEach()
        {
            var result = string.Empty;
            _list.ForEach(i => result = i);
            return result;
        }

        [Benchmark]
        public string While()
        {
            var i = 0;
            var result = string.Empty;
            var size = _list.Count;
            while (i<size)
            {
                result = _list[i];
                i++;
            }
            return result;
        }

        [Benchmark]
        public string Span()
        {
            var result= string.Empty;
            var size= _list.Count;
            Span<string> span = CollectionsMarshal.AsSpan(_list);
            for (var i = 0; i < size; i++)
            {
                result= span[i];
            }

            return result;
        }

    }
}
