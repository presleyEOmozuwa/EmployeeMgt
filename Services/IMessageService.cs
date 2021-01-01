using System.Threading.Tasks;

namespace EmployeeMgt.Services
{
    public interface IMessageService
    {
        Task SendEmailAsync
        (
            string userName,
            string fromEmailAddress,
            string toName,
            string toEmailAddress,
            string subject,
            string message
           /*  params Attachment[] attachments */
        );
    }
}