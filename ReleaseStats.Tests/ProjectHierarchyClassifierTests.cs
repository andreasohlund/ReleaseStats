namespace ReleaseStats.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectHierarchyClassifierTests
    {
        [Test]
        public void ShouldArrangeUsingDotConventionAndSortByName()
        {
            var rawProjects = new List<Project>
            {
                "NServiceBus.RabbitMQ",
                "NServiceBus",
                "SomeOtherProject",
                "NServiceBus.AutoFac",
            };


            var result = ProjectHierarchyClassifier.Classify(rawProjects);

            var nsb = result.First();

            Assert.AreEqual("NServiceBus",nsb.Name);

            Assert.AreEqual("NServiceBus.AutoFac", nsb.Subprojects.First().Name);
            Assert.AreEqual("NServiceBus.RabbitMQ", nsb.Subprojects.Skip(1).First().Name);


            Assert.AreEqual("SomeOtherProject", result.Skip(1).First() .Name);
        }
    }
}