function SubmitClick() {
    var phoneDto = document.getElementById("Phone").value;
    var emailDto = document.getElementById("Email").value;
    var loginDto = document.getElementById("Login").value;
    var passwordDto = document.getElementById("Password").value;
    var roleDto = document.getElementById("Role").value;

    if (phoneDto == "" || emailDto == "" || loginDto == "" || passwordDto == "" || roleDto=="") {
        alert("All fields must be filled!");
        return false;
    }

    var hashedPassword = MD5(passwordDto);
    var userInfo =
    {
        MyUser:
        {
            Phone: phoneDto,
            Email: emailDto,
            Login: loginDto,
            PasswordHash: hashedPassword,
        },
        Role: roleDto
    };
    console.log(axios);
    GoToNextPage(userInfo);

}

function GoToNextPage(userInfo) {
    axios.post("https://localhost:5001/api/User/Create", userInfo).then((response) => {
        var getResponse = response.status;

        if (getResponse == 200) {
            window.location = "../html/SignIn.html";
        }
    }).catch((error) => {
        console.log(error);
        document.getElementById("ErrorLinkReg").innerHTML = "You entered login that already exist of !";
    });
}
//axios.get("https://localhost:5001/api/User/Get").then(response =>
//{
//    let users = response.data;
//    console.log(users);
//});

