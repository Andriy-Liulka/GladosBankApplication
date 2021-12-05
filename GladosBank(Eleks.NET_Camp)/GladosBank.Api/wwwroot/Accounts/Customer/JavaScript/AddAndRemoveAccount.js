

function AddAccountRemoveElementClick() {
    $(function () {
        $("body").append(div);
        $(div).html(htmlString2);
    });

    for (var i = 0; i < 10; i++) {
        varible = 5;
        document.getElementById("board").innerHTML += `<div style="color:black;background-color: red; width: 400px;"><p>${varible}</p><p>Amount</p><p>Note</p><p>Date</p></div>`;
    }

}

function RedirectGenTitlePage() {
    window.location = "../../General/html/GenTitlePage.html";
}

function RedirectUpdateCustomerDataPage() {
    window.location = "UpdateCustomerDataPage.html";
}


function RedirectTransferPageChooseDistinationPage() {
    window.location = "TransferPageChooseDistination.html";
}

function RedirectTransferMoneyPageChooseAmountDestinationPage() {
    window.location = "TransferMoneyPageChooseAmountDestination.html";
}

function RedirectTransferMoneyPage() {
    window.location = "TransferMoneyPage.html";
}

function RedirectTransferMoneyPageChooseAmountDestinationPage() {
    window.location = "TransferMoneyPageChooseAmountDestination.html";
}

function RedirectAddPage() {
    window.location = "PageWithAdd.html";
}


function RedirectCheckListPage() {
    window.location = "CheckListPage.html";
}

function RedirectBasePage() {
    window.location = "CustomerBasePage.html";
}

function RedirectReplenishPage(id) {
    window.location = "ReplenishAccountPage.html";

    localStorage.setItem("AccountId", id);
}

function AddClick(){
    var amount = 0;
    var note = document.getElementById("Note").value;
    var currency = document.getElementById("Currency").value;

    if (currency == "") {
        alert("Currency can't be empty !");
        return false;
    }

    document.getElementById("Note").value="";
    document.getElementById("Currency").value = "";

    if (note == "" || currency == "") {
        alert("All fields must be filled!");
        return false;
    }

    var accountArgs =
    {

        "account": {
            "currencyCode": currency.split("(").pop().split(")").shift(),
            "amount": amount,
            "notes": note,
        }
    };

    axios.post("https://localhost:5001/api/Account/Create", accountArgs,
        {
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((response) => {
        var responceResult = response.status;
        if (responceResult==200) {
            console.log("Added successfully");
            console.log(responceResult);
        }
    })
        .catch((error) => {
            console.log(error);
        });


}

function DeleteClick(id) {
    var element = document.getElementById(id);
    element.remove();
    var accountId =
    {
        "id": parseInt(id)
    }

    axios.post("https://localhost:5001/api/Account/Delete", accountId ,
        {
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` },
        })
        .then((response) => {
            var responceResult = response.status;
            if (responceResult == 200) {
                console.log("Added successfully");
                console.log(responceResult);
            }
            console.log(response);
        })
        .catch((error) => {
             console.log(error);
        });
}

function ReplenishAccountClick() {
    var item = localStorage.getItem('AccountId');
    var id = parseInt(item);

    var amount = document.getElementById('Amount').value;

    if (amount == "" || amount == null) {
        alert("Amount can't be empty !");
        return false;
    }

    var replenishAccountArgs = {
        "id": id,
        "amount": amount
    }

    axios.post("https://localhost:5001/api/Account/Replenish", replenishAccountArgs,
        {
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` },
        })
        .then((response) => {
            var responceResult = response.status;
            if (responceResult == 200) {
                console.log("Replenished successfully");
                console.log(responceResult);

                AccountReplenishmentRecord(item, amount);

                document.getElementById('Amount').value = "";
                document.getElementById('ErrorList').innerHTML = "";
            }
            console.log(response);
            document.getElementById('ErrorList').value = "Cannot add money";
        })
        .catch((error) => {
            console.log(error);
            document.getElementById('ErrorList').innerHTML = "Cannot add money";
        });

}