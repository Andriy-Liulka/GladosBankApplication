function FillTableCustomers() {
    var pageSize = Number(localStorage.getItem("PaginationPageSize"));
    var pageIndex = Number(localStorage.getItem("PaginationPageIndex"));

    var inputObject = {
        "PaginatedArgs": {
            "pageIndex": pageIndex,
            "pageSize": pageSize
        }
    }

    axios.get("https://localhost:5001/api/User/GetPaginatedListOfCustomers",
        {
            params:
            {

                "pageIndex": pageIndex, "pageSize": pageSize
            },
            headers:
            {
                Authorization: `Bearer ${localStorage.getItem('jwtToken')}`
            }
        })
        .then((responce) => {
            console.log(responce.data);
            var accounts = responce.data;
            console.log(accounts);
            var currentUser = localStorage.getItem("CurrentUserLogin");

            if (accounts.length === 0) {
                localStorage.setItem("PaginationPageIndex", pageIndex - 1);
                return false;
            }
            var index = 0;
            var tableContent = "<Table>";
            for (var i = 0; i < 3; i++) {
                if (accounts[index] == undefined) {
                    break;
                }
                tableContent += "<tr>";
                for (var j = 0; j < 2; j++) {
                    if (accounts[index] == undefined) {
                        tableContent += "</tr>";
                        break;
                    }
                    if (accounts[index].user.login === currentUser) {
                        index++;
                    }

                    tableContent += "<td>";

                    tableContent +=
                        `<div  style = "margin: 10px; color:black;background-color: rgb(153, 164, 89); width: 400px; height: 200px;" >` +
                    `<p>Account owner ->${accounts[index].user.login}</p>` +
                    `<p>PhoneNumber->${accounts[index].user.phone}</p>` +
                    `<p>email->${accounts[index].user.email}</p>` +
                    `<p>IsActive->${accounts[index].user.isActive}</p>` +
                        `</div>`;

                    tableContent += "</td>";
                    index++;
                }
                tableContent += "</tr>";
            }

            tableContent += "</Table>";

            document.getElementById("CustomersListDiv").innerHTML = tableContent;

        })
        .catch((error) => {
            console.log(error);
        });


}