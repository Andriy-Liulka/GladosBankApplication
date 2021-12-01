
function ToBeginCustomerListClick() {
    var pageIndex = localStorage.getItem("PaginationPageIndex");
    var numberIndex = Number(pageIndex);
    if (numberIndex != 0) {
        localStorage.setItem("PaginationPageIndex", numberIndex - 1);
    }

    FillTableCustomers();
}

function ToEndCustomerListClick() {
    var pageIndex = localStorage.getItem("PaginationPageIndex");
    var numberIndex = Number(pageIndex);
    localStorage.setItem("PaginationPageIndex", numberIndex + 1);

    FillTableCustomers();
}
