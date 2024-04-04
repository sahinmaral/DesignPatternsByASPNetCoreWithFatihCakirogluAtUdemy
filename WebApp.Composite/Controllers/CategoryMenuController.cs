using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Composite.Composite;
using WebApp.Composite.Models;

namespace WebApp.Composite.Controllers
{
    [Authorize]
    public class CategoryMenuController(AppIdentityDbContext dbContext) : Controller
    {
        // GET: CategoryMenuController
        public async Task<IActionResult> Index()
        {
            // category => bookcomposite;
            // book => bookcomponent
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var categories = await dbContext.Categories.Include(x => x.Books).Where(x => x.UserId == userId).OrderBy(x => x.Id).ToListAsync();

            var menu = GetMenus(categories, new Category { Name = "TopCategory", Id = 0 }, new BookComposite(0, "TopMenu"));

            ViewBag.menu = menu;
            ViewBag.selectedList =
                menu.Components.SelectMany(component => ((BookComposite)component).GetSelectedListItems(""));
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(int categoryId, string bookName)
        {
            await dbContext.Books.AddAsync(new Book { CategoryId = categoryId, Name = bookName });
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public BookComposite GetMenus(List<Category> categories, Category topCategory, BookComposite topBookComposite, BookComposite? last=null)
        {
            var filteredCategories = categories.Where(category => category.ReferenceId == topCategory.Id).ToList();
            foreach (var categoryItem in filteredCategories)
            {
                var bookComposite = new BookComposite(categoryItem.Id, categoryItem.Name);
                foreach (var bookItem in categoryItem.Books.ToList())
                {
                    bookComposite.Add(new BookComponent(bookItem.Id, bookItem.Name));
                }

                if (last is not null)
                {
                    last.Add(bookComposite);
                }
                else
                {
                    topBookComposite.Add(bookComposite);
                }

                GetMenus(categories, categoryItem, topBookComposite, bookComposite);
            }

            return topBookComposite;
        }

    }
}
