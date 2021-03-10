namespace TestWebService.DA
{
    using System;

    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, long id, Exception innerException)
            : base($"{entity} with id={id} not found", innerException) { }
    }
}