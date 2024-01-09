using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Chamedoon.Application.Common.Utilities.AutoMapper;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static PaginatedList<TDestination> PaginatedListAsync<TDestination>(this List<TDestination> list, int pageNumber, int pageSize) where TDestination : class
    {
        return PaginatedList<TDestination>.Create(list, pageNumber, pageSize);
    }

    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
    (this IMappingExpression<TSource, TDestination> expression)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;
        var sourceType = typeof(TSource);
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        foreach (var property in destinationProperties)
        {
            if (sourceType.GetProperty(property.Name, flags) == null)
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
        }
        return expression;
    }



}
