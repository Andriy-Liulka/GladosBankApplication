function SignInClick() {
  var login = document.getElementById("Login").value;
  var password = document.getElementById("Password").value;

    if (login == "" || password == "") {
        alert("All fields must be filled!");
        return false;
    }

    var passowrdHash = MD5(password);
    var userAuthenticationParameters =
    {
        Login: login,
        PasswordHash: passowrdHash,
    };
    SendRequest(userAuthenticationParameters);

}


function SendRequest(userAuthenticationParameters) {

    axios.post("https://localhost:5001/api/User/Login", userAuthenticationParameters).then((responce) => {
        var getResponse = responce.status;
        if (getResponse == 200) {
            var JwtToken = responce.data.jwtToken;

            localStorage.setItem("jwtToken", JwtToken);
            
            document.getElementById("ErrorLinkLog").innerHTML = "";

            window.location = "../Accounts/General/html/GenTitlePage.html";
        }
    }).catch((error)=>{
        document.getElementById("ErrorLinkLog").innerHTML = "Incorrect login ot password !";
    });
}
