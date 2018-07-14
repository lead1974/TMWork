using Microsoft.AspNetCore.Identity;
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
    public class MissionRepository:IMissionRepository
    {
        private TMDbContext _context;
        private ILogger<MissionRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;

        public MissionRepository(TMDbContext context,
            UserManager<AuthUser> userManager,
            ILogger<MissionRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(Mission newMission)
        {
            _context.Add(newMission);
        }

        public void Update(Mission theMission)
        {
            _context.Update(theMission);
        }

        public void Remove(Mission theMission)
        {
            _context.Remove(theMission);
        }

        public IEnumerable<Mission> GetAll()
        {
            try
            {
                return _context.Missions.OrderByDescending(t => t.DateCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get Missions from database", ex);
                return null;
            }
        }

        public Mission FindById(int Id)
        {
            return _context.Missions
                           .Where(p => p.MissionId == Id)
                           .FirstOrDefault();
        }

        public Mission FindByName(string Name)
        {
            return null;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
