using ASE_ProgrammingLanguage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DrawerTest
{
    /// <summary>
    /// Unit tests for the <see cref="CommandParser"/> class.
    /// </summary>
    [TestClass]
    public class MethodTest
    {
        private CommandParser commandParser;
        private TestContext testContextInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodTest"/> class.
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
        /// Tests the <see cref="CommandParser.AssertMethod"/> method with a valid block, expecting the result to be "true".
        /// </summary>
        [TestMethod]
        public void AssertMethod_ValidBlock_ReturnsTrue()
        {
            // Arrange
            string validBlock = "method SomeMethod()";

            // Act
            string result = commandParser.AssertMethod(validBlock);

            // Assert
            Assert.AreEqual("true", result);
        }

        /// <summary>
        /// Tests the <see cref="CommandParser.AssertMethod"/> method with an empty block, expecting the result to be "False".
        /// </summary>
        [TestMethod]
        public void AssertMethod_EmptyBlock_ReturnsFalse()
        {
            // Arrange
            string emptyBlock = "";

            // Act
            string result = commandParser.AssertMethod(emptyBlock);

            // Assert
            Assert.AreEqual("False", result);
        }
    }
    
}
