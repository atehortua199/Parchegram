﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parchegram.Model.Common;
using Parchegram.Model.Request.User;
using Parchegram.Model.Response;
using Parchegram.Model.Response.Post;
using Parchegram.Model.User.Request;
using Parchegram.Service.Services.Interfaces;

namespace Parchegram.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        [HttpGet("GetFrase")]
        [Authorize]
        public string GetFrase()
        {
            return "Holaa";
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            Response response = new Response();
            if (ModelState.IsValid)
            {
                var userResponse = _userService.Login(loginRequest, _appSettings);
                if (userResponse == null)
                {
                    response.Message = "User does not exist";
                    response.Success = 0;
                    response.Data = userResponse;
                    return BadRequest(response);
                }

                response.Message = "Login success!";
                response.Success = 1;
                response.Data = userResponse;
            } else
            {
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            Response response = new Response();
            if (ModelState.IsValid)
            {
                var userResponse = _userService.Register(model, _appSettings);
                if (userResponse == null)
                {
                    response.Message = "User already exists";
                    response.Success = 0;
                    response.Data = userResponse;
                    return BadRequest(response);
                }

                response.Message = "Register succcess!";
                response.Success = 1;
                response.Data = userResponse;
            } else
            {
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpPost("ConfirmEmail")]
        public IActionResult ConfirmEmail([FromBody] string codeConfirmEmail)
        {
            if (_emailService.ConfirmEmail(codeConfirmEmail))
                return Ok();

            return BadRequest();
        }

        [HttpPost("UserExists")]
        public IActionResult UserExists([FromBody] LoginRequest loginRequest) 
        {
            bool result = _userService.UserExists(loginRequest);

            return Ok(result);
        }

        [HttpGet("EmailConfirmed/{nameUser}")]
        public IActionResult EmailConfirmed([FromRoute] string nameUser)
        {
            bool result = _userService.EmailConfirmed(nameUser);
            
            return Ok(result);
        }

        [HttpGet("NameUserUnique/{nameUser}")]
        public IActionResult NameUserUnique([FromRoute] string nameUser)
        {
            bool result = _userService.NameUserUnique(nameUser);

            return Ok(result);
        }

        [HttpGet("EmailUnique/{email}")]
        public IActionResult EmailUnique([FromRoute] string email)
        {
            bool result = _userService.EmailUnique(email);

            return Ok(result);
        }

        [HttpPost("UserConfig")]
        public IActionResult UserConfig([FromForm] ConfigUserRequest configUserRequest)
        {
            bool result = _userService.UserConfig(configUserRequest);

            return Ok();
        }
    }
}
