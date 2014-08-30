namespace ReleaseStats.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.Validators;

    [TestFixture]
    public class DuplicateVersionsValidatorTests
    {
        [Test]
        public void ShouldDetectDuplicates()
        {
            var releases = new List<Release>
            {
                new Release("1.0.0"),
                new Release("1.0.1"),
                new Release("1.0.1"),
                new Release("1.0.2"),
                new Release("1.1.1"),
                new Release("1.1.1")
            };


            var validator = new DuplicateVersionsValidator();

            var result = validator.Validate(releases);


            Assert.True(result.Any(r => r.Version == "1.0.1"));
            Assert.True(result.Any(r => r.Version == "1.1.1"));
            Assert.False(result.Any(r => r.Version == "1.0.0"));
            Assert.False(result.Any(r => r.Version == "1.0.2"));
            
            Assert.True(result.Single(e => e.Version == "1.0.1").Message.Contains("duplicate"));
        }
    }
}