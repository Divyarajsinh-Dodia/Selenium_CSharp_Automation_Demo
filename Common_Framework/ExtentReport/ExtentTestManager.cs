using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Framework.ExtentReport
{
    public static class ExtentTestManager
    {
        [ThreadStatic]
        private static ExtentTest _test;
        [ThreadStatic]
        private static ExtentTest _step;

        /// <summary>
        /// Create a new test in the extent report
        /// </summary>
        /// <param name="testName">Name for the test</param>
        /// <param name="description">Description for the test</param>
        /// <returns></returns>
        public static ExtentTest CreateTest(string testName, string description = "")
        {
            _test = ExtentService.Instance.CreateTest(testName, description);
            return _test;
        }

        /// <summary>
        /// Create a new step in the extent report
        /// </summary>
        /// <param name="extentTest">Test to put this step inside</param>
        /// <param name="methodName">Name of the method for node</param>
        /// <param name="description">A description for the step</param>
        /// <returns></returns>
        public static ExtentTest CreateStep(this ExtentTest extentTest, string methodName, string description = "")
        {
            _step = extentTest.CreateNode(methodName, description);
            return _step;
        }

        /// <returns>Returns the current test in extent report</returns>
        public static ExtentTest GetTest() => _test;

        /// <returns>Returns the current step in extent report</returns>
        public static ExtentTest GetStep() => _step;
    }
}
