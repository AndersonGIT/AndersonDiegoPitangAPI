using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Handlers;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Infra.Repositories;
using AndersonDiego.Models;
using System;
using System.Net;
using System.Web.Http;

namespace AndersonDiego.Controllers
{
    public class UserController : ApiController
    {
        // I.O.C

        //IHttpActionResult actionResult;
        //ResponseError responseError;
        //IUserRepository userRepository;
        //public UserController(IHttpActionResult pActionResult, ResponseError pResponseError, IUserRepository pUserRepository)
        //{
        //    actionResult = pActionResult;
        //    responseError = pResponseError;
        //    userRepository = pUserRepository;
        //}

        [HttpPost]
        [Route("api/signup")]
        public IHttpActionResult SignUp(User pUser)
        {
            try
            {
                Response response = new Response();
                ResponseError responseError;
                IUserRepository userRepository = new UserRepository();

                responseError = pUser.Validate();

                if (responseError.ContainsError)
                    return Content(HttpStatusCode.BadRequest, responseError);
                else if (userRepository.Insert(pUser)){
                    response.Message = "Signed Up Successfuly.";
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/signin")]
        public IHttpActionResult SignIn(Login pLogin)
        {
            try
            {
                Response response = new Response();
                HandlerLogin handlerLogin = new HandlerLogin();
                string result = string.Empty;

                object objectLogin = handlerLogin.Login(pLogin.Email, pLogin.Password);

                if (objectLogin is ResponseError loginError)
                {
                    if (loginError.ContainsError)
                        return Content(HttpStatusCode.BadRequest, loginError);
                }
                else if (objectLogin is User user)
                {
                    response.Message = $"The user: {user.FirstName}, has just SignedIn Successfuly.";

                    if (!user.LastLogin.Equals(default(DateTime)))
                        response.Message += $" LastLogin ocurred at: {user.LastLogin}";
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/me")]
        public IHttpActionResult Me()
        {
            //TODO: Neeed to implement authentication for taking it at this Endpoint.
            //The beyond implementation is simulating an existing User with Id 1, for taking it.

            try
            {
                ResponseError responseError;
                IUserRepository userRepository = new UserRepository();

                if (Request.Headers.Authorization != null)
                {
                    User user = userRepository.GetUserById(1);
                    if (user == null)
                    {
                        responseError = new ResponseError()
                        {
                            ErrorCode = ConstantError.USER_NOT_FOUND,
                            Message = HandlerError.GetErrorDescription(ConstantError.USER_NOT_FOUND, string.Empty),
                            ContainsError = true
                        };
                        return Content(HttpStatusCode.NotFound, responseError);
                    }
                    else return Json(user);
                }
                else
                {
                    responseError = new ResponseError()
                    {
                        ErrorCode = ConstantError.UNAUTHORIZED,
                        Message = HandlerError.GetErrorDescription(ConstantError.UNAUTHORIZED, string.Empty),
                        ContainsError = true
                    };

                    return Content(HttpStatusCode.Unauthorized, responseError);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
