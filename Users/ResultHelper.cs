using Microsoft.AspNetCore.Http;
using Model.Other;

namespace Users.WebApi
{
    public class ResultHelper
    {
        public static ApiResult Success(object res)
        {
            return new ApiResult() { IsSuccess = true, Result = res };
        }
        public static ApiResult Error(string message)
        {
            return new ApiResult() { IsSuccess = false, Msg = message };
        }
    }
}
