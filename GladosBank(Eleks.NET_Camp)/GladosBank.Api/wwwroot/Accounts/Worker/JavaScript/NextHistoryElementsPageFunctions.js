function ToBeginHistoryListClick() {

    var pageIndex = localStorage.getItem("PaginationTransferHistoryPageIndex");
    var numberIndex = Number(pageIndex);
    if (numberIndex > 0) {
        localStorage.setItem("PaginationTransferHistoryPageIndex", numberIndex - 1);
    }

    GetTransactionHistoryElements();
}

function ToEndHistoryListClick() {

    var pageIndex = localStorage.getItem("PaginationTransferHistoryPageIndex");
    var numberIndex = Number(pageIndex);
    localStorage.setItem("PaginationTransferHistoryPageIndex", numberIndex + 1);

    GetTransactionHistoryElements();

}