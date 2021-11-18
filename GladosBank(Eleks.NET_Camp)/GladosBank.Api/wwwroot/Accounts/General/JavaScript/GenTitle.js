
var token = localStorage.getItem('jwtToken');

axios.get("https://localhost:5001/api/User/GetUserData", {
    headers: { Authorization: `Bearer ${token}` }
})
    .then((responce) => {
        var getResponse = responce.status;
        if (getResponse == 200) {
            let role = responce.data.role;
            if (role == "Customer") {
                window.location = "../../Customer/html/CustomerBasePage.html";
            }
            else if (role == "Worker") {
                window.location = "../../Worker/html/WorkerBasePage.html";
            }
            else if (role == "Admin") {
                window.location = "../../Admin/html/AdminBasePage.html";
            }
            else {
                console.log(`Role: ${role} doesn't exist of!`);
            }
        }
        else {
            console.log(getResponse);
        }

}).catch((error) => {
    console.log(error);
});

