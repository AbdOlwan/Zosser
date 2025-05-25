using BLL_OnlineStore.Interfaces;
using DAL_OnlineStore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Services
{
    public class CultureService : ICultureService
    {
        private readonly AppDbContext _dbContext;
        public CultureService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string CurrentCulture
            => _dbContext.CurrentCulture;

        public void SetCulture(string culture)
        {
            _dbContext.CurrentCulture = culture;
        }
    }

}
