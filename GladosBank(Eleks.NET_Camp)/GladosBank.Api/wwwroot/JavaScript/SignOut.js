function SignOut() {
    localStorage.setItem("jwtToken", "");
    window.location = "../../../html/SignIn.html";
    
}