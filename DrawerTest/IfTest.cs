using ASE_ProgrammingLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DrawerTest
{
    [TestClass]
    public class IfTest
    {
        private CommandParser commandParser;

        private TestContext testContextInstance;

        [TestInitialize]
        public void TestInitialize()
        {
            commandParser = CommandParser.Instance;
            ResetCommandParserState();
        }

        private void ResetCommandParserState()
        {
            commandParser.GetType().GetField("instance", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
        }

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        public void AssertSelection_WhenIfStatementWithTrueCondition_ShouldReturnTrue()
        {
            // Arrange
            var ifTest = new IfTest(); 
            commandParser.variables.Add("var1", 5);

            string ifStatement = "if var1 == 5";

            // Act
            string result = commandParser.AssertSelection(ifStatement);

            // Assert
            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void AssertSelection_WhenIfStatementWithFalseCondition_ShouldReturnNonMatch()
        {
            // Arrange
            var ifTest = new IfTest();
            commandParser.variables.Add("var1", 10);

            string ifStatement = "if var1 == 5";

            // Act
            string result = commandParser.AssertSelection(ifStatement);

            // Assert
            Assert.AreEqual("NonMatch", result);
        }

        [TestMethod]
        public void AssertSelection_WhenInvalidIfStatement_ShouldThrowOtherException()
        {
            // Arrange
            var ifTest = new IfTest();

            string invalidIfStatement = "if invalid statement";

            // Act & Assert
            Assert.ThrowsException<OtherException>(() => commandParser.AssertSelection(invalidIfStatement));
        }
    }
}
