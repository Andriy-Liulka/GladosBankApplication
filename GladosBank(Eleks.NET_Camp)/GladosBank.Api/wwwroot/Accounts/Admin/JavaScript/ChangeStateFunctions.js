function ChangeStateClick(userId) {
    var id = Number(userId);

    axios.post("https://localhost:5001/api/User/BlockUnblockUser", null,
        {
            params:{"UserId":id},
            headers: { Authorization: `Bearer ${localStorage.getItem('jwtToken')}` }
        })
        .then((responce) => {
            var getResponse = responce.status;
            if (getResponse == 200) {
                document.location.reload();
            }
        })
        .catch((error) => {
            console.log(error);
        });
}