using System.Configuration;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace Sender
{
    public class QueueManager
    {
        private readonly string _AzureWebJobsStorage;
        private readonly QueueClient _queueClient;
        private readonly TimeSpan _visibilityTimeout;
        private QueueManager(string queueName)
        {
            _AzureWebJobsStorage = GetConnectionString();
            _queueClient = new QueueClient(_AzureWebJobsStorage, queueName);
            _visibilityTimeout = TimeSpan.FromSeconds(5.0);
        }

        private string GetConnectionString()
        {
            var connectionStringKey = "AzureWebJobsStorage";

            var consoleConnectionString = ConfigurationManager.AppSettings[connectionStringKey];
            var functionConnectionString = Environment.GetEnvironmentVariable(connectionStringKey);

            var connectionString = consoleConnectionString ?? functionConnectionString;

            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Please enter the storage connection key using the \"AzureWebJobsStorage\" key");
            return connectionString;
        }

        public static QueueManager CreateInstace(string queueName)
        {
            var queueManager = new QueueManager(queueName);
            queueManager.CreateIfNotExists();
            return queueManager;
        }

        private void CreateIfNotExists()
        {
            try
            {
                _queueClient.Create();
            }
            catch (Exception ex)
            {
                throw new Exception($"Make sure the Azurite storage emulator running and try again.\n\nException: {ex.Message}");
            }
        }

        public Task<Response<SendReceipt>> InsertMessage(string message)
        {
            return _queueClient.SendMessageAsync(Base64Encode(message));
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public Task<Response<PeekedMessage[]>> PeekMessages(int messageQuantity = 1)
        {
            return _queueClient.PeekMessagesAsync(messageQuantity);
        }

        public Task<Response<QueueMessage[]>> ReceiveMessages(int messageQuantity = 1)
        {
            return _queueClient.ReceiveMessagesAsync(messageQuantity);
        }

        public Task<Response<UpdateReceipt>> UpdateMessage(QueueMessage message, string messageText = null)
        {
            return _queueClient.UpdateMessageAsync(message.MessageId, message.PopReceipt, messageText, _visibilityTimeout);
        }

        public Task<Response> DequeueMessage(QueueMessage message)
        {
            return _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        public async Task<int> GetLengthAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            return properties.ApproximateMessagesCount;
        }

        public async void DeleteQueueAsync()
        {
            await _queueClient.DeleteAsync();
        }

    }
}
