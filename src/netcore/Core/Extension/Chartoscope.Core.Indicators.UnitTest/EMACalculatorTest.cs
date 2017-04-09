using Chartoscope.Core.Indicators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators.UnitTest
{
    [TestClass]
    public class EMACalculatorTest
    {
        [TestMethod]
        public void EMACalculator_Constructor_DoesNotRaiseAnException()
        {
            new EMACalculator(10, 3);
        }

        [TestMethod]
        public void EMACalculator_CalculatesCorrectly()
        {
            //Arrange
            var ema= new EMACalculator(10, 3);

            ema.Calculate(10);
            ema.Calculate(10);

            //Act
            var actual= ema.Calculate(10);

            //Assert
            Assert.AreEqual(1, ema.Length);
            Assert.AreEqual(10, actual);
        }
    }
}
