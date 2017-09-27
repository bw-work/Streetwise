using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetwise.page_objects;

namespace Streetwise.tests
{
    [TestFixture]
    class StreetwiseRegression : CommonMethods
    {
        private string DataFile = "test_specific\\StreetwiseRegression.xls";

        [Test]
        [Category("Regression")]
        public void StreetwiseRegressionTest()
        {
            Login();
        }

        [SetUp]
        public void SetUp()
        {
            BaseSetup(DataFile);
        }

        [TearDown]
        public void TearDown()
        {
            BaseTearDown();
        }
    }
}
