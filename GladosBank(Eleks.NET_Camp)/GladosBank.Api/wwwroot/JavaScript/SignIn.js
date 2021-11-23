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

    axios.post("https://localhost:5001/api/User/Login", userAuthenticationParameters).then((response) => {
        var getResponse = response.status;
        if (getResponse == 200) {
            var JwtToken = response.data.jwtToken;

            localStorage.setItem("jwtToken", JwtToken);
            
            document.getElementById("ErrorLinkLog").innerText = "";

            window.location = "../Accounts/General/html/GenTitlePage.html";
        }
    }).catch((error) => {

        document.getElementById("ErrorLinkLog").innerText = error.response.data;
        
    });
}
