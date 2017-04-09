using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.IntegrationTest
{
    [TestClass]
    public class FilePersistenceTest
    {
        [TestMethod]
        public void CandlestickBar_Persistence_Works()
        {
            var destinationFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var filePersistence = new FilePersistenceService();

            var fileWriter = filePersistence.CreateFileWriter(destinationFile);

            //Arrange
            var candlesticks = new CandlestickBars(100, fileWriter);

            //Action
            candlesticks.Write(DateTime.Now.Ticks, 1, 2, 3, 4);

            fileWriter.Close();

            //Assert
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.IsTrue(new FileInfo(destinationFile).Length > 0);           
        }

        [TestMethod]
        public void HeikenAshiBar_Persistence_Works()
        {
            var destinationFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            var filePersistence = new FilePersistenceService();

            var fileWriter = filePersistence.CreateFileWriter(destinationFile);

            //Arrange
            var heikenAshiBars = new HeikenAshiBars(100, fileWriter);

            //Action
            heikenAshiBars.Write(DateTime.Now.Ticks, 1, 2, 3, 4);

            fileWriter.Close();

            //Assert
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.IsTrue(new FileInfo(destinationFile).Length > 0);
        }

        [TestMethod]
        public void OHLCBar_Persistence_Works()
        {
            //Arrange
            var destinationFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var filePersistence = new FilePersistenceService();
            var fileWriter = filePersistence.CreateFileWriter(destinationFile);
            
            var ohlcBars = new OHLCBars(100, fileWriter);

            //Action
            ohlcBars.Write(DateTime.Now.Ticks, 1, 2, 3, 4);

            fileWriter.Close();

            //Assert
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.IsTrue(new FileInfo(destinationFile).Length > 0);
        }

        [TestMethod]
        public void RenkoBar_Persistence_Works()
        {
            //Arrange
            var destinationFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var filePersistence = new FilePersistenceService();
            var fileWriter = filePersistence.CreateFileWriter(destinationFile);

            var renkoBars = new RenkoBars(100, 10, 1, fileWriter);

            //Action
            renkoBars.Write(DateTime.Now.Ticks, 1);

            fileWriter.Close();

            //Assert
            Assert.IsTrue(File.Exists(destinationFile));
            Assert.IsTrue(new FileInfo(destinationFile).Length > 0);
        }
    }
}
