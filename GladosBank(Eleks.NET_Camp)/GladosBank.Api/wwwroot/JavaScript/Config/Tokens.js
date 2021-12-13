

setInterval(function () {
    IncreaseTime();
}, 1000);



async function UpdateToken() {

    var login = localStorage.getItem("CurrentLogin");
    var passowrdHash = localStorage.getItem("CurrentPasswordHash");

    var userParameters =
    {
        "Login": login,
        "PasswordHash": passowrdHash,
    };

    await axios.post("https://localhost:5001/api/User/Login", userParameters).then((response) => {
        var getResponse = response.status;
        if (getResponse == 200) {
            var JwtToken = response.data.jwtToken;

            localStorage.setItem("jwtToken", JwtToken);

        }
    }).catch((error) => {

        document.getElementById("ErrorLinkLog").innerText = error.response.data;

    });
}


function IncreaseTime() {
    
    var time = localStorage.getItem("JwtTokenTimeLiving");

    if (Number(time) == Number(600)) {
        localStorage.setItem("JwtTokenTimeLiving", Number(0));
        time = 0;
        UpdateToken();
    }

    localStorage.setItem("JwtTokenTimeLiving", Number(Number(time)+1));
}