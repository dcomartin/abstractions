using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Build.Context;
using Unity.Build.Selected;
using Unity.Registration;

namespace Unity.Abstractions.Tests.Build.Selected
{
    [TestClass]
    public class SelectedMethodFixture
    {
        #region Setup

        private static readonly IDictionary<Type, object> _values =
            new Dictionary<Type, object> { { typeof(int[]),      new int[0] },
                                           { typeof(GenClass<int, string>[][]),      new GenClass<int, string>[0][] },
                                           { typeof(GenClass<string, Delegate>[][]), new GenClass<string, Delegate>[0][] },
                                           { typeof(GenClass<string, Delegate>),     new GenClass<string, Delegate>() },
                                           { typeof(GenClass<int, string>),          new GenClass<int, string>() },
                                           { typeof(GenClass<int, Delegate>),        new GenClass<int, Delegate>() },
                                           { typeof(string[][]),                     new string[0][] },
                                           { typeof(Type[][][]),                     new Type[0][][] },
                                           { typeof(Type),                           typeof(Type) },
                                           { typeof(Type[]),                         new Type[0] },
                                           { typeof(string),                         string.Empty },
                                           { typeof(Delegate),                       new Action(() => { } ) },
                                           { typeof(int),                            0 }};
        public static IEnumerable<object[]> TestMethodInputData
        {
            get
            {
                yield return new object[] { 00, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 00 };
                yield return new object[] { 01, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 01 };
                yield return new object[] { 02, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 02 };
                yield return new object[] { 03, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 03 };
                yield return new object[] { 04, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 04 };
                yield return new object[] { 05, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 05 };
                yield return new object[] { 06, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 06 };
                yield return new object[] { 07, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 07 };
                yield return new object[] { 08, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 08 };
                yield return new object[] { 09, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 09 };
                yield return new object[] { 10, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 10 };
                yield return new object[] { 11, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 11 };
                yield return new object[] { 12, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 12 };
                yield return new object[] { 13, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 13 };

                yield return new object[] { 20, typeof(TestClass<,>), typeof(TestClass<int, string>), 1 };
                yield return new object[] { 21, typeof(TestClass<,>), typeof(TestClass<int, string>), 2 };
                yield return new object[] { 22, typeof(TestClass<,>), typeof(TestClass<int, string>), 3 };
                yield return new object[] { 23, typeof(TestClass<,>), typeof(TestClass<int, string>), 0 };
            }
        }


        #endregion


        #region Tests

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputData))]
        public void Abstractions_Build_Selected_SelectedMethod(int test, Type registerType, Type resolveType, int position)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => _values[t], Existing = Activator.CreateInstance(resolveType) };

            var methodInfo = registerType.GetTypeInfo().DeclaredMethods.ElementAt(position);
            var selectedMethod = new SelectedMethod(methodInfo);
            var resolvePipeline = selectedMethod.ResolveMethodFactory(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.AreEqual(resolvePipeline(ref context), position);
        }

        [TestMethod]
        public void Abstractions_Build_Selected_SelectedMethod_garbage_collected()
        {
            var methodInfo = typeof(TestClass<int, string>).GetTypeInfo().DeclaredMethods.ElementAt(1);
            var selectedMethod = new SelectedMethod(methodInfo);

            var ctorRef = new WeakReference(methodInfo);
            var cnstRef = new WeakReference(selectedMethod);

            var resolver = selectedMethod.ResolveMethodFactory(typeof(TestClass<int, string>));
            Assert.IsNotNull(resolver);

            methodInfo = null;
            selectedMethod = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

            Assert.IsFalse(cnstRef.IsAlive);
            Assert.IsTrue(ctorRef.IsAlive);

            resolver = null;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            Assert.IsFalse(ctorRef.IsAlive);
        }

        #endregion


        #region Test Data

        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local

        private class TestClass<TA, TB>
        {
            // Simple types                          
            public int Test0() => 0;
            public int Test1(TA a) => 1;
            public int Test2(TB b) => 2;
            public int Test3(TA a, TB b) => 3;

        }

        private class TestClass<A, B, C, D>
        {
            // Simple types                         
            public int TestMethod() => 0;
            public int TestMethod(A a) => 1;
            public int TestMethod(B b) => 2;
            public int TestMethod(A a, B b) => 3;
            public int TestMethod(A a, B b, C c) => 4;
            public int TestMethod(A a, B b, C c, D d) => 5;
            public int TestMethod(GenClass<B, C> c) => 6;
            public int TestMethod(A a, GenClass<int, C> c) => 7;
            public int TestMethod(GenClass<int, string> c) => 8;
            public int TestMethod(A[] a, D[] d) => 9;
            public int TestMethod(A[] a, B[][] b, D[][][] d) => 10;
            public int TestMethod(GenClass<B, C>[][] b) => 11;
            public int TestMethod(int[] i, A[] a, GenClass<B, C>[][] b) => 12;
            public int TestMethod(A[] a, GenClass<int, string>[][] b) => 13;
        }

        private class GenClass<T, V>
        {
            // Simple types                         // Index
            public GenClass() { }                   //  0
            public GenClass(int i) { }              //  1
            public GenClass(int i, string s) { }    //  2
            public GenClass(T t) { }                //  3
            public GenClass(V v) { }                //  4
            public GenClass(T t, V v) { }           //  5
        }

        #endregion
    }
}
