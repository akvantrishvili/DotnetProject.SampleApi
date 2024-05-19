

using System;

namespace DotnetProject.SampleApi.Domain.Common
{
    public interface IAggregateRoot : IEntity
    {
    }

    public interface IAggregateRoot<out TKey> : IAggregateRoot, IEntity<TKey> where TKey : IComparable
    {
    }
}
