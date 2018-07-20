using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMWork.Data.Models.Team;
using TMWork.Data.Models.User;

namespace TMWork.Data.Repos
{
    public class MemberRepository:IMemberRepository
    {
        private TMDbContext _context;
        private ILogger<MemberRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;

        public MemberRepository(TMDbContext context,
            UserManager<AuthUser> userManager,
            ILogger<MemberRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(Member newMember)
        {
            _context.Add(newMember);
        }

        public void Update(Member theMember)
        {
            _context.Update(theMember);
        }

        public void Remove(Member theMember)
        {
            _context.Remove(theMember);
        }

        public IEnumerable<Member> GetAll()
        {
            try
            {
                return _context.Members.OrderByDescending(t => t.DateCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get Members from database", ex);
                return null;
            }
        }

        public Member FindById(int Id)
        {
            return _context.Members
                           .AsNoTracking()
                           .Where(p => p.MemberId == Id)
                           .FirstOrDefault();
        }

        public Member FindByName(string Name)
        {
            return _context.Members
                           .AsNoTracking()
                           .Where(p => p.Name.Contains(Name))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
