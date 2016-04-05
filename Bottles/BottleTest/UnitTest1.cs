using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bottles;

namespace BottleTest
{
    [TestClass]
    public class UnitTest1
    {
        //Fill
        [TestMethod]
        public void FillEmptyBottle_ShouldReturnTrue()
        {
            var bottle = new Bottle(5);
            var filled = bottle.Fill();

            Assert.IsTrue(filled);
        }
        [TestMethod]
        public void FillFullBottle_ShouldReturnFalse()
        {
            var bottle = new Bottle(5);
            bottle.Fill();
            var filled = bottle.Fill();

            Assert.IsFalse(filled);
        }
        [TestMethod]
        public void FillEmptyBottle_ShouldSetCorrectAmmount()
        {
            var bottle = new Bottle(5);
            bottle.Fill();

            Assert.AreEqual(5, bottle.Ammount);
        }

        //Pour
        [TestMethod]
        public void PourEmptyBottle_ShouldReturnFalse()
        {
            var bottle = new Bottle(5);
            var poured = bottle.Pour();

            Assert.IsFalse(poured);
        }
        [TestMethod]
        public void PourFullBottle_ShouldReturnTrue()
        {
            var bottle = new Bottle(5);
            bottle.Fill();
            var poured = bottle.Pour();

            Assert.IsTrue(poured);
        }
        [TestMethod]
        public void PourFullBottle_ShouldSetCorrectAmmount()
        {
            var bottle = new Bottle(5);
            bottle.Fill();
            bottle.Pour();

            Assert.AreEqual(0, bottle.Ammount);
        }

        //Transfer
        [TestMethod]
        public void TransferToFullBottle_ShouldReturnFalse()
        {
            var bottle1 = new Bottle(3);
            var bottle2 = new Bottle(5);

            bottle1.Fill();
            bottle2.Fill();

            var trans = bottle1.Transfer(ref bottle2);

            Assert.IsFalse(trans);
        }
        [TestMethod]
        public void TransferFromEmptyBottle_ShouldReturnFalse()
        {
            var bottle1 = new Bottle(3);
            var bottle2 = new Bottle(5);

            var trans = bottle1.Transfer(ref bottle2);

            Assert.IsFalse(trans);
        }
        [TestMethod]
        public void TransferBetweenBottles_ShouldSetCorrectAmmount()
        {
            var bottle1 = new Bottle(3);
            var bottle2 = new Bottle(5);

            bottle1.Fill();
            bottle1.Transfer(ref bottle2);

            Assert.AreEqual(0, bottle1.Ammount);
            Assert.AreEqual(3, bottle2.Ammount);

            bottle1.Fill();
            bottle1.Transfer(ref bottle2);

            Assert.AreEqual(1, bottle1.Ammount);
            Assert.AreEqual(5, bottle2.Ammount);
        }

        [TestMethod]
        public void IsBottleFull_ShouldReturnTrue()
        {
            var bottle1 = new Bottle(3);

            bottle1.Fill();
            var full = bottle1.IsFull();

            Assert.IsTrue(full);
        }
    }
}
