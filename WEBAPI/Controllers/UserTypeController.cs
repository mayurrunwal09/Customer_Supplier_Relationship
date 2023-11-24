using Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoAndService.Services.Custom.UserTypeService;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeService _serviceUserType;
        public UserTypeController(IUserTypeService serviceUserType)
        {
            _serviceUserType = serviceUserType;
        }

        [Route("GetAllUserType")]
        [HttpGet]
        public async Task<ActionResult<UserTypeViewModel>> GetAllUserType()
        {
            var result = await _serviceUserType.GetAll();
            if (result == null)
            {
                return BadRequest("No Records found ");
            }
                return Ok(result);
            
        }

        [Route("GetUserType")]
        [HttpGet]
        public async Task<ActionResult<UserTypeViewModel>> GetUserType(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceUserType.Get(Id);
                if(result == null)
                
                    return BadRequest("Not Found");                
                return Ok(result);
            }
            else           
                return NotFound();
            
        }

        [Route("InsertUserType")]
        [HttpPost]
        public async Task<IActionResult> InsertUserType(UserTypeInsertModel userTypeInsertModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUserType.Insert(userTypeInsertModel);
                if (result == true)
                    return Ok("UserType inserted");
                else
                    return BadRequest("Something went wrong");
            }
            else
                return BadRequest("Invalid User Information");
        }

        [Route("UpdateUserType")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserType(UserTypeUpdateModel userTypeModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceUserType.Update(userTypeModel);
                if (result == true)
                    return Ok("Usertype updated");
                else
                    return BadRequest("Something went wrong");
            }
            else
                return BadRequest("Invalid User Information");
        }
        [Route("DeleteUserType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserType(Guid Id)
        {
            
             var result = await _serviceUserType.Delete(Id);
            if (result == true)
                return Ok("UserType Deleted Successfully...!");
            else
                return BadRequest("UserType is not deleted, Please Try again later...!");

        }

    }
}
