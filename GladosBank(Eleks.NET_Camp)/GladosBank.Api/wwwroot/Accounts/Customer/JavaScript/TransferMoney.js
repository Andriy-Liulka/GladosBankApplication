
function TransferMoneyClick(id) {
    RedirectTransferMoneyPageChooseAmountDestinationPage();

    localStorage.setItem('SourceAccountId', id);


}

function SendClick() {

    var amount = document.getElementById("Amount").value;
    var destinationLogin = document.getElementById("Distant").value;
    if (amount == "" || destinationLogin == "" || destinationLogin == null || amount == null) {
        alert("Fields must be filled !");
        return false;
    }

    localStorage.setItem("DestinationLogin", destinationLogin);


    var sourceId = localStorage.getItem("SourceAccountId");

    axios.get("https://localhost:5001/api/Account/GetCurrencyCodeFromAccountId",
        {
            params: { id: sourceId} ,
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse==200) {
                var currency = responce.data;

                localStorage.setItem("AmountToTransfer", amount);
                localStorage.setItem("DestinationLogin", destinationLogin);
                localStorage.setItem("TransferCurrency", currency);

                RedirectTransferPageChooseDistinationPage();
            }
            
        }).catch((error) => {
            console.log(error);
        });
}

function TransferMoneyGetAllPossibleDistinationAccounts() {
    var currency = localStorage.getItem("TransferCurrency");
    var destinationLogin = localStorage.getItem("DestinationLogin");
    axios.get("https://localhost:5001/api/Account/GetAccountsForCurrencyCode",
        {
            params:
            {
                 "CurrencyCode": currency ,  "Login": destinationLogin 
            },
            headers:
            {
                Authorization: `Bearer ${localStorage.getItem('jwtToken')}`
            }
        }).
        then((responce) => {
            console.log(responce.data);
            var accounts = responce.data;
            console.log(accounts);
            var destinationAccountLogin = localStorage.getItem("DestinationLogin");
            for (var i = 0; i < accounts.length; i++) {
                var destinationAccountId = accounts[i].id;
                document.getElementById("board").innerHTML += `<div onclick="TransactMoney(this.id)" id=${destinationAccountId} style = "margin: 10px; color:black;background-color: red; width: 400px; min-height: 80px;" >` +
                    `<p>Account wwner -> ${destinationAccountLogin}</p>`+
                    `<p>Number-> ${i + 1}</p>` +
                    `<p>Currency-> ${accounts[i].currencyCode} ` +
                    `</div>`;
            }
            console.log(localStorage);
        })
        .catch((error) => {
            console.log(error);
        });
}

function TransactMoney(id) {
    var distinationId = id;
    var sourceId = localStorage.getItem('SourceAccountId');
    var amount = localStorage.getItem("AmountToTransfer");

    var transactData = {
        "Amount": amount,
        "sourceId": sourceId,
        "destinationId": distinationId
    }

    axios.post("https://localhost:5001/api/Account/TransactMoney",transactData,
        {

            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                
                TransferMoneyRecord(sourceId, distinationId);
                alert("Transaction finiched successfully !");
                RedirectBasePage();
            }
        })
        .catch((error) => {
            console.log(error);
        });
}