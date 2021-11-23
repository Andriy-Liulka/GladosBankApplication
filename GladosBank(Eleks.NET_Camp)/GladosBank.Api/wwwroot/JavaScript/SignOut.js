function SignOut() {
    localStorage.setItem("jwtToken", "");
    window.location = "../../../html/Title.html";
    
}