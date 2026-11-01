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
    public BookData BookD { get; set; }
    public int BookID { get; set; }
    public int CategoryID { get; set; }
    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }
    public string CurrentFilter { get; set; }
    public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
    {
        BookD = new BookData();
        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";
        CurrentFilter = searchString;
        BookD.Books = await _context.Book
               .Include(b => b.Publisher)
               .Include(b => b.Authors)
               .Include(b => b.BookCategories)
               .ThenInclude(b => b.Category)
               .AsNoTracking()
               .OrderBy(b => b.Title)
               .ToListAsync();

        if (!String.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(s =>
                s.Title.Contains(searchString)
                || s.Authors.FullName.Contains(searchString)).ToList();
        }

        if (id != null)
        {
            BookID = id.Value;

            Book book = BookD.Books
                .Where(i => i.ID == id.Value).Single();

            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Title);
                break;
            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Authors.FullName);
                break;
            case "author":
                BookD.Books = BookD.Books.OrderBy(s => s.Authors.FullName);
                break;
            default:
                BookD.Books = BookD.Books.OrderBy(s => s.Title);
                break;
        }
        Book = BookD.Books.ToList();
    }
}

