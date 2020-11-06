using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer;
using Domain;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace hddtRestfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongBanController : ControllerBase
    {
        private readonly PhongBanRepository _repository;
        public PhongBanController(PhongBanRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET api/phongban/N'CNTT'
        [Authorize]
        [HttpGet("{tencty}")]
        public async Task<List<TenBoPhan>> LayTenBoPhanTheoTenCty(string tencty)
        {   
            return await _repository.LayTenBoPhanTheoTenCty(tencty);
        }
    }
}
