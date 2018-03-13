﻿using System;
using Unity.Attributes;

namespace Unity.Abstractions.Tests.TestData
{
    public class ObjectWithAmbiguousConstructors : IService
    {
        public const string One =   "1";
        public const string Two =   "2";
        public const string Three = "3";
        public const string Four =  "4";
        public const string Five =  "5";

        public string Signature { get; }

        public ObjectWithAmbiguousConstructors()
        {
            Signature = One;
        }

        public ObjectWithAmbiguousConstructors(object first)
        {
            Signature = Two;
        }

        public ObjectWithAmbiguousConstructors(int first, string second, string third)
        {
            Signature = Three;
        }

        public ObjectWithAmbiguousConstructors(Type first, Type second, Type third)
        {
            Signature = Four;
        }

        public ObjectWithAmbiguousConstructors(int first, string second, double third)
        {
            Signature = Five;
        }

        public ObjectWithAmbiguousConstructors(string first, string second, string third)
        {
            Signature = first;
        }

        public ObjectWithAmbiguousConstructors(string first, [Dependency(Five)]string second, IUnityContainer third)
        {
            Signature = second;
        }
    }
}
