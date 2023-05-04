namespace AddressBookAPI.DTO
{
    public class ContactDTO
    {
        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? MobilePhone { get; set; }

        public string? Email { get; set; }
    }
}
