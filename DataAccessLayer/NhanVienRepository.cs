using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class NhanVienRepository
    {
        private readonly string _connectionString;
        public NhanVienRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        // Lấy danh sách nhân viên trong bang NhanVien
        public async Task<List<NhanVien>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<NhanVien>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToNhanVien(reader));
                        }
                    }
                    return response;
                }
            }
        }

        //Lấy những hợp đồng có tình trạng là "Đã Ký" trong bảng NhanVien
        public async Task<List<NhanVien>> GetSignedContract()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSignedContract", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<NhanVien>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToNhanVien(reader));
                        }
                    }
                    return response;
                }
            }
        }

        // Lấy một nhân viên dựa vào mã nhân viên trong bảng NhanVien
        public async Task<NhanVien> GetByMaNhanVien(int manhanvien)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetNhanVienByMaNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MaNhanVien", manhanvien));
                    NhanVien response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToNhanVien(reader);
                        }
                    }
                    return response;
                }
            }
        }

        // Thêm  1 nhân viên mới vào table NhanVien
        public async Task Insert(NhanVien nhanvien)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MaNhanVien", nhanvien.MaNhanVien));
                    cmd.Parameters.Add(new SqlParameter("@HoTen", nhanvien.HoTen));
                    cmd.Parameters.Add(new SqlParameter("@NgayVaoLam", nhanvien.NgayVaoLam));
                    cmd.Parameters.Add(new SqlParameter("@ChucVu", nhanvien.ChucVu));
                    cmd.Parameters.Add(new SqlParameter("@PhongBan", nhanvien.PhongBan));
                    cmd.Parameters.Add(new SqlParameter("@NgayKy", nhanvien.NgayKy));
                    cmd.Parameters.Add(new SqlParameter("@LoaiHopDong", nhanvien.LoaiHopDong));
                    cmd.Parameters.Add(new SqlParameter("@TinhTrang", nhanvien.TinhTrang));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Xóa nhân viên dựa vào Mã Nhân Viên
        public async Task DeleteByMaNhanVien(int manhanvien)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteByMaNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MaNhanVien", manhanvien));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        // Lấy thông tin một nhân viên trong bảng ThongTinNhanVien
        public async Task<ThongTinNhanVien> GetThongTinNhanVienByMaNhanVien(int manhanvien)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetThongTinNhanVienByMaNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MaNhanVien", manhanvien));
                    ThongTinNhanVien response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToThongTinNhanVien(reader);
                        }
                    }
                    return response;
                }
            }
        }

        // Cập nhật lại nhân viên trong bảng NhanVien
        public async Task UpdateTheoMaNhanVien(int manhanvien, NhanVien nhanvien)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateNhanVien", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MaNhanVien", manhanvien));
                    cmd.Parameters.Add(new SqlParameter("@HoTen", nhanvien.HoTen));
                    cmd.Parameters.Add(new SqlParameter("@NgayVaoLam", nhanvien.NgayVaoLam));
                    cmd.Parameters.Add(new SqlParameter("@ChucVu", nhanvien.ChucVu));
                    cmd.Parameters.Add(new SqlParameter("@PhongBan", nhanvien.PhongBan));
                    cmd.Parameters.Add(new SqlParameter("@NgayKy", nhanvien.NgayKy));
                    cmd.Parameters.Add(new SqlParameter("@LoaiHopDong", nhanvien.LoaiHopDong));
                    cmd.Parameters.Add(new SqlParameter("@TinhTrang", nhanvien.TinhTrang));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        #region Định dạng lại kiểu dữ liệu trả về
        private NhanVien MapToNhanVien(SqlDataReader reader)
        {
            return new NhanVien()
            {
                MaNhanVien = (int)reader["MaNhanVien"],
                HoTen = (string)reader["HoTen"],
                NgayVaoLam = (DateTime)reader["NgayVaoLam"],
                ChucVu = (string)reader["ChucVu"],
                PhongBan = (string)reader["PhongBan"],
                NgayKy = (DateTime)reader["NgayKy"],
                LoaiHopDong = (string)reader["LoaiHopDong"],
                TinhTrang = (string)reader["TinhTrang"],
            };
        }

        private ThongTinNhanVien MapToThongTinNhanVien(SqlDataReader reader)
        {
            return new ThongTinNhanVien()
            {
                MaNhanVien = (int)reader["MaNhanVien"],
                SinhNgay = (DateTime)reader["SinhNgay"],
                ChungMinhNhanDan = (string)reader["ChungMinhNhanDan"],
                NgayCap = (DateTime)reader["NgayCap"],
                NoiCap = (string)reader["NoiCap"],
                HoKhauThuongTru = (string)reader["HoKhauThuongTru"],
                DanToc = (string)reader["DanToc"],
                GioiTinh = (string)reader["GioiTinh"],
            };
        }
        #endregion
    }
}
