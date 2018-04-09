using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Build.Parameters;
using Unity.Container;

// ReSharper disable RedundantAssignment
// ReSharper disable RedundantExplicitArrayCreation

namespace Unity.Abstractions.Tests.Build.Parameters
{
    [TestClass]
    public class ParameterFactoryFixture
    {
        #region Setup

        public static IEnumerable<object[]> TestDataSource
        {
            get
            {
                //                        test,                  ParameterInfo        resolveType  expectedType       Name
                yield return new object[] { 4, typeof(PocoClass).ParameterInfo(1, 1), null,        typeof(int[][][]), null };
                yield return new object[] { 3, typeof(PocoClass).ParameterInfo(1, 0), null,        typeof(string[]),  null };
                yield return new object[] { 2, typeof(PocoClass).ParameterInfo(0, 2), null,        typeof(object),    null };
                yield return new object[] { 1, typeof(PocoClass).ParameterInfo(0, 1), null,        typeof(int),       null };
                yield return new object[] { 0, typeof(PocoClass).ParameterInfo(0, 0), null,        typeof(string),    null };

                //yield return new object[] { 0,                            typeof(GenClass<,,>).ParameterInfo(0, 0), typeof(object), null };
                //yield return new object[] { 0, typeof(GenClass<string,string,IUnityContainer>).ParameterInfo(0, 0), typeof(object), null };
            }
        }

        #endregion


        #region Tests


        [DataTestMethod]
        [DynamicData(nameof(TestDataSource))]
        public void Abstractions_Build_Parameters_ParameterFactory_Type_Name(int test, ParameterInfo parameter, Type resolveType, Type expectedType, string expectedName)
        {
            // Setup
            ResolveContext context = new ResolveContext
            {
                Resolve = (Type t, string n) => new Tuple<Type, string>(t, n)
            };

            // Act
            var factory = parameter.ToFactory();

            // Verify
            Assert.IsNotNull(factory);
            var resolver = factory(resolveType);
            Assert.IsNotNull(resolver);
            var result = (Tuple<Type, string>)resolver(ref context);
            Assert.AreEqual(expectedType, result.Item1);
            Assert.AreEqual(expectedName, result.Item2);
        }

        #endregion


        #region Test Data

        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedParameter.Local

        private class PocoClass
        {
            public PocoClass(string a, int b, object c) { }  // 0
            public PocoClass(string[] a, int[][][] b) { }    // 1
        }

        private class GenClass<TA, TB, TC>
        {
            public GenClass(TA a, TB b, TC c) { }  // 0
        }

        #endregion
    }

    #region Helpers

    internal static class ParameterFactoryFixtureExtensions
    {
        internal static ParameterInfo ParameterInfo(this Type type, int constructor, int parameter)
        {
            return type.GetTypeInfo().GetConstructors()[constructor].GetParameters()[parameter];
        }
    }

    #endregion
}
