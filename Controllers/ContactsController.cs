using AddressBookAPI.DTO;
using AddressBookAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddressBookAPI.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AddressBookContext DBContext;

        public ContactsController(AddressBookContext DBContext)
        {
            this.DBContext = DBContext;
            // Check for default user
            Console.WriteLine("Checking default user");
            User defaultUser = DBContext.Users.First(u => u.UserId == 1);
            bool noDefault = defaultUser == null;
            if (noDefault)
            {
                defaultUser = new User()
                {
                    UserId = 1,
                };
                DBContext.Users.Add(defaultUser);
            }
            if (defaultUser != null)
            {
                defaultUser.FirstName = "Default";
                defaultUser.LastName = "User";
            }
            DBContext.SaveChanges();
        }

        // GET: api/contacts
        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <param name="search">Search string to be used as filter</param>
        /// <returns>List of contacts satisfying the search string if it is given, all contacts otherwise</returns>
        [HttpGet]
        public IEnumerable<Contact> Get([FromQuery] string? search)
        {
            return DBContext.Contacts.Where(c => (
                    search == null ||
                    c.Name.Contains(search) ||
                    c.Address.Contains(search) ||
                    c.Phone.Contains(search) ||
                    c.MobilePhone != null && c.MobilePhone.Contains(search) ||
                    c.Email != null && c.Email.Contains(search)
                )).Select(c => new Contact()
                {
                    UserId = c.UserId,
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    MobilePhone = c.MobilePhone,
                    Email = c.Email,
                }).ToList();
        }

        // POST api/contacts
        /// <summary>
        /// Adds new contact if the following conditions are satisfied:
        /// - Required name, address and phone fields must be given
        /// - No contact must exist having the given name
        /// - If email is given, it must be valid
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost]
        public void Post([FromBody] ContactDTO dto)
        {
            try
            {
                // Validate email (if given)
                if (dto.Email != null)
                {
                    new MailAddress(dto.Email);
                }

                Contact newContact = new Contact()
                {
                    UserId = 1,
                    Name = dto.Name,
                    Address = dto.Address,
                    Phone = dto.Phone,
                    MobilePhone = dto.MobilePhone,
                    Email = dto.Email,
                };

                DBContext.Contacts.Add(newContact);
                DBContext.SaveChanges();
            } 
            catch (FormatException)
            {
                // Email validation failed
                throw new BadHttpRequestException("Wrong Email Format");
            }
        }

        // PUT api/contacts/5
        /// <summary>
        /// Updates the contact having the given id with the given info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ContactDTO dto)
        {
            try
            {
                // Validate email (if given)
                if (dto.Email != null)
                {
                    new MailAddress(dto.Email);
                }
                Contact contactToUpdate = DBContext.Contacts.First(c => c.ContactId == id);
                // Check if the contact really exists
                if (contactToUpdate == null)
                {
                    throw new BadHttpRequestException("Contact does not exist", (int) HttpStatusCode.UnprocessableEntity);
                }
                contactToUpdate.Name = dto.Name;
                contactToUpdate.Address = dto.Address;
                contactToUpdate.Phone = dto.Phone;
                contactToUpdate.MobilePhone = dto.MobilePhone;
                contactToUpdate.Email = dto.Email;

                DBContext.SaveChanges();
            }
            catch (FormatException e)
            {
                // Email validation failed
                throw new BadHttpRequestException("Wrong Email Format", e);
            }
        }

        // DELETE api/contacts/5
        /// <summary>
        /// Updates the contact having the given id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var contact = new Contact()
            {
                ContactId = id,
            };
            DBContext.Contacts.Attach(contact);
            DBContext.Contacts.Remove(contact);
            DBContext.SaveChanges();
        }
    }
}
