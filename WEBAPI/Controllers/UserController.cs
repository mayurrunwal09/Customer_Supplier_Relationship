using Azure.Core;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoAndService.Services.Custom.Customer_Service;
using RepoAndService.Services.Custom.SupplierServices;
using RepoAndService.Services.Custom.UserTypeService;
using System.Text.RegularExpressions;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISupplierService _supplierService;
        private readonly ICustomerService _customerService;
        private readonly IUserTypeService _UserTypeService;

        public UserController(ILogger<UserController> logger, IWebHostEnvironment webHostEnvironment, ISupplierService supplierService, ICustomerService customerService, IUserTypeService userTypeService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _supplierService = supplierService;
            _customerService = customerService;
            _UserTypeService = userTypeService;
        }


        [HttpGet(nameof(GetAllSupplier))]
        public async Task<ActionResult<UserViewModel>> GetAllSupplier()
        {
            var result = await _supplierService.GetAll();
            if (result == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            return Ok(result);

        }

        [HttpGet(nameof(GetSupplier))]

        public async Task<IActionResult> GetSupplier(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _supplierService.Get(Id);
                if (result == null)
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");
                return Ok(result);
            }
            else
                return NotFound("Invalid Supplier Id, Please Entering a Valid One...!");
        }
        [HttpPut(nameof(EditSupplier))]
        public async Task<IActionResult> EditSupplier([FromForm] UserUpdateModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var checkuser = await _supplierService.Find(x=>x.UserCode == customerModel.UserCode && x.Id != customerModel.Id );
                if(checkuser != null)
                {
                    return BadRequest("UserId:  " + customerModel.UserCode + " AlreadyExist");
                }
                else
                {
                    var checkusername = await _supplierService.Find(x=>x.UserName == customerModel.UserName && x.Id != customerModel.Id);
                    if(checkusername != null)
                    {
                        return BadRequest("UserId:  " + customerModel.UserCode + " AlreadyExist");
                    }
                }
                if(customerModel.Image != null)
                {
                    var photo = await UploadPhoto(customerModel.Image, customerModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                        return BadRequest("Error while uploading supplier photo");
                    var result = await _supplierService.Update(customerModel, photo);
                    if (result == true)
                        return Ok("Photo uploaded Successfully");
                    else
                        return BadRequest("something went wrong");                    
                }
                else
                {
                    var result = await _supplierService.Update(customerModel, " ");
                    if (result == true)
                        return Ok("Supplier Update Successfully");
                    else
                        return BadRequest("Something went wrong");
                }
            }
            else
                return BadRequest("Supplier not found with Id: "+customerModel.Id );
        }
        [HttpDelete(nameof(DeleteSupplier))]
        public async Task<IActionResult >DeleteSupplier(Guid Id)
        {
            var result = await _supplierService.Delete(Id);
            if (result == true)
                return Ok(result);
            else return BadRequest("Record not found");
        }


        // customer section
        [HttpGet(nameof(GetAllCustomer))]
        public async Task<ActionResult<UserViewModel>> GetAllCustomer()
        {
            var result = await _customerService.GetAll();
                if (result == null)
                return BadRequest("No record found");
                else
                return Ok(result);
        }
        [HttpGet(nameof(GetCustomer))]
        public async Task<IActionResult >GetCustomer(Guid Id)
        {
            if(Id!=Guid.Empty)
            {
                var result = await _customerService.Get(Id);
                if (result == null)
                    return BadRequest("No record found");
                return Ok(result);
            }
            return NotFound("Invalid customerId");
        }
        [HttpPut(nameof(EditCustomer))]
        public async Task<IActionResult> EditCustomer([FromForm] UserUpdateModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var checkuser = await _customerService.Find(x=>x.UserCode == customerModel.UserCode && x.Id!=customerModel.Id);
                if (checkuser != null)
                {
                    return BadRequest("UserId : " + customerModel.Id + " already exits");
                }
                    
                else
                {
                    var checkusername = await _customerService.Find(x=>x.UserName == customerModel.UserName && x.Id!=customerModel.Id);
                    if(checkusername != null)
                    {
                        return BadRequest("UserName: " + customerModel.UserName + " already exist");
                    }
                    
                }
                if(customerModel.Image!= null)
                {
                    var photo = await UploadPhoto(customerModel.Image, customerModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                    if (string.IsNullOrEmpty(photo))
                        return BadRequest("Error while uploading customer photo");
                    var result = await _customerService.Update(customerModel,photo);
                    if (result == true)
                        return Ok("Customer updated successfully");
                    else
                        return BadRequest("Something went wrong");
                }
                else
                {
                    var result = await _customerService.Update(customerModel," ");
                    if (result == true)
                        return Ok("Customer updated successfully");
                    else
                        return BadRequest("Something went wrong");
                }
            }
            else
                return NotFound("Customer not found with id: "+customerModel.Id);
        }
        [HttpDelete(nameof(DeleteCustomer))]
        public async Task<IActionResult >DeleteCustomer(Guid Id)
        {
            var result = await _customerService.Delete(Id);
            if (result == true)
                return Ok(result);
            else
                return BadRequest("Something Went wrong");
        }

        //Image upload section
        private async Task<string >UploadPhoto(IFormFile file,string Id,string Date)
        {
            string fileName;
            Console.WriteLine(Id);
            _logger.LogInformation("Started uploading User Profile Photo");

            string contentPath = this._webHostEnvironment.ContentRootPath;
            var extension = "." + file.FileName.Split('.')[^1];
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg")
            {
                fileName = Id.ToLower() + "-" + Date + extension;
                string outputFileName = Regex.Replace(fileName, @"[^0-9a-zA-Z.]+", "");
                var pathBuilt = Path.Combine(contentPath, "Images\\User");

                if (!Directory.Exists(pathBuilt))
                    Directory.CreateDirectory(pathBuilt);
                var path = Path.Combine(contentPath, "Images\\User", outputFileName);

                Console.WriteLine(path);

                using (var stream = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(stream);
                _logger.LogInformation("Successfully uploaded User photo with the file Name : " + outputFileName);
                return outputFileName;
            }
            else
                return "";
        }
    }
}
