using Lamar;
using NUnit.Framework;
using ProATA.SharedKernel;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TaskProcessing.Core.Interfaces;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Services;
using TaskProcessing.Core.Services.TaskProcessor;
using TaskProcessing.Test.DependencyResolution;

namespace TaskProcessing.Test
{

    [TestFixture]
    public class TaskShould
    {
        private readonly IContainer _container;

        public TaskShould()
        {
            var registry = new ProATARegistry();

            _container = new Container(registry);
            var report = _container.WhatDoIHave();

            Console.WriteLine(report);
        }

        [Test]
        public void TaskShouldNotRun()
        {
            var runStrategy = new TaskProcessing.Core.Strategies.Test();

            var task = new APITask(Guid.NewGuid(), "Test");

            var result = task.Run();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void TaskShouldRun()
        {
            var title = "Test";
            var task = new APITask(Guid.NewGuid(), title);

            var taskManager = new TaskProcessorManager();
            _ = taskManager.RunTask(task);
        }
    }
}