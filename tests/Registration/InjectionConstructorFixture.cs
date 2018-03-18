using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Build.Pipeline;
using Unity.Policy;
using Unity.Registration;

// ReSharper disable RedundantAssignment
// ReSharper disable RedundantExplicitArrayCreation

namespace Unity.Abstractions.Tests.Registration
{
    [TestClass]
    public class InjectionConstructorFixture
    {
        #region Setup

        private IPolicySet _set;

        [TestInitialize]
        public void Setup() { _set = new TestData.TestPolicySet(); }

        public static IEnumerable<object[]> TestMethodInput
        {
            get
            {
                yield return new object[] { typeof(TestClass), 0, null };
                yield return new object[] { typeof(TestClass), 0, new object[0] };

                yield return new object[] { typeof(TestClass), 1, new object[] { typeof(object) } };
                yield return new object[] { typeof(TestClass), 2, new object[] { typeof(int), typeof(string), typeof(string) } };
                yield return new object[] { typeof(TestClass), 3, new object[] { typeof(Type), typeof(Type), typeof(Type) } };
                yield return new object[] { typeof(TestClass), 4, new object[] { typeof(int), typeof(string), typeof(double) } };
                yield return new object[] { typeof(TestClass), 5, new object[] { typeof(string), typeof(string), typeof(string) } };
                yield return new object[] { typeof(TestClass), 6, new object[] { typeof(string), typeof(string), typeof(IUnityContainer) } };

                yield return new object[] { typeof(TestClass), 1, new object[] { new object() } };
                yield return new object[] { typeof(TestClass), 2, new object[] {   1, "0", "1" } };
                yield return new object[] { typeof(TestClass), 4, new object[] {   2, "2", 1.1 } };
                yield return new object[] { typeof(TestClass), 5, new object[] { "2", "3",  "" } };

                //yield return new object[] { typeof(TestClass<,,,>), 3, new[] { new InjectionParameter(typeof(Type)), new InjectionParameter(typeof(Type)), new InjectionParameter(typeof(Type)) } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new[] { new InjectionParameter(typeof(Type), typeof(int)), new InjectionParameter(typeof(Type), typeof(string)), new InjectionParameter(typeof(Type), typeof(double)) } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new[] { new InjectionParameter(typeof(int)), new InjectionParameter(typeof(string)), new InjectionParameter(typeof(double)) } };
                //yield return new object[] { typeof(TestClass<,,,>), 1, new object[] { new object() } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new object[] { 1, string.Empty, (double)0 } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new object[] { "1", "3", "5" } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new object[] { "2", "4", typeof(IUnityContainer) } };
                //yield return new object[] { typeof(TestClass<,,,>), 3, new object[] { new InjectionParameter(1), new InjectionParameter(string.Empty), new InjectionParameter(1.1) } };
                // TODO: Add generic cases
            }
        }

        public static IEnumerable<object[]> TestMethodInputFail
        {
            get
            {
                yield return new object[] { typeof(TestClass), new object[] { typeof(int), typeof(int), typeof(Type) } };
                yield return new object[] { typeof(TestClass), new object[] { "3", "4", null } };
            }
        }

        #endregion

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        public void Abstractions_Registration_InjectionConstructor(Type type, int index, object[] injects)
        {
            new InjectionConstructor(injects).AddPolicies(type, null, null, _set);

            var selectConstructorPipeline = _set.Get<SelectConstructor>();
            Assert.IsNotNull(selectConstructorPipeline);

            var selectedConstructor = selectConstructorPipeline(type);
            Assert.IsNotNull(selectedConstructor);

            var ctor = type.GetTypeInfo().DeclaredConstructors.ElementAt(index);
            Assert.AreEqual(ctor, selectedConstructor.Constructor);
        }

        [DataTestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [DynamicData(nameof(TestMethodInputFail))]
        public void Abstractions_Registration_InjectionConstructor_fail(Type implementation, object[] injects)
        {
            new InjectionConstructor(injects).AddPolicies(null, null, implementation, _set);
        }

        [TestMethod]
        public void Abstractions_Registration_InjectionConstructor_garbage_collected()
        {
            var param = new InjectionParameter(typeof(object));
            var ctor = new InjectionConstructor(param);

            var wr = new WeakReference(ctor);
            var pr = new WeakReference(param);

            ctor.AddPolicies(typeof(TestClass), null, null, _set);

            ctor = null;
            param = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            Assert.IsFalse(wr.IsAlive);
            Assert.IsFalse(pr.IsAlive);

            var resolver = _set.Get<SelectConstructor>();
            Assert.IsNotNull(resolver);

            var selection = resolver(typeof(TestClass));
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Constructor.GetParameters().Length);
        }

        [TestMethod]
        public void Abstractions_Registration_InjectionConstructor_DefaultConstructor()
        {
            new InjectionConstructor()
                .AddPolicies(typeof(TestClass), null, null, _set);

            var resolver = _set.Get<SelectConstructor>();
            Assert.IsNotNull(resolver);

            var selection = resolver(typeof(TestClass));
            Assert.IsNotNull(selection);
            Assert.AreEqual(0, selection.Constructor.GetParameters().Length);
        }


        #region Test Data

        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local

        private class TestClass
        {
            public TestClass() { }                                                    // 0
            public TestClass(object first) { }                                        // 1
            public TestClass(int first, string second, string third) { }              // 2
            public TestClass(Type first, Type second, Type third) { }                 // 3
            public TestClass(int first, string second, double third) { }              // 4
            public TestClass(string first, string second, string third) { }           // 5
            public TestClass(string first, string second, IUnityContainer third) { }  // 6
        }

        public class GenericInjectionTestClass<TA, TB, TC>
        {
            public TB CollectionName { get; }

            public GenericDependencyClass<TA, TC> GenDependency { get; }

            public GenericInjectionTestClass(TB name)
            {
                CollectionName = name;
            }

            public GenericInjectionTestClass(GenericDependencyClass<TA, TC> genericDependency)
            {
                GenDependency = genericDependency;
            }
        }


        public class GenericDependencyClass<TT, TV>
        {
            public GenericDependencyClass(TT t, TV v)
            {
                ItemT = t;
                ItemV = v;
            }       

            public TT ItemT { get; }

            public TV ItemV { get; }
        }

        #endregion

        /* RegisterType(null, typeof(GenericInjectionTestClass<,,>), null, null, new InjectionConstructor(typeof(GenericDependencyClass<,>) */

    }
}
