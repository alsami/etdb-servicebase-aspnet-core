﻿using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using ETDB.API.ServiceBase.Abstractions.Repositories;
using ETDB.API.ServiceBase.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Base;
using ETDB.API.ServiceBase.Domain.Abstractions.Bus;
using ETDB.API.ServiceBase.Domain.Abstractions.Notifications;
using ETDB.API.ServiceBase.Entities;
using ETDB.API.ServiceBase.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ETDB.API.ServiceBase.Builder
{
    public class ServiceContainerBuilder
    {
        private readonly ContainerBuilder containerBuilder;

        public ServiceContainerBuilder(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder;
        }

        public ServiceContainerBuilder UseConfiguration(IConfigurationRoot configurationRoot)
        {
            this.containerBuilder
                .RegisterInstance(configurationRoot)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseEnvironment(IHostingEnvironment hostingEnvironment)
        {
            this.containerBuilder
                .RegisterInstance(hostingEnvironment)
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder UseGenericRepositoryPattern<TDbContext>() where TDbContext : DbContext
        {    
            this.containerBuilder.RegisterGeneric(typeof(EntityRepository<>))
                .As(typeof(IEntityRepository<>))
                .InstancePerRequest()
                .WithParameter(this.GetDbContextResolvedParameter<TDbContext>())
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder UseEventSourcing<TDbContext>() where TDbContext : DbContext
        {
            this.containerBuilder.RegisterType<IHttpContextAccessor>()
                .As<HttpContextAccessor>()
                .SingleInstance();

            this.containerBuilder.RegisterType<IMediatorHandler>()
                .As<InMemoryBus>()
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<IEventStoreRepository>()
                .As<EventStoreRepository>()
                .InstancePerRequest()
                .WithParameter(this.GetDbContextResolvedParameter<TDbContext>())
                .InstancePerLifetimeScope();

            this.containerBuilder.RegisterType<IEventUser>()
                .As<EventUser>()
                .InstancePerRequest();

            this.containerBuilder.RegisterType<IUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest()
                .WithParameter(this.GetDbContextResolvedParameter<TDbContext>())
                .InstancePerLifetimeScope();

            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation>()
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .SingleInstance();
            return this;
        }

        public ServiceContainerBuilder RegisterTypeAsSingleton<TImplementation, TInterface>() 
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .AsSelf()
                .SingleInstance();

            return this;
        }

        public ServiceContainerBuilder RegisterTypePerRequest<TImplementation, TInterface>()
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerRequest();

            return this;
        }

        public ServiceContainerBuilder RegisterTypePerDependency<TImplementation, TInterface>()
            where TImplementation : TInterface
        {
            this.containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .InstancePerDependency();

            return this;
        }

        public IContainer Build(IServiceCollection serviceCollection)
        {
            this.containerBuilder.Populate(serviceCollection);
            return this.containerBuilder
                .Build();
        }

        private ResolvedParameter GetDbContextResolvedParameter<TDbContext>() where TDbContext : DbContext
        {
            return new ResolvedParameter(
                (parameterInfo, componentContext) => parameterInfo.ParameterType == typeof(DbContext),
                (parameterInfo, componentContext) => componentContext.Resolve<TDbContext>());
        }
    }
}
