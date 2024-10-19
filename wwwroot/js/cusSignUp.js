async function signUp() {
    let email = document.getElementById("email")
    let password = document.getElementById("password")

    let response = await fetch('customer/sign-up', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            email: email,
            password: password
        }) 
    })

    window.location.replace("/customer/sign-in")
}
