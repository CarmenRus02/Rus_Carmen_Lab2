using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Carmen_Lab2.Models;

namespace Rus_Carmen_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Rus_Carmen_Lab2Context _context;

    public IndexModel(Rus_Carmen_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;
    public int CategoryID { get; set; }

    public IEnumerable<Book> Books { get; set; } = new List<Book>();

    public async Task OnGetAsync(int? id)
    {
        Category = await _context.Category.ToListAsync();
        if (id != null)
        {
            CategoryID = id.Value;

            Books = await _context.Book
                .Include(b => b.Authors)
                .Include(b => b.BookCategories)
                .Where(b => b.BookCategories.Any(bc => bc.CategoryID == id.Value))
                .ToListAsync();
        }
    }
}
