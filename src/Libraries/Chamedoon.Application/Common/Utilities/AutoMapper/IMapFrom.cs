using AutoMapper;

namespace Chamedoon.Application.Common.Utilities.AutoMapper;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}

