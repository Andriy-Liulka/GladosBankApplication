function FillTable() {
    var pageSize = Number(localStorage.getItem("PaginationPageSize"));
    var pageIndex = Number(localStorage.getItem("PaginationPageIndex"));

    var inputObject = {
        "PaginatedArgs": {
            "pageIndex": pageIndex,
            "pageSize": pageSize
        }
    }

    axios.get("https://localhost:5001/api/User/GetPaginatedList",
        {
            params:
            {

                "pageIndex": pageIndex , "pageSize": pageSize
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
                    if (accounts[index].login === currentUser) {
                        index++;
                    }

                    tableContent += "<td>";

                    tableContent +=
                        `<div  style = "margin: 10px; color:black;background-color: red; width: 400px; height: 200px;" >` +
                            `<p>Account owner ->${accounts[index].login}</p>` +
                            `<p>PhoneNumber->${accounts[index].phone}</p>` +
                            `<p>email->${accounts[index].email}</p>` +
                            `<p>PasswordHash->${accounts[index].passwordHash}</p>` +
                            `<p>IsActive->${accounts[index].isActive}</p>` +
                            `<button id=${accounts[index].id} onclick="ChangeStateClick(this.id)" style="margin-top: 7px;">Change state</button>&nbsp&nbsp&nbsp` +
                        `</div>`;

                    tableContent += "</td>";
                    index++;
                }
                tableContent += "</tr>";
            }

            tableContent += "</Table>";

            document.getElementById("UsersListDiv").innerHTML = tableContent;

            })
        .catch((error) => {
            console.log(error);
        });


}
