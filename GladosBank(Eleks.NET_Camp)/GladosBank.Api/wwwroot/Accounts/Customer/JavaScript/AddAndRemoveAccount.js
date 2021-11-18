var div = document.createElement("div");
var htmlString =
    "<br>" +
    '<div style="background-color: rgb(32, 77, 95); width: 1100px; min-height: 200px; float:right">' +
    '<div style="float: left;">' +
    '<label for="Amount" style="font-size: 30px;">&nbsp Amount</label>' +
    '<input id="Amount" name="Amount" type="text" style="width: 230px; font-size: 20px;">' +
    "</div>" +
    '<div style=" float:right;  margin-right: 15px;  margin-top: 5px; ">' +
    '<button id="AddButton" style="height: 35px; width: 100px; background-color:rgb(219, 169, 31); font-size: 20px;">Add</button>' +
    "</div>" +
    '<div style="float:inline-start;">' +
    '<label style="font-size: 30px;" for="Currency">&nbsp&nbspChoose currency</label>' +
    '<select  name="Currency" id="Currency">' +
    "</select>" +
    "</div>" +
    "<br>" +
    "<div>" +
    '<textarea id="Note"  type="textarea" style="float: left; margin-left: 15px; font-size: 23px; width: 1060px; height: 125px;">' +
    "</textarea>" +
    "</div>" +
    "</div>";

var htmlString2 =
    '<div id="board" style="float: right; min-width: 1000px; min-height: 825px; background-color:rgb(30, 194, 126);">' +
    "</div>";

function AddAccountClick() {
    $(function () {
        $("body").append(div);
        $(div).html(htmlString);
    });

    daySelect = document.getElementById('Currency');
    daySelect.options[daySelect.options.length] = new Option('Text 1', 'Value1');
    
    //var min = 1,
    //    max = 100,
    //    select = document.getElementById("Currency");

    //for (var i = min; i <= max; i++) {
    //    var opt = document.createElement("option");
    //    opt.value = i;
    //    opt.innerHTML = i;
    //    select.appendChild(opt);
    //}

}



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
    window.location = "ReplenishAccountOage.html";

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
            }
            console.log(response);
            document.getElementById('ErrorList').value = "Cannot add modey";
        })
        .catch((error) => {
            console.log(error);
            alert("Error");
        });

}