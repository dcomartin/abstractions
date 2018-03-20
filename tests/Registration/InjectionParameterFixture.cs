using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Dependency;
using Unity.Registration;

namespace Unity.Abstractions.Tests.Registration
{
    [TestClass]
    public class InjectionParameterFixture
    {
        #region Setup

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

        #endregion

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputSuccess))]
        public void Abstractions_Registration_InjectionParameter(object match, Type type)
        {
            Assert.IsTrue(new InjectionParameter(match).MatchesType(type));
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputFail))]
        public void Abstractions_Registration_InjectionParameter_fail(object match, Type type)
        {
            Assert.IsFalse(new InjectionParameter(match).MatchesType(type));
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputSuccess))]
        public void Abstractions_Registration_InjectionParameter_TypeMatching(object match, Type type)
        {
            Assert.IsTrue(new TestMember().Match(match, type));
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInputFail))]
        public void Abstractions_Registration_InjectionParameter_TypeMatching_fail(object match, Type type)
        {
            Assert.IsFalse(new TestMember().Match(match, type));
        }

        #region Test Data

        private class TestMember : InjectionMemberWithParameters<ConstructorInfo>
        {
            public TestMember(params object[] args)
                : base(args)
            {
            }

            protected override string NoMemberFound => throw new NotImplementedException();

            protected override string MultipleFound => throw new NotImplementedException();

            public bool Match(object obj, Type type) => Matches(obj, type);

            protected override IEnumerable<ConstructorInfo> GetMemberInfos(Type type)
            {
                throw new NotImplementedException();
            }

            protected override ParameterInfo[] GetParameters(ConstructorInfo memberInfo)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
