﻿using AutoMapper;
using EntertainmentDatabase.REST.Domain.Entities;

namespace EntertainmentDatabase.REST.Domain.DataTransferObjects.Mappings
{
    public class MovieDTOMapping : Profile
    {
        public MovieDTOMapping()
        {
            this.CreateMap<Movie, MovieDTO>()
                .ForMember(destination => destination.ConcurrencyToken,
                    option => option.MapFrom(source => source.RowVersion))
                .ReverseMap();
            //.ForMember(destination => destination.ConsumerMediaType, option => option.MapFrom(source => source.));
            //.ReverseMap();
        }
    }
}
