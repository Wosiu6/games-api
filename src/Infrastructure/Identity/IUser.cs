namespace Infrastructure.Identity
{
    public interface IUser
    {
        string? Id { get; set; }
        List<string>? Roles { get; set; }
    }
}
