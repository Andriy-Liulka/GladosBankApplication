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
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserDTO, User>();
                }
                );
            IMapper mapper = new Mapper(config);

            var localUser = mapper.Map<User>(user.MyUser);
            try
            {
                newUserId = -_service.CreateUser(localUser);
            }
            catch (AddingExistUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation("User was created sucessfuly");
            return Ok(newUserId);
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

        [HttpGet(nameof(Get))]
        public  IActionResult Get()
        {
            User[] users = _service.GetAllUsers();
            return Ok(users);
        }

        private readonly IMapper _mapper;
        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;
    }
}
