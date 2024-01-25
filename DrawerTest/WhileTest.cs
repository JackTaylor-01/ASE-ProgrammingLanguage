using ASE_ProgrammingLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DrawerTest
{
    [TestClass]
    public class WhileTest
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

        public void AssertIteration_WhenWhileStatementWithTrueCondition_ShouldReturnTrue()
        {
            // Arrange
            commandParser.variables.Add("var1", 5); // Add any necessary variables

            string whileStatement = "while var1 == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("true", result);
        }

        [TestMethod]
        public void AssertIteration_WhenWhileStatementWithFalseCondition_ShouldReturnNonMatch()
        {
            // Arrange
            commandParser.variables.Add("var1", 10); // Add any necessary variables

            string whileStatement = "while var1 == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("NonMatch", result);
        }

        [TestMethod]
        public void AssertIteration_WhenWhileStatementWithUndefinedVariable_ShouldReturnNonVar()
        {
            // Arrange
            string whileStatement = "while undefinedVar == 5";

            // Act
            string result = commandParser.AssertIteration(whileStatement);

            // Assert
            Assert.AreEqual("NonVar", result);
        }

        [TestMethod]
        public void AssertIteration_WhenInvalidWhileStatement_ShouldThrowOtherException()
        {
            // Arrange

            string invalidWhileStatement = "while invalid statement";

            // Act & Assert
            Assert.ThrowsException<OtherException>(() => commandParser.AssertIteration(invalidWhileStatement));
        }
    }
}
