using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrleansWireSerializer;
namespace OrleansWireSerializer.Tests
{
    public struct AStruct
    {
        public string AStringField;
        public int AnIntField;
    }

    public class AClass
    {
        public string AStringField;
        public int AnIntField;
        public string AStringProp { get; set; }
        public int AnIntProp { get; set; }
    }


    public class A
    {
        public B B { get; set; }
        public string AStringProp { get; set; }
    }

    public class B
    {
        public A A { get; set; }
        public int AnIntProp { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanDeepCopyStruct()
        {
            var astruct = new AStruct()
            {
                AStringField = "hello",
                AnIntField = 123,
            };
            var serializer = new OrleansWireSerializer();
            var res = (AStruct)serializer.DeepCopy(astruct);
            Assert.AreEqual(astruct.AnIntField, res.AnIntField);
            Assert.AreEqual(astruct.AStringField, res.AStringField);
        }

        [TestMethod]
        public void CanDeepCopyClass()
        {
            var astruct = new AClass()
            {
                AStringField = "hello",
                AnIntField = 123,
                AStringProp = "you",
                AnIntProp = 456
            };
            var serializer = new OrleansWireSerializer();
            var res = (AClass)serializer.DeepCopy(astruct);
            Assert.AreEqual(astruct.AnIntField, res.AnIntField);
            Assert.AreEqual(astruct.AStringField, res.AStringField);
            Assert.AreEqual(astruct.AnIntProp, res.AnIntProp);
            Assert.AreEqual(astruct.AStringProp, res.AStringProp);
        }

        [TestMethod]
        public void CanDeepCopyCyclicClass()
        {
            var a = new A
            {
                B = new B
                {
                    AnIntProp = 123
                },
                AStringProp = "hello"
            };

            //make cyclic
            a.B.A = a;

            var serializer = new OrleansWireSerializer();
            var res = (A)serializer.DeepCopy(a);
            Assert.AreEqual(a.AStringProp,res.AStringProp);
            Assert.AreEqual(a.B.AnIntProp, res.B.AnIntProp);
            Assert.AreSame(res, res.B.A);
        }
    }
}
