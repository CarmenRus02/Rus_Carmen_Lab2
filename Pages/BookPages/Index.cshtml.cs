using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Carmen_Lab2.Models;

namespace Rus_Carmen_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Rus_Carmen_Lab2Context _context;

    public IndexModel(Rus_Carmen_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Book = await _context.Book
    .Include(b => b.Publisher)
    .Include(b => b.Authors)
    .ToListAsync(); 
    }
}
