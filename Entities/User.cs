using System;
using System.Collections.Generic;

namespace AddressBookAPI.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
}
