using ASE_ProgrammingLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DrawerTest
{
    /// <summary>
    /// Unit tests for the <see cref="CommandParser"/> class related to if statements.
    /// </summary>
    [TestClass]
    public class IfTest
    {
        private CommandParser commandParser;

        private TestContext testContextInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="IfTest"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
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

        /// <summary>
        /// Asserts selection when an if statement has a true condition, expecting the result to be "true".
        /// </summary>
        public void AssertSelection_WhenIfStatementWithTrueCondition_ShouldReturnTrue()
        {
            // Arrange
            commandParser.variables.Add("var1", 5);

            string ifStatement = "if var1 == 5";

            // Act
            string result = commandParser.AssertSelection(ifStatement);

            // Assert
            Assert.AreEqual("true", result);
        }

        /// <summary>
        /// Asserts selection when an if statement has a false condition, expecting the result to be "NonMatch".
        /// </summary>
        [TestMethod]
        public void AssertSelection_WhenIfStatementWithFalseCondition_ShouldReturnNonMatch()
        {
            // Arrange
            commandParser.variables.Add("var1", 10);

            string ifStatement = "if var1 == 5";

            // Act
            string result = commandParser.AssertSelection(ifStatement);

            // Assert
            Assert.AreEqual("NonMatch", result);
        }

        /// <summary>
        /// Asserts selection when an invalid if statement is provided, expecting an <see cref="OtherException"/> to be thrown.
        /// </summary>
        [TestMethod]
        public void AssertSelection_WhenInvalidIfStatement_ShouldThrowOtherException()
        {
            // Arrange
            string invalidIfStatement = "if invalid statement";

            // Act & Assert
            Assert.ThrowsException<OtherException>(() => commandParser.AssertSelection(invalidIfStatement));
        }
    }
}
