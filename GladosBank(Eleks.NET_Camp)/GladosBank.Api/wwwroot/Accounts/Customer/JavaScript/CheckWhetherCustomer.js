
var token = localStorage.getItem('jwtToken');
axios.get("https://localhost:5001/api/User/GetUserData", {
    headers: { Authorization: `Bearer ${token}` }
})
    .then((responce) => {
        var getResponse = responce.status;
        if (getResponse == 200 && responce.data.role =="Customer") {
            console.log("Authorized successfully");
        }
        else {
            console.log(getResponse);
            window.location = "../../../html/SignIn.html";
        }

    }).catch((error) => {
        console.log(error);
        window.location = "../../../html/SignIn.html";
    });