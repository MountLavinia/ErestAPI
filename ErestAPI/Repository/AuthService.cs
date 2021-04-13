using Dapper;
using ErestAPI.Models;
using ErestAPI.Models.CommonModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ErestAPI.Repository
{
    public class AuthService
    {
        HttpRequest _request;

        private readonly string _connectionString;
        public AuthService(string connectionString)
        {
            this._connectionString = connectionString;

        }
        public void SetRequest(HttpRequest httpRequest)
        {
            _request = httpRequest;
        }

        //Select
        public List<UserModel> SelectCurrentUser(UserModel data)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                DynamicParameters para = new DynamicParameters();
                string JsonData = JsonConvert.SerializeObject(data);
                para.Add("@jsonData", JsonData);
                var result = connection.Query<UserModel>("[dbo].[SelectCurrentUser]", para, commandType: CommandType.StoredProcedure).ToList<UserModel>();
                return result;
                //return new BaseResponseService().GetSuccessResponse(result);

                //return new BaseResponseService().GetErrorResponse(ex);


            }
        }
        public async Task<BaseResponse> RegisterUser(UserModel data)
        {
            try
            {
                data.APIURL = _request.HttpContext.Request.Host.Host + ":" + _request.HttpContext.Request.Host.Port + _request.HttpContext.Request.Path.Value;
                data.ClientIp = _request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
              
                using (var connection = new SqlConnection(_connectionString))
                {
                    DynamicParameters para = new DynamicParameters();
                    string JsonData = JsonConvert.SerializeObject(data);
                    para.Add("@JsonData", JsonData, DbType.String);
                    para.Add("@Operation", "I", DbType.String);
                    para.Add("@SkipApproval", false, DbType.Boolean);

                    await connection.ExecuteAsync("[dbo].[Register]", para, commandType: CommandType.StoredProcedure);

                    return new BaseResponseService().GetSuccessResponse();
                }
            }
            catch (SqlException ex)
            {
                
                return new BaseResponseService().GetErrorResponse(ex);
            }
            catch (Exception ex)
            {
                
                return new BaseResponseService().GetErrorResponse(ex);
            }
        }
    }
}
