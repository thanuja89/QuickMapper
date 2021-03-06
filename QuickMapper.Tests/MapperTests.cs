﻿using System;
using QuickMapper.Core;
using Xunit;

namespace QuickMapper.Tests
{
    public class MapperTests
    {
        private readonly A _a;
        private readonly B _b;
        private readonly C _c;
        private readonly D _d;

        public MapperTests()
        {
            _a = new A
            {
                Prop1 = 1,
                Prop2 = "Test",
                Prop4 = new Guid("decc0cf8-3428-495a-971f-bc88b948c6dc"),
                Prop5 = 6.00
            };

            _b = new B();

            _c = new C
            {
                F1 = 1,
                F2 = "Test"
            };

            _d = new D();
        }

        [Fact]
        public void Map_WhenCalled_MapsAllMatchingProperties()
        {
            Mapper<A, B>.MapTo(_a, _b);

            Assert.Equal(_a.Prop1, _b.Prop1);
            Assert.Equal(_a.Prop2, _b.Prop2);
            Assert.Equal(10, _b.Prop3);
            Assert.Equal(_a.Prop4, _b.Prop4);
        }

        [Fact]
        public void Map_WhenCalled_IgnoresNotMatchingProperties()
        {
            Mapper<A, B>.MapTo(_a, _b);

            Assert.Null(_b.Prop6);
        }

        [Fact]
        public void Map_WhenCalled_MapsAllMatchingFields()
        {
            Mapper<C, D>.MapTo(_c, _d);

            Assert.Equal(_c.F1, _d.F1);
            Assert.Equal(_c.F2, _d.F2);
        }

        [Fact]
        public void Map_WhenCalled_ShouldThrowIfAnyArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Mapper<A, B>.MapTo(null, _b));
            Assert.Throws<ArgumentNullException>(() => Mapper<A, B>.MapTo(_a, null));
        }
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

    public class C
    {
        public int F1;
        public string F2;
    }


    public class D
    {
        public int F1;
        public string F2;
    }
}
