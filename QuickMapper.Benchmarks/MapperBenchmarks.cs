using AutoMapper;
using BenchmarkDotNet.Attributes;
using System;

namespace QuickMapper.Benchmarks
{
    public class MapperBenchmarks
    {
        private readonly IMapper _autoMapper;
        private readonly A _a;
        private readonly B _b;

        public MapperBenchmarks()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<A, B>();
            });

            _autoMapper = config.CreateMapper();

            _a = new A
            {
                Prop1 = 1,
                Prop2 = "Test",
                Prop4 = new Guid("decc0cf8-3428-495a-971f-bc88b948c6dc"),
                Prop5 = 6.00
            };

            _b = new B();
        }

        [Benchmark]
        public B QuickMap() => QuckMapper.Core.Mapper<A, B>.MapTo(_a, _b);

        [Benchmark]
        public B AutoMap() => _autoMapper.Map(_a, _b);
    }

    public class A
    {
        public int Prop1 { get; set; }
        public string Prop2 { get; set; }

        public int Prop3 => 10;

        public Guid Prop4 { get; set; }

        public double Prop5 { get; set; }
    }

    public class B
    {
        private int _prop3;

        public int Prop1 { get; set; }
        public string Prop2 { get; set; }

        public int Prop3
        {
            set => _prop3 = value;
            get => _prop3;
        }

        public Guid Prop4 { get; set; }

        public int? Prop6 { get; set; }
    }
}
