using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.Models;

namespace MyWeb
{
    public partial class ListBook : System.Web.UI.Page
    {
        //Role
        // ListBook
        private List<Book> listBook = new List<Book>();
        public List<Book> curListBook = new List<Book>();
        //Category
        private List<String> checkedCategoryList = new List<string>();
        private Dictionary<string, int> categoryDict = new Dictionary<string, int>();
        // Search Bar
        // Page
        private int curStartingPage = 0;
        private const int bookPerPage = 30;
        protected void Page_Load(object sender, EventArgs e)
        {
            LibraryContext db = new LibraryContext();
            // INIT
            // Lấy danh sách tất cả từ db
            listBook = db.Books.ToList<Book>();

            if (!IsPostBack) {
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
                curStartingPage = (int.Parse(Session["lastPage"].ToString()) - 1) * bookPerPage;
            }
            

        }

        // CẦN ĐƯỢC GỌI SAU KHI ĐÃ LỌC BẰNG CHECK BOX VÀ SEARCH (vì có chạy hàm loadPageNumber)
        // kiểm tra xem textbox của phân trang có bị thay đổi không !
 


        // Truyền danh sách các category vào Hash
        public void parseCategory(){
            if (curListBook == null) return;
            string[] categories;
            for (int i = 0; i < listBook.Count; i++)
            {
                if (listBook[i].category == null) continue;
                categories = listBook[i].category.Split(';');
                foreach(string c in categories)
                {
                    int val;
                    if (categoryDict.TryGetValue(c,out val))
                    {
                        categoryDict[c]=val+1;
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
                c =>  new Category
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
            if (checkedCategoryList.Count == 0) return;

            List<Book> afterClarify = new List<Book>();
            foreach (var book in listBook)
            {
                if (book.category == null) continue;
                if (inCategory(book)) afterClarify.Add(book);
            }
            listBook = afterClarify;
        }
        // Kiểm tra 1 sách nằm trong bộ lọc (category) hiện tại hay không
        // Chỉ cần 1 trong các category của sách đó nằm trong category hiện tại
        public bool inCategory(Book book)
        {
            foreach (string c in book.category.Split(';'))
            {
                if (checkedCategoryList.Any(checkedVal => checkedVal==c)) 
                    return true;
            }
            return false;
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            string inp = searchTextBox.Text.Trim();
            // Nếu thanh tìm kiếm không chứa gì cả, hoặc chỉ chứa khoảng trắng thì không làm gì cả, 
            // hay không tác động gì lên danh sách sách cả
            if (inp == "") {
                Session["SearchingContent"] = null;
                LibraryContext db = new LibraryContext();
                listBook = db.Books.ToList();
            }
            else
            {
                searchFilter(inp);
            }
            // vì mỗi post back listBook đều được lấy lại toàn bộ, nên cần phải lọc lại qua category
            curStartingPage = 0;
            inpPage.Text = "1";
            Session["lastPage"] = 1;
            loadPageNumber();
        }
        private void searchFilter(string inp)
        {
            // khi tìm kiếm
            // mọi thứ sẽ quay trở lại như đầu ?? không , listBook cần được lọc qua Search trước khi Lọc trang
            // đã được hãy đảm bảo listBook đã được lọc qua Category rồi
            List<Book> afterClarify = new List<Book>();
            foreach (var book in listBook)
            {
                if (inBookContent(inp, book)) afterClarify.Add(book);
            }
            listBook = afterClarify;
        }

        private bool inBookContent(string str, Book book)
        {
            if (book.bookName.ToLower().IndexOf(str.ToLower()) != -1) return true;
            if (book.description!=null)
                if(book.description.ToLower().IndexOf(str.ToLower()) != -1) return true;
            
            /*if (book.extraSearch!=null)
                if (book.extraSearch.ToLower().IndexOf(str.ToLower()) != -1) return true;*/

            return false;
        }

        protected void removeCheck_Click(object sender, EventArgs e)
        {
            categoryCheckList.ClearSelection();
            LibraryContext db = new LibraryContext();
            listBook = db.Books.ToList();
            if (Session["SearchingContent"] != null)
                searchFilter(Session["SearchingContent"].ToString());
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
   
    
}