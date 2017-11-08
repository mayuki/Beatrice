using Beatrice.Web.Models.Configuration;
using Beatrice.Web.ViewModels.Account;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Beatrice.Web.Models.UseCase
{
    public class ValidateUser
    {
        private BeatriceSecurityConfiguration _config;
        public ValidateUser(IOptions<BeatriceSecurityConfiguration> config)
        {
            _config = config.Value;
        }

        public Task<IEnumerable<(string Key, string Message)>> ExecuteAsync(SignInFormModel form)
        {
            var errors = new List<(string Key, string Message)>();
            if (String.Compare(_config.User, form.UserName, StringComparison.Ordinal) != 0)
            {
                errors.Add(("UserName", "UserName or Password is incorrect."));
            }
            else if (_config.Password != String.Join("", new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(_config.User + ":" + form.Password)).Select(x => x.ToString("x2"))))
            {
                errors.Add(("UserName", "UserName or Password is incorrect."));
            }

            return Task.FromResult(errors.AsEnumerable());
        }
    }
}
