function filterUser() {
    let inpFilter = document.getElementById("inpFilter");
    let listUser = document.getElementById("listBoxUser");
    let users = listUser.getElementsByTagName("option");
    //console.log(users);
    //console.log(users.length);
    for (var i = 0; i < users.length; i++) {
        if (users[i].innerHTML.toLowerCase().indexOf(inpFilter.value.toLowerCase()) == -1) {
            users[i].style.display = "none";
        } else {
            users[i].style.display = "block";
        }
    }

}

function borView_userFilter() {
    let inpFilter = document.getElementById("borViewUserFilter");
    let listUser = document.getElementById("listBorUser");
    let users = listUser.getElementsByTagName("option");
    //console.log(users);
    //console.log(users.length);
    for (var i = 0; i < users.length; i++) {
        if (users[i].innerHTML.toLowerCase().indexOf(inpFilter.value.toLowerCase()) == -1) {
            users[i].style.display = "none";
        } else {
            users[i].style.display = "block";
        }
    }
}

function borView_bookFilter() {
    let inpFilter = document.getElementById("borViewBookFilter");
    let listUser = document.getElementById("listBorBook");
    let books = listUser.getElementsByTagName("option");
    //console.log(users);
    //console.log(users.length);
    for (var i = 0; i < books.length; i++) {
        if (books[i].innerHTML.toLowerCase().indexOf(inpFilter.value.toLowerCase()) == -1) {
            books[i].style.display = "none";
        } else {
            books[i].style.display = "block";
        }
    }
}

function addBookView_bookFilter() {
    let inpFilter = document.getElementById("addBookViewBookFilter");
    let listUser = document.getElementById("addBorBookList");
    let books = listUser.getElementsByTagName("option");
    //console.log(users);
    //console.log(users.length);
    for (var i = 0; i < books.length; i++) {
        if (books[i].innerHTML.toLowerCase().indexOf(inpFilter.value.toLowerCase()) == -1) {
            books[i].style.display = "none";
        } else {
            books[i].style.display = "block";
        }
    }
}