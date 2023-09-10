
public class BadRequestException : Exception
{

	public BadRequestException(string message) : base(message)
	{ }
	public BadRequestException(string message, Exception? innerExpn) : base(message, innerExpn)
	{ }
}
