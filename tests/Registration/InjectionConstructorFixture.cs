using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Unity.Policy;
using Unity.Registration;

namespace Unity.Abstractions.Tests.Registration
{
    [TestClass]
    public class InjectionConstructorFixture
    {
        private IPolicySet set; 

        [TestInitialize]
        public void Setup()
        {
            set = new TestData.TestPolicySet();
        }

        [TestMethod]
        public void Abstractions_Registration_InjectionConstructor_garbage_collected()
        {
            var param = new InjectionParameter(typeof(object));
            var ctor = new InjectionConstructor(param);
            var wr = new WeakReference(ctor);
            var pr = new WeakReference(param);

            ctor.AddPolicies(null, null, typeof(ObjectWithAmbiguousConstructors), set);

            ctor = null;
            param = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
            Assert.IsFalse(wr.IsAlive);
            Assert.IsFalse(pr.IsAlive);

            var resolver = set.Get<SelectConstructorPipeline>();
            Assert.IsNotNull(resolver);

            var selection = resolver(null, typeof(ObjectWithAmbiguousConstructors), null);
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Constructor.GetParameters().Length);
        }

        [TestMethod]
        public void Abstractions_Registration_InjectionConstructor_DefaultConstructor()
        {
            new InjectionConstructor()
                .AddPolicies(null, null, typeof(ObjectWithAmbiguousConstructors), set);

            var resolver = set.Get<SelectConstructorPipeline>();
            Assert.IsNotNull(resolver);

            var selection = resolver(null, typeof(ObjectWithAmbiguousConstructors), null);
            Assert.IsNotNull(selection);
            Assert.AreEqual(0, selection.Constructor.GetParameters().Length);
        }

        public static IEnumerable<object[]> TestMethodInputSuccess
        {
            get
            {
                return new[]
                {
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 0, null,           new object[0] },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 1, typeof(object), new [] { typeof(object) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(int),    new [] { typeof(int),    typeof(string), typeof(double) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(string), new [] { typeof(string), typeof(string), typeof(string) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(string), new [] { typeof(string), typeof(string), typeof(IUnityContainer) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(Type),   new [] { typeof(Type),   typeof(Type),   typeof(Type) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(Type),   new [] { new InjectionParameter(typeof(Type)),
                                                                                                                    new InjectionParameter(typeof(Type)),
                                                                                                                    new InjectionParameter(typeof(Type)) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(int),    new [] { new InjectionParameter(typeof(Type), typeof(int)),
                                                                                                                    new InjectionParameter(typeof(Type), typeof(string)),
                                                                                                                    new InjectionParameter(typeof(Type), typeof(double)) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(int),    new [] { new InjectionParameter(typeof(int)),
                                                                                                                    new InjectionParameter(typeof(string)),
                                                                                                                    new InjectionParameter(typeof(double)) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 1, typeof(object), new object[] { new object() } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(int),    new object[] { 1, string.Empty, (double)0 } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(string), new object[] { "1", "3", "5" } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(string), new object[] { "2", "4", typeof(IUnityContainer) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), 3, typeof(int),    new object[] { new InjectionParameter(1),
                                                                                                                          new InjectionParameter(string.Empty),
                                                                                                                          new InjectionParameter(1.1) } }
                    // TODO: Add generic cases
                };
            }
        }

        public static IEnumerable<object[]> TestMethodInputFail
        {
            get
            {
                return new[]
                {
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), new object[] { 0, typeof(string), typeof(Exception) } },
                    new object[] { null, null, typeof(ObjectWithAmbiguousConstructors), new object[] { typeof(int), typeof(string), typeof(Exception) } }
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputSuccess))]
        public void Abstractions_Registration_InjectionConstructor_success(Type registered, string name, Type implementation, int length, Type type, object[] injects)
        {
            new InjectionConstructor(injects)
                .AddPolicies(registered, name, implementation, set);

            var resolver = set.Get<SelectConstructorPipeline>();
            Assert.IsNotNull(resolver);

            var selection = resolver(null, implementation, name);
            Assert.IsNotNull(selection);

            var parameters = selection.Constructor.GetParameters();
            Assert.AreEqual(length, parameters.Length);
            if (0 < length) Assert.AreEqual(type, parameters[0].ParameterType);
        }

        [DataTestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [DynamicData(nameof(TestMethodInputFail))]
        public void Abstractions_Registration_InjectionConstructor_fail(Type registered, string name, Type implementation, object[] injects)
        {
            new InjectionConstructor(injects).AddPolicies(registered, name, implementation, set);
        }

        private class ObjectWithAmbiguousConstructors 
        {
            public ObjectWithAmbiguousConstructors() { }
            public ObjectWithAmbiguousConstructors(object first) { }
            public ObjectWithAmbiguousConstructors(int first, string second, string third) { }
            public ObjectWithAmbiguousConstructors(Type first, Type second, Type third) { }
            public ObjectWithAmbiguousConstructors(int first, string second, double third) { }
            public ObjectWithAmbiguousConstructors(string first, string second, string third) { }
            public ObjectWithAmbiguousConstructors(string first, string second, IUnityContainer third) { }
        }
    }
}
