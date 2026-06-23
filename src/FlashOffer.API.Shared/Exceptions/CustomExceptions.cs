namespace FlashOffer.API.Shared.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException() : base() { }

	public NotFoundException(string message) : base(message) { }

	public NotFoundException(string message, Exception innerException)
		: base(message, innerException) { }

	public NotFoundException(string name, object key)
		: base($"Entity \"{name}\" ({key}) was not found.") { }

	public NotFoundException(Guid id)
		: base($"Entity with id '{id}' was not found.") { }

	public NotFoundException(Guid id, string entityName)
		: base($"Entity \"{entityName}\" with id '{id}' was not found.") { }
}