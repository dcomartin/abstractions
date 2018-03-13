using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Abstractions.Tests.TestData;
using Unity.Dependency;
using Unity.Policy;

namespace Unity.Abstractions.Tests.Dependency
{
    [TestClass]
    public class SelectedConstrucotrFixture
    {
        private IPolicySet set;

        [TestInitialize]
        public void Setup()
        {
            set = new TestPolicySet();
        }

        [TestMethod]
        [Ignore]
        public void Abstractions_Dependency_SelectedConstrucotr_garbage_collected()
        {
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        public void Abstractions_Dependency_SelectedConstrucotr_default(int value)
        {
            var ss = 1;
        }

    }
}
