using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Build.Context;
using Unity.Build.Selected;

namespace Unity.Abstractions.Tests.Build.Selected
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
                yield return new object[] { 6, typeof(TestClass<Type, string, Delegate, int>), typeof(TestClass<Type, string, Delegate, int>), 1, new object[] { typeof(int) } };
                yield return new object[] { 5, typeof(TestClass<,,,>), typeof(TestClass<Type, string, Delegate, int>), 1, new object[] { typeof(int) } };
                yield return new object[] { 4, typeof(TestClass<int, string, Delegate, Type>), typeof(TestClass<int, string, Delegate, Type>), 1, new object[0] };
                yield return new object[] { 3, typeof(TestClass<int, string, Delegate, Type>), typeof(TestClass<int, string, Delegate, Type>), 1, new object[] { typeof(int) } };
                yield return new object[] { 2, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 1, new object[] { typeof(int) }  };
                yield return new object[] { 1, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 0, new object[0]};
                yield return new object[] { 0, typeof(object), typeof(object), 0, new object[0]};
            }
        }

        public static IEnumerable<object[]> TestMethodFailInput
        {
            get
            {
                yield return new object[] { 1, typeof(TestClass<,,,>), typeof(TestClass<int, string, object, Type>),   1, new object[] { typeof(int) } };
                yield return new object[] { 0, typeof(TestClass<,,,>), typeof(TestClass<int, string, object, Type>),   1, new object[] { 0 } };
            }
        }

        #endregion


        #region Tests


        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Build_Selected_SelectedConstrucotr(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => Values[t] };
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            var constructor = new SelectedConstructor(ctor);
            var resolvePipeline = constructor.ResolveMethodFactory(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.IsInstanceOfType(resolvePipeline(ref context), resolveType);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Build_Selected_SelectedConstrucotr_null(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => Values[t] };
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            var constructor = 0 == args.Length
                            ? new SelectedConstructor(ctor, null)
                            : new SelectedConstructor(ctor, args);
            var resolvePipeline = constructor.ResolveMethodFactory(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.IsInstanceOfType(resolvePipeline(ref context), resolveType);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Build_Selected_SelectedConstrucotr_args(int test, Type registerType, Type resolveType, int ctorPosition, object[] args)
        {
            ResolutionContext context = new ResolutionContext { Resolve = (t, n) => Values[t] };
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(ctorPosition);
            var constructor = new SelectedConstructor(ctor, args);
            var resolvePipeline = constructor.ResolveMethodFactory(resolveType);

            Assert.IsNotNull(resolvePipeline);
            Assert.IsInstanceOfType(resolvePipeline(ref context), resolveType);
        }

        [TestMethod]
        public void Abstractions_Build_Selected_SelectedConstrucotr_garbage_collected()
        {
            var ctor = typeof(TestClass<int, string, Delegate, Type>).GetTypeInfo().DeclaredConstructors.ElementAt(0);
            var constructor = new SelectedConstructor(ctor);

            var ctorRef = new WeakReference(ctor);
            var cnstRef = new WeakReference(constructor);

            var resolver = constructor.ResolveMethodFactory(typeof(TestClass<int, string, Delegate, Type>));
            Assert.IsNotNull(resolver);

            ctor = null;
            constructor = null;
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
