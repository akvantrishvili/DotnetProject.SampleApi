

using System;

namespace DotnetProject.SampleApi.Application.Exceptions
{
    public class ObjectAlreadyExistsException(object objectId, string objectType, Exception? innerException = null)
        : ApplicationException("ApplicationError",
           "Object already exists",
            $"{objectType}:{objectId} already exists",
            innerException)
    {
        public string ObjectType { get; private set; } = objectType;
        public object ObjectId { get; private set; } = objectId;
    }

    public class ObjectAlreadyExistsException<T>(object objectId, Exception? innerException = null)
        : ObjectAlreadyExistsException(objectId, typeof(T).Name, innerException);
}

