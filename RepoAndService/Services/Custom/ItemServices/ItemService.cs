using Domain.Models;
using Domain.ViewModels;
using RepoAndService.Repository;
using RepoAndService.Services.Custom.CatagoryServices;
using RepoAndService.Services.Custom.Customer_Service;
using RepoAndService.Services.Custom.SupplierServices;
using RepoAndService.Services.Custom.UserTypeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepoAndService.Services.Custom.ItemServices
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _item;
        private readonly IRepository<User> _user;
        private readonly IUserTypeService _userType;
        private readonly ISupplierService _supplier;
        private readonly ICustomerService _customer;
        private readonly IRepository<ItemImages> _itemImages;
        private readonly ICatagoryServices _category;
        private readonly IRepository<ItemSupplier> _supplierItem;
        private readonly IRepository<CustomerItem> _customerItem;

        public ItemService(IRepository<Item> item, IUserTypeService userType, IRepository<User> user, IRepository<ItemSupplier> serviceSupplierItem, ICustomerService serviceCustomer, ISupplierService serviceSupplier, IRepository<CustomerItem> customerItem, ICatagoryServices serviceCategory, IRepository<ItemImages> serviceItemImages)
        {
            _item = item;
            _user = user;
            _userType = userType;
            _supplierItem = serviceSupplierItem;
            _category = serviceCategory;
            _supplier = serviceSupplier;
            _customer = serviceCustomer;
            _itemImages = serviceItemImages;
            _customerItem = customerItem;
        }
        public async Task<bool> Delete(Guid Id)
        {
            Item item = await _item.Get(Id);
            if (item != null)
            {
                ItemSupplier supplier = await _supplierItem.Find(x=>x.ItemId == item.Id);
                ItemImages itemImages = await _itemImages.Find(x => x.ItemId == item.Id);
                if (supplier != null)
                {
                    var resultSupplier = await _supplierItem.Delete(supplier);
                    if (resultSupplier == true)
                    {
                        var result = await DeleteItemAndItemImages(itemImages, item);
                        return result;
                    }
                    else
                    {
                        var result = await DeleteItemAndItemImages(itemImages, item);
                        return result;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> DeleteItemAndItemImages(ItemImages itemImages, Item item)
        {
            if (itemImages != null)
            {
                var resultItemImages = await _itemImages.Delete(itemImages);
                if (resultItemImages == true)
                {
                    var resultItem = await _item.Delete(item);
                    if (resultItem == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                var resultItem = await _item.Delete(item);
                if (resultItem == true)
                    return true;
                else
                    return false;
            }
        }

        public Task<Item> Find(Expression<Func<Item, bool>> match)
        {
            return _item.Find(match);
        }

        public async Task<ItemViewModel> Get(Guid Id)
        {
            ItemViewModel itemViewModel = new();
            ItemSupplier result = await _supplierItem.Find(x=>x.ItemId == Id);  
            if(result != null)
            {
                Item item = await _item.Find(x => x.Id == result.ItemId);
                ItemViewModel itemView = new()
                {
                    Id = result.ItemId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Description = item.Description,
                    Price = item.Price,
                };
                Catagory catagory = await _category.Find(x => x.Id == item.CatagoryId);
                CatagoryViewModel catagoryView = new()
                {
                    Id = catagory.Id,
                    CatagoryName = catagory.CatagoryName
                };
                itemView.Catagories .Add(catagoryView);
                User supplier = await _supplier.Find(x => x.Id == result.UserId);
                UserView supplierview = new()
                {
                    Id = supplier.Id,
                    UserCode = supplier.UserCode,
                    UserName = supplier.UserName,
                    PhoneNo = supplier.Phoneno,
                    Email = supplier.Email,
                    UserImage = supplier.Image

                };
                itemView.Catagories.Add(catagoryView);
                ICollection<ItemImages> image = await _itemImages.FindAll(x=>x.ItemId == result.ItemId);
                foreach (var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.ItemImage,
                        IsActive = img.IsActive
                    };
                    itemView.ItemImages.Add(imgView);
                }
                return itemView;

                
            }
            else
            {
                CustomerItem customerItem = await _customerItem.Find(x => x.ItemId == Id);
                Item item = await _item.Find(x=>x.Id == customerItem.ItemId);

                ItemViewModel itemview = new()
                {
                    Id = customerItem.ItemId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    Description = item.Description,
                    Price = item.Price,
                };
                Catagory category = await _category.Find(x => x.Id == item.CatagoryId);
                CatagoryViewModel categoryView = new()
                {
                    Id = category.Id,
                    CatagoryName = category.CatagoryName
                };
                itemview.Catagories.Add(categoryView);
                User customer = await _customer.Find(x => x.Id == customerItem.UserId);
                UserView customerView = new()
                {
                    Id = customer.Id,
                    UserName = customer.UserName,
                    PhoneNo = customer.Phoneno,
                    Email = customer.Email,
                    
                    UserCode = customer.UserCode,
                    UserImage = customer.Image
                };
                itemview.User.Add(customerView);
                ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == customerItem.ItemId);
                foreach (var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.ItemImage,
                        IsActive = img.IsActive
                    };
                    itemview.ItemImages.Add(imgView);
                }
                return itemview;
            }
        }

        public async Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id)
        {
            ICollection<ItemViewModel> itemViewModels = new List<ItemViewModel>();
           var supplier = await _supplier.Find(x => x.Id == id);
            var customer = await    _customer.Find(x=>x.Id == id);
            var usertype = await _userType.Find(x=>x.Id==id);

            if(usertype.TypeName == "Supplier")
            {
                ICollection<ItemSupplier> result = await _supplierItem.FindAll(x => x.UserId == id);
                if(result != null)
                {
                    foreach(ItemSupplier item in result)
                    {
                        Item items = await _item.Find(x => x.Id == item.ItemId);
                        ItemViewModel itemView = new()
                        {
                            Id = item.Id,
                            ItemCode = items.ItemCode,
                            ItemName = items.ItemName,
                            Description = items.Description,
                            Price = items.Price,
                        };
                        Catagory catagory = await _category.Find(x => x.Id == items.CatagoryId);
                        CatagoryViewModel catagoryView = new()
                        {
                            Id = catagory.Id,
                            CatagoryName = catagory.CatagoryName
                        };
                        itemView.Catagories.Add(catagoryView);
                        User user = await _supplier.Find(x => x.Id == item.UserId);
                        UserView view = new()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            PhoneNo = user.Phoneno,
                            Email = user.Email,
                           
                            UserCode = user.UserCode,
                            UserImage = user.Image
                        };
                        itemView.User.Add(view);
                        ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                        foreach (var img in image)
                        {
                            ItemImagesView imgView = new()
                            {
                                Id = img.Id,
                                ItemId = img.ItemId,
                                ItemImage = img.ItemImage,
                                IsActive = img.IsActive
                            };
                            itemView.ItemImages.Add(imgView);
                        }
                        itemViewModels.Add(itemView);
                    }
                    return itemViewModels;
                }
                else
                    return itemViewModels;
            }
            else
            {
                ICollection<CustomerItem> result = await _customerItem.FindAll(x => x.UserId == id);
                foreach (CustomerItem item in result)
                {
                    Item items = await _item.Find(x => x.Id == item.ItemId);
                    ItemViewModel itemView = new()
                    {
                        Id = item.Id,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        Description = items.Description,
                        Price = items.Price
                    };
                    Catagory category = await _category.Find(x => x.Id == items.CatagoryId);
                    CatagoryViewModel categoryView = new()
                    {
                        Id = category.Id,
                        CatagoryName = category.CatagoryName
                    };
                    itemView.Catagories.Add(categoryView);
                    User user = await _customer.Find(x => x.Id == item.UserId);
                    UserView view = new()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        PhoneNo = user.Phoneno,
                        Email = user.Email,
                     
                        UserCode = user.UserCode,
                        UserImage = user.Image
                    };
                    itemView.User.Add(view);
                    //Get all itemimages and make forloop to storing it in single view
                    ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                    foreach (var img in image)
                    {
                        ItemImagesView imgView = new()
                        {
                            Id = img.Id,
                            ItemId = img.ItemId,
                            ItemImage = img.ItemImage,
                            IsActive = img.IsActive
                        };
                        itemView.ItemImages.Add(imgView);
                    }
                    itemViewModels.Add(itemView);
                }
                return itemViewModels;
            
                
            }
        }

        public async Task<bool> Insert(ItemInsertModel itemInsertModel, string photo)
        {
            var user = await _user.Find(x => x.Id == itemInsertModel.UserId);
            var usertype = await _userType.Find(x => x.Id == user.UsertypeId);
            Console.WriteLine(user.UserName + "  " + usertype.TypeName);
            if (usertype.TypeName == "Supplier")
            {
                Item item = new()
                {
                    ItemCode = itemInsertModel.ItemCode,
                    ItemName = itemInsertModel.ItemName,
                    Description = itemInsertModel.Description,
                    Price = itemInsertModel.Price,
                    CatagoryId = itemInsertModel.CatagoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _item.Insert(item);
                if (result == true)
                {
                    ItemSupplier supplierItem = new()
                    {
                        ItemId = item.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    ItemImages itemImage = new()
                    {
                        ItemId = item.Id,
                        ItemImage = photo,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultItemImage = await _itemImages.Insert(itemImage);
                    if (resultItemImage == true)
                    {
                        await _supplierItem.Insert(supplierItem);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                Item item = new()
                {
                    ItemCode = itemInsertModel.ItemCode,
                    ItemName = itemInsertModel.ItemName,
                    Description = itemInsertModel.Description,
                    Price = itemInsertModel.Price,
                    CatagoryId = itemInsertModel.CatagoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _item.Insert(item);
                if (result == true)
                {
                    CustomerItem customerItem = new()
                    {
                        ItemId = item.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    ItemImages itemImage = new()
                    {
                        ItemId = item.Id,
                        ItemImage = photo,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultItemImage = await _itemImages.Insert(itemImage);
                    if (resultItemImage == true)
                    {
                        await _customerItem.Insert(customerItem);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public async Task<bool> InsertExistingItem(ExistingItemInsertModel itemModel)
        {
           var user = await _user.Find(x=>x.Id== itemModel.UserId);
            var usertype = await _userType.Find(x => x.Id == user.UsertypeId);
            Console.WriteLine(user.UserName + "     " + usertype.TypeName);
            if (usertype.TypeName == "Supplier")
            {
                ItemSupplier supplierItem = new()
                {
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _supplierItem.Insert(supplierItem);
                if (result == true)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                CustomerItem customerItem = new()
                {
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _customerItem.Insert(customerItem);
                if (result == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public async Task<bool> Update(ItemUpdateModel itemUpdateModel, string image)
        {
            Item item = await _item.Get(itemUpdateModel.Id);
            item.ItemCode = itemUpdateModel.ItemCode;
            item.ItemName = itemUpdateModel.ItemName;
            item.Description = itemUpdateModel.Description;
            item.Price = itemUpdateModel.Price;
            item.CreatedOn = item.CreatedOn;
            item.UpdatedOn = DateTime.Now;
            item.IsActive = itemUpdateModel.IsActive;

            ItemImages itemImage = await _itemImages.Find(x => x.ItemId == itemUpdateModel.Id);
            itemImage.ItemId = item.Id;
            itemImage.CreatedOn = DateTime.Now;
            itemImage.UpdatedOn = DateTime.Now;
            itemImage.IsActive = itemUpdateModel.IsActive;
            if (image == null)
                itemImage.ItemImage = itemImage.ItemImage;
            else
                itemImage.ItemImage = image;
            var result = await _item.Update(item);
            if (result == true)
            {
                var resultItemImage = await _itemImages.Update(itemImage);
                if (resultItemImage == true)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
