async function signUp() {
    let name = document.getElementById("name").value
    let lastName = document.getElementById("last-name").value
    let email = document.getElementById("email").value
    let phone = document.getElementById("phone").value
    let password = document.getElementById("password").value

    let response = await fetch('/customer/account', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            name: name,
            lastName: lastName,
            paymentType: "",
            deliveryType: "",
            address: "",
            email: email,
            phone: phone,
            password: password
        }) 
    })

    if (response.ok) {
        window.location.replace("/customer/sign-in")
    }   
}
