using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS
{
    public class AnalyticsServiceBus
    {
        const string ServiceBusConnectionString = "Endpoint=sb://caseventsservicebusstaging.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=z6HZ7yHY0+yLTCanKrxC8i4ionGszHQjGP4YorCTN7o=";
        const string TopicName = "casalertstopic";
        static ITopicClient topicClient;

        public async Task MainAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);           
            // Send messages.
            await SendMessagesAsync();
            await topicClient.CloseAsync();
        }

        public static async Task SendMessagesAsync()
        {
            try
            {
                int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                // Create a new message to send to the topic
                string messageBody = @"{""policy_id"": ""B9A73370-7BE5-11E8-B57F-3D108964CAB1"", 
                                            ""alert_id"": ""aa6662f0-7b11-11e8-adc0-fa7ae01bbebe"", 
                                            ""alert_value"": ""xa_receiver_unsupported_os"", 
                                            ""tenant_id"": ""casstaging"", 
                                            ""entity_name"": ""user"", 
                                            ""alert_type"": ""Risk Indicator"", 
                                            ""alert_details"": [{""XenApp"": {""samaccountname"": ""Elliot.anderson"", ""domains"": ""[xd17-hk]""}}], 
                                            ""entity_id"": ""jose.soares@citrix.com"", 
                                            ""alert_message"": ""Indicator observed ok?"", 
                                            ""alert_time"": " + epoch + "}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                // Set message body type and content encoding.                
                message.ContentType = "application/json";            

                // Write the body of the message to the console
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the topic
                await topicClient.SendAsync(message);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}