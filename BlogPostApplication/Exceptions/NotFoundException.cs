namespace BlogPostApplication.Exceptions
{
    public class NotFoundException(string entityName, int id) : Exception($"Could not find {entityName} with id {id}")
    {
    }
}
