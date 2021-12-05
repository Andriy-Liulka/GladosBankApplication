function RedirectCheckCustomersListClick() {
    window.location = "CustomersListsPage.html";
}
 

function RedirectWorkerBasePageClick() {
    localStorage.setItem("PaginationTransferHistoryPageIndex", 0);
    localStorage.setItem("CurrentCustomerIdForTransactionHistory", 0);
    window.location = "WorkerBasePage.html";
}