

using System;

namespace DotnetProject.SampleApi.Application.Exceptions
{
    public class ObjectNotFoundException(object objectId, string objectType, Exception? innerException = null)
        : ApplicationException("ApplicationError",
            "Requested object was not found",
            $"{objectType}: {objectId} not found",
            innerException)
    {
        public string ObjectType { get; private set; } = objectType;
        public object ObjectId { get; private set; } = objectId;
    }

    public class ObjectNotFoundException<T>(object objectId, Exception? innerException = null)
        : ObjectNotFoundException(objectId, typeof(T).Name, innerException);
}

