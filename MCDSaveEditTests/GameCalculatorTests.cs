﻿using MCDSaveEdit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
#nullable enable

namespace MCDSaveEditTests
{
    [TestClass]
    public class GameCalculatorTests {
        [TestMethod]
        public void TestXpAndLevel()
        {
            var data = new List<Tuple<long, int>> {
                new Tuple<long, int>(0, 1),
                new Tuple<long, int>(500, 2),
                new Tuple<long, int>(1_600, 3),
                new Tuple<long, int>(3_300, 4),
                new Tuple<long, int>(5_600, 5),
                new Tuple<long, int>(8_500, 6),
                new Tuple<long, int>(12_000, 7),
                new Tuple<long, int>(16_100, 8),
                new Tuple<long, int>(20_800, 9),
                new Tuple<long, int>(26_100, 10),
                new Tuple<long, int>(32_000, 11),
                new Tuple<long, int>(38_500, 12),
                new Tuple<long, int>(45_600, 13),
                new Tuple<long, int>(53_300, 14),
                new Tuple<long, int>(61_600, 15),
                new Tuple<long, int>(70_500, 16),
                new Tuple<long, int>(80_000, 17),
                new Tuple<long, int>(90_100, 18),
                new Tuple<long, int>(100_800, 19),
                new Tuple<long, int>(112_100, 20),
                new Tuple<long, int>(258_100, 30),
                new Tuple<long, int>(464_100, 40),
                new Tuple<long, int>(730_100, 50),
                new Tuple<long, int>(1_056_100, 60),
                new Tuple<long, int>(1_442_100, 70),
                new Tuple<long, int>(1_888_100, 80),
                new Tuple<long, int>(2_394_100, 90),
                new Tuple<long, int>(2_960_100, 100),
                new Tuple<long, int>(11_920_100, 200),
                new Tuple<long, int>(26_880_100, 300),
                new Tuple<long, int>(47_840_100, 400),
                new Tuple<long, int>(74_800_100, 500),
                new Tuple<long, int>(107_760_100, 600),
                new Tuple<long, int>(146_720_100, 700),
                new Tuple<long, int>(191_680_100, 800),
                new Tuple<long, int>(242_640_100, 900),
                new Tuple<long, int>(299_600_100, 1000),
                new Tuple<long, int>(1_199_200_100, 2000),
            };
            foreach (var pair in data)
            {
                Assert.AreEqual(pair.Item2, GameCalculator.levelForExperience(pair.Item1));
                Assert.AreEqual(pair.Item1, GameCalculator.experienceForLevel(pair.Item2));
            }
        }

        [TestMethod]
        public void TestPowerAndLevel()
        {
            var data = new List<Tuple<double, int>> {
                new Tuple<double, int>(11.42291355133057, 105),
                new Tuple<double, int>(11.49747276306152, 105),
                new Tuple<double, int>(11.5050516128540, 106),
                new Tuple<double, int>(11.79001903533936, 108),
                new Tuple<double, int>(12.14500808715820, 112),
                new Tuple<double, int>(20.89999961853027, 200),
                new Tuple<double, int>(20.9, 200),
                new Tuple<double, int>(11.35, 104),
                new Tuple<double, int>(11.39, 104),
            };
            foreach (var pair in data)
            {
                Assert.AreEqual(pair.Item2, GameCalculator.levelFromPower(pair.Item1));
                //Assert.AreEqual(pair.Item1, GameCalculator.powerFromLevel(pair.Item2));
            }
        }

        [TestMethod]
        public void TestLevelToPowerToLevel()
        {
            for(int i=1; i<1_000_000; i++)
            {
                var power = GameCalculator.powerFromLevel(i);
                var level = GameCalculator.levelFromPower(power);
                Assert.AreEqual(i, level);
            }
        }


    }
}
