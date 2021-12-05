function GetTransactionHistoryElementsClick(id){
    console.log(id);
    localStorage.setItem("CurrentCustomerIdForTransactionHistory", id);
    localStorage.setItem("PaginationTransferHistoryPageIndex", 0);

    GetTransactionHistoryElements();

}

function GetTransactionHistoryElements() {

    var customerId = localStorage.getItem("CurrentCustomerIdForTransactionHistory");
    if (customerId == 0) {
        localStorage.setItem("PaginationTransferHistoryPageIndex",0);
        return false;
    }
    var pageSize = localStorage.getItem("PaginationTransferHistoryPageSize");
    var pageIndex = localStorage.getItem("PaginationTransferHistoryPageIndex");

    axios.get("https://localhost:5001/api/Account/GetTransactionHistoryElements",
        {
            params:
            {

                "pageIndex": pageIndex, "pageSize": pageSize, "customerId": customerId
            },
            headers:
            {
                Authorization: `Bearer ${localStorage.getItem('jwtToken')}`
            }
        })
        .then((responce) => {
            console.log(responce.data);
            var historyElements = responce.data;
            console.log(historyElements);
            if (historyElements.length === 0) {
                var index = localStorage.getItem("PaginationTransferHistoryPageIndex");
                if (index==0) {
                    document.getElementById("historyElementsStorage").innerHTML = "";
                }
                localStorage.setItem("PaginationTransferHistoryPageIndex", pageIndex - 1);
                return false;
            }
            var elements = "";
            for (var i = 0; i < historyElements.length; i++) {
                if (historyElements[i] == undefined) {
                    break;
                }
                elements += `<p style="font-size:14px">DateTime->${historyElements[i].dateTime}&nbsp&nbspTransaction message-> ${historyElements[i].description}</p>`;
            }
            document.getElementById("historyElementsStorage").innerHTML = elements;
        })
        .catch((error) => {
            console.log(error);
        });

}

