using EShop.Domain.Domain_models;
using EShop.Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class BackgroundEmailServer : IBackgroundEmailServer
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailMessage;
        public BackgroundEmailServer(IEmailService emailService, IRepository<EmailMessage> mailMessage)
        {
            _emailService = emailService;
            _mailMessage = mailMessage;
        }
        public async Task Dowork()
        {
            await _emailService.SendEmailAsync(_mailMessage.GetAll().Where(z => !z.Status).ToList());
        }
    }
}
