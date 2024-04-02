using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Models;
using WebApp.Strategy.Repositories;
using WebApp.Strategy.ViewModels;

namespace WebApp.Strategy.Controllers;

[Authorize]
public class ProductsController(IProductRepository productRepository, UserManager<User> userManager, IMapper mapper)
    : Controller
{
    // GET: Products
    public async Task<IActionResult> Index()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        return View(await productRepository.GetAllByUserId(user.Id));
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var product = await productRepository.GetById(id);
        if (product == null) return NotFound();

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel product)
    {
        var addedProduct = mapper.Map<Product>(product);
        
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            addedProduct.UserId = user.Id;
            addedProduct.CreatedAt = DateTime.Now;
            await productRepository.Save(addedProduct);
            return RedirectToAction(nameof(Index));
        }

        return View(addedProduct);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var product = await productRepository.GetById(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Price,Stock,UserId,CreatedAt")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await productRepository.Update(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(product.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var product = await productRepository.GetById(id);
        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var product = await productRepository.GetById(id);
        if (product != null) await productRepository.Delete(product);

        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ProductExists(string id)
    {
        return await productRepository.GetById(id) is not null;
    }
}