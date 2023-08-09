using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested");
            message.AppendLine($"<br/>Total: {cartDto.CartHeader.CartTotal}");
            message.AppendLine("<br/>");
            message.AppendLine("<ul>");
            foreach (var detail in cartDto.CartDetails)
            {
                message.AppendLine($"<li>{detail.Product.Name} x {detail.Count}</li>");
            }
            message.AppendLine("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailNewUserAndLog(UserDto userDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>New User registered");
            message.AppendLine($"<br/>User: {userDto.Name}");
            message.AppendLine($"<br/>Email: {userDto.Email}");
            message.AppendLine("<br/>");

            await LogAndEmail(message.ToString(), userDto.Email);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now
                };

                await using var db = new AppDbContext(_dbOptions);
                await db.EmailLoggers.AddAsync(emailLogger);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
