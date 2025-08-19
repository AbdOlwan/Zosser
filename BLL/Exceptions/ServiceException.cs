namespace BLL.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
