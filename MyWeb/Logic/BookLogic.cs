using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MyWeb.Models
{
    public class BookLogic
    {
        public static bool validExtensionImagePath(string imagePath)
        {
            String fileExtension = System.IO.Path.GetExtension(imagePath).ToLower();
            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isValid(Book book,ref List<String> messages)
        {
            if (book.imagePath!=null && validExtensionImagePath(book.imagePath))
            {
                ValidationContext ctx = new ValidationContext(book, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(book, ctx, results, true);
                messages = results.Select(res => res.ErrorMessage.ToString()).ToList();
                return isValid;
            }
            else
            {
                messages.Add("File không hợp lệ! (Các định dạng cho phép: .gif, .png, .jpeg, .jpg ) ");
                return false;
            }
        }

        //Hàm sinh các Borrowable book cho một Book với một số lượng (amount) truyền vào
        public static void genBorBook(Book book)
        {
            LibraryContext db = new LibraryContext();
            int idBorBook = db.BorBooks.Max(bb => bb.BookId)+1;

            for (int i = 0; i < Int16.Parse(book.amount); i++)
                db.BorBooks.Add(
                    new BorBook
                    {
                        id = idBorBook++,
                        BookId = book.bookId,
                        borrowDate = DateTime.Now,
                        returnDate = DateTime.Now,
                        state = 0,
                    }
                    );

            db.SaveChanges();
        }
        public  static void genBorBook(ref Book book, int count)
        {
            LibraryContext db = new LibraryContext();
            int curid = db.BorBooks.Max(bb => bb.id) + 1;
            for(int i = 0; i < count; i++) {
                book.BorBooks.Add(
                        new BorBook
                        {
                            id = curid++,
                            state = 0,
                            returnDate = DateTime.Now,
                            BookId = book.bookId,
                            Book = book
                        }
                    );
            }
            db.SaveChanges();
        }
        public static void removeBorBook(ref Book book, int count)
        {
            LibraryContext db = new LibraryContext();
            int tmpId = book.bookId;
            List<BorBook> Listbb = db.BorBooks.Where( bb=> bb.state==0 && bb.BookId==tmpId).ToList();
            for(int i = 0; i < Math.Min(count, Listbb.Count); i++)
            {
                book.BorBooks.Remove(Listbb[i]);
                db.BorBooks.Remove(Listbb[i]);
            }
            db.SaveChanges();
        }
    }
}