function AccountReplenishmentRecord(DistinationId,Amount) {
    var currentUserLogin = localStorage.getItem("CurrentUserLogin");

    axios.get("https://localhost:5001/api/Account/GetCurrencyCodeFromAccountId",
        {
            params: { id: DistinationId },
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                currency = responce.data;
                var record = `Customer with login ${currentUserLogin} replenished account with Id ${DistinationId} on Amount ${Amount} ${currency}`;
                RecordOperation(record);

            }

        }).catch((error) => {
            console.log(error);
        });

}

function TransferMoneyRecord(SourceId, DistinationId) {
    var currentUserLogin = localStorage.getItem("CurrentUserLogin");
    var destinationUserLogin = localStorage.getItem("DestinationLogin");
    var amount = localStorage.getItem("AmountToTransfer");``

    axios.get("https://localhost:5001/api/Account/GetCurrencyCodeFromAccountId",
        {
            params: { id: DistinationId },
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                currency = responce.data;
                var record = `Customer-> ${currentUserLogin} sent ${amount} ${currency} from account with Id-> ${SourceId} to Customer->${destinationUserLogin} account with Id-> ${DistinationId}`;
                RecordOperation(record);
            }

        }).catch((error) => {
            console.log(error);
        });
}

function GetAccountCurrencyFromId(accountId) {
    var numberAccountId = Number(accountId);
    axios.get("https://localhost:5001/api/Account/GetCurrencyCodeFromAccountId",
        {
            params: { id: numberAccountId },
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                currency = responce.data;



            }

        }).catch((error) => {
            console.log(error);
        });
}

function RecordOperation(record) {

    var operationArgs = {
        "Description": record
    };

    axios.post("https://localhost:5001/api/User/KeepHistoryOfOperation", operationArgs,
        {

            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                alert("Was recorded successfully !");
            }
        })
        .catch((error) => {
            console.log(error);
        });
}

