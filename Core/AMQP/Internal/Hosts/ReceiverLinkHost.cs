﻿using Amqp;
using Newtonsoft.Json;
using ReinhardHolzner.Core.AMQP.Internal.Impl;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReinhardHolzner.Core.AMQP.Internal.Hosts
{
    internal class ReceiverLinkHost<TMessage> : LinkHost
    {
        private ReceiverLink _receiverLink;
        
        private AMQP10MessengerImpl<TMessage> _messenger;

        public Task MessageProcessorTask { get; private set; }

        public ReceiverLinkHost(ConnectionFactory connectionFactory, string connectionString, string address, AMQP10MessengerImpl<TMessage> messenger, CancellationToken cancellationToken)
            : base(connectionFactory, connectionString, address, cancellationToken)
        {
            _messenger = messenger;
        }
        
        protected override void InitializeLink(Session session)
        {
            _receiverLink = new ReceiverLink(session, $"{Address}-receiver", Address);

            MessageProcessorTask = Task.Run(async () =>
            {
                await RunMessageProcessorAsync().ConfigureAwait(false);
            });            
        }

        private async Task RunMessageProcessorAsync()
        {
            do
            {
                if (CancellationToken.IsCancellationRequested)
                    break;

                if ((_receiverLink == null || _receiverLink.IsClosed))
                    await InitializeAsync().ConfigureAwait(false);

                try
                {
                    using (var message = await _receiverLink.ReceiveAsync(TimeSpan.FromSeconds(10)).ConfigureAwait(false))
                    {                    
                        try
                        {
                            TMessage messageBody = (TMessage) message.Body;

                            await _messenger.ProcessMessageAsync(Address, messageBody).ConfigureAwait(false);

                            _receiverLink.Accept(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Exception during processing AMQP message, rejecting: {e}");

                            _receiverLink.Reject(message);
                        }                        
                    }
                }
                catch (AmqpException e)
                {
                    if (!CancellationToken.IsCancellationRequested)                    
                        Console.WriteLine($"AMQP exception in receiver link for address {Address}: {e}");

                    await CloseAsync().ConfigureAwait(false);                   
                }                 
            } while (!CancellationToken.IsCancellationRequested);

            // normal end

            await CloseAsync().ConfigureAwait(false);            
        }

        public override async Task CloseAsync()
        {
            await base.CloseAsync().ConfigureAwait(false);

            _receiverLink = null;
            MessageProcessorTask = null;            
        }
    }
}
