using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;
namespace MyWeb
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        private LibraryContext db = new LibraryContext();

        // ListBook
        private List<listBookItem> listBook = new List<listBookItem>();
        // danh sách được lọc từ listBook
        public List<listBookItem> curListBook = new List<listBookItem>();
        //Category
        private List<String> checkedCategoryList = new List<string>();
        private Dictionary<string, int> categoryDict = new Dictionary<string, int>();
        // Search Bar
        // Page
        private int curStartingPage = 0;
        private const int bookPerPage = 30;
        public string username;
        private User curUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            curUser = (User)Session["user"];



            loadListBook();


            if (!IsPostBack)
            {
                //Only in the first load
                Session["lastPage"] = 1;
                inpPage.Text = "1";
                //Chỉ cần load dữ liệu vào control một lần, các lần sau k bị reset, chỉ cần k ghi đè là được
                // viewstate true để k phải load lại
                parseCategory();// phân tích tất cả các sách truyền category truyền vào Hash
                loadCategoryCheckList();// truyền dữ liệu Hash vào control list check box
                loadPageNumber();
            }
            else
            {
                //is postback
                // cho trường hợp Session hết hạn, nhưng trang web vẫn duy trì
                if (Session["lastPage"] == null)
                {
                    curStartingPage = 0;
                }
                else
                {
                    curStartingPage = (int.Parse(Session["lastPage"].ToString()) - 1) * bookPerPage;

                }
            }
        }
        private void loadListBook()
        {
            var tmpBookList = db.Books.ToList<Book>();

            listBook = new List<listBookItem>();

            for (int i = 0; i < tmpBookList.Count; i++)
            {
                int available = tmpBookList[i].BorBooks.Where(bb => bb.state == 0).Select(bb => bb.LocationId).Distinct().ToList().Count;
                string res;
                if (available == 0) res = "Đã hết"; else res = available.ToString();

                listBook.Add(
                    new listBookItem
                    {
                        bookId = tmpBookList[i].bookId,
                        bookName = tmpBookList[i].bookName,
                        category = tmpBookList[i].category,
                        description = tmpBookList[i].description,
                        imagePath = tmpBookList[i].imagePath,
                        availableLocation = res
                    }
                    );
            }
        }

        // Truyền danh sách các category vào Hash
        public void parseCategory()
        {
            if (listBook == null) return;
            string[] categories;
            for (int i = 0; i < listBook.Count; i++)
            {
                if (listBook[i].category == null) continue;
                categories = listBook[i].category.ToLower().Split(';');
                foreach (string c in categories)
                {
                    int val;
                    if (categoryDict.TryGetValue(c, out val))
                    {
                        categoryDict[c] = val + 1;
                    }
                    else
                    {
                        categoryDict.Add(c, 1);

                    }
                }
            }

        }
        private void loadCategoryCheckList()
        {
            // truyền dữ liệu vào checkboxList
            categoryCheckList.ItemType = "Category";
            categoryCheckList.DataSource = categoryDict.Select(
                c => new Category
                {
                    text = c.Key + "(" + c.Value + ")",
                    value = c.Key
                }
                );
            categoryCheckList.DataTextField = "text";
            categoryCheckList.DataValueField = "value";
            categoryCheckList.DataBind();
        }
        private void loadCheckedList()
        {
            for (int i = 0; i < categoryCheckList.Items.Count; i++)
            {
                if (categoryCheckList.Items[i].Selected)
                    checkedCategoryList.Add(categoryCheckList.Items[i].Value);
            }
        }
        protected void categoryCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // luôn lọc bằng thanh tìm kiếm trước, vì trong thanh tìm kiếm có thao tác hoàn lại toàn bộ danh sách
            searchFilter();

            loadCheckedList();
            categoryFilter();

            curStartingPage = 0;
            inpPage.Text = "1";
            Session["lastPage"] = 1;
            loadPageNumber();
        }

        // lọc  listBook bằng category
        private void categoryFilter()
        {
            if (checkedCategoryList == null || checkedCategoryList.Count == 0) return;
            List<listBookItem> afterClarify = new List<listBookItem>();
            foreach (var book in listBook)
            {
                if (book.category == null) continue;
                if (inCategory(book)) afterClarify.Add(book);
            }
            listBook = afterClarify;
        }
        // Kiểm tra 1 sách nằm trong bộ lọc (category) hiện tại hay không
        // Chỉ cần 1 trong các category của sách đó nằm trong category hiện tại
        public bool inCategory(listBookItem book)
        {
            foreach (string c in book.category.ToLower().Split(';'))
            {
                if (checkedCategoryList.Any(checkedVal => checkedVal == c))
                    return true;
            }
            return false;
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            searchFilter();

            // cần lọc lại qua nhưng category đang được check
            // kể cả khi thanh ghi không có j
            loadCheckedList();
            categoryFilter();

            curStartingPage = 0;
            inpPage.Text = "1";
            Session["lastPage"] = 1;
            loadPageNumber();
        }
        private void searchFilter()
        {
            string inp = searchTextBox.Text.Trim();
            if (inp == "")
            {
                // *** cẩn thận  search phải được gọi trước category
                Session["SearchingContent"] = null;
                LibraryContext db = new LibraryContext();
                loadListBook();
            }
            else
            {
                List<listBookItem> afterClarify = new List<listBookItem>();
                foreach (var book in listBook)
                {
                    if (inBookContent(inp, book)) afterClarify.Add(book);
                }
                listBook = afterClarify;
            }

        }

        private bool inBookContent(string str, listBookItem book)
        {
            if (book.bookName.ToLower().IndexOf(str.ToLower()) != -1) return true;
            if (book.description != null)
                if (book.description.ToLower().IndexOf(str.ToLower()) != -1) return true;

            /*if (book.extraSearch!=null)
                if (book.extraSearch.ToLower().IndexOf(str.ToLower()) != -1) return true;*/

            return false;
        }


        protected void removeCheck_Click(object sender, EventArgs e)
        {
            categoryCheckList.ClearSelection();
            //listBook = db.Books.ToList();
            loadListBook();
            // mặc dù loại bỏ lọc theo category nhưng vẫn phải xét nội dung đang tìm kiêm
            searchFilter();
            curStartingPage = 0;
            inpPage.Text = "1";
            Session["lastPage"] = 1;
            loadPageNumber();
        }
        private void loadPageNumber()
        {
            curListBook.Clear();
            for (int i = curStartingPage; i < Math.Min(curStartingPage + bookPerPage, listBook.Count); i++)
                curListBook.Add(listBook[i]);
        }

        protected void nextPageBtn_Click(object sender, EventArgs e)
        {
            int tmp = (listBook.Count % bookPerPage == 0 && listBook.Count / bookPerPage > 0) ? 1 : 0;
            int safeMaximumStarting = (listBook.Count / bookPerPage - tmp) * bookPerPage;

            curStartingPage = Math.Min(safeMaximumStarting, curStartingPage + bookPerPage);
            loadPageNumber();
            Session["lastPage"] = curStartingPage / bookPerPage + 1;
            inpPage.Text = Session["lastPage"].ToString();
        }

        protected void prevPageBtn_Click(object sender, EventArgs e)
        {
            curStartingPage = Math.Max(0, curStartingPage - bookPerPage);
            loadPageNumber();
            Session["lastPage"] = curStartingPage / bookPerPage + 1;
            inpPage.Text = Session["lastPage"].ToString();
        }

    }
    class Category
    {
        public string text { get; set; }
        public string value { get; set; }
    }
    public class listBookItem
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public string category { get; set; }
        public string description { get; set; }

        public string imagePath { get; set; }

        // giúp hiển thị xem sách này liệu có còn ở location nào khoonng ?
        public string availableLocation { get; set; }
    }
    }
