namespace ReleaseStats.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    public class DuplicateVersionsValidator:IProviderValidator
    {
        public IEnumerable<ValidationError> Validate(IEnumerable<Release> releases)
        {
            return releases.GroupBy(r => r.Version)
                .Where(g => g.Count() > 1)
                .Select(g => new ValidationError
                {
                    Message = string.Format("{0} duplicate versions detected", g.Count()),
                    Version = g.Key
                }).ToList();
        }
    }

    public interface IProviderValidator
    {
        IEnumerable<ValidationError> Validate(IEnumerable<Release> releases);
    }

    public class ValidationError
    {
        public SemVer Version { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Version, Message);
        }
    }
}