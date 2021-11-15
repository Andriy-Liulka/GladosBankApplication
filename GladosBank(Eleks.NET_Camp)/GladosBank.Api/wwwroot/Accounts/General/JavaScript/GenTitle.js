

axios.get("https://localhost:5001/api/User/GetUserDataFromJwt", {
    params: { jwtToken: localStorage.getItem('jwtToken') },
    headers: { Authorization: token }
})
    .then((responce) => {
        console.log(responce.data.role);


}).catch((error) => {
    console.log(error);
});

