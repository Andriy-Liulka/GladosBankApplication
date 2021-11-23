
function ConfirmNewUserDataClick() {

    var login = document.getElementById("loginUpdate").value;
    var phone = document.getElementById("phoneUpdate").value;
    var email = document.getElementById("emailUpdate").value;
    

    if (login == "" || login == null || email == "" || email == null || phone == "" || phone == null) {
        alert("All fields must be filled !");
    }
    let role = localStorage.getItem("Role");

    var userUpdatedData={
        "Login": login,
        "Email": email,
        "Phone": phone,
        "Role":  role
    }

    axios.post("https://localhost:5001/api/User/Update", userUpdatedData,
        {
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` },
        })
        .then((response) => {
            var responceResult = response.status;
            if (responceResult == 200) {
                CleanInputFields();
                console.log("Updated successfully");
                console.log(response);

                var newToken = response.data.token;
                localStorage.setItem("jwtToken", newToken);
                //localStorage.getItem("jwtToken").value = newToken;
                UpdateLocalStorage(login, phone, email);

                RedirectGenTitlePage();
                //RedirectBasePage();
            }

        })
        .catch((error) => {
            console.log(error);
            console.log("Error");
        });
     
}
function CleanInputFields() {
    document.getElementById("loginUpdate").value = "";
    document.getElementById("phoneUpdate").value = "";
    document.getElementById("emailUpdate").value = "";
}


function UpdateLocalStorage(login,phone,email) {
    localStorage.setItem("CurrentUserLogin", login);
    localStorage.setItem("CurrentUserPhone", phone);
    localStorage.setItem("CurrentUserEmail", email);
}