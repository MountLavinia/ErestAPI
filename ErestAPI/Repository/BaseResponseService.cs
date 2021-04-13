using ErestAPI.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ErestAPI.Repository
{
    public class BaseResponseService
    {
        public BaseResponse GetSuccessResponse()
        {
            return new BaseResponse() { Success = true, Message = "Success", ErrorType = "NA" };
        }

        public BaseResponse GetSuccessResponse(object data)
        {
            return new BaseResponse() { Success = true, Message = "Success", ErrorType = "NA", Data = data };
        }

        public BaseResponse GetErrorResponse(SqlException ex)
        {
            if (ex.Number == 50005)
            {
                return new BaseResponse() { Success = false, Message = ex.Message, ErrorType = "VAL", Data = ex, ExceptionNumber = ex.Number };
            }

            return GetErrorResponse((Exception)ex);
        }

        public BaseResponse GetErrorResponse(Exception ex)
        {
            return new BaseResponse() { Success = false, Message = "Action will be canceled!", ErrorType = "EX" };
        }
    }
}
