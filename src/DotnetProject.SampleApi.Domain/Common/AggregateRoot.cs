

using System;


namespace DotnetProject.SampleApi.Domain.Common
{
    public class AggregateRoot : AggregateRoot<int>;
    public class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey> where TKey : IComparable;

}
