using System.Collections.Generic;

namespace QZCHY.Services.AccountUsers
{
    public class AccountUserRegistrationResult
    {
        public AccountUserRegistrationResult()
        {
            this.Errors = new List<string>();
        }

        public bool Success
        {
            get { return this.Errors.Count == 0; }
        }

        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

        public IList<string> Errors { get; set; }
    }
}