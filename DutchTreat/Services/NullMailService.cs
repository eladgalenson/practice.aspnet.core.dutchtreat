using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private ILogger logger;
        public NullMailService(ILogger<NullMailService> logger)
        {
            this.logger = logger;
        }
        public void SendMail(string to, string subject,   string message)
        {
            logger.LogInformation($"To { to}, subject {subject}, message: {message}");
        }
    }
}
