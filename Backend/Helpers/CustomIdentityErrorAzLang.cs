using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Helpers
{
    public class CustomIdentityErrorAzLang:IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError 
            {
                Code=nameof(PasswordRequiresNonAlphanumeric),
                Description="Qaqi hansisa simvol istifade ele. Etden zaddan gor nagarirsan" 
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Qaqi parolun uzunlugu minimum {length} olmalidir.Gor nagarirsan"
            };
        }
    }
}
