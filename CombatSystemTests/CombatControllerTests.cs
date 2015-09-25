using System;
using CombatSystem;
using CombatSystem.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CombatSystemTests
{
    [TestClass]
    public class CombatControllerTests
    {
        [TestMethod]
        public void AttackWith2Combos()
        {
            var starts = 0;
            var ends = 0;
            var comboName = "Combo1";

            OnStart onStart = () => starts++;
            OnEnd onEnd = () => ends++;

            var system =
                CombatConfiguration
                    .Configure()
                    .StartCombo(comboName)
                    .WithDuration(1000)
                    .OnStart(onStart)
                    .OnEnd(onEnd)
                    .NextCombo()
                    .WithDuration(1000)
                    .OnStart(onStart)
                    .OnEnd(onEnd)
                    .EndCombo();

            var controller = new CombatController(system);
            controller.Perform(comboName);
            controller.Elapse(600);
            controller.Perform(comboName);
            controller.Elapse(400);
            controller.Elapse(600);
            controller.Perform(comboName);
            controller.Elapse(400);

            Assert.AreEqual(2, starts);
            Assert.AreEqual(2, ends);
        }
    }
}
