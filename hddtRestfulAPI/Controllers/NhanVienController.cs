using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.IO;

namespace hddtRestfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly NhanVienRepository _repository;

        private IConfiguration _config;

        public NhanVienController(NhanVienRepository repository, IConfiguration config)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }


        #region JWT
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
             audience: _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            if (login.Username == "admin" && login.Password == "1")
            {
                user = new UserModel { Username = "Phan Phong", EmailAddress = "phong9x@gmail.com", Password = "123" };
            }
            return user;
        }
        #endregion

        // GET api/nhanvien
        [Authorize]
       [HttpGet]
        public async Task<List<NhanVien>> Get()
        {
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "GET",
                log = "Lấy toàn bộ danh sách Nhân Viên trong table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);
            
            return await _repository.GetAll();
        }

        //GET api/nhanvien/GetSignedContract
        [Authorize]
        [HttpGet]
        [Route("GetSignedContract")]
        public async Task<List<NhanVien>> GetSignedContract()
        {
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "GET",
                log = "Lấy toàn bộ danh sách Nhân Viên đã ký hợp đồng trong table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);
            return await _repository.GetSignedContract();
        }

        // GET api/nhanvien/5
        [Authorize]
        [HttpGet("{manhanvien}")]
        public async Task<ActionResult<NhanVien>> Get(int manhanvien)
        {
            var response = await _repository.GetByMaNhanVien(manhanvien);
            if (response == null) { return NotFound(); }
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "GET",
                log = "Lấy Nhân Viên theo mã nhân viên trong table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);

            return response;
        }

        // POST api/nhanvien
        [Authorize]
        [HttpPost]
        public async Task Post([FromBody] NhanVien nhanvien)
        {
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "POST",
                log = "Thêm nhân viên có mã nhân viên là "+ nhanvien.MaNhanVien.ToString() + " vào table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);
            await _repository.Insert(nhanvien);
        }

        // DELETE api/nhanvien/5
        [Authorize]
        [HttpDelete("{manhanvien}")]
        public async Task Delete(int manhanvien)
        {
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "DELETE",
                log = "Xóa nhân viên có mã nhân viên là " + manhanvien.ToString() + " vào table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);
            await _repository.DeleteByMaNhanVien(manhanvien);
        }

        // PUT api/nhanvien/5
        [Authorize]
        [HttpPut("{manhanvien}")]
        public async Task Update(int manhanvien, [FromBody] NhanVien nhanvien) 
        {
            var auditTrailLog = new RequestCustomerLog()
            {
                Action = "PUT",
                log = "Cập nhật nhân viên có mã nhân viên là " + manhanvien.ToString() + " vào table NhanVien",
                Timestamp = DateTime.Now
            };

            string text = auditTrailLog.Action + " | " + auditTrailLog.log + " | " + auditTrailLog.Timestamp;
            string path = @"D:\Thế giới di động\hddtRestfulAPI\log.txt";
            System.IO.File.AppendAllText(path, text + Environment.NewLine);
            await _repository.UpdateTheoMaNhanVien(manhanvien, nhanvien);
        }
    }
}
