

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace DotnetProject.SampleApi.Domain.Common
{
    [Serializable]
    public abstract class Entity : Entity<int>;

    [Serializable]
    public abstract class Entity<TKey> : IEntity<TKey> where TKey : IComparable
    {
        /// <summary>
        /// Entity Identifier
        /// </summary>
        [Key]
        public virtual TKey Id { get; protected set; } = default!;

        /// <summary>
        /// Id getter
        /// </summary>
        /// <returns>Returns boxed id</returns>
        public virtual object GetId() => Id;

        /// <summary>
        /// Checks if entity is transient
        /// </summary>
        /// <returns>True if Id have its default value assigned</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TKey>.Default.Equals(Id, default))
                return true;

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TKey) == typeof(int))
                return Convert.ToInt32(Id, CultureInfo.InvariantCulture) <= 0;

            if (typeof(TKey) == typeof(long))
                return Convert.ToInt64(Id, CultureInfo.InvariantCulture) <= 0;

            return false;
        }

        /// <summary>
        /// Overriden equals method.
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>True if both objects have same identifier or same instance</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TKey> other)
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Transient objects are not considered as equal
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// Override of GetHashCode method
        /// </summary>
        /// <returns>Returns a hash code for current object</returns>
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }

        /// <summary>
        /// Override of equals (==) operator
        /// </summary>
        /// <param name="left">object to compare</param>
        /// <param name="right">object to compare</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/></returns>
        public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
        {
            return left?.Equals(right) ?? Equals(right, null);
        }

        /// <summary>
        /// Override of not equals (!=) operator
        /// </summary>
        /// <param name="left">object to compare</param>
        /// <param name="right">object to compare</param>
        /// <returns>True if <paramref name="left"/> not equals <paramref name="right"/></returns>
        public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Override of ToString method
        /// </summary>
        /// <returns>String in format of [TypeOfId Id]</returns>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
    }
}
