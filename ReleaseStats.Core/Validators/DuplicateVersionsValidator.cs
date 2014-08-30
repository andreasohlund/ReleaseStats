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
}