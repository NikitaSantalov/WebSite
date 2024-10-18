async function signIn() {
    let email = document.getElementById("email").value
    let password = document.getElementById("password").value

    let response = await fetch('/customer/sign-in', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            email: email,
            password: password
        }) 
    })

    if (response.ok) {
        response.text().then((bearer) => {
            let token = bearer.split(' ')[1]
            console.log(token)
            document.cookie = `token=${token}; path=/;`
            document.location.replace('/')
        })
    }
}
