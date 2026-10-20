using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rus_Carmen_Lab2.Models;

namespace Rus_Carmen_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Rus_Carmen_Lab2Context _context;

    public EditModel(Rus_Carmen_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

        if (Book == null)
        {
            return NotFound();
        }

        PopulateAssignedCategoryData(_context, Book);

        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(), "ID", "PublisherName", Book.PublisherID);
       
        ViewData["AuthorsID"] = new SelectList(
           _context.Set<Authors>(), "ID", "LastName", Book.AuthorsID);

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    
public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)

    {

        if (id == null)
        {
            return NotFound();
        }

        var bookToUpdate = await _context.Book
            .Include(i => i.Publisher)
            .Include(i => i.Authors)
            .Include(i => i.BookCategories)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);

        if (bookToUpdate == null)
        {
            return NotFound();
        }


        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title,
            i => i.AuthorsID,
            i => i.Price,
            i => i.PublishingDate,
            i => i.PublisherID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
       
        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData(_context, bookToUpdate);
        
        ViewData["AuthorsID"] = new SelectList(
          _context.Set<Authors>(), "ID", "LastName", bookToUpdate.AuthorsID);

        ViewData["PublisherID"] = new SelectList(
         _context.Set<Publisher>(), "ID", "PublisherName", bookToUpdate.PublisherID);

        return Page();
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
