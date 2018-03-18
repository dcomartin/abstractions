using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Build.Pipeline;
using Unity.Build.Selected;
using Unity.Registration;

namespace Unity.Abstractions.Tests.Dependency
{
    [TestClass]
    public class SelectedMemberWithParametersFixture
    {
        #region Setup

        private static IDictionary<Type, object> _values = new Dictionary<Type, object> { { typeof(int[]),                          new int[0] },
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
                                                                                          { typeof(ConstructorInfo),                typeof(TestMember).GetTypeInfo().DeclaredConstructors.ElementAt(0) },
                                                                                          { typeof(int),                            0 }};

        public static IEnumerable<object[]> TestMethodInput
        {
            get
            {
                yield return new object[] { 01, typeof(TestMember),     typeof(TestMember),                              0, new object[] { typeof(ConstructorInfo) } };

                yield return new object[] { 11, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 15, new object[] { typeof(int[]), typeof(GenClass<int, string>[][]) } };
                yield return new object[] { 12, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 14, new object[] { typeof(int[]), typeof(int[]), typeof(GenClass<string, Delegate>[][]) } };
                yield return new object[] { 13, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 13, new object[] { typeof(GenClass<string, Delegate>[][]) } };
                yield return new object[] { 14, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 12, new object[] { typeof(int[]), typeof(string[][]), typeof(Type[][][]) } };
                yield return new object[] { 15, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 11, new object[] { typeof(int[]), typeof(Type[]) } };
                yield return new object[] { 16, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 10, new object[] { typeof(GenClass<int, string>) } };
                yield return new object[] { 17, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  9, new object[] { typeof(int), typeof(GenClass<int, Delegate>) } };
                yield return new object[] { 18, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  8, new object[] { typeof(GenClass<string, Delegate>) } };
                yield return new object[] { 19, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  1, new object[] { typeof(int) } };
                                            
                yield return new object[] { 21, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 15, new object[] { _values[typeof(int[])], _values[typeof(GenClass<int, string>[][])] } };
                yield return new object[] { 22, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 14, new object[] { _values[typeof(int[])], _values[typeof(int[])], _values[typeof(GenClass<string, Delegate>[][])] } };
                yield return new object[] { 23, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 13, new object[] { _values[typeof(GenClass<string, Delegate>[][])] } };
                yield return new object[] { 24, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 12, new object[] { _values[typeof(int[])], _values[typeof(string[][])], _values[typeof(Type[][][])] } };
                yield return new object[] { 25, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 11, new object[] { _values[typeof(int[])], _values[typeof(Type[])] } };
                yield return new object[] { 26, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 10, new object[] { _values[typeof(GenClass<int, string>)] } };
                yield return new object[] { 27, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  9, new object[] { _values[typeof(int)], _values[typeof(GenClass<int, Delegate>)] } };
                yield return new object[] { 28, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  8, new object[] { _values[typeof(GenClass<string, Delegate>)] } };
                yield return new object[] { 29, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  1, new object[] { _values[typeof(int)] } };

                yield return new object[] { 41, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 15, new object[] { new InjectionParameter( typeof(int[]), _values[typeof(int[])]),
                                                                                                                                           new InjectionParameter( typeof(GenClass<int, string>[][]), 
                                                                                                                                                                   _values[typeof(GenClass<int, string>[][])]) } };
                yield return new object[] { 42, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 14, new object[] { new InjectionParameter( typeof(int[]), _values[typeof(int[])]),
                                                                                                                                           new InjectionParameter( typeof(int[]), _values[typeof(int[])]),
                                                                                                                                           new InjectionParameter( typeof(GenClass<string, Delegate>[][]), 
                                                                                                                                                                   _values[typeof(GenClass<string, Delegate>[][])]) } };
                yield return new object[] { 43, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 13, new object[] { new InjectionParameter( typeof(GenClass<string, Delegate>[][]), 
                                                                                                                                                                   _values[typeof(GenClass<string, Delegate>[][])]) } };
                yield return new object[] { 44, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 12, new object[] { new InjectionParameter( typeof(int[]), _values[typeof(int[])]),
                                                                                                                                           new InjectionParameter( typeof(string[][]), _values[typeof(string[][])]),
                                                                                                                                           new InjectionParameter( typeof(Type[][][]), _values[typeof(Type[][][])]) } };
                yield return new object[] { 45, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 11, new object[] { new InjectionParameter( typeof(int[]), _values[typeof(int[])]),
                                                                                                                                           new InjectionParameter( typeof(Type[]), _values[typeof(Type[])]) } };
                yield return new object[] { 46, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>), 10, new object[] { new InjectionParameter( typeof(GenClass<int, string>), 
                                                                                                                                                                   _values[typeof(GenClass<int, string>)]) } };
                yield return new object[] { 47, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  9, new object[] { new InjectionParameter( typeof(int), _values[typeof(int)]),
                                                                                                                                           new InjectionParameter( typeof(GenClass<int, Delegate>), 
                                                                                                                                                                   _values[typeof(GenClass<int, Delegate>)]) } };
                yield return new object[] { 48, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  8, new object[] { new InjectionParameter( typeof(GenClass<string, Delegate>), 
                                                                                                                                                                   _values[typeof(GenClass<string, Delegate>)]) } };
                yield return new object[] { 49, typeof(TestClass<,,,>), typeof(TestClass<int, string, Delegate, Type>),  1, new object[] { new InjectionParameter( typeof(int), _values[typeof(int)]) } };

                yield return new object[] { 00, typeof(object), typeof(object), 0, new object[0] };
            }
        }

        #endregion


        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        [Ignore]
        public void Abstractions_Dependency_SelectedMember_type(int test, Type registerType, Type resolveType, int index, object[] args)
        {
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(index);
            var member = new TestMember(ctor);

            //var pipeline = member.ResolvePipeline(resolveType);
            //var result = (object[])pipeline(MockDependencyResolver);

            //var parameters = resolveType.GetTypeInfo().DeclaredConstructors.ElementAt(index).GetParameters();
            //for (var i = 0; i < parameters.Length; i++)
            //    Assert.AreEqual(((Tuple<Type, string>)result[i]).Item1, parameters[i].ParameterType);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestMethodInput))]
        [Ignore]
        public void Abstractions_Dependency_SelectedMember_objects(int test, Type registerType, Type resolveType, int index, object[] args)
        {
            var ctor = registerType.GetTypeInfo().DeclaredConstructors.ElementAt(index);
            var memeber = new TestMember(ctor, args);
            var pipeline = memeber.ResolvePipeline(resolveType);
            //var result = (object[])pipeline(MockDictionaryResolver);

            //var parameters = resolveType.GetTypeInfo().DeclaredConstructors.ElementAt(index).GetParameters();
            //for (var i = 0; i < parameters.Length; i++)
            //{
            //    var arg = args[i];
            //    switch (result[i])
            //    {
            //        case Tuple<Type, string> tuple:
            //            Assert.AreEqual(tuple.Item1, arg is Type ? arg : arg.GetType());
            //            break;

            //        case Type type:
            //            Assert.AreEqual(type, arg is Type ? arg : arg.GetType());
            //            break;

            //        default:
            //            Assert.AreEqual(result[i].GetType(), 
            //                            arg is Type ? arg : arg is InjectionParameter injectionParameter 
            //                                        ? injectionParameter.ParameterType : arg.GetType());
            //            break;
            //    }

            //}
        }


        #region Test Data

        private class TestMember : SelectedMemberWithParameters<ConstructorInfo>
        {
            public TestMember(ConstructorInfo ctor)
                : base(ctor, ctor.GetParameters())
            { }

            public TestMember(ConstructorInfo ctor, object[] args)
                : base(ctor, ctor.GetParameters(), args)
            { }

            public Resolve ResolvePipeline(Type type) => CreateResolver(type);
        }

        private class TestClass<A, B, C, D>
        {
            // Simple types                                            // Index
            public TestClass() { }                                     //  0
            public TestClass(A a) { }                                  //  1
            public TestClass(B b) { }                                  //  2
            public TestClass(C c) { }                                  //  3
            public TestClass(D d) { }                                  //  4
            public TestClass(A a, B b) { }                             //  5
            public TestClass(A a, B b, C c) { }                        //  6
            public TestClass(A a, B b, C c, D d) { }                   //  7
            public TestClass(GenClass<B, C> c) { }                     //  8
            public TestClass(A a, GenClass<int, C> c) { }              //  9
            public TestClass(GenClass<int, string> c) { }              // 10
            public TestClass(A[] a, D[] d) { }                         // 11
            public TestClass(A[] a, B[][] b, D[][][] d) { }            // 12
            public TestClass(GenClass<B, C>[][] b) { }                 // 13
            public TestClass(int[] i, A[] a, GenClass<B, C>[][] b) { } // 14
            public TestClass(A[] a, GenClass<int, string>[][] b) { }   // 15
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
