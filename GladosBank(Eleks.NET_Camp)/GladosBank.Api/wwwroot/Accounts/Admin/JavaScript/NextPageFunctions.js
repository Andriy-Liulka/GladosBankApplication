//localStorage.setItem("pageIndex",0);

function ToBeginClick() {
    var pageIndex = localStorage.getItem("PaginationPageIndex");
    var numberIndex = Number(pageIndex);
    if (numberIndex != 0) {
        localStorage.setItem("PaginationPageIndex", numberIndex - 1);
    }

    FillTable();
}

function ToEndClick() {
    var pageIndex = localStorage.getItem("PaginationPageIndex");
    var numberIndex = Number(pageIndex);
    localStorage.setItem("PaginationPageIndex", numberIndex + 1);

    FillTable();
}
