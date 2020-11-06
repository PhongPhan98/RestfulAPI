using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class PhongBanRepository
    {
        private readonly string _connectionString;
        public PhongBanRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        // Lấy tên của các bộ phận theo tên công ty được truyền vào
        public async Task<List<TenBoPhan>> LayTenBoPhanTheoTenCty(string tencty)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("LayTenPhongBanTheoCty", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TenCty", tencty));
                    var response = new List<TenBoPhan>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToBoPhan(reader));
                        }
                    }
                    return response;
                }
            }
        }

        private TenBoPhan MapToBoPhan(SqlDataReader reader)
        {
            return new TenBoPhan()
            {
                TenBP = (string)reader["BoPhan"]
            };
        }


    }
}
