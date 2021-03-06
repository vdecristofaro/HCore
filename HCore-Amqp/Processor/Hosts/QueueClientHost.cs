﻿using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HCore.Amqp.Message;
using HCore.Amqp.Messenger.Impl;
using HCore.Amqp.Exceptions;

namespace HCore.Amqp.Processor.Hosts
{
    internal class QueueClientHost
    {
        private readonly string _connectionString;
        private readonly int _amqpListenerCount;

        private readonly string _lowLevelAddress;

        protected string Address { get; set; }

        private QueueClient _queueClient;

        private readonly ServiceBusMessengerImpl _messenger;

        protected readonly CancellationToken CancellationToken;

        private readonly static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public QueueClientHost(string connectionString, int amqpListenerCount, string address, ServiceBusMessengerImpl messenger, CancellationToken cancellationToken)
        {
            _connectionString = connectionString;
            _amqpListenerCount = amqpListenerCount;

            Address = address;
            _lowLevelAddress = Address.ToLower();

            _messenger = messenger;

            CancellationToken = cancellationToken;
        }

        public async Task InitializeAsync()
        {
            await CloseAsync().ConfigureAwait(false);

            _queueClient = new QueueClient(_connectionString, _lowLevelAddress);

            _queueClient.RegisterMessageHandler(ProcessMessageAsync, new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = _amqpListenerCount,
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromHours(2)               
            });            
        }

        private async Task ProcessMessageAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            QueueClient queueClient = _queueClient;

            try
            {
                if (!string.Equals(message.ContentType, "application/json"))
                    throw new Exception($"Invalid content type for AMQP message: {message.ContentType}");

                if (message.Body != null)
                {
                    await _messenger.ProcessMessageAsync(Address, Encoding.UTF8.GetString(message.Body)).ConfigureAwait(false);
                } 
                else
                {
                    await _messenger.ProcessMessageAsync(Address, null).ConfigureAwait(false);
                }

                await queueClient.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }
            catch (RescheduleException)
            {
                // no log, this is "wanted"

                await queueClient.AbandonAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception during processing AMQP message, not abandoning it for timeout (this will avoid duplicates): {exception}");
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"AMQP message handler encountered an exception: {exceptionReceivedEventArgs.Exception}");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            Console.WriteLine("Exception context for troubleshooting:");

            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public virtual async Task CloseAsync()
        {
            if (_queueClient != null)
            {
                await _queueClient.CloseAsync();

                _queueClient = null;
            }
        }

        public async Task SendMessageAsync(AMQPMessage messageBody, DateTimeOffset? whenToRun = null)
        {
            QueueClient queueClient;
            bool error;

            do
            {
                queueClient = _queueClient;
                error = false;

                if (CancellationToken.IsCancellationRequested)
                    throw new Exception("AMQP cancellation is requested, can not send message");

                try
                {
                    if (queueClient == null || queueClient.IsClosedOrClosing)
                        await InitializeAsync().ConfigureAwait(false);

                    var message = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)))
                    {
                        ContentType = "application/json",
                        MessageId = Guid.NewGuid().ToString()
                    };

                    if (whenToRun == null)
                        await queueClient.SendAsync(message).ConfigureAwait(false);
                    else
                        await queueClient.ScheduleMessageAsync(message, (DateTimeOffset)whenToRun).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    error = true;

                    await semaphore.WaitAsync().ConfigureAwait(false);

                    try { 
                        if (queueClient == _queueClient)
                        {
                            // nobody else handled this before

                            if (!CancellationToken.IsCancellationRequested)
                                Console.WriteLine($"AMQP exception in sender link for address {Address}: {e}");

                            await CloseAsync().ConfigureAwait(false);
                        }
                    }
                    catch (Exception e2)
                    {
                        throw e2;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }
            } while (error);
        }        
    }
}
