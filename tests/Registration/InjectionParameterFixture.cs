using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Registration;

namespace Unity.Abstractions.Tests.Registration
{
    [TestClass]
    public class InjectionParameterFixture
    {
        public static IEnumerable<object[]> TestMethodInputSuccess
        {
            get
            {
                return new[]
                {
                    new object[] { typeof(int),              typeof(int) },
                    new object[] { 0,                        typeof(int) },
                    new object[] { new[] { 1, 2 },           (new[] { 1, 2 }).GetType() },
                    new object[] { typeof(Array),            (new[] { 1, 2 }).GetType() },
                    new object[] { typeof(Array),            typeof(Array) },
                    new object[] { typeof(IEnumerable<>),    typeof(IEnumerable<>) },
                    new object[] { typeof(IEnumerable<>),    typeof(IEnumerable<int>) },
                    new object[] { typeof(IEnumerable<>),    typeof(IEnumerable<string>) },
                    new object[] { typeof(IEnumerable<int>), typeof(IEnumerable<int>) },
                    new object[] { typeof(object),           typeof(object) }
                };
            }
        }

        public static IEnumerable<object[]> TestMethodInputFail
        {
            get
            {
                return new[]
                {
                    new object[] { typeof(IEnumerable<int>), typeof(IEnumerable<>) },
                    new object[] { typeof(IEnumerable<int>), typeof(IEnumerable<string>) },
                    new object[] { typeof(string), typeof(int) }
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputSuccess))]
        public void Abstractions_Registration_InjectionParameter_success(object injector, Type type)
        {
            Assert.IsTrue(new InjectionParameter(injector).MatchesType(type));
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputFail))]
        public void Abstractions_Registration_InjectionParameter_fail(object injector, Type type)
        {
            Assert.IsFalse(new InjectionParameter(injector).MatchesType(type));
        }
    }
}
