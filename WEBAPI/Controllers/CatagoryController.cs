using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoAndService.Services.Custom.CatagoryServices;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatagoryController : ControllerBase
    {
        private readonly ICatagoryServices _serviceCatagory;
        public CatagoryController(ICatagoryServices serviceCatagory)
        {
            _serviceCatagory = serviceCatagory;
        }

        [Route("GetAllCatagory")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _serviceCatagory.GetAll();
            if (result == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            return Ok(result);
        }
        [Route("GetCatagory")]
        [HttpGet]
        public async Task<IActionResult> GetCatagory(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _serviceCatagory.Get(Id);
                if (result == null)
                    return BadRequest("Not Found");
               
                return Ok(result);
            }
            else
                return BadRequest("Invalid Catagory Id");
        }
        [Route("InsertCatagory")]
        [HttpPost]
        public async Task<IActionResult>InsertCatagory(CatagoryInsertModel insertModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceCatagory.Insert(insertModel);
                if (result == true)
                    return Ok("Category Inserted Successfully...!");
                else
                    return BadRequest("Something Went Wrong, Category Is Not Inserted, Please Try After Sometime...!");
            }
            else
                return BadRequest("Invalid Category Information, Please Entering a Valid One...!");
        }
        [Route("UpdateCatagory")]
        [HttpPut]
        public async Task<IActionResult> UpdateCatagory(CatagoryUpdateModel updateModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceCatagory.Update(updateModel);
                if (result == true)
                    return Ok("Category Inserted Successfully...!");
                else
                    return BadRequest("Something Went Wrong, Please Try After Sometime...!");
            }
            else
                return BadRequest("Invalid Category Information, Please Entering a Valid One...!");
        }
        [Route("DeleteCatagory")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCatagory(Guid Id)
        {
            var result = await _serviceCatagory.Delete(Id);
            if (result == true)
                return Ok("Category Deleted SUccessfully...!");
            else
                return BadRequest("Category is not deleted, Please Try again later...!");
    
        }

    }
}
