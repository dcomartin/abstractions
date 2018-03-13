using System;

namespace Unity.Abstractions.Tests.TestData
{
    // A dummy class to support testing type mapping
    public class Service : IService, IDisposable
    {
        public string ID { get; } = Guid.NewGuid().ToString();

        public bool Disposed = false;
        public void Dispose()
        {
            Disposed = true;
        }
    }
}
