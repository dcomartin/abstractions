using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Build.Context;
using Unity.Build.Selected;

namespace Unity.Abstractions.Tests.Dependency
{
    [TestClass]
    public class SelectedConstrucotrFixture
    {
        #region Setup

        private static readonly IDictionary<Type, object> Values = 
            new Dictionary<Type, object> { { typeof(int[]),      new int[0] },
                                           { typeof(string[][]), new string[0][] },
                                           { typeof(Type[][][]), new Type[0][][] },
                                           { typeof(Type),       typeof(Type) },
                                           { typeof(Type[]),     new Type[0] },
                                           { typeof(string),     string.Empty },
                                           { typeof(Delegate),   new Action(() => { } ) },
                                           { typeof(int),        0 }};
        public static IEnumerable<object[]> TestMethodInput
        {
            get
            {
                yield return new object[] { 4, typeof(TestClass<int, string, Delegate, Type>),
                                               typeof(TestClass<int, string, Delegate, Type>), 1, new object[0] };
                yield return new object[] { 3, typeof(TestClass<int, string, Delegate, Type>),
                                               typeof(TestClass<int, string, Delegate, Type>), 1, new object[] { typeof(int) } };
                yield return new object[] { 2, typeof(TestClass<,,,>),
                                               typeof(TestClass<int, string, Delegate, Type>), 1, new object[] { typeof(int) }  };
                yield return new object[] { 1, typeof(TestClass<,,,>),
                                               typeof(TestClass<int, string, Delegate, Type>), 0, new object[0]};
                yield return new object[] { 0, typeof(object),
                                               typeof(object),                                 0, new object[0]};
            }
        }

        public static IEnumerable<object[]> TestMethodFailInput
        {
            get
            {
                yield return new object[] { 1, typeof(TestClass<,,,>),
                                               typeof(TestClass<int, string, object, Type>),   1, new object[] { typeof(int) } };
                yield return new object[] { 0, typeof(TestClass<,,,>),
                                               typeof(TestClass<int, string, object, Type>),   1, new object[] { 0 } };
            }
        }

        #endregion



        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Dependency_SelectedConstrucotr(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => Values[t] };
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            var constructor = new SelectedConstructor(ctor);
            var resolvePipeline = constructor.CreateResolver(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.IsInstanceOfType(resolvePipeline(ref context), resolveType);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Dependency_SelectedConstrucotr_index(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => Values[t] };
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            var constructor = 0 == args.Length 
                            ? new SelectedConstructor(ctor) 
                            : new SelectedConstructor(ctor, args);
            var resolvePipeline = constructor.CreateResolver(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.IsInstanceOfType(resolvePipeline(ref context), resolveType);
        }

        [DataTestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [DynamicData(nameof(TestMethodFailInput))]
        [Ignore]
        public void Abstractions_Dependency_SelectedConstrucotr_fail(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            //var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            //var constructor = 0 == args.Length
            //                ? new SelectedConstructor(ctor)
            //                : new SelectedConstructor(ctor, args);
            //var resolvePipeline = constructor.CreateResolver(resolveType);

            //Assert.IsNotNull(resolvePipeline);
            //Assert.IsInstanceOfType(resolvePipeline(MockDictionaryResolver), resolveType);
        }


        #region Test Data

        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local

        private class TestClass<TA, TB, TC, TD>
        {
            // Simple types                              // Index
            public TestClass() { }                        //  0
            public TestClass(TA a) { }                    //  1
            public TestClass(TB b) { }                    //  2
            public TestClass(TC c) { }                    //  3
            public TestClass(TD d) { }                    //  4
            public TestClass(TA a, TB b) { }              //  5
            public TestClass(TA a, TB b, TC c) { }        //  6
            public TestClass(TA a, TB b, TC c, TD d) { }  //  7

        }

        #endregion
    }
}
