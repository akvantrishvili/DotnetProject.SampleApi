

using System;

namespace DotnetProject.SampleApi.Domain.Common
{
    public interface IEntity
    {
        bool IsTransient();

        object GetId();
    }

    public interface IEntity<out TKey> : IEntity
        where TKey : IComparable
    {
        TKey Id { get; }
    }
}
