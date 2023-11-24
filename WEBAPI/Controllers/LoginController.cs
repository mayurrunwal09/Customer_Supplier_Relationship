using Domain.Helpers;
using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoAndService.Common;
using RepoAndService.Services.Custom.Customer_Service;
using RepoAndService.Services.Custom.SupplierServices;
using RepoAndService.Services.General;
using System.Net;
using System.Text.RegularExpressions;
using WEBAPI.Middleware.Auth;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        
        #region Private Variables and Constructor
        private readonly ILogger _logger;
        private readonly ISupplierService _serviceSupplier;
        private readonly ICustomerService _serviceCustomer;
        private readonly IJWTAuthManager _authManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IServices<UserType> _serviceUserType;
        public LoginController(ILogger<LoginController> logger, ISupplierService serviceSupplier, ICustomerService serviceCustomer, IServices<UserType> serviceUserType, IJWTAuthManager authManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _serviceSupplier = serviceSupplier;
            _serviceCustomer = serviceCustomer;
            _authManager = authManager;
            _environment = environment;
            _serviceUserType = serviceUserType;
        }
        #endregion

        #region Login Section
        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin(LoginModel Loginuser)
        {
            Response<string> response = new();
            if (ModelState.IsValid)
            {
                var user = await _serviceSupplier.Find(x => x.UserName == Loginuser.UserName && x.Password == Encryptor.EncryptString(Loginuser.Password));
                if (user == null)
                {
                    response.Message = "Invalid Username / Password, Please Enter Valid Credentials...!";
                    response.Status = (int)HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                response.Message = _authManager.GenerateJWT(user);
                response.Status = (int)HttpStatusCode.OK;
                return Ok(response);
            }
            else
            {
                response.Message = "Invalid Login Information, Please Enter Valid Credentials...!";
                response.Status = (int)HttpStatusCode.NotAcceptable;
                return BadRequest(response);
            }

        }

        [HttpPost(nameof(RegisterSupplier))]
        public async Task<IActionResult> RegisterSupplier([FromForm] UserInsertModel supplierModel)
        {
            if (ModelState.IsValid)
            {
                var usertype = await _serviceUserType.Find(x => x.TypeName == "Supplier");
                if (usertype != null)
                {
                    if (supplierModel.Image != null)
                    {
                        var CheckUser = await _serviceSupplier.Find(x => x.UserCode == supplierModel.UserCode);
                        if (CheckUser != null)
                        {
                            return BadRequest("User ID : " + supplierModel.UserCode + " already Exist...!");
                        }
                        else
                        {
                            var CheckUsername = await _serviceSupplier.Find(x => x.UserName == supplierModel.UserName);
                            if (CheckUsername != null)
                            {
                                return BadRequest(" UserName :" + supplierModel.UserName + " already Exist...!");
                            }
                        }
                        var photo = await UploadPhoto(supplierModel.Image, supplierModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                        if (string.IsNullOrEmpty(photo))
                        {
                            return BadRequest("Error While Uploading Supplier Profile Photo, Please Try again Later...!");
                        }
                        var result = await _serviceSupplier.Insert(supplierModel, photo);
                        if (result == true)
                            return Ok("Supplier Registered Successfully...!");
                        else
                            return BadRequest("Error While Registering Supplier, please Try again Later...!");
                    }
                    else
                        return BadRequest("Please Upload Profile Photo...!");
                }
                else
                    return BadRequest("Something Went Wrong, Please try again later...!");
            }
            else
                return BadRequest("Invalid Supplier Information, Please Enter Valid Credentials...!");

        }

        [HttpPost(nameof(RegisterCustomer))]
        public async Task<IActionResult> RegisterCustomer([FromForm] UserInsertModel supplierModel)
        {
            if (ModelState.IsValid)
            {
                var usertype = await _serviceUserType.Find(x => x.TypeName == "Customer");
                if (usertype != null)
                {
                    if (supplierModel.Image != null)
                    {
                        var CheckUser = await _serviceCustomer.Find(x => x.UserCode == supplierModel.UserCode);

                        if (CheckUser != null)
                        {
                            return BadRequest("User ID : " + supplierModel.UserCode + " already Exist...!");
                        }
                        else
                        {
                            var CheckUsername = await _serviceCustomer.Find(x => x.UserName == supplierModel.UserName);
                            if (CheckUsername != null)
                            {
                                return BadRequest(" UserName :" + supplierModel.UserName + " already Exist...!");
                            }
                        }
                        var photo = await UploadPhoto(supplierModel.Image, supplierModel.UserName, DateTime.Now.ToString("dd/MM/yyyy"));
                        if (string.IsNullOrEmpty(photo))
                            return BadRequest("Error While Uploading Customer Profile Photo, Please Try again Later...!");

                        var result = await _serviceCustomer.Insert(supplierModel, photo);
                        if (result == true)
                            return Ok("Customer Registered Successfully...!");
                        else
                            return BadRequest("Error While Registering Customer, Please Try again Later...!");
                    }
                    else
                    {
                        _logger.LogWarning("Error : Uploading Profile Photo...!");
                        return BadRequest("Please Upload Profile Photo...!");
                    }
                }
                else
                {
                    return BadRequest("Something Went Wrong, Please try again later...!");
                }
            }
            else
            {
                _logger.LogWarning("Error: Invalid Customer Information...!");
                return BadRequest("Invalid Customer Information, Please Enter Valid Credentials...!");
            }
        }


        #region "Image Upload section"
        private async Task<string> UploadPhoto(IFormFile file, string Id, string date)
        {
            string fileName;
            string contentPath = this._environment.ContentRootPath;
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                fileName = Id.ToLower() + "-" + date + extension;
                string outputFileName = Regex.Replace(fileName, @"[^0-9a-zA-Z.]+", "");
                var pathBuilt = Path.Combine(contentPath, "Images\\User");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }
                var path = Path.Combine(contentPath, "Images\\User", outputFileName);

                Console.WriteLine(path);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return outputFileName;
            }
            else
                return "";
        }

        #endregion "Image Upload section"

        #endregion


    }
}