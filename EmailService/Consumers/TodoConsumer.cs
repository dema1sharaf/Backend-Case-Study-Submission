using EmailService.WebApi.Models;
using MassTransit;
using System.Threading.Tasks;
using SharedLayer;
using EmailService.WebApi.Services;

namespace Microservice.TodoApp.Consumers
{
    public class TodoConsumer : IConsumer<MailEvent>
    {
        public IMailService mailService;
        public TodoConsumer(IMailService mailService)
        {
            this.mailService = mailService;
        }
        public async Task Consume(ConsumeContext<MailEvent> context)
        {
         
            var mail__ = new MailRequest();
            mail__.ToEmail = "dema.sharaf1@gmail.com";
            mail__.Subject = "New Product";
            mail__.Body = context.Message.info;
            Console.Out.WriteLineAsync($"Notification sent: todo id {context.Message.info}");
            mailService.SendEmailAsync(mail__);
          //  Console.WriteLine("consumer called");
           

        }
    }
}
