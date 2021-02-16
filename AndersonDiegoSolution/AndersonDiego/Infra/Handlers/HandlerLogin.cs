using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Infra.Repositories;
using AndersonDiego.Models;

namespace AndersonDiego.Infra.Handlers
{
    public class HandlerLogin
    {
        ResponseError _ResponseError;
        IUserRepository _UserRepository;

        public HandlerLogin(ResponseError pResponseError, IUserRepository pUserRepository)
        {
            _ResponseError = pResponseError;
            _UserRepository = pUserRepository;
        }

        public object Login(string pEmail, string pPassword)
        {
            object result = new object();

            if (string.IsNullOrWhiteSpace(pEmail) || string.IsNullOrWhiteSpace(pPassword))
            {
                _ResponseError.ErrorCode = ConstantError.MISSING_FIELDS;
                _ResponseError.Message = HandlerError.GetErrorDescription(ConstantError.MISSING_FIELDS, string.Empty);
                _ResponseError.ContainsError = true;

                result = _ResponseError;
            }

            User userFound = _UserRepository.Login(pEmail, pPassword);

            if (userFound?.UserId >= 1)
            {
                result = userFound;
            }
            else
            {
                _ResponseError.ErrorCode = ConstantError.EMAIL_OR_PASSWD_NT_EXISTS;
                _ResponseError.Message = HandlerError.GetErrorDescription(ConstantError.EMAIL_OR_PASSWD_NT_EXISTS, string.Empty);
                _ResponseError.ContainsError = true;
                result = _ResponseError;
            }

            return result;
        }
    }
}