async function signIn(token) {
    let email = document.getElementById('email').value
    let password = document.getElementById('password').value

    let response = await fetch('/customer/sign-in', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': token
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
    })

    if (response.ok) {
        response.text().then((bearer) => {
            let today = new Date();
            today.setDate(today.getDate() + 7)
            document.cookie = `token=${bearer}; path=/; expires=${today}`
            document.location.replace('/')
        })
    }
}

function getToken() {
    var values = document.cookie.split(';')
    var token = ''

    for (let i = 0; i < values.length; i++) {
        let key = values[i].split('=')[0]
        let value = values[i].split('=')[1]

        if (key === 'token') {
            return value
        }
    }

    return token
}

token = getToken();

if (token) {
    signIn(token)
}