using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Handlers;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Models;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AndersonDiego.Controllers
{
    public class UserController : ApiController
    {
        private IUserRepository _UserRepository;
        private Response _Response;
        private ResponseError _ResponseError;
        private HandlerLogin _HandlerLogin;

        public UserController(IUserRepository pUserRepository, Response pResponse, ResponseError pResponseError, HandlerLogin pHandlerLogin)
        {
            _UserRepository = pUserRepository;
            _Response = pResponse;
            _ResponseError = pResponseError;
            _HandlerLogin = pHandlerLogin;
        }

        [HttpPost]
        [Route("api/signup")]
        public IHttpActionResult SignUp(User pUser)
        {
            try
            {
                _ResponseError = pUser.Validate();

                if (_ResponseError.ContainsError)
                    return Content(HttpStatusCode.BadRequest, _ResponseError);
                else if (_UserRepository.Insert(pUser))
                {
                    _Response.Message = "Signed Up Successfuly.";
                }

                return Json(_Response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[HttpPost]
        //[Route("api/signin")]
        //public IHttpActionResult SignIn(Login pLogin)
        //{
        //    try
        //    {
        //        object objectLogin = _HandlerLogin.Login(pLogin.Email, pLogin.Password);

        //        if (objectLogin is ResponseError loginError)
        //        {
        //            if (loginError.ContainsError)
        //                return Content(HttpStatusCode.BadRequest, loginError);
        //        }
        //        else if (objectLogin is User user)
        //        {
        //            _Response.Message = $"The user: {user.FirstName}, has just SignedIn Successfuly.";

        //            if (!user.LastLogin.Equals(default(DateTime)))
        //                _Response.Message += $" LastLogin ocurred at: {user.LastLogin}";
        //        }

        //        return Json(_Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //[HttpPost]
        //[Route("api/me")]
        //[Authorize]
        //public IHttpActionResult Me()
        //{
        //    //TODO: Neeed to implement authentication for taking it at this Endpoint.
        //    //The beyond implementation is simulating an existing User with Id 1, for taking it.

        //    try
        //    {
        //        if (Request.Headers.Authorization != null)
        //        {
        //            User user = _UserRepository.GetUserById(1);
        //            if (user == null)
        //            {
        //                _ResponseError.ErrorCode = ConstantError.USER_NOT_FOUND;
        //                _ResponseError.Message = HandlerError.GetErrorDescription(ConstantError.USER_NOT_FOUND, string.Empty);
        //                _ResponseError.ContainsError = true;

        //                return Content(HttpStatusCode.NotFound, _ResponseError);
        //            }
        //            else return Json(user);
        //        }
        //        else
        //        {
        //            _ResponseError.ErrorCode = ConstantError.UNAUTHORIZED;
        //            _ResponseError.Message = HandlerError.GetErrorDescription(ConstantError.UNAUTHORIZED, string.Empty);
        //            _ResponseError.ContainsError = true;

        //            return Content(HttpStatusCode.Unauthorized, _ResponseError);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}


        [HttpPost]
        [Route("api/me")]
        [Authorize]
        public IHttpActionResult Me()
        {
            try
            {
                User user = null;

                IOwinContext owinContext = Request.GetOwinContext();
                var userSerial = owinContext.Authentication.User.Claims.FirstOrDefault(c => c.Type == Constant.CLAIM_USER_OBJECT);

                if (userSerial != null)
                {
                    user = JsonConvert.DeserializeObject<User>(userSerial.Value);
                    return Json(user);
                }
                else
                    return Content(HttpStatusCode.BadRequest, "Error trying to obtain the user.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
