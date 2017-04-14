﻿using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntertainmentDatabase.REST.API.ContextConfiguration
{
    public class MovieFileConfiguration : MediaMappingConfiguration<MovieFile>
    {
        protected override void Configure(EntityTypeBuilder<MovieFile> builder)
        {
            base.Configure(builder);
            // ddsad
        }
    }
}
