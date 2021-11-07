using GladosBank.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GladosBank.Services;
using GladosBank.Services.Exceptions;
using GladosBank.Api.Models.Args.UserControllerArgs;
using AutoMapper;
using GladosBank.Domain.Models_DTO;

namespace GladosBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController : ControllerBase
    {
        //TO DO fix bug with adding service of IMapper
        public UserController(ILogger<UserController> logger, UserService service,IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost(nameof(Create))]
        public IActionResult Create(CreateUserArgs user)
        {
            int newUserId = default;

            var localUser = _mapper.Map<User>(user.MyUser);

            try
            {
                newUserId = -_service.CreateUser(localUser, user.Role);
                if (newUserId==0)
                {
                    throw new InvalidUserSavingException("Exception occurs during a saving procedure");
                }
            }
            catch (AddingExistUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidRoleException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(ExistingUserLoginException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation("User was created sucessfuly");
            return Ok(newUserId);
        }
        [HttpPost(nameof(Login))]
        public IActionResult Login(LoginUserArgs largs)
        {
            return Ok();
        }

        [HttpPost(nameof(Delete))]
        public IActionResult Delete(DeleteUserArgs user)
        {
            try
            {
                _service.DeleteUser(user.UserId);
            }
            catch (InvalidUserIdException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("User was deleted sucessfuly");
            return Ok(user.UserId);
        }
        [HttpPost(nameof(Update))]
        public IActionResult Update(UpdateUserArgs user,string currentLogin)
        {


            //current login must be finded on it's own
            User currentUser = default;
            try
            {
               currentUser = _service.GetUserByLogin(currentLogin);
                if (!string.IsNullOrWhiteSpace(user.Login))
                {
                    currentUser.Login = user.Login;
                }
                if (!string.IsNullOrWhiteSpace(user.Phone))
                {
                    currentUser.Phone = user.Phone;
                }
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    currentUser.Email = user.Email;
                }
                

                _service.UpdateUser(currentUser.Id, currentUser);
            }
            catch (InvalidUserLoginException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidUserIdException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            _logger.LogInformation("User was updated sucessfuly");
            return Ok(currentUser?.Id);
        }
        [HttpGet(nameof(Get))]
        public  IActionResult Get()
        {
            IEnumerable<User> users = _service.GetAllUsers();
            return Ok(users);
        }

        private readonly IMapper _mapper;
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
    }
}
