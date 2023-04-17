using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using cli_life;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual(result.genCount, 558);
        }
        [TestMethod]
        public void TestMethod2()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual(result.hives, 1);
        }
        [TestMethod]
        public void TestMethod3()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual(result.alive, 34);
        }
        [TestMethod]
        public void TestMethod4()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual((double)result.alive / result.total, 0.034);
        }
        [TestMethod]
        public void TestMethod5()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual(result.total - result.alive, 966);
        }
        [TestMethod]
        public void TestMethod6()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start3.txt");
            Assert.AreEqual(result.symmetry, false);
        }
        [TestMethod]
        public void TestMethod7()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start.txt");
            Assert.AreEqual(result.genCount, 248);
        }
        [TestMethod]
        public void TestMethod8()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start.txt");
            Assert.AreEqual(result.blocks, 4);
        }
        [TestMethod]
        public void TestMethod9()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start.txt");
            Assert.AreEqual(result.hives, 2);
        }
        [TestMethod]
        public void TestMethod10()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start.txt");
            Assert.AreEqual(result.total, 1000);
        }
        [TestMethod]
        public void TestMethod11()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start2.txt");
            Assert.AreEqual(result.genCount, 45);
        }
        [TestMethod]
        public void TestMethod12()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start2.txt");
            Assert.AreEqual(result.hives + result.ponds + result.blocks + result.boxes, 4);
        }
        [TestMethod]
        public void TestMethod13()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start2.txt");
            Assert.AreEqual(result.alive, 22);
        }
        [TestMethod]
        public void TestMethod14()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start4.txt");
            Assert.AreEqual(result.hives, 6);
        }
        [TestMethod]
        public void TestMethod15()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start4.txt");
            Assert.AreEqual(result.ponds, 3);
        }
        [TestMethod]
        public void TestMethod16()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start4.txt");
            Assert.AreEqual(result.blocks, 6);
        }
        [TestMethod]
        public void TestMethod17()
        {
            LifeGame life = new LifeGame();
            var result = life.Run("start4.txt");
            Assert.AreEqual(result.boxes, 2);
        }
    }
}
