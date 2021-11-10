function SubmitClick() {
    var phoneDto = document.getElementById("Phone").value;
    var emailDto = document.getElementById("Email").value;
    var loginDto = document.getElementById("Login").value;
    var passwordDto = document.getElementById("Password").value;
    var roleDto = document.getElementById("Role").value;

    var userInfo =
    {
        MyUser:
        {
            Phone: phoneDto,
            Email: emailDto,
            Login: loginDto,
            Password: passwordDto,
        },
        Role: roleDto
    };


    axios.post("https://localhost:5001/api/User/Create", userInfo);



}
//axios.get("https://localhost:5001/api/User/Get").then(response =>
//{
//    let users = response.data;
//    console.log(users);
//});

