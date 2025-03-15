using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebApi.Models;

namespace MyWebApi.Intefaces
{
    public interface ICommentRepository<T> where T : Comment
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task<T> CreateAsync(T commentModel);

        Task<T?> UpdateAsync(int id , T commentModel);

        Task<T?> DeleteAsync(int id);
    }
}