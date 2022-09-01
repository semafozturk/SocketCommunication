using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocketCommunication.Api.Infrastructure;
using SocketCommunication.Api.Models;

namespace SocketCommunication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAllUsers());
        }
        [HttpGet("[action]")]
        public IActionResult GetById(string tc)
        {
            return Ok(_userService.GetById(tc));
        }
        [HttpGet("[action]")]
        public IActionResult GetByNameSurname(string name,string surname)
        {
            return Ok(_userService.GetByNameSurname(name,surname));
        }
        [HttpPost]
        public IActionResult InsertUser(User user)
        {
            _userService.InsertUser(user);
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            _userService.UpdateUser(user);
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteUser(User user)
        {
            _userService.DeleteUser(user);
            return Ok();
        }


    }
}
