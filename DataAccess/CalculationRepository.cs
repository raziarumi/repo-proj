using Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CalculationRepository : GenericRepository<Calculation>, ICalculationRepository
    {
        private readonly ApplicationDataContext context;
        public CalculationRepository(ApplicationDataContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<SummationDataViewModel>> GetAllSummationResult()
        {
            var calculations = (from calc in context.Calculations
                                join usr in context.Users on calc.UserId equals usr.Id
                                select new SummationDataViewModel
                                {
                                    CreatedDate = calc.CreatedDate,
                                    UserName = usr.Name,
                                    Number1 = calc.Number1,
                                    Number2 = calc.Number2,
                                    Sum = calc.Summation
                                });
            return await calculations.ToListAsync();
        }
    }
}
