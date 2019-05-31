﻿using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ProblemSolving.Common
{
    public static class TaskExecutor
    {
        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input, String expectedOutput)
        {
            Execute(taskFactory, input, expectedOutput.Split("\r\n"));
        }

        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String[] input, String[] expectedOutput)
        {
            Execute(taskFactory, String.Join("\r\n", input), expectedOutput);
        }

        public static void Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input, String[] expectedOutput)
        {
            String[] actualOutput = Execute(taskFactory, input);
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        public static void Execute<TValue>(Func<TextReader, TextWriter, ITask> taskFactory, String[] input, TValue[] expectedOutput, Action<TValue, String> valueChecker)
        {
            Execute(taskFactory, String.Join("\r\n", input), expectedOutput, valueChecker);
        }

        public static void Execute<TValue>(Func<TextReader, TextWriter, ITask> taskFactory, String input, TValue[] expectedOutput, Action<TValue, String> valueChecker)
        {
            String[] actualOutput = Execute(taskFactory, input);
            Assert.AreEqual(expectedOutput.Length, actualOutput.Length);
            for (Int32 index = 0; index < expectedOutput.Length; ++index)
            {
                valueChecker(expectedOutput[index], actualOutput[index]);
            }
        }

        private static String[] Execute(Func<TextReader, TextWriter, ITask> taskFactory, String input)
        {
            using (TextReader inputReader = new StringReader(input))
            {
                using (TextWriter outputWriter = new StringWriter())
                {
                    ITask task = taskFactory(inputReader, outputWriter);
                    Int32 result = task.Execute(new String[0]);
                    Assert.AreEqual(0, result);
                    return outputWriter.ToString()
                        .Split(new[] {"\r\n", "\n"}, System.StringSplitOptions.None)
                        .Select(value => value.TrimEnd())
                        .Reverse()
                        .SkipWhile(String.IsNullOrWhiteSpace)
                        .Reverse()
                        .ToArray();
                }
            }
        }
    }
}
