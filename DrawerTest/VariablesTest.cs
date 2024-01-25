using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASE_ProgrammingLanguage;
using System.Collections;
using System.Reflection;

namespace DrawerTest
{
    /// <summary>
    /// Summary description for VariablesTest
    /// </summary>
    [TestClass]
    public class VariablesTest
    {
        private CommandParser commandParser;
        public VariablesTest()
        {
            //
            // TODO: Add constructor logic here
            //

        }

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

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void ParseCommands_WhenGivenValidVariable_ShouldParseSuccessfully()
        {
            // Arrange
            var variablesTest = new VariablesTest();
            string input = "varName = 10";

            // Act
            commandParser.ParseCommands(input);

            // Assert
            Assert.IsNotNull(commandParser.variables);
            Assert.IsTrue(commandParser.variables.ContainsKey("varName"));
            Assert.AreEqual(10, commandParser.variables["varName"]);

        }

        [TestMethod]
        public void ParseCommands_WhenGivenInvalidVariable_ShouldHandleGracefully()
        {
            // Arrange
            var variablesTest = new VariablesTest();
            string input = "var == 60";

            // Act
            commandParser.ParseCommands(input);

            // Assert
            try
            {
                commandParser.ParseCommands(input);

                Assert.IsNull(commandParser.variables);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

        }
    }
}
