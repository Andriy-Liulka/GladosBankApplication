﻿
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

    axios.get("https://localhost:5001/api/Currency/GetCurrencyCodeFromAccountId",
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
    var currency = localStorage.getItem("DestinationCurrency");
    var destinationLogin = localStorage.getItem("DestinationLogin");
    axios.get("https://localhost:5001/api/Account/GetAccountsFromCurrencyCode",
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
            document.getElementById("board").innerHTML = `<p style="font - size: 30px; text - align: center; background - color: orangered">Choose account to send money</p>`;
            var destinationAccountLogin = localStorage.getItem("DestinationLogin");
            for (var i = 0; i < accounts.length; i++) {
                var destinationAccountId = accounts[i].id;
                document.getElementById("board").innerHTML += `<div onclick="TransactMoney(this.id)" id=${destinationAccountId} style = "margin: 10px; color:black;background-color: red; width: 400px; min-height: 80px;" >` +
                    `<p>Account owner -> ${destinationAccountLogin}</p>`+
                    `<p>Number-> ${i + 1}</p>` +
                    `<p>Currency-> ${accounts[i].currencyCode} ` +
                    `</div>`;
            }
            ConvertMoney();
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
                alert("Transaction finished successfully !");
                RedirectBasePage();
            }
        })
        .catch((error) => {
            console.log(error);
        });
}

function ConvertMoney() {

    var sourceCurrency = localStorage.getItem("TransferCurrency");
    var destinationCurrency = localStorage.getItem("DestinationCurrency");
    var amount = localStorage.getItem("AmountToTransfer");

    var operationArgs = {
        "SourceCurrency": sourceCurrency,
        "DestinationCurrency": destinationCurrency,
        "Amount": amount
    };

    axios.post("https://localhost:5001/api/Account/ConvertMoney", operationArgs,
        {
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                localStorage.setItem("FinalAmount", responce.data);
                console.log(responce);
                document.getElementById("AmountField").innerText = localStorage.getItem("FinalAmount");
            }
        })
        .catch((error) => {
            console.log(error);
        });

}