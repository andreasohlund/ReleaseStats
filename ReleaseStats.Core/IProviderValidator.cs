namespace ReleaseStats
{
    using System.Collections.Generic;

    public interface IProviderValidator
    {
        IEnumerable<ValidationError> Validate(IEnumerable<Release> releases);
    }
}