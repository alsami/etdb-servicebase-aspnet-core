﻿using System;
using System.Collections.Generic;
using System.Text;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Commands;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using MediatR;

namespace ETDB.API.ServiceBase.Handler
{
    public class CommandHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediatorHandler bus;
        private readonly DomainNotificationHandler notificationsHandler;

        public CommandHandler(IUnitOfWork unitOfWork, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            this.unitOfWork = unitOfWork;
            notificationsHandler = (DomainNotificationHandler)notifications;
            this.bus = bus;
        }

        protected void NotifyValidationErrors(Command message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                bus.RaiseEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        public bool Commit()
        {
            if (notificationsHandler.HasNotifications()) return false;
            var commandResponse = unitOfWork.Commit();
            if (commandResponse.Success) return true;

            bus.RaiseEvent(new DomainNotification("Commit", "We had a problem during saving your data."));
            return false;
        }
    }
}
