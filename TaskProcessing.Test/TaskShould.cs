using Lamar;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
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
        private IConfiguration _configuration;

        public TaskShould()
        {
            _configuration = InitConfiguration();

            var registry = new ProATARegistry();

            _container = new Container(registry);
            var report = _container.WhatDoIHave();

            Console.WriteLine(report);
        }

        [Test]
        public void TaskShouldNotRun()
        {
            var runStrategy = new TaskProcessing.Core.Strategies.NewTask();

            var task = new APITask(Guid.NewGuid(), "Test", true, new Scheduler(Guid.NewGuid(), "TestHost", false));

            var result = task.Run();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void TaskShouldRun()
        {
            var title = "Test";
            var task = new APITask(Guid.NewGuid(), title, true, new Scheduler(Guid.NewGuid(), "TestHost", false));

            var taskManager = new TaskProcessorManager(_configuration);
            _ = taskManager.RunTask(task);
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}