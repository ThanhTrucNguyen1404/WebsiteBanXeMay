﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteBanXeMay___Coding.Models;

namespace WebsiteBanXeMay___Coding.Repositories
{
    public interface INhaCungCapRepository
    {
        Task<IEnumerable<NhaCungCap>> GetAllAsync();
        Task<NhaCungCap> GetByIdAsync(int id);
        Task AddAsync(NhaCungCap nhaCungCap);
        Task UpdateAsync(NhaCungCap nhaCungCap);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
