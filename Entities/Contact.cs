using System;
using System.Collections.Generic;

namespace AddressBookAPI.Entities;

public partial class Contact
{
    public int ContactId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? MobilePhone { get; set; }

    public string? Email { get; set; }

    public virtual User User { get; set; } = null!;
}
