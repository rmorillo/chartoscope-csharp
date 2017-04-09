using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class LookBehindPoolDerivedClassTest
    {
        [TestMethod]
        public void LookBehindPool_DerivedClass_WriteOverloadWorks()
        {
            //Arrange
            int capacity = 10;
            var pool = new DerivedTypePool(capacity);
            //Act

            pool.Write(1);

            //Assert
            Assert.IsNotNull(pool[0].Value==1);
        }

        [TestMethod]

        public void LookBehindPool_DerivedClass_SequenceLimitExceededThrowsAnException()
        {
            //Arrange
            int capacity = 200;

            var pool = new DerivedTypePool(capacity);

            for (int i = 0; i < 100; i++)
            {
                pool.Write(i);
            }

            //Act 
            Assert.ThrowsException<Exception>(() => pool.Write(0));
        }
    }
}
