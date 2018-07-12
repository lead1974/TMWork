using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMWork.Data.Models.Customer;
using TMWork.Data.Models.User;

namespace TMWork.Data.Repos
{
    public class ContactRepository:IContactRepository
    {
        private TMDbContext _context;
        private ILogger<ContactRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;

        public ContactRepository(TMDbContext context,
            UserManager<AuthUser> userManager,
            ILogger<ContactRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(Contact newContact)
        {
            _context.Add(newContact);
        }

        public void Update(Contact theContact)
        {
            _context.Update(theContact);
        }

        public void Remove(Contact theContact)
        {
            _context.Remove(theContact);
        }

        public IEnumerable<Contact> GetAll()
        {
            try
            {
                return _context.Contacts.OrderByDescending(t => t.DateCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get Contacts from database", ex);
                return null;
            }
        }

        public Contact FindById(int Id)
        {
            return _context.Contacts
                           .Where(p => p.ContactId == Id)
                           .FirstOrDefault();
        }

        public Contact FindByName(string Name)
        {
            return _context.Contacts
                           .Where(p => p.Name.Contains(Name))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
