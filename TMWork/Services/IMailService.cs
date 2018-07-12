using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMWork.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string from_email, string subject, string message);
    }
}
