using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string emailCartQueue;
        private readonly string emailNewUserQueue;
        private readonly string serviceBusConnectionString;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _emailNewUserProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            var cliente = new ServiceBusClient(serviceBusConnectionString);

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _emailCartProcessor = cliente.CreateProcessor(emailCartQueue);

            emailNewUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailNewUserQueue");
            _emailNewUserProcessor = cliente.CreateProcessor(emailNewUserQueue);


        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequesRecived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _emailNewUserProcessor.ProcessMessageAsync += OnEmailNewUserRequesRecived;
            _emailNewUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailNewUserProcessor.StartProcessingAsync();
        }

        private async Task OnEmailNewUserRequesRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UserDto userDto = JsonConvert.DeserializeObject<UserDto>(body);
            try
            {
                // send email
                await _emailService.EmailNewUserAndLog(userDto);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private async Task OnEmailCartRequesRecived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                // send email
                await _emailService.EmailCartAndLog(cartDto);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _emailNewUserProcessor.StopProcessingAsync();
            await _emailNewUserProcessor.DisposeAsync();
        }
    }
}
