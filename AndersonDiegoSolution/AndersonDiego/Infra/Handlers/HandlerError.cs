using AndersonDiego.Infra.Constants;
using System;

namespace AndersonDiego.Infra.Handlers
{
    public static class HandlerError
    {
        public static void GenerateError(int pCodeError, string pMessageInfo)
        {
            throw new Exception(GetErrorDescription(pCodeError, pMessageInfo));
        }

        public static string GetErrorDescription(int pCodeError, string pMessageInfo)
        {
            string result = string.Empty;
            switch (pCodeError)
            {

                case ConstantError.MISSING_FIELDS:
                    result = "Missing Fields";
                    break;
                case ConstantError.EMAIL_ALREADY_EXISTS:
                    result = "E-mail already exists";
                    break;
                case ConstantError.INVALID_FIELDS:
                    result = "Invalid fields";
                    break;
                case ConstantError.INVALID_EMAIL:
                    result = "E-mail is not valid";
                    break;
                case ConstantError.EMAIL_OR_PASSWD_NT_EXISTS:
                    result = "Invalid e-mail or password";
                    break;
                case ConstantError.UNAUTHORIZED:
                    result = "Unauthorized";
                    break;
                case ConstantError.USER_NOT_FOUND:
                    result = "User was not found";
                    break;
                case ConstantError.UNAUTHORIZED_INVALID_SESSION:
                    result = "Unauthorized - invalid session";
                    break;

                default:
                    result = string.Empty;
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(pMessageInfo))
                result += string.Format(" {0}", pMessageInfo);

            return result;
        }        
    }
}