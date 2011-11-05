﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace F9S1.RememberMe
{
    class Tester
    {
        Controller testDispatch;
        List<string> testCases, testResults, expectedResults;
        
        public Tester()
        {
            checkFiles();
            testDispatch = new Controller();
            testCases = new List<string>();
            testResults = new List<string>();
            expectedResults = new List<string>();
        }

        /// <summary>
        /// If the files do not exist, creates the files.
        /// </summary>
        private void checkFiles()
        {
            if (!File.Exists(Utility.INPUT_FILE))
            {
                StreamWriter inputStream = new StreamWriter(Utility.INPUT_FILE);
                inputStream.Close();
            }
            if (!File.Exists(Utility.OUTPUT_FILE))
            {
                StreamWriter outputStream = new StreamWriter(Utility.OUTPUT_FILE);
                outputStream.Close();
            }
        }

        /// <summary>
        /// Gets input, expected output and runs the tests. If there are any failed cases, get the failed cases.
        /// </summary>
        public void Test()
        {
            testCases = ReadLines(Utility.INPUT_FILE);
            expectedResults = ReadLines(Utility.OUTPUT_FILE);
            RunTests();
            if (!AreResultsCorrect())
                AssertResults();
        }

        /// <summary>
        /// Reads the contents of the given file and returns the List of Strings.
        /// </summary>
        /// <param name="fileName">The file to read from.</param>
        /// <returns>The contents of the file.</returns>
        private List<String> ReadLines(string fileName)
        {
            List<string> contents = new List<string>();
            Debug.Assert(File.Exists(fileName));
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    contents.Add(line);
            }
            return contents;
        }
 
        /// <summary>
        /// Runs the tests, stores the output in a local List of Strings.
        /// </summary>
        private void RunTests()
        {
            foreach (string line in testCases)
                testResults.Add(ListToString(testDispatch.UserDispatch(line)));
        }

        /// <summary>
        /// Takes in a list of strings and concatenates them into a single string.
        /// </summary>
        /// <param name="lines">Strings to be concatenated</param>
        /// <returns>The concatenated result</returns>
        private string ListToString(List<string> lines)
        {
            string singleLine = "";
            foreach (string line in lines)
                singleLine += Utility.FILE_SEPARATER + line;
            return singleLine;
        }

        /// <summary>
        /// Returns true if the current output matches the expected output.
        /// </summary>
        /// <returns>True if all cases match, false otherwise</returns>
        private bool AreResultsCorrect()
        {
            return testResults.Equals(expectedResults);
        }

        /// <summary>
        /// Using assert, displays input, output and expected output of failed testcases.
        /// </summary>
        private void AssertResults()
        {
            for (int i = 0; i < testCases.Count; i++)
                Debug.Assert(expectedResults[i] == testResults[i], "Input: " + testCases[i] + "\nOutput: " + testResults + "\nExpected: " + expectedResults[i]);
        }

        /// <summary>
        /// Writes the output for the testcases read from the input file.
        /// Use this only when the expected output changes, and you are sure that this output is correct.
        /// </summary>
        public void GetOutputFile()
        {
            testCases = ReadLines(Utility.INPUT_FILE);
            Debug.Assert(File.Exists(Utility.INPUT_FILE));
            Debug.Assert(File.Exists(Utility.OUTPUT_FILE));
            using (TextWriter testStream = new StreamWriter(Utility.OUTPUT_FILE))
                foreach (string line in testCases)
                    testStream.WriteLine(ListToString(testDispatch.UserDispatch(line)));
        }
    }
}