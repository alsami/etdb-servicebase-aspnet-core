﻿using ETDB.API.ServiceBase.EventSourcing.Abstractions.Base;

namespace ETDB.API.ServiceBase.Controller.Abstractions.Response
{
    public class EventSourcedResponseSuccess : EventSourcedRepsonse
    {
        public EventSourcedResponseSuccess()
        {
            this.Success = true;
        }

        public IEventSourcedDTO Data { get; set; }
    }
}