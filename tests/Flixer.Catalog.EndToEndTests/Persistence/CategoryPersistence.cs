// using Microsoft.EntityFrameworkCore;
// using Flixer.Catalog.Domain.Entities;
// using Flixer.Catalog.Infra.Data.EF.Context;
//
// namespace Flixer.Catalog.EndToEndTests.Persistence;
//
// public class CategoryPersistence
// {
//     private readonly FlixerCatalogDbContext _context;
//
//     public CategoryPersistence(FlixerCatalogDbContext context)
//     {
//         _context = context;
//     }
//     
//     public async Task<Category?> GetById(Guid id)
//     { 
//         return await _context.Categories.AsNoTracking()
//             .FirstOrDefaultAsync(c => c.Id == id);
//     }
//
//     public async Task Insert(Category category) 
//     { 
//         await _context.Categories.AddAsync(category); 
//         await _context.SaveChangesAsync();
//     }
//
//     public async Task InsertList(List<Category> categories)
//     { 
//         await _context.Categories.AddRangeAsync(categories); 
//         await _context.SaveChangesAsync();
//     }
// }