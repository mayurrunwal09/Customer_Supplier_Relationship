using Domain.Models;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoAndService.Services.Custom.ItemServices
{
    public interface IItemService
    {
        Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id);
        Task<ItemViewModel> Get(Guid Id);
        Task<bool> Insert(ItemInsertModel itemInsertModel, string photo);
        Task<bool> Update(ItemUpdateModel itemUpdateModel, string image);
        Task<bool> Delete(Guid Id);
        Task<Item> Find(Expression<Func<Item, bool>> match);
        Task<bool> InsertExistingItem(ExistingItemInsertModel itemModel);
    }
}
