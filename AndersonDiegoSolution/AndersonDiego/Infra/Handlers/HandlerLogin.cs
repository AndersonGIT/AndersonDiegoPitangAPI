using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Infra.Repositories;
using AndersonDiego.Models;

namespace AndersonDiego.Infra.Handlers
{
    public class HandlerLogin
    {
        ResponseError responseError;
        IUserRepository userRepository;

        // I.O.C

        //public HandlerLogin(ResponseError pResponseError, IUserRepository pUserRepository)
        //{
        //    responseError = pResponseError;
        //    userRepository = pUserRepository;
        //}

        public HandlerLogin()
        {
            responseError = new ResponseError();
            userRepository = new UserRepository();
        }

        public object Login(string pEmail, string pPassword)
        {
            object result = new object();

            if (string.IsNullOrWhiteSpace(pEmail) || string.IsNullOrWhiteSpace(pPassword))
            {
                responseError.ErrorCode = ConstantError.MISSING_FIELDS;
                responseError.Message = HandlerError.GetErrorDescription(ConstantError.MISSING_FIELDS, string.Empty);
                responseError.ContainsError = true;

                result = responseError;
            }

            User userFound = userRepository.Login(pEmail, pPassword);

            if (userFound?.UserId >= 1)
            {
                result = userFound;
            }
            else
            {
                responseError.ErrorCode = ConstantError.EMAIL_OR_PASSWD_NT_EXISTS;
                responseError.Message = HandlerError.GetErrorDescription(ConstantError.EMAIL_OR_PASSWD_NT_EXISTS, string.Empty);
                responseError.ContainsError = true;
                result = responseError;
            }

            return result;
        }
    }
}