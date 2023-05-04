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
        }

        // GET: api/contacts
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
                )).ToList();
        }

        // POST api/contacts
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
