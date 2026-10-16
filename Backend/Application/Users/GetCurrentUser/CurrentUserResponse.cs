namespace Application.Users.GetCurrentUser;

public sealed class CurrentUserResponse
{
	public int UserId { get; set; }

	public string? Email { get; set; }

	public string? Name { get; set; }
}
