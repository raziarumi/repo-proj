using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.ViewModels;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace SummationOfBigNumbers.Controllers
{
    [Route("api/calculation")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        public CalculationController(ApplicationDataContext context, IServiceScopeFactory serviceScopeFactory, IUserRepository userRrepository, ICalculationRepository calculationRrepository)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _userRrepository = userRrepository;
            _calculationRrepository = calculationRrepository;

        }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationDataContext _context;
        private readonly IUserRepository _userRrepository;
        private readonly ICalculationRepository _calculationRrepository;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DataInputViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await Task.Run(() => CalCulateAsync(model));


                //var result = CalCulateAsync(model);
                return Ok(new { summation = result });

            }
            catch (Exception ex)
            {
                throw ex;
                return Ok(); ;

            }
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllResults()
        {
            try
            {
                var response = await Task.Run(() => GetAllAsync());

                return Ok(response);

            }
            catch (Exception ex)
            {
                throw ex;
                return Ok(); ;

            }
        }

        private async Task<List<SummationDataViewModel>> GetAllAsync()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {

                    var summationList = await _calculationRrepository.GetAllSummationResult();
                    return summationList;
                }
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }


        public async System.Threading.Tasks.Task<string> CalCulateAsync([FromBody]DataInputViewModel model)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    User user = null;
                    if (!String.IsNullOrEmpty(model.UserName))
                    {
                        long userId = 0;
                        user = await _userRrepository.GetByName(model.UserName);
                        if (user == null)
                        {
                            user = new User() { Name = model.UserName, CreatedDate = DateTime.Now };
                            await _userRrepository.Save(user);
                        }
                        userId = user.Id;
                        if (userId > 0)
                        {
                            string Summation = new CalculationService().GetSummation(model);
                            // "";
                            var calculation = new Calculation()
                            {
                                UserId = userId,
                                Number1 = model.Number1,
                                Number2 = model.Number2,
                                Summation = Summation,
                                CreatedDate = DateTime.Now
                            };
                            await _calculationRrepository.Save(calculation);
                            return Summation;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
                //throw;
            }
            return "";
        }

    }
}
