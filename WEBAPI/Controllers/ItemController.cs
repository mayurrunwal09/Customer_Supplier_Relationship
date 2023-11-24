using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoAndService.Services.Custom.CatagoryServices;
using RepoAndService.Services.Custom.ItemServices;
using RepoAndService.Services.Custom.SupplierServices;
using System.Text.RegularExpressions;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IItemService _itemservice;
        private readonly ISupplierService _supplerservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICatagoryServices _catagoryservice;

        public ItemController(ILogger<ItemController> logger, IItemService itemservice, ISupplierService supplerservice, IWebHostEnvironment webHostEnvironment, ICatagoryServices catagoryservice)
        {
            _logger = logger;
            _itemservice = itemservice;
            _supplerservice = supplerservice;
            _webHostEnvironment = webHostEnvironment;
            _catagoryservice = catagoryservice;
        }
        [HttpGet(nameof(GetAllItemBySupplier))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemBySupplier(Guid id)
        {
            ICollection<ItemViewModel> items = await _itemservice.GetAllItemByUser(id);
            if (items == null)
                return BadRequest("No Records Found, Please Try Again After Adding them...!");
            else
                return Ok(items);

        }
        [HttpGet(nameof(GetAllItemByCustomer))]
        public async Task<ActionResult<ItemViewModel>> GetAllItemByCustomer(Guid id)
        {
            ICollection<ItemViewModel> items = await _itemservice.GetAllItemByUser(id);
            if (items.ToList().Count == 0)
                return BadRequest("Customer is not valid, Please Enter Valid Customer Details...!");
            else
                return Ok(items);
        }
        [HttpGet(nameof(GetItem))]
        public async Task<ActionResult<ItemViewModel>> GetItem(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _itemservice.Get(Id);
                if (result == null)
                {
                    return BadRequest("No Records Found, Please Try Again After Adding them...!");
                }
                return Ok(result);
            }
            else
                return NotFound("Invalid Item Id, Please Entering a Valid One...!");

        }
        [HttpPost(nameof(AddExistingItemToSupplier))]
        public async Task<IActionResult> AddExistingItemToSupplier([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _itemservice.Find(x => x.Id == itemModel.Id);

                    if (CheckItem == null)
                        return BadRequest("Item not Exist with Id :" + itemModel.Id + ", Please Try Again After Sometime...!");
                    else
                    {

                        var CheckUser = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                        if (CheckUser != null)
                        {
                            _logger.LogInformation("Starting Item Insert to Supplier Item...!");
                            var result = await _itemservice.InsertExistingItem(itemModel);
                            if (result == true)
                                return Ok("Succesfully Inserted Item to Supplier Item...!");
                            else
                                return BadRequest("Invalid Supplier Item Information, Please Entering a Valid One...!");

                        }
                        else
                        {
                            _logger.LogWarning("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                            return BadRequest("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                        }
                    }
                }
                else
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
                return BadRequest("Invalid Supplier Item Information, Please Entering a Valid One...!");
        }
        [HttpPost(nameof(AddExistingItemToCustomer))]
        public async Task<IActionResult> AddExistingItemToCustomer([FromForm] ExistingItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _itemservice.Find(x => x.Id == itemModel.Id);

                    if (CheckItem == null)
                        return BadRequest("Item not Exist with Id :" + itemModel.Id + ", Please Try Again After Sometime...!");
                    else
                    {

                        var CheckUser = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                        if (CheckUser != null)
                        {
                            var result = await _itemservice.InsertExistingItem(itemModel);
                            if (result == true)
                                return Ok("Succesfully Inserted Item to Customer Item...!");
                            else
                                return BadRequest("Invalid Customer Item Information, Please Entering a Valid One...!");
                        }
                        else
                            return BadRequest("User not Exist with Id :" + itemModel.UserId + ", Please Try Again After Sometime...!");
                    }
                }
                else
                    return BadRequest("Unauthorized User, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
                return BadRequest("Invalid Customer Item Information, Please Entering a Valid One...!");


        }
        [HttpPost(nameof(AddSupplierItem))]
        public async Task<IActionResult> AddSupplierItem([FromForm] ItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User supplier = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                if (supplier != null)
                {
                    var CheckItem = await _itemservice.Find(x => x.ItemCode == itemModel.ItemCode);
                    if (CheckItem != null)
                        return BadRequest("Item already Exist with Id :" + itemModel.ItemCode + ", Please Try Again After Sometime...!");
                    else
                    {
                        var CheckItemName = await _itemservice.Find(x => x.ItemName == itemModel.ItemName);
                        if (CheckItemName != null)
                            return BadRequest(" ItemName :" + itemModel.ItemName + " already Exist...!");
                    }
                    var category = await _catagoryservice.Get(itemModel.CatagoryId);
                    if (category == null)
                        return NotFound("Category Not Found, Please Provide Valid Category...!");

                    var photo = await UploadPhoto(itemModel.Images, itemModel.ItemName);
                    var result = await _itemservice.Insert(itemModel, photo);
                    if (result == true)
                        return Ok("Succesfully Item inserted...!");
                    else
                        return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                }
                else
                    return BadRequest("Unauthorized Supplier, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
            {
                _logger.LogWarning("Invalid Customer Information, Please Entering a Valid One...!");
                return BadRequest("Invalid Customer Information, Please Entering a Valid One...!");
            }

        }
        [HttpPost(nameof(AddCustomerItem))]
        public async Task<IActionResult> AddCustomerItem([FromForm] ItemInsertModel itemModel)
        {
            if (ModelState.IsValid)
            {
                User customer = await _supplerservice.Find(x => x.Id == itemModel.UserId);
                if (customer != null)
                {
                    var CheckItem = await _itemservice.Find(x => x.ItemCode == itemModel.ItemCode);

                    if (CheckItem != null)
                        return BadRequest("Item already Exist with Id :" + itemModel.ItemCode + ", Please Try Again After Sometime...!");
                    else
                    {
                        var CheckItemName = await _itemservice.Find(x => x.ItemName == itemModel.ItemName);
                        if (CheckItemName != null)
                        {
                            return BadRequest(" ItemName :" + itemModel.ItemName + " already Exist...!");
                        }
                    }
                    var category = await _catagoryservice.Get(itemModel.CatagoryId);
                    if (category == null)
                        return NotFound("Category Not Found, Please Provide Valid Category...!");

                    var photo = await UploadPhoto(itemModel.Images, itemModel.ItemName);
                    var result = await _itemservice.Insert(itemModel, photo);
                    if (result == true)
                        return Ok("Succesfully Item inserted...!");
                    else
                        return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");

                }
                else
                    return BadRequest("Unauthorized Customer, Please Provide Valid Credentials and Try Again Later...!");
            }
            else
            {
                return BadRequest("Invalid Customer Information, Please Entering a Valid One...!");
            }
        }
        [HttpPut(nameof(EditItem))]
        public async Task<IActionResult> EditItem([FromForm] ItemUpdateModel itemModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Finding for Customer Update id" + itemModel.Id);
                ItemViewModel item = await _itemservice.Get(itemModel.Id);
                if (item.Id == itemModel.Id)
                {
                    if (item != null)
                    {
                        var CheckItem = await _itemservice.Find(x => x.ItemCode == itemModel.ItemCode && x.Id != itemModel.Id);

                        if (CheckItem != null)
                        {
                            return BadRequest("Item Code : " + itemModel.ItemCode + " already Exist...!");
                        }
                        else
                        {
                            var CheckItemname = await _itemservice.Find(x => x.ItemName == itemModel.ItemName);
                            if (CheckItemname != null)
                            {
                                return BadRequest(" ItemName : " + itemModel.ItemName + " already Exist...!");
                            }
                        }

                        if (itemModel.Images == null)
                        {
                            var result = await _itemservice.Update(itemModel, null);
                            if (result == true)
                                return Ok("Succesfully Updated Customer...!");
                            else
                                return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                        }
                        else
                        {
                            var photo = await UploadPhoto(itemModel.Images, itemModel.ItemName);
                            var result = await _itemservice.Update(itemModel, photo);
                            if (result == true)
                                return Ok("Item Information Updated...!");
                            else
                                return BadRequest("Something Went Wrong, Please Try Again After Sometime...!");
                        }
                    }
                    else
                        return NotFound("Item Not Found. id :" + itemModel.Id + ", Please Provide Valid Details and Try Again...!");
                }
                else
                    return NotFound("Invalid Item Id, Please Provide Valid Details and Try Again...!");
            }
            else
                return BadRequest("Invalid Item Information, Please Entering a Valid One...!");

        }
        [HttpDelete(nameof(DeleteItem))]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                var result = await _itemservice.Delete(Id);
                if (result == true)
                    return Ok("Item Deleted SUccessfully...!");
                else
                    return BadRequest("Something Went Wrong, Item Not Deleted, Please Try Again After Sometime...!");
            }
            else
                return BadRequest("Invalid Item Id, Please Provide Valid Details and Try Again...!");

        }
        [HttpPost("FileUpload")]
        private async Task<string> UploadPhoto(IFormFile file, string id)
        {
            string filename;
            try
            {
                Console.WriteLine(id);
                _logger.LogInformation("Starting uploading item image");
                string contentpath = this._webHostEnvironment.ContentRootPath;
                var extension = "." + file.FileName.Split('.')[^1];
                if(extension == ".jpg" || extension ==".jpeg"|| extension == ".png")
                {
                    filename = id.ToLower() + extension;
                    string OutputFileName = Regex.Replace(filename, @"[^0-9a-zA-Z._]+", "");
                    var pathbuild = Path.Combine(contentpath, "Images\\Item");

                    if (!Directory.Exists(pathbuild))
                    {
                        Directory.CreateDirectory(pathbuild);
                    }
                    var path = Path.Combine(contentpath, "Images\\Item", OutputFileName);

                    Console.WriteLine(path);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    _logger.LogInformation("Successfully uploaded Item Image with Name : " + OutputFileName);
                    return OutputFileName;
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Error while uploading Item Image with Name : " + ex.StackTrace);

            }

            return "";
        
            
        }
    }
}
