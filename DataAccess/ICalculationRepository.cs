using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ICalculationRepository : IGenericRepository<Calculation>
    {
        Task<List<SummationDataViewModel>> GetAllSummationResult();
    }
}
