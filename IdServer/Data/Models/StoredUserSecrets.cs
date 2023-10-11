using System;

namespace IdServer.Data.Models;

public class StoredUserSecrets
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public Guid StoredUserId { get; set; }
    public StoredUser StoredUser { get; set; }
}
