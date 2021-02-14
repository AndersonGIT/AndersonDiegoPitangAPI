using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Handlers;
using AndersonDiego.Infra.Interfaces;
using AndersonDiego.Infra.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AndersonDiego.Models
{
    public class User
    {
        ResponseError _responseError;
        IUserRepository _userRepository;

        public User()
        {
            Phones = new List<Phone>();

            _responseError = new ResponseError();
            _userRepository = new UserRepository();
        }

        [JsonProperty("userId")]
        [JsonIgnore]
        public Int64 UserId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("phones")]
        public List<Phone> Phones { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        public ResponseError Validate()
        {
            string fieldsValidation = ValidateModelFields();
            _responseError.ContainsError = false;

            if (!string.IsNullOrEmpty(fieldsValidation))
            {
                fieldsValidation = HandlerError.GetErrorDescription(ConstantError.MISSING_FIELDS, fieldsValidation);
                _responseError.Message = fieldsValidation;
                _responseError.ErrorCode = ConstantError.MISSING_FIELDS;
                _responseError.ContainsError = true;
            }
            else if (_userRepository.VerifyIfExists(Email))
            {
                _responseError.Message = "E-mail already exists";
                _responseError.ErrorCode = ConstantError.EMAIL_ALREADY_EXISTS;
                _responseError.ContainsError = true;
            }
            else if (!ValidateEmail())
            {
                _responseError.Message = "Email is not valid";
                _responseError.ErrorCode = ConstantError.INVALID_EMAIL;
                _responseError.ContainsError = true;
            }

            return _responseError;
        }

        public string ValidateModelFields()
        {
            StringBuilder result = new StringBuilder();

            if (string.IsNullOrEmpty(FirstName))
                result.AppendLine("firstName;");
            if (string.IsNullOrEmpty(LastName))
                result.AppendLine("lastName;");
            if (string.IsNullOrEmpty(Email))
                result.AppendLine("email;");
            if (string.IsNullOrEmpty(Password))
                result.AppendLine("password;");
            if (Phones?.Count <= 0)
                result.AppendLine("phones;");

            foreach(Phone phone in Phones)
            {
                if (!phone.Number.HasValue || phone.Number.Value <= 0)
                    result.AppendLine("Phone -> Number;");
                if (string.IsNullOrEmpty(phone.CountryCode))
                    result.AppendLine("Phone -> CountryCode;");
                if (!phone.AreaCode.HasValue || phone.AreaCode.Value <= 0)
                    result.AppendLine("Phone -> AreaCode;");
            }

            return result.ToString();
        }

        public bool ValidateEmail()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            return match.Success;
        }

        public bool ValidatePhone()
        {
            return false;
        }
    }
}