using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;

namespace hddtRestfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinNhanVienController : ControllerBase
    {

        private readonly NhanVienRepository _repository;
        public ThongTinNhanVienController(NhanVienRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET api/thongtinnhanvien/1   
        [Authorize]
        [HttpGet("{manhanvien}")]
        public async Task<ActionResult<ThongTinNhanVien>> ThongTinNhanVien(int manhanvien)
        {
            var response = await _repository.GetThongTinNhanVienByMaNhanVien(manhanvien);
            if (response == null) { return NotFound(); }
            return response;
        }
    }
}
